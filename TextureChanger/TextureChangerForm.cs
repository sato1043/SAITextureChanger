using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using TextureChanger.util;
using Win32;

namespace TextureChanger
{
    // TODO : 現在のフォルダを逐次保存する。特にプログラム終了時

	//TODO ファイル-バックアップ機能の実装

    public partial class TextureChangerForm : Form
	{
		private TextureChangerOptions _textureChangerOptions;

	    private bool _forceExitProgram = false;

	    private const string SAI = "sai";
		
		private Timer _saiProcessCheckTimer;

	    private TextureManager _textureManager;

	    public TextureChangerForm( )
		{
			InitializeComponent( );
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
					MessageBox.Show(this,
						"SAIのインストール先を設定できませんでした。\n" +
						"操作先のSAIがわからないため、処理を継続できません。\n"+
						"申し訳ありませんが、プログラムの起動を中断します。"
						);
					_forceExitProgram = true;
					Application.Exit();
					return;
				}
			}
			//このプログラムの特性上、PathToSaiFolderが空ということは無しの前提で動かす
			#endregion

            #region メニューのチェック状態の再構築
		    SetCheckingForMenuFirstExpandingFolder();
		    mniPromptToExitProgram.Checked = _textureChangerOptions.PromptToExitProgram;
			foreach (var mniEditTexture in new[]{ mniEditBlotmap, mniEditElemap, mniEditBrushtex, mniEditPapertex })
				mniEditTexture.Checked = (mniEditTexture.Text == _textureChangerOptions.LastEditingTextureName);
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
			splTextureImage.SplitterDistance =
				_textureChangerOptions.SplitterDistanceTextureImage <= splTextureImage.Panel1MinSize
					? splTextureImage.Panel1MinSize
					: _textureChangerOptions.SplitterDistanceTextureImage;
            #endregion

			#region 起動確認
			CenteredMessageBox.Show(this,
				"SAIに対する編集は、直接即座に上書きされます。\n" +
				"\n" +
				"また、TextureChangerとSAIはお互いを知らないので、\n" +
				"SAI起動中にTextureChangerを実行することができません。\n" +
				"\n" +
				"TextureChanger起動中にSAIを起動した場合、SAIでの変更が再読み込みされます。　　　\n"
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
					_forceExitProgram = true;
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
			}

			// TODO : テクスチャ情報再読み込みに伴うUIの全更新

			_saiProcessCheckTimer.Start();
		}
		#endregion

		#region SAIフォルダの問い合わせ

		private string RequestPathToSaiFolder()
		{
			#region フォルダ指定ダイアログの初期表示先取得
			SH.IShellFolder desktopShellFolder = null;
			SH.SHGetDesktopFolder(ref desktopShellFolder);

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
			if (_forceExitProgram == false 
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
					: splTreeList.SplitterDistance,
				splTextureImage.SplitterDistance <= splTextureImage.Panel1MinSize
					? splTextureImage.Panel1MinSize
					: splTextureImage.SplitterDistance	
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

		#region 編集メニュー：編集対象のテクスチャ種を変更する
		private void mniEditTexture_Click(object sender, EventArgs e)
		{
			foreach (var mniEditTexture in new[] { mniEditBlotmap, mniEditElemap, mniEditBrushtex, mniEditPapertex })
				mniEditTexture.Checked = (mniEditTexture == (ToolStripMenuItem)sender);

			_textureChangerOptions.SaveLastEditings(((ToolStripMenuItem) sender).Text, "");

			//TODO テクスチャを表示しているビューの変更
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


	}
}
