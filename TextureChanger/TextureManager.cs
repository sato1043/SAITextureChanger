using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TextureChanger.util;
using Win32;
using Ionic.Zip;

namespace TextureChanger
{
	class TextureManager
	{
		#region 「にじみ形状」のための定数
		/**
		"にじみ形状"
		  1. 256x256, 512x512, 1024x1024いずれかのサイズでグレースケールビットマップを作成
		  2. blotmap フォルダにその画像を置く
		  3. brushform.conf に" 1,blotmap\\画像名.bmp "と追記する
		*/
		public const string BLOTMAP_NAME = "にじみ形状(blotmap)";
		private const string BRUSHFORM_CONF = "\\brushform.conf";
		private const string BLOTMAP_CONF = BRUSHFORM_CONF;
		private const string BLOTMAP_DIR = "blotmap";
		private const string BLOTMAP_SIGN = "1,";
		#endregion

		#region 「筆形状」のための定数
		/**
		"筆形状"
		  1. 63x63のビットマップの任意の位置にRGB(0,0,0)の点を打つ(63個まで)
		  2. elemap フォルダにその画像を置く
		  3. brushform.conf に" 2,elemap\\画像名.bmp "と追記する
		*/
		public const string ELEMAP_NAME = "筆形状(elemap)";
		private const string ELEMAP_CONF = BRUSHFORM_CONF;
		private const string ELEMAP_DIR = "elemap";
		private const string ELEMAP_SIGN = "2,";
		#endregion

		#region 「テクスチャ」のための定数
		/**
		"テクスチャ"
		  1. 256x256, 512x512, 1024x1024いずれかのサイズでグレースケールビットマップを作成
		  2. brushtex フォルダにその画像を置く
		  3. brushtex.conf に" 1,brushtex\\画像名.bmp "と追記する
		*/
		public const string BRUSHTEX_NAME = "テクスチャ(brushtex)";
		private const string BRUSHTEX_CONF = "\\brushtex.conf";
		private const string BRUSHTEX_DIR = "brushtex";
		private const string BRUSHTEX_SIGN = "1,";
		#endregion

		#region 「用紙質感」のための定数
		/**
		"用紙質感"
		  1. 256x256, 512x512, 1024x1024いずれかのサイズでグレースケールビットマップを作成
		  2. papertex フォルダにその画像を置く
		  3. papertex.conf に" 1,papertex\\画像名.bmp "と追記する
		*/
		public const string PAPERTEX_NAME = "用紙質感(papertex)";
		private const string PAPERTEX_CONF = "\\papertex.conf";
		private const string PAPERTEX_DIR = "papertex";
		private const string PAPERTEX_SIGN = "1,";

		#endregion

		#region SAITextureFormat：テクスチャ種毎の書式情報
		private class SAITextureFormat
		{
			public string confpath;
			public string directory;
			public string name;
			public string sign;
			public Size[] sizes;
			public List<string> image_vector;

			public SAITextureFormat(
				  string confpath_
				, string directory_
				, string sign_
				, string name_
				, Size[] sizes_
				)
			{
				confpath = confpath_;
				directory = directory_;
				name = name_;
				sign = sign_;
				sizes = sizes_;
				image_vector = new List<string>();
			}

			public string CreateConfFileText()
			{
				var lines = new List<string>();
				foreach (string imagePath in image_vector)
				{
					lines.Add(sign + imagePath);
				}
				return string.Join(Environment.NewLine, lines.ToArray()) + Environment.NewLine;
			}
		};
		#endregion

        public const int ListViewPreviewWidth = 200;
        public const int ListViewPreviewHeight = 200;

		private const string SignFileName = "\\TextureChangerBackup";


		private readonly List<SAITextureFormat> _saiTextureFormatList;

		private readonly string _pathToSaiFolder;

		public TextureManager(string pathToSaiFolder, IWin32Window owner)
		{
			if (string.IsNullOrEmpty(pathToSaiFolder))
			{
				throw new ArgumentNullException("pathToSaiFolder");
			}

			_pathToSaiFolder = pathToSaiFolder;

			_saiTextureFormatList = new List<SAITextureFormat>();

			#region SAIのテクスチャ種毎書式情報の定義
			_saiTextureFormatList.Add(
				new SAITextureFormat(
					BLOTMAP_CONF
					, BLOTMAP_DIR
					, BLOTMAP_SIGN
					, BLOTMAP_NAME
					, new[] { new Size(256, 256), new Size(512, 512), new Size(1024, 1024) })
				);
			_saiTextureFormatList.Add(
				new SAITextureFormat(
					ELEMAP_CONF
					, ELEMAP_DIR
					, ELEMAP_SIGN
					, ELEMAP_NAME
					, new[] { new Size(63, 63) })
				);
			_saiTextureFormatList.Add(
				new SAITextureFormat(
					BRUSHTEX_CONF
					, BRUSHTEX_DIR
					, BRUSHTEX_SIGN
					, BRUSHTEX_NAME
					, new[] { new Size(256, 256), new Size(512, 512), new Size(1024, 1024) })
				);
			_saiTextureFormatList.Add(
				new SAITextureFormat(
					PAPERTEX_CONF
					, PAPERTEX_DIR
					, PAPERTEX_SIGN
					, PAPERTEX_NAME
					, new[] { new Size(256, 256), new Size(512, 512), new Size(1024, 1024) })
				);
			#endregion

			#region SAIのテクスチャ種毎書式をロード
			LoadFormats();
			#endregion

			#region 初期バックアップ
			string zipFileBaseName = "backup-" + System.DateTime.Now.ToString("yyyyMMdd-HHmm") + "(I)";
            string[] files = Directory.GetFiles(_pathToSaiFolder, "backup-*(I).zip");
            if (files.Length == 0)
            {
                try
                {
                    createBackupZipArchive(owner, zipFileBaseName);
                    CenteredMessageBox.Show(owner,
                        "あなたが指定したSAIフォルダに、現時点でのテクスチャ情報をバックアップしました。\n" +
                        "バックアップファイルは「" + _pathToSaiFolder + "\\" + zipFileBaseName + ".zip」です。　　　\n" +
                        "\n" +
                        "SAIフォルダにある４つのconfと４つのフォルダ、\n" +
                        "「blotmap」「elemap」「brushtex」「papertex」を元に戻すことで、設定を現時点の状態に戻せます。\n" +
                        "（戻すときはSAIとTextureChangerと両方終了した状態で行なってください）"
                        , "SAIのテクスチャ設定のバックアップ報告"
                        , MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    CenteredMessageBox.Show(owner,
                        "編集前のテクスチャ情報の、バックアップに失敗しました。\n" +
                        "編集を行う前にSAIフォルダにある４つのconfと４つのフォルダ、\n" +
                        "「blotmap」「elemap」「brushtex」「papertex」を手動でバックアップすることをおすすめします。　　\n" +
                        "編集をした後でも、これらのファイルを元に戻すことで、設定を現時点の状態に戻せます。" +
                        "（戻すときはSAIとTextureChangerと両方終了した状態で行なってください）"
                        , "SAIのテクスチャ設定のバックアップ失敗"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            #endregion
        }

        #region テクスチャ種別名からサブディレクトリ名を得る
        public string GetDirectoryFromConfname(string targetConfName)
        {
            var targetFormat =
                _saiTextureFormatList
                    .FirstOrDefault(saiTextureFormat =>
                        targetConfName == saiTextureFormat.name);
            if (targetFormat == null)
            {
                throw new ArgumentOutOfRangeException("targetConfName");
            }

            return targetFormat.directory;
        }
        #endregion

        #region 文字列を改行コードまでに切り詰める
        private void CutOffToCrLf(ref string line)
		{
			int pos = line.IndexOf("\r\n", System.StringComparison.OrdinalIgnoreCase);
			if (0 < pos)
				line = line.Substring(0, pos + 1);
		}
		#endregion

		#region テクスチャ情報をファイルから読み込み
		private void LoadFormats()
		{
			foreach (SAITextureFormat saiTextureFormat in _saiTextureFormatList)
			{
				int signLen = saiTextureFormat.sign.Length;
			
				string[] lines = File.ReadAllLines(
					_pathToSaiFolder + saiTextureFormat.confpath
					, Encoding.GetEncoding(932) );

				foreach (string temp in lines)
				{
					string line = temp;
					
					CutOffToCrLf(ref line);

					if (line == "")
						continue;
					if (line.Substring(0, signLen) != saiTextureFormat.sign)
						continue;

					string imgfile = line.Substring(signLen);

					if (File.Exists(_pathToSaiFolder + "\\" + imgfile) == false)
						continue;

					saiTextureFormat.image_vector.Add(imgfile);
				}
			}
		}
		#endregion

		#region テクスチャ情報をファイルに書き込み
		public void SaveFormats(params string[] targetConfs)
		{
			//保存対象が省略（無指定）された場合、すべてを保存する
			if (targetConfs.Length == 0)
			{
				targetConfs = new[]
				{
					BLOTMAP_NAME, ELEMAP_NAME
					, BRUSHTEX_NAME, PAPERTEX_NAME
				};
			}

			//テクスチャ種毎に保存
			var fileList = new List<string>();
			foreach (SAITextureFormat saiTextureFormat in _saiTextureFormatList)
			{
				//保存対象でなければスキップ
				if (targetConfs.Contains(saiTextureFormat.name) == false)
				{
					//TODO ひとつのファイルで複数のテクスチャ種が保存されているファイルで、一方だけ保存対象にされた場合にもう一方が消えてしまう
					continue;
				}

				//このメソッドの中で一度も開いたことがない設定ファイルはtruncate
				//このメソッド中で開いたことがある設定ファイルならばappend
				if (fileList.Contains(saiTextureFormat.confpath) == false)
				{
					fileList.Add(saiTextureFormat.confpath);
					File.WriteAllText(
						_pathToSaiFolder + saiTextureFormat.confpath
						, saiTextureFormat.CreateConfFileText()
						, Encoding.GetEncoding(932));
				}
				else
				{
					File.AppendAllText(
						_pathToSaiFolder + saiTextureFormat.confpath
						, saiTextureFormat.CreateConfFileText()
						, Encoding.GetEncoding(932));
				}
			}
		}
		#endregion

		#region テクスチャ情報ファイルのバックアップ
		public void Backup(IWin32Window owner)
		{
            string zipFileBaseName = "backup-" + System.DateTime.Now.ToString("yyyyMMdd-HHmm");
            string[] files = Directory.GetFiles(_pathToSaiFolder, zipFileBaseName+".zip");
            if (files.Length == 0)
            {
                try
                {
                    createBackupZipArchive(owner, zipFileBaseName);
                    CenteredMessageBox.Show(owner,
                        "あなたが指定したSAIフォルダに、現時点でのテクスチャ情報をバックアップしました。\n" +
                        "バックアップファイルは「" + _pathToSaiFolder + "\\" + zipFileBaseName + ".zip」です。　　　\n" +
                        "\n" +
                        "SAIフォルダにある４つのconfと４つのフォルダ、\n" +
                        "「blotmap」「elemap」「brushtex」「papertex」を元に戻すことで、設定を現時点の状態に戻せます。\n" +
                        "（戻すときはSAIとTextureChangerと両方終了した状態で行なってください）"
                        , "SAIのテクスチャ設定のバックアップ報告"
                        , MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    CenteredMessageBox.Show(owner,
                        "編集前のテクスチャ情報の、バックアップに失敗しました。\n" +
                        "編集を行う前にSAIフォルダにある４つのconfと４つのフォルダ、\n" +
                        "「blotmap」「elemap」「brushtex」「papertex」を手動でバックアップすることをおすすめします。　　\n" +
                        "編集をした後でも、これらのファイルを元に戻すことで、設定を現時点の状態に戻せます。" +
                        "（戻すときはSAIとTextureChangerと両方終了した状態で行なってください）"
                        , "SAIのテクスチャ設定のバックアップ失敗"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                CenteredMessageBox.Show(owner,
                    "バックアップファイルが既に存在しました。\n" +
                    "バックアップファイルは「" + _pathToSaiFolder + "\\" + zipFileBaseName + ".zip」です。　　　\n"
                    , "SAIのテクスチャ設定のバックアップ報告"
                    , MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
		#endregion

		#region バックアップZIPファイルを作る
		private void createBackupZipArchive( IWin32Window owner, string zipFileBaseName )
		{
			try
			{
				using( ZipFile zip = new ZipFile( Encoding.GetEncoding( 932 ) ) )
				{
					zip.Comment = "This zip was created at " + DateTime.Now.ToString( "G" );

					string signFilePath = _pathToSaiFolder + SignFileName;
					StreamWriter sw = new StreamWriter( signFilePath, false, Encoding.GetEncoding( "shift_jis" ) );
					sw.Write( "Date: " + DateTime.Now.ToString( "G" ) );
					sw.Close( );
					zip.AddFile( signFilePath ).FileName = SignFileName;

					var fileList = new List<string>( );

					foreach( SAITextureFormat saiTextureFormat in _saiTextureFormatList )
					{
						if( fileList.Contains( saiTextureFormat.confpath ) == false )
						{
							fileList.Add( saiTextureFormat.confpath );
							zip.AddFile( _pathToSaiFolder + saiTextureFormat.confpath ).FileName = saiTextureFormat.confpath;
						}

						foreach( var f in Directory.GetFiles( _pathToSaiFolder + "\\" + saiTextureFormat.directory ) )
						{
							zip.AddFile( f ).FileName = "\\" + saiTextureFormat.directory + "\\" + Path.GetFileName( f );
						}
					}
					zip.Save( _pathToSaiFolder + "\\" + zipFileBaseName + ".zip" );

					File.Delete( signFilePath );

				}
			}
			catch
			{
				throw;
			}
		}
		#endregion

		#region テクスチャ情報ファイルのリストア
		public void Restore(IWin32Window owner)
		{
			CenteredMessageBox.Show( owner,
				"テクスチャ情報のリストアをはじめます。\n"
				+ "現在のSAIのテクスチャ情報が消去され、\n"
				+ "指定したZIPファイルの内容で上書きされます。\n"
				+ "リストア内容が壊れていたり、リストアが途中失敗したりすると\n"
				+ "SAIが動作異常を起こす場合がありますので\n"
				+ "注意してお使いください。\n"
				, "SAIのテクスチャ設定のリストア"
				, MessageBoxButtons.OK, MessageBoxIcon.Information );

			// リストア前バックアップ
			DialogResult res = CenteredMessageBox.Show( owner,
				"リストアする前に、今のテクスチャ情報をバックアップしますか？\n"
				, "SAIのテクスチャ設定のリストア"
				, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information );
			if( res == DialogResult.Cancel )
				return;
			if( res == DialogResult.Yes )
			{
				string zipFileBaseName = "backup-" + DateTime.Now.ToString( "yyyyMMdd-HHmm" );
				string[] files = Directory.GetFiles( _pathToSaiFolder, zipFileBaseName + ".zip" );
				if( files.Length != 0 )
				{
					res = CenteredMessageBox.Show( owner,
						"現在のSAIのテクスチャ設定をバックアップできませんでした：\n"
						+ _pathToSaiFolder + zipFileBaseName + ".zip" + "\n"
						+ "続行しますか？"
						, "SAIのテクスチャ設定のリストア報告"
						, MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
					if( res == DialogResult.No )
						return;
				}
				else
				{
					try
					{
						createBackupZipArchive( owner, zipFileBaseName );
					}
					catch
					{
						res = CenteredMessageBox.Show( owner,
							"現在のSAIのテクスチャ設定をバックアップできませんでした：\n"
							+ _pathToSaiFolder + zipFileBaseName + ".zip" + "\n"
							+ "続行しますか？"
							, "SAIのテクスチャ設定のリストア報告"
							, MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
						if( res == DialogResult.No )
							return;
					}
				}
			}
			
			// リストア元ファイルパスを問合せ
			var openFileDialog = new OpenFileDialog
			{
				FileName = "",
				InitialDirectory = _pathToSaiFolder,
				Filter = @"ZIPファイル(*.zip)|*.zip|すべてのファイル(*.*)|*.*",
				FilterIndex = 1,
				Title = @"リストア対象のZIPファイルの選択",
				RestoreDirectory = true,
				CheckFileExists = true,
				CheckPathExists = true
			};
			if (openFileDialog.ShowDialog() == DialogResult.Cancel)
				return;

			// リストア
			ExtractBackupZipFile(owner, openFileDialog.FileName);

		}
		#endregion

		#region バックアップZIPファイルの上書き展開
		private void ExtractBackupZipFile( IWin32Window owner, string zipFileFullPath )
		{
			// zipファイルの内容確認
			bool signFileFound = false;
			ReadOptions options = new ReadOptions
			{
				Encoding = Encoding.GetEncoding( "shift_jis" )
			};
			using( ZipFile zip = ZipFile.Read( zipFileFullPath, options ) )
			{
				foreach( ZipEntry entry in zip )
				{
					if( entry.FileName == SignFileName.Substring(1) )
					{
						signFileFound = true;
						break;
					}
				}
			}

			if( signFileFound == false )
			{
				CenteredMessageBox.Show( owner,
					"TextureChangerが作成したZIPファイルではありませんでした：\n"
					+ zipFileFullPath + "\n"
					, "SAIのテクスチャ設定のリストア報告"
					, MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			// 現在のファイルを削除
			var fileList = new List<string>( );

			foreach( SAITextureFormat saiTextureFormat in _saiTextureFormatList )
			{
				if( fileList.Contains( saiTextureFormat.confpath ) == false )
				{
					File.Delete( _pathToSaiFolder + saiTextureFormat.confpath );
					fileList.Add( saiTextureFormat.confpath );
				}
				foreach( var f in Directory.GetFiles( _pathToSaiFolder + "\\" + saiTextureFormat.directory ) )
				{
					File.Delete( f );
				}
			}

			// zipファイルを展開
			using( ZipFile zip = ZipFile.Read( zipFileFullPath, options ) )
			{
				zip.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
				zip.ExtractAll( _pathToSaiFolder );
				File.Delete(_pathToSaiFolder + SignFileName);
			}
		}
		#endregion


		#region ImageListオブジェクトの生成
		public static Image createThumbnail(Image image, int w, int h)
		{
			var canvas = new Bitmap(w, h);

			var g = Graphics.FromImage(canvas);
			g.FillRectangle(new SolidBrush(Color.White), 0, 0, w, h);
			
			var fw = (float)w / (float)image.Width;
			var fh = (float)h / (float)image.Height;

			var scale = Math.Min(fw, fh);
			fw = image.Width * scale;
			fh = image.Height * scale;

			g.DrawImage(image, (w - fw) / 2, (h - fh) / 2, fw, fh);
			g.Dispose();

			return canvas;
		}
		public string[] GetImagePathList(string targetConfName)
		{
			var targetFormat =
				_saiTextureFormatList
					.FirstOrDefault(saiTextureFormat =>
						targetConfName == saiTextureFormat.name);
			if (targetFormat == null)
			{
				throw new ArgumentOutOfRangeException("targetConfName");
			}
			
			return targetFormat.image_vector.ToArray();
		}
		public void GetImageList(string targetConfName, ImageList imageList)
		{
			var targetFormat =
				_saiTextureFormatList
					.FirstOrDefault(saiTextureFormat =>
						targetConfName == saiTextureFormat.name);
			if (targetFormat == null)
			{
				throw new ArgumentOutOfRangeException("targetConfName");
			}

			imageList.Images.Clear();
			imageList.ImageSize = targetFormat.sizes[0];//GUI側ではサイズを決められない

			foreach (var imagePath in targetFormat.image_vector)
			{
				var original = Bitmap.FromFile(_pathToSaiFolder + "\\" + imagePath);
                var thumbnail = createThumbnail(original, ListViewPreviewWidth, ListViewPreviewHeight);
				imageList.Images.Add(thumbnail);
				original.Dispose();
				thumbnail.Dispose();
			}
			
		}
		#endregion

		#region イメージ追加(処理しなかったときfalse)
		public bool AddImage(
			string targetConfName
			, string fromPath
			, IWin32Window owner
		)
		{
			var targetFormat =
				_saiTextureFormatList
					.FirstOrDefault(saiTextureFormat =>
						targetConfName == saiTextureFormat.name);
			if (targetFormat == null)
			{
				throw new ArgumentOutOfRangeException("targetConfName");
			}

			//ビットマップサイズのチェック
			var original = Bitmap.FromFile( fromPath );
			bool sizeFound = false;
			foreach(Size sz in targetFormat.sizes)
			{
				if (sz == original.Size)
				{
					sizeFound = true;
					break;
				}
			}
			if (sizeFound == false)
			{

				string availableSizesString = "";
				foreach( Size sz in targetFormat.sizes )
				{
					availableSizesString += "「" + sz.Width + " x " + sz.Height + "」";
				}
				CenteredMessageBox.Show( owner
					, targetConfName+"に登録可能な画像サイズではありませんでした。\n"
						+"登録しようとしたサイズ： 幅 "+ original.Size.Width + " x 高さ "+ original.Height +"\n"
						+ "登録可能なサイズ： " + availableSizesString
					, "登録失敗"
					, MessageBoxButtons.OK
					, MessageBoxIcon.Error );
				return false;
			}
			original.Dispose();

			//登録済みでないかチェック
			var imagePath = targetFormat.directory + "\\" + Path.GetFileName(fromPath);
			if (targetFormat.image_vector.Contains(imagePath))
			{
				return false; //登録済みだったので処理しなかった
			}

			var dst = _pathToSaiFolder+ "\\" + imagePath;

			if (File.Exists(dst))
			{
				//シェルのコピーコマンドで「上書きかスキップ」を選択できるのでファイル操作はそれにまかせる。
				//それだけだとSAIへの登録自体はそのまま続行されるので問い合わせ
				DialogResult res = CenteredMessageBox.Show( owner
					, "ビットマップファイルがすでに存在します\n"
						+ "登録を続行しますか？：\n"
						+ dst
					, "登録確認"
					, MessageBoxButtons.OKCancel
					, MessageBoxIcon.Question );

				if( res == DialogResult.Cancel )
					return false;
			}
			
			// Exploreシェルでファイルをコピーする。
			var shfop = new SH.SHFILEOPSTRUCT
			{
				hwnd = owner.Handle,
				wFunc = SH.FileFuncFlags.FO_COPY,
				pFrom = fromPath + '\0' + '\0',
				pTo = dst + '\0' + '\0',
				fFlags = SH.FILEOP_FLAGS.FOF_ALLOWUNDO
			};
			if (SH.SHFileOperation(ref shfop) != 0)
			{
				return false; //throw new System.IO.IOException(); //something happened
			}
			if (shfop.fAnyOperationsAborted == true)
			{
				return false; //ファイル処理中断
			}

			targetFormat.image_vector.Add(imagePath);
			return true;
		}
		#endregion

		#region イメージ削除(処理しなかったときfalse)
		public bool RemoveImage(
			string targetConfName
			,  string targetImagePath
			, IWin32Window owner
		)
		{
			var targetFormat =
				_saiTextureFormatList
					.FirstOrDefault(saiTextureFormat =>
						targetConfName == saiTextureFormat.name);
			if (targetFormat == null)
			{
				throw new ArgumentOutOfRangeException("targetConfName");
			}

			targetImagePath =
				targetFormat.image_vector
					.FirstOrDefault(imagePath =>
						targetImagePath == imagePath);
			if (targetImagePath == null)
			{
				throw new ArgumentOutOfRangeException("targetImagePath");
			}

			//エクスプローラーでファイル削除
			var shfop = new SH.SHFILEOPSTRUCT
			{
				hwnd = owner.Handle,
				wFunc = SH.FileFuncFlags.FO_DELETE,
				pFrom = _pathToSaiFolder + "\\" + targetImagePath + '\0' + '\0',
				fFlags = SH.FILEOP_FLAGS.FOF_ALLOWUNDO | SH.FILEOP_FLAGS.FOF_WANTNUKEWARNING | SH.FILEOP_FLAGS.FOF_NOCONFIRMATION
			};
			int res = SH.SHFileOperation(ref shfop);
			if (res == 1223)
			{
				shfop.fAnyOperationsAborted = true;
			}
			else if (res != 0)
			{
				targetFormat.image_vector.Remove(targetImagePath);//IOエラーなファイルを登録として残しておくのも不健康なので。
				throw new System.IO.IOException(); //something happened
			}
			if (shfop.fAnyOperationsAborted == true)
			{
				return false; //ファイル処理中断
			}

			//登録を削除
			targetFormat.image_vector.Remove(targetImagePath);
			return true;
		}
		#endregion
	}
}
