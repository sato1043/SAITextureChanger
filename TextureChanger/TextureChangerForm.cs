using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using TextureChanger.util;
using Win32;

namespace TextureChanger
{
    // TODO : 現在のフォルダを逐次保存する。特にプログラム終了時

    public partial class TextureChangerForm : Form
	{
		private TextureChangerOptions _textureChangerOptions;

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
						"インストール先はオプションメニューから設定できます。"
						);
				}
			}
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
            #endregion
		}


		#region SAIフォルダの問い合わせ

		private string RequestPathToSaiFolder()
		{
			#region フォルダ指定ダイアログの初期表示先取得
			SH.IShellFolder desktopShellFolder = null;
			SH.SHGetDesktopFolder(ref desktopShellFolder);

			const string defaultSaiExeFolder = "C:\\Program Files\\PaintToolSAI";
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
				"デフォルトでは C:\\Program Files\\PaintToolSAI です。\n" +
				"再度別の場所を指定したいときはオプションメニューから変更してください。";
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
            if (_textureChangerOptions.PromptToExitProgram)
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
				_textureChangerOptions.PathToSaiFolder = temp;
			}

			// TODO: UI上、処理上、SAIフォルダ再指定に対応する
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

	}
}
