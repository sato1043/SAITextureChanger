using System;
using System.Windows.Forms;
using TextureChanger.util;
using System.Text;
using System.IO;
using Win32;

namespace TextureChanger
{
	// TODO : 設定オブジェクトを定義する

	/*
		[WindowPosition]
		nCmdShow = 3
		top = 256
		left = 150
		wide = 512
		high = 300
		upperHigh = 284
		upperLeftWide = 311
		lowerLeftWide = 256

		[SAI]
		folder = C:\Documents and Settings\na\デスクトップ\PaintToolSAI

		[Browse]
		use_fixed = 1
		folder = C:\Documents and Settings\na\デスクトップ
		recent = C:\Documents and Settings\na\My Documents
	 */


	public class TextureChangerOptions
	{
		readonly IniFile _iniFile;

		public string PathToSaiFolder { get; private set; }

		public bool   UseFixed { get; private set; }
		public string PathToBrowseFolder { get; private set; }
		public string PathToRecentFolder { get; private set; }



	    #region 初回起動時のsai.exeのフォルダ指定ダイアログの入力チェック用コールバックデリゲート
	    private int SpecifySaiExeBffCallback(IntPtr hwnd, UInt32 uMsg, IntPtr lParam, IntPtr lpData)
	    {
	        StringBuilder sb;
	        switch (uMsg)
	        {
	            case (uint) SH.BFFM.INITIALIZED:
	                //はじめに選択されるフォルダをitemIDLでメッセージ
	                Api.SendMessage(hwnd, (uint) SH.BFFM.SETSELECTION, IntPtr.Zero, lpData);
	                sb = new StringBuilder((int) MAX.PATH);
	                SH.SHGetPathFromIDListW(lpData, sb);
	                Api.SendMessage(hwnd, (uint) SH.BFFM.SETSTATUSTEXTW, IntPtr.Zero, sb);
	                Api.SendMessage(hwnd, (uint) SH.BFFM.ENABLEOK, 0,
	                    File.Exists(sb + "\\sai.exe") ? 1 : 0);
	                break;

	            case (uint) SH.BFFM.SELCHANGED:
	                // ユーザーがフォルダを変更した時
	                // SAI_EXEが含まれるかによってOKボタンの有効化を制御
	                sb = new StringBuilder((int) MAX.PATH);
	                SH.SHGetPathFromIDListW(lParam, sb);
	                Api.SendMessage(hwnd, (uint) SH.BFFM.SETSTATUSTEXTW, IntPtr.Zero, sb);
	                Api.SendMessage(hwnd, (uint) SH.BFFM.ENABLEOK, 0,
	                    File.Exists(sb + "\\sai.exe") ? 1 : 0);
	                break;
	        }
	        return 0;
	    }
	    #endregion


		public TextureChangerOptions( )
		{
			_iniFile = new IniFile( );

			UseFixed = (_iniFile["Browse", "use_fixed"] == "1");
			PathToBrowseFolder = _iniFile["Browse", "folder"];
			PathToRecentFolder = _iniFile["Browse", "recent"];




			PathToSaiFolder = _iniFile[ "SAI", "folder" ];

			#region SAIのフォルダが未指定のときはユーザに指定させる
			if (PathToSaiFolder == "")
			{
				#region フォルダ指定ダイアログの初期表示先取得

				SH.IShellFolder desktopShellFolder = null;
				SH.SHGetDesktopFolder(ref desktopShellFolder);

				string defaultSaiExeFolder = "C:\\Program Files\\PaintToolSAI";
				UInt32 chEaten = 0;
				UInt32 dwAttributes = 0;
				IntPtr pItemIdl = IntPtr.Zero;
				try
				{
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
					"はじめに、操作するSAIのインストール先(sai.exeの在処)を指定下さい。\n" +
					"デフォルトでは C:\\Program Files\\PaintToolSAI です。\n" +
					"再度別の場所を指定したいときはオプションメニューから変更してください。";
				bfdlg.fStatusText = false;
				bfdlg.fReturnOnlyFsDirs = true;
				bfdlg.fNoNewFolderButton = true;
				bfdlg.Procedure = SpecifySaiExeBffCallback;
				bfdlg.lParam = (uint)pItemIdl;

				if (bfdlg.ShowDialog() == DialogResult.Cancel)
				{
					MessageBox.Show(
						"SAIのインストール先が特定できませんでした。\n" +
						"操作先のSAIがわからず処理を継続できないため、起動を中断します。"
						);
				    throw new ArgumentOutOfRangeException();
				}

				PathToSaiFolder = bfdlg.DirectoryPath;
				_iniFile[ "SAI", "folder" ] = PathToSaiFolder;
			}
			#endregion



		}
	}
}