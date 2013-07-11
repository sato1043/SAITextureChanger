using System.Windows.Forms;
using TextureChanger.util;

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


		public TextureChangerOptions( )
		{
			_iniFile = new IniFile( );

			PathToSaiFolder = _iniFile[ "SAI", "folder" ];

			if( PathToSaiFolder == "" )
			{
				// TODO : SAIのフォルダーを指定させる
				BrowseFolderDialog folderBrowser1 = new BrowseFolderDialog( );
				if( DialogResult.OK == folderBrowser1.ShowDialog( ) )
					MessageBox.Show( folderBrowser1.DirectoryPath );

			
			}


		}
	}
}