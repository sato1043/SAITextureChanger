using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TextureChanger.util;
using Win32;

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
		private struct SAITextureFormat
		{

			public string confpath;
			public string directory;
			public string name;
			public string sign;
			public SIZE[] sizes;
			public List<string> image_vector;

			public SAITextureFormat(
				  string confpath_
				, string directory_
				, string sign_
				, string name_
				, SIZE[] sizes_
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
					, new[] { new SIZE(256, 256), new SIZE(512, 512), new SIZE(1024, 1024) })
				);
			_saiTextureFormatList.Add(
				new SAITextureFormat(
					ELEMAP_CONF
					, ELEMAP_DIR
					, ELEMAP_SIGN
					, ELEMAP_NAME
					, new[] { new SIZE(63, 63) })
				);
			_saiTextureFormatList.Add(
				new SAITextureFormat(
					BRUSHTEX_CONF
					, BRUSHTEX_DIR
					, BRUSHTEX_SIGN
					, BRUSHTEX_NAME
					, new[] { new SIZE(256, 256), new SIZE(512, 512), new SIZE(1024, 1024) })
				);
			_saiTextureFormatList.Add(
				new SAITextureFormat(
					PAPERTEX_CONF
					, PAPERTEX_DIR
					, PAPERTEX_SIGN
					, PAPERTEX_NAME
					, new[] { new SIZE(256, 256), new SIZE(512, 512), new SIZE(1024, 1024) })
				);
			#endregion

			LoadFormats();

			InitialBackupFormats(owner);
		}

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

		#region テクスチャ情報ファイルのバックアップ・リストア
		private void InitialBackupFormats(IWin32Window owner)
		{
			BackupOrRestoreFormats(false, true, owner);
		}
		private void BackupFormats(IWin32Window owner)
		{
			BackupOrRestoreFormats(false, false, owner);
		}
		private void RestoreFormats(IWin32Window owner)
		{
			BackupOrRestoreFormats(true, false, owner);
		}
		private void BackupOrRestoreFormats(bool isRestore, bool isInitial, IWin32Window owner)
		{
			var fileList = new List<string>();
			bool initialMessageDone = false;

			foreach (SAITextureFormat saiTextureFormat in _saiTextureFormatList)
			{
				if (fileList.Contains(saiTextureFormat.confpath) == true)
				{
					continue; //filesetに登録済み、すなわちバックアップ済み
				}

				fileList.Add(saiTextureFormat.confpath);

				//パスを合成して
				var src = _pathToSaiFolder + saiTextureFormat.confpath + (isRestore ? "$" : "");
				var dst = _pathToSaiFolder + saiTextureFormat.confpath + (isRestore ? "" : "$");

				if (!isRestore && isInitial)
				{
					if (File.Exists(dst))
						continue;
					if (initialMessageDone == false)
					{
						CenteredMessageBox.Show(owner,
							"貴方が指定したSAIのフォルダにテクスチャ情報の現時点のバックアップが作成されます。　　　\n" +
							"\n" +
							"バックアップファイル名は「～.conf$」です。\n" +
							"同名の「～.conf」をこのバックアップで置き換えることで現時点の状態に戻せます。"
							, "SAIのテクスチャ設定のバックアップ報告"
							, MessageBoxButtons.OK, MessageBoxIcon.Information);
						initialMessageDone = true;
					}
				}

				// Exploreシェルでファイルをコピーする。
				var shfop = new SH.SHFILEOPSTRUCT
				{
					hwnd = owner.Handle,
					wFunc = SH.FileFuncFlags.FO_COPY,
					pFrom = src + '\0' + '\0',
					pTo = dst + '\0' + '\0',
					fFlags = SH.FILEOP_FLAGS.FOF_SILENT
							 | SH.FILEOP_FLAGS.FOF_ALLOWUNDO
							 | SH.FILEOP_FLAGS.FOF_NOCONFIRMATION
				};
				if (SH.SHFileOperation(ref shfop) != 0)
				{
					throw new System.IO.IOException();
				}
				if (shfop.fAnyOperationsAborted == true)
				{
					; //ファイル処理中断
				}

			}
		}
		#endregion


	}
}
