using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TextureChanger.util;

namespace TextureChanger
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main( )
		{
			Application.EnableVisualStyles( );
			Application.SetCompatibleTextRenderingDefault( false );

            // TODO : SAIのフォルダーを指定させる
            // TODO : 設定オブジェクトを定義する
            // TODO : INIファイルの取り扱いを調べる
            BrowseFolderDialog folderBrowser1 = new BrowseFolderDialog();
            if (DialogResult.OK == folderBrowser1.ShowDialog())
                MessageBox.Show(folderBrowser1.DirectoryPath);



			Application.Run( new TextureChangerForm( ) );
		}
	}
}
