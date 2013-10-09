using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Windows.Forms;
using TextureChanger.util;
using Win32;

namespace TextureChanger
{
	// バージョンをあげるとき
	//  Releaseビルドに切り替えてから
	//   1) app.config
	//   2) TextureChangerプロジェクトのプロパティのアセンブリ情報
	//   2) セットアッププロジェクトのTextureChangerSetupのプロパティウィンドウ


    public partial class TextureChangerForm : Form
	{
		private TextureChangerOptions _textureChangerOptions;

	    public bool ForceExitProgram = false;

	    private const string SAI = "sai";

        #region ドラッグ中のカーソルの指定
		private Cursor moveCursor = Cursors.Hand;
        private Cursor copyCursor = Cursors.Hand;
        private Cursor linkCursor = Cursors.Hand;
        #endregion

        private DragImagesForm dragImagesForm;

        private Timer _saiProcessCheckTimer;

	    private TextureManager _textureManager;

	    public TextureChangerForm( )
		{
			InitializeComponent( );
            dragImagesForm = new DragImagesForm();
        }

		private void TextureChangerForm_Load(object sender, EventArgs e)
		{
			_textureChangerOptions = new TextureChangerOptions();

			#region SAIのフォルダが未指定のときはユーザに指定してもらう
			if (_textureChangerOptions.PathToSaiFolder == "")
			{
				string temp = this.RequestPathToSaiFolder();
				if (temp != "")
				{
					_textureChangerOptions.PathToSaiFolder = temp;
				}
				else
				{
					CenteredMessageBox.Show( this
						, "SAIのインストール先を設定できませんでした。\n" +
						  "操作先のSAIがわからないため、処理を継続できません。\n"+
						  "申し訳ありませんが、プログラムの起動を中断します。"
						, "TexureChanger起動確認"
						, MessageBoxButtons.OK, MessageBoxIcon.Error);
					ForceExitProgram = true;
					Application.Exit();
					return;
				}
			}
			//このプログラムの特性上、PathToSaiFolderが空ということは無しの前提で動かす
			#endregion

            #region メニューのチェック状態の再構築
		    SetCheckingForMenuFirstExpandingFolder();
		    mniPromptToExitProgram.Checked = _textureChangerOptions.PromptToExitProgram;
			#endregion

            #region ウィンドウの状況を復元
            Rectangle bounds;
            FormWindowState state;
		    _textureChangerOptions.LoadWindowConditions(out bounds, out state);
            Bounds = bounds;
            WindowState = state;

			splNorthSouth.SplitterDistance =
				_textureChangerOptions.SplitterDistanceNorthSouth <= splNorthSouth.Panel1MinSize
					? splNorthSouth.Panel1MinSize
					: _textureChangerOptions.SplitterDistanceNorthSouth;
			splTreeList.SplitterDistance =
				_textureChangerOptions.SplitterDistanceTreeList <= splTreeList.Panel1MinSize
					? splTreeList.Panel1MinSize
					: _textureChangerOptions.SplitterDistanceTreeList;
            #endregion

			#region 起動確認
			CenteredMessageBox.Show(this,
				"SAIに対するテクスチャの設定変更は、直接即座に上書きされます。　　　\n" +
				"また、TextureChanger起動中にSAIを起動した場合、\n" +
                "それまでの状況に関わらずSAIの設定が再読み込みされます。\n" +
				"\n" +
				"TextureChangerとSAIはお互いを知らないので、\n" +
                "SAI起動中にTextureChangerを実行することができません。\n"
				, "TexureChanger起動確認"
				, MessageBoxButtons.OK, MessageBoxIcon.Information);
			#endregion

			#region SAIの起動中確認とテクスチャ情報の読み込み
			_saiProcessCheckTimer = new Timer();
			_saiProcessCheckTimer.Tick += new EventHandler(SaiProcessCheckTimerHandler);
			_saiProcessCheckTimer.Interval = 1000;
			_textureManager = null;
			SaiProcessCheckTimerHandler(sender, e);
			#endregion

			#region エクスプローラの初期表示
			trvFolder.InitFolderTreeView(this);
			trvFolder.DrillToFolder(_textureChangerOptions.FirstExpandingFolder);
			#endregion

            #region ステータスバーに編集中のテクスチャのフォルダのフルパスを表示する
            lblStatus.Text = _textureChangerOptions.LastEditingTextureName + " - "
                + _textureChangerOptions.PathToSaiFolder
                + "\\" + _textureManager.GetDirectoryFromConfname(_textureChangerOptions.LastEditingTextureName);
            #endregion

			#region アプリケーションの準備ができた時点で更新を確認
			if (_textureChangerOptions.CheckUpdateAtStartUp)
			{
				HttpUpdater httpUpdater = new HttpUpdater(this);
				httpUpdater.BeginAsyncCheckAppConfigUpdated();
			}
			#endregion
		}

		#region タイマーハンドラ: SAI起動中確認と最新情報読み込み
		private void SaiProcessCheckTimerHandler(object sender, EventArgs e)
		{
			//リロード目的で呼ばれる場合：あらかじめ_textureManager=nullしてからこれを呼ぶ
			//プロセス監視目的で呼ばれる場合：既存の_textureManager!=nullで呼ばれる
			_saiProcessCheckTimer.Stop();

			Process[] hProcesses = Process.GetProcessesByName(SAI);

			while (hProcesses.Length != 0)
			{
				_textureManager = null;

				DialogResult res = CenteredMessageBox.Show(this,
					"SAIの起動が検出されました。\n" +
					"このプログラムはSAIの起動中は処理を行えません。\n" +
					"\n" +
					"SAIを通常終了し、３つほど数えてから「OK」してみて下さい。　　　　\n" +
					"最新のテクスチャ情報を再読み込みします。\n" +
					"\n" +
					"確認しますか？　（「キャンセル」でこちらが強制終了です）"
					, "--- SAI起動中 ---"
					, MessageBoxButtons.OKCancel, MessageBoxIcon.Information );

				if (res == DialogResult.Cancel)
				{
					ForceExitProgram = true;
					Application.Exit();
					return;
				}
				else
				{
					hProcesses = Process.GetProcessesByName(SAI);
				}
			}
			
			// この先は、まったく引っかからなかったか、OKして抜けてきている

			if (_textureManager == null)
			{
				_textureManager = new TextureManager(_textureChangerOptions.PathToSaiFolder,this);

				lsvTextureImage_UpdateImages(sender, e);
			}

			_saiProcessCheckTimer.Start();
		}
		#endregion

		#region SAIフォルダの問い合わせ

		private string RequestPathToSaiFolder()
		{
			#region フォルダ指定ダイアログの初期表示先取得
			SH.IShellFolder desktopShellFolder = SH.GetDesktopFolder();

			string defaultSaiExeFolder = "C:\\Program Files\\PaintToolSAI";
		    if (_textureChangerOptions.PathToSaiFolder != "")
		        defaultSaiExeFolder = _textureChangerOptions.PathToSaiFolder;

			IntPtr pItemIdl = IntPtr.Zero;
			try
			{
				UInt32 chEaten = 0;
				UInt32 dwAttributes = 0;
				desktopShellFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero,
					defaultSaiExeFolder, ref chEaten, out pItemIdl, ref dwAttributes);
			}
			catch
			{
				SH.SHGetSpecialFolderLocation(IntPtr.Zero, SH.CSIDL.DESKTOP, ref pItemIdl);
				if (pItemIdl == IntPtr.Zero)
				{
					throw;
				}
			}
			#endregion

			BrowseFolderDialog bfdlg = new BrowseFolderDialog();

			bfdlg.DialogMessage =
				"操作するSAIのインストール先(sai.exeの在処)を指定下さい。\n" +
				//"大抵は C:\\Program Files\\PaintToolSAI 辺りです。\n" +
				"後でから指定を変更できます。オプションメニューから変更してください。";
			bfdlg.fStatusText = false;
			bfdlg.fReturnOnlyFsDirs = true;
			bfdlg.fNoNewFolderButton = true;
			bfdlg.Procedure = SpecifySaiExeBffCallback;
			bfdlg.lParam = (uint)pItemIdl;

			if (bfdlg.ShowDialog(this) == DialogResult.Cancel)
			{
				return "";
			}

			return bfdlg.DirectoryPath;
		}

		#region 初回起動時のsai.exeのフォルダ指定ダイアログの入力チェック用コールバックデリゲート
		private int SpecifySaiExeBffCallback(IntPtr hwnd, UInt32 uMsg, IntPtr lParam, IntPtr lpData)
		{
			StringBuilder sb;
			switch (uMsg)
			{
				case (uint)SH.BFFM.INITIALIZED:
					//はじめに選択されるフォルダをitemIDLでメッセージ
					Api.SendMessage(hwnd, (uint)SH.BFFM.SETSELECTION, IntPtr.Zero, lpData);
					sb = new StringBuilder((int)MAX.PATH);
					SH.SHGetPathFromIDListW(lpData, sb);
					Api.SendMessage(hwnd, (uint)SH.BFFM.SETSTATUSTEXTW, IntPtr.Zero, sb);
					Api.SendMessage(hwnd, (uint)SH.BFFM.ENABLEOK, 0,
						File.Exists(sb + "\\sai.exe") ? 1 : 0);
					break;

				case (uint)SH.BFFM.SELCHANGED:
					// ユーザーがフォルダを変更した時
					// SAI_EXEが含まれるかによってOKボタンの有効化を制御
					sb = new StringBuilder((int)MAX.PATH);
					SH.SHGetPathFromIDListW(lParam, sb);
					Api.SendMessage(hwnd, (uint)SH.BFFM.SETSTATUSTEXTW, IntPtr.Zero, sb);
					Api.SendMessage(hwnd, (uint)SH.BFFM.ENABLEOK, 0,
						File.Exists(sb + "\\sai.exe") ? 1 : 0);
					break;
			}
			return 0;
		}
		#endregion

		#endregion

        #region フォームのクローズ中：終了問い合わせ、ウィンドウ状況保存
        private void TextureChangerForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            #region 終了問い合わせ
			if (ForceExitProgram == false 
				&& _textureChangerOptions.PromptToExitProgram)
			{
				DialogResult result = CenteredMessageBox.Show(this
					, "終了しますか？", "確認"
					, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (result == DialogResult.No)  // [いいえ] の場合
				{
					e.Cancel = true;  // 終了処理を中止
				}
            }
            #endregion

            #region ウィンドウの状況を保存
            _textureChangerOptions.SaveWindowConditions(
                WindowState == FormWindowState.Normal ? Bounds : RestoreBounds,
                WindowState);

			_textureChangerOptions.SaveSplitterDistances(
				splNorthSouth.SplitterDistance <= splNorthSouth.Panel1MinSize
					? splNorthSouth.Panel1MinSize
					: splNorthSouth.SplitterDistance,
				splTreeList.SplitterDistance <= splTreeList.Panel1MinSize
					? splTreeList.Panel1MinSize
					: splTreeList.SplitterDistance
				);
            #endregion
        }
        #endregion

        #region ファイルメニュー：終了：クローズを試みるだけ
        private void mniExit_Click( object sender, EventArgs e )
		{
			Close( );
		}
        #endregion

        #region ヘルプメニュー：アバウト：ダイアログを表示するだけ
        private void mniAbout_Click( object sender, EventArgs e )
		{
			new AboutForm().ShowDialog();
		}
        #endregion

        #region オプション値保持に関して、SAIフォルダの再指定
        private void mniRequestSaiFolder_Click(object sender, EventArgs e)
		{
			string temp = RequestPathToSaiFolder();
			if (temp != "")
			{
				_saiProcessCheckTimer.Stop();
				_textureChangerOptions.PathToSaiFolder = temp;
				_textureManager = null;
				SaiProcessCheckTimerHandler(sender, e);
			}
		}
		#endregion

        #region オプションメニュー：起動時に表示するフォルダ：最近のフォルダ
        private void mniFirstExpandingRecentFolder_Click(object sender, EventArgs e)
		{
            _textureChangerOptions.SetToUseFirstExpandingRecentFolder();

			SetCheckingForMenuFirstExpandingFolder();
		}
        #endregion

        #region オプションメニュー：起動時に表示するフォルダ：固定フォルダ
        private void mniFirstExpandingFixedFolder_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbdlg = new FolderBrowserDialog();
			fbdlg.Description = "このプログラムの起動時に、ツリーに表示するフォルダを指定してください。";
			fbdlg.RootFolder  = Environment.SpecialFolder.Desktop;
			fbdlg.SelectedPath = _textureChangerOptions.FirstExpandingFixedFolder;
			fbdlg.ShowNewFolderButton = true;
			if (fbdlg.ShowDialog(this) == DialogResult.Cancel)
			{
				return;
			}

            _textureChangerOptions.SetToUseFirstExpandingFixedFolder(fbdlg.SelectedPath);

			SetCheckingForMenuFirstExpandingFolder();
		}
		#endregion

        #region オプションメニュー：起動時に表示するフォルダ：チェック状態の追随
        private void SetCheckingForMenuFirstExpandingFolder()
        {
            mniFirstExpandingRecentFolder.Checked = !_textureChangerOptions.FirstExpandingUseFixed;
            mniFirstExpandingFixedFolder.Checked = _textureChangerOptions.FirstExpandingUseFixed;
        }
        #endregion

		#region オプションメニュー：終了前に問い合わせる
		private void mniPromptToExitProgram_Click(object sender, EventArgs e)
		{
            mniPromptToExitProgram.Checked
			    = _textureChangerOptions.PromptToExitProgram
				= !_textureChangerOptions.PromptToExitProgram;
		}
		#endregion

		#region オプションメニュー：起動時に更新を確認する
		private void mniCheckUpdateAtStartUp_Click( object sender, EventArgs e )
		{
			mniCheckUpdateAtStartUp.Checked
				= _textureChangerOptions.CheckUpdateAtStartUp
				= !_textureChangerOptions.CheckUpdateAtStartUp;
		}
		#endregion

		#region 編集メニューとラジオグループ：編集対象のテクスチャ種を変更する
		private void mniEditTexture_Click(object sender, EventArgs e)
		{
			_textureChangerOptions.SaveLastEditings(((ToolStripMenuItem) sender).Text, "");

			lsvTextureImage_UpdateImages(sender, e);

            lblStatus.Text = ((ToolStripMenuItem) sender).Text + " - "
                + _textureChangerOptions.PathToSaiFolder
                + "\\" + _textureManager.GetDirectoryFromConfname(_textureChangerOptions.LastEditingTextureName);
		}
		private void rdoEditTexture_Click( object sender, EventArgs e )
		{
			_textureChangerOptions.SaveLastEditings((string)((RadioButton) sender).Tag, "");

			lsvTextureImage_UpdateImages( sender, e );

            lblStatus.Text = (string)((RadioButton)sender).Tag + " - "
                + _textureChangerOptions.PathToSaiFolder
                + "\\" + _textureManager.GetDirectoryFromConfname(_textureChangerOptions.LastEditingTextureName);
        }
		#endregion

		#region テクスチャ画像のリストビューを内容更新する
		private void lsvTextureImage_UpdateImages( object sender, EventArgs e )
		{
			foreach (var mniEditTexture in new[] { mniEditBlotmap, mniEditElemap, mniEditBrushtex, mniEditPapertex })
				mniEditTexture.Checked = (mniEditTexture.Text == _textureChangerOptions.LastEditingTextureName);
			foreach (var rdoEditTexture in new[] { rdoEditBlotmap, rdoEditElemap, rdoEditBrushtex, rdoEditPapertex })
				rdoEditTexture.Checked = ((string)rdoEditTexture.Tag == _textureChangerOptions.LastEditingTextureName);

            lsvTextureImages.Items.Clear();
            ilsTextureImage.Images.Clear();

			_textureManager.GetImageList(_textureChangerOptions.LastEditingTextureName
				, lsvTextureImages.LargeImageList);

			// TODO: 再表示後のリストビューのスクロール位置

			var imagePathList = _textureManager.GetImagePathList(_textureChangerOptions.LastEditingTextureName);
			for (int index = 0; index < imagePathList.Length; index++)
			{
				lsvTextureImages.Items.Add( imagePathList[ index ], imagePathList[ index ], index );
			}
		}
		#endregion

		#region スプリットコンテナの、バーの真ん中に点を書く
		private void splSplitContainer_Paint(object sender, PaintEventArgs e)
		{
			var control = sender as SplitContainer;
			Debug.Assert(control != null, "control != null");

			//paint the three dots'
			var points = new Point[3];
			var w = control.Width;
			var h = control.Height;
			var d = control.SplitterDistance;
			var sW = control.SplitterWidth;

			//calculate the position of the points'
			if (control.Orientation == Orientation.Horizontal)
			{
				points[0] = new Point((w / 2), d + (sW / 2));
				points[1] = new Point(points[0].X - 10, points[0].Y);
				points[2] = new Point(points[0].X + 10, points[0].Y);
			}
			else
			{
				points[0] = new Point(d + (sW / 2), (h / 2));
				points[1] = new Point(points[0].X, points[0].Y - 10);
				points[2] = new Point(points[0].X, points[0].Y + 10);
			}

			foreach (Point p in points)
			{
				p.Offset(-2, -2);
				e.Graphics.FillEllipse(SystemBrushes.ControlDark,
					new Rectangle(p, new Size(3, 3)));

				p.Offset(1, 1);
				e.Graphics.FillEllipse(SystemBrushes.ControlLight,
					new Rectangle(p, new Size(3, 3)));
			}
		}
		#endregion

		#region 編集メニュー、テクスチャの削除ボタンとポップアップメニュー：選択されたテクスチャを削除
		private void mniTextureRemove_Click(object sender, EventArgs e)
		{
			//項目が１つも選択されていない場合処理を抜ける
			if (lsvTextureImages.SelectedItems.Count == 0)
				return;

			foreach (ListViewItem item in lsvTextureImages.SelectedItems)
			{
				DialogResult res = CenteredMessageBox.Show(this
					, item.Name + "をSAIから登録解除しますか？\n"
						+ "ファイルはゴミ箱へ移動されます。"
					, "削除確認"
					, MessageBoxButtons.YesNoCancel
					, MessageBoxIcon.Question);
				
				if (res == DialogResult.Cancel)
					break;
				if (res == DialogResult.Yes)
				{
					_textureManager.RemoveImage(
						_textureChangerOptions.LastEditingTextureName
						, item.Name
						, this);
					_textureManager.SaveFormats( );

					lsvTextureImage_UpdateImages(sender, e);
				}
			}
		}
		private void btnTextureRemove_Click( object sender, EventArgs e )
		{
			mniTextureRemove_Click( sender, e );
		}
		private void mniTextureRemovePopup_Click( object sender, EventArgs e )
		{
			mniTextureRemove_Click( sender, e );
		}
		#endregion

		#region 編集メニュー：すべてのテクスチャを選択
		private void mniSelectAll_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in lsvTextureImages.Items)
			{
				item.Selected = true;
			}
			lsvTextureImages.Focus();
		}
		#endregion

		#region ツリービューのクリックとファイルリストの一覧(再)作成
		private void trvFolder_AfterSelect( object sender, TreeViewEventArgs e )
		{
			_textureChangerOptions.SetToUseFirstExpandingRecentFolder(
				trvFolder.GetSelectedNodePath() );

			lsvFileList.Items.Clear();
            ilsFileList.Images.Clear();

			string imageDir = trvFolder.GetSelectedNodePath();

			if (imageDir == "")
			{
				return;
			}

			string[] bmpFiles = Directory.GetFiles( imageDir, "*.bmp" );

            int width = ilsFileList.ImageSize.Width;
            int height = ilsFileList.ImageSize.Height;

			for( int i = 0; i < bmpFiles.Length; i++ )
			{
				Image original = Bitmap.FromFile( bmpFiles[ i ] );
				string sizeString = "   （ 幅 " + original.Size.Width + " x 高さ " + original.Height + " ）";

				Image thumbnail = TextureManager.createThumbnail( original, width, height );
				
				FileInfo fi = new FileInfo( bmpFiles[ i ] );

				ilsFileList.Images.Add( thumbnail );
				lsvFileList.Items.Add( fi.Name, fi.Name + sizeString, i );

				original.Dispose( );
				thumbnail.Dispose( );
			}
		}
		#endregion

		#region ファイルリストのすべて選択
		private void mniFileListSelectAll_Click( object sender, EventArgs e )
		{
			foreach( ListViewItem item in lsvFileList.Items )
			{
				item.Selected = true;
			}
			lsvFileList.Focus( );
		}
		private void mniFileListPopupSelectAll_Click( object sender, EventArgs e )
		{
			mniFileListSelectAll_Click( sender, e );
		}
		#endregion

		#region ファイルリストからテクスチャ登録

		private void resigtToTexture( object sender, EventArgs e, string targetConfName, ListViewItem item )
	    {
			DialogResult res = CenteredMessageBox.Show( this
				, item.Name + "を" + targetConfName + "へ登録しますか？", "登録確認"
				, MessageBoxButtons.YesNoCancel
				, MessageBoxIcon.Question );

			string targetPath = trvFolder.GetSelectedNodePath( ) + "\\" + item.Name;

			if( res == DialogResult.Cancel )
				return;
			if( res == DialogResult.Yes )
			{
				if( _textureManager.AddImage(
					targetConfName
					, targetPath
					, this ) )
				{
					_textureManager.SaveFormats( );
				}
				else
				{
					CenteredMessageBox.Show( this
						, "登録に失敗しました：\n"
							+ targetPath + "を\n"
							+ targetConfName + "へ"
						, "登録失敗"
						, MessageBoxButtons.OK
						, MessageBoxIcon.Warning );
				}
			}
		}

		private void registTextureTo( object sender, EventArgs e, string targetConfName )
		{
			//選択項目を登録
			foreach( ListViewItem item in lsvFileList.SelectedItems )
				resigtToTexture(sender, e, targetConfName, item);

			//テクスチャビューを更新
			lsvTextureImage_UpdateImages( sender, e );
		}

		private void mniFileListRegistToCurrent_Click( object sender, EventArgs e )
		{
			registTextureTo( sender, e, _textureChangerOptions.LastEditingTextureName );
		}
		private void mniFileListRegistToBlotmap_Click( object sender, EventArgs e )
		{
			registTextureTo( sender, e, TextureManager.BLOTMAP_NAME );
		}
		private void mniFileListRegistToElemap_Click( object sender, EventArgs e )
		{
			registTextureTo( sender, e, TextureManager.ELEMAP_NAME );
		}
		private void mniFileListRegistToBrushtex_Click( object sender, EventArgs e )
		{
			registTextureTo( sender, e, TextureManager.BRUSHTEX_NAME );
		}
		private void mniFileListRegistToPapertex_Click( object sender, EventArgs e )
		{
			registTextureTo( sender, e, TextureManager.PAPERTEX_NAME );
		}

		private void mniFileListPopupRegistToCurrent_Click( object sender, EventArgs e )
		{
			mniFileListRegistToCurrent_Click( sender, e );
		}
		private void mniFileListPopupRegistToBlotmap_Click( object sender, EventArgs e )
		{
			mniFileListRegistToBlotmap_Click( sender, e );
		}
		private void mniFileListPopupRegistToElemap_Click( object sender, EventArgs e )
		{
			mniFileListRegistToElemap_Click( sender, e );
		}
		private void mniFileListPopupRegistToBrushtex_Click( object sender, EventArgs e )
		{
			mniFileListRegistToBrushtex_Click( sender, e );
		}
		private void mniFileListPopupRegistToPapertex_Click( object sender, EventArgs e )
		{
			mniFileListRegistToPapertex_Click( sender, e );
		}
		#endregion

        #region 最新の情報に更新
        private void mniFileListPopupUpdateList_Click(object sender, EventArgs e)
        {
             mniFileListUpdateList_Click(sender,e);
        }
        private void mniFileListUpdateList_Click(object sender, EventArgs e)
        {
            trvFolder_AfterSelect(sender, new TreeViewEventArgs(null));
        }
        #endregion

        #region バックアップ
        private void mniBackup_Click(object sender, EventArgs e)
        {
            _textureManager.Backup(this);
        }
        #endregion

		#region リストア
		private void mniRestore_Click( object sender, EventArgs e )
		{
			_saiProcessCheckTimer.Stop();
			_textureManager.Restore(this);
			_textureManager = null;
			SaiProcessCheckTimerHandler( sender, e );
		}
		#endregion

		#region ヘルプメニュー：更新を確認
		private void mniCheckUpdate_Click( object sender, EventArgs e )
		{
			DialogResult res = CenteredMessageBox.Show( this,
				"オンラインでTextureChangerの更新あるかを確認します。\n" +
				"更新がある場合、プログラムが終了してインストーラが起動します。\n" +
				"更新がない場合は特になにもしません。\n" +
				"よろしいですか？"
				, "TexureChanger更新を確認"
				, MessageBoxButtons.OKCancel, MessageBoxIcon.Information );
			if (res == DialogResult.Cancel)
				return;

			HttpUpdater httpUpdater = new HttpUpdater( this );
			httpUpdater.BeginAsyncCheckAppConfigUpdated( );
		}
		#endregion

		#region ファイルビューからドラッグ

		//ドラッグ側のリストビューでアイテムをドラッグ開始
		private void lsvFileList_ItemDrag( object sender, ItemDragEventArgs e )
		{
			if( lsvFileList.SelectedIndices.Count <= 0 )
				return;

			// Dragアイテムを記録
			List<ListViewItem> dragItems = new List<ListViewItem>( );
			foreach( ListViewItem item in lsvFileList.SelectedItems )
				dragItems.Add( item );
			
            // ドラッグ開始
			dragImagesForm.BeginDrag( lsvFileList, Cursor.Position );
			lsvFileList.DoDragDrop( dragItems, DragDropEffects.Copy | DragDropEffects.Move );
			dragImagesForm.EndDrag( );
		}

		//ファイルリストビューからのドラッグの間中ずっと判定しつづけることとしては
		private void lsvFileList_QueryContinueDrag(object sender, QueryContinueDragEventArgs e )
		{
            //マウスの右ボタンが押されていればドラッグをキャンセルする
            if ((e.KeyState & 2) == 2) //"2"はマウスの右ボタンを表す
				e.Action = DragAction.Cancel;
            
            //ドラッグイメージを描画
			dragImagesForm.MoveDrag( Cursor.Position );
		}

        //テクスチャ画像ビュー上でアイテムを移動させている間は
        private void lsvTextureImages_DragOver(object sender, DragEventArgs e)
		{
			//ListViewItem型でなければ受け入れない
			if( !e.Data.GetDataPresent( typeof( List<ListViewItem> ) ) )
			{
				e.Effect = DragDropEffects.None;
				return;
			}

			// キー状態によって効果を変化させる
			if( ( e.KeyState & 0x8 ) > 0 &&
				( e.AllowedEffect & DragDropEffects.Copy ) == DragDropEffects.Copy )
			{
				//Ctrlキーが押されていればCopy ("8"はCtrlキーを表す)
				e.Effect = DragDropEffects.Copy;
			}
			else if( ( e.KeyState & 0x4 ) > 0 &&
					 ( e.AllowedEffect & DragDropEffects.Move ) == DragDropEffects.Move )
			{
				// Shiftキーが押されていたら MOVE
				e.Effect = DragDropEffects.Move;
			}
			else if( ( e.AllowedEffect & DragDropEffects.Move ) == DragDropEffects.Move )
			{
				// 何も押されていなければMove
				e.Effect = DragDropEffects.Move;
			}
			else
			{
				// 想定外は受け入れない
				e.Effect = DragDropEffects.None;
				return;
			}

            //挿入マークを表示する
            Point targetPoint = lsvTextureImages.PointToClient(new Point(e.X, e.Y));
            int targetIndex = lsvTextureImages.InsertionMark.NearestIndex(targetPoint);
            if (targetIndex > -1)
            {
                Rectangle itemBounds = lsvTextureImages.GetItemRect(targetIndex);
                if (targetPoint.X > itemBounds.Left + (itemBounds.Width / 2))
                {
                    lsvTextureImages.InsertionMark.AppearsAfterItem = true;
                }
                else
                {
                    lsvTextureImages.InsertionMark.AppearsAfterItem = false;
                }
            }
            lsvTextureImages.InsertionMark.Index = targetIndex;



			// カーソル下のアイテムをハイライト TODO
			//ListViewItem srcItem = (ListViewItem)e.Data.GetData( typeof( ListViewItem ) );
			//Point p = this.lsvTextureImages.PointToClient( new Point( e.X, e.Y ) );
			//ListViewItem item = this.lsvTextureImages.GetItemAt( p.X, p.Y );
			//if( item != null )
			//	item.Selected = true;
		}

        //テクスチャ画像ビューからカーソルが外れたら
        private void lsvTextureImages_DragLeave(object sender, EventArgs e)
        {
            //挿入マークを非表示する
            lsvTextureImages.InsertionMark.Index = -1;
        }

        //テクスチャ画像ビュー上でアイテムドロップされたら
		private void lsvTextureImages_DragDrop( object sender, DragEventArgs e )
		{
			// Unlock updates
			//DragHelper.ImageList_DragLeave( ilsDrag.Handle );

			// ListViewItem型でなければ受け入れない
			if( !e.Data.GetDataPresent( typeof( List<ListViewItem> ) ) )
				return;

			// ドロップしたデータを取得
			List<ListViewItem> dropedItems = (List<ListViewItem>)e.Data.GetData( typeof( List<ListViewItem> ) );

			//選択項目を登録
			foreach (ListViewItem item in dropedItems)
			{
				//resigtToTexture( sender, (EventArgs)e, _textureChangerOptions.LastEditingTextureName, item );
			}

			//テクスチャビューを更新
			lsvTextureImage_UpdateImages( sender, e );
			
			// Move の場合には、ソースのアイテムを削除してあげる必要があります。
			if( ( e.KeyState & 0x4 ) > 0 )
			{
				//this.lsvTextureImages.Items.Remove( srcItem );
			}

			lsvTextureImages.Focus();
		}


		#endregion

	}
}
