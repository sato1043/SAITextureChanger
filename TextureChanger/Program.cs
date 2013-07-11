using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TextureChanger
{
	static class Program
	{
		static TextureChangerOptions _textureChangerOptions;

		[STAThread]
		static void Main( )
		{
			Application.EnableVisualStyles( );
			Application.SetCompatibleTextRenderingDefault( false );

			_textureChangerOptions = new TextureChangerOptions( );


			Application.Run( new TextureChangerForm( ) );
		}
	}
}
