using System;
using System.Windows.Forms;
using TextureChanger.util;
using System.Text;
using System.IO;

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

        int BffCallback(IntPtr hwnd, UInt32 uMsg, IntPtr lParam, IntPtr lpData)
        {
            switch (uMsg)
            {
                case (uint)Win32.SH.BFFM.INITIALIZED:
                    break;

                case (uint)Win32.SH.BFFM.SELCHANGED:
                    // ユーザーがフォルダを変更した時
                    // SAI_EXEが含まれるかによってOKボタンの有効化を制御
                    StringBuilder sb = new StringBuilder((int)Win32.MAX.PATH);
                    Win32.SH.SHGetPathFromIDListW(lParam, sb);
                    Win32.Api.SendMessage(hwnd, (uint)Win32.SH.BFFM.SETSTATUSTEXTW, IntPtr.Zero, sb);
                    Win32.Api.SendMessage(hwnd, (uint)Win32.SH.BFFM.ENABLEOK, 0,
                            File.Exists(sb.ToString() + "\\sai.exe") ? 1 : 0 );
                    break;
            }
            return 0;
        }

		public TextureChangerOptions( )
		{
			_iniFile = new IniFile( );

            UseFixed = (_iniFile["Browse", "use_fixed"] == "1");
            PathToBrowseFolder = _iniFile["Browse", "folder"];
            PathToRecentFolder = _iniFile["Browse", "recent"];

            Win32.SH.ITEMIDLIST itemIdl;
            
            
            
            
            PathToSaiFolder = _iniFile[ "SAI", "folder" ];

			if( PathToSaiFolder == "" )
			{
				// TODO : SAIのフォルダーを指定させる
				BrowseFolderDialog bfdlg = new BrowseFolderDialog( );

                bfdlg.DialogMessage = "SAIのインストール先フォルダ(sai.exeがあるフォルダ)を選択してください";
                bfdlg.fStatusText = true;
                bfdlg.fReturnOnlyFsDirs = true;
                bfdlg.fNoNewFolderButton = true;
                bfdlg.Procedure = BffCallback;

                if (DialogResult.OK == bfdlg.ShowDialog())
                    MessageBox.Show(bfdlg.DirectoryPath);

			
			}


		}
	}
}