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

			try
			{
				_textureChangerOptions = new TextureChangerOptions();
			}
			catch (ArgumentOutOfRangeException e)
			{
				return;
			}


			Application.Run( new TextureChangerForm( ) );
		}
	}
}
