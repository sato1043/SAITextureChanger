using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TextureChanger
{
	static class Program
	{
		public static TextureChangerOptions TextureChangerOptions;

		[STAThread]
		static void Main( )
		{
			Application.EnableVisualStyles( );
			Application.SetCompatibleTextRenderingDefault( false );

			try
			{
				TextureChangerOptions = new TextureChangerOptions();
			}
			catch (ArgumentOutOfRangeException e)
			{
				return;
			}


			Application.Run( new TextureChangerForm( ) );
		}
	}
}
