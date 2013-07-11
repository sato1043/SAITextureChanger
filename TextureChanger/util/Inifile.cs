using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TextureChanger.util
{
	/**
		//ファイルを指定して初期化
		IniFile ini = new IniFile("./test.ini");

		ini["section", "key"] = "value"; //書き込み

		string value = ini["section", "key"]; //読み込み

		//この様に書き込まれる。
		//[section]
		//key=value
	 */
	class IniFile
	{
		[DllImport( "kernel32.dll" )]
		private static extern int GetPrivateProfileString(
			string lpApplicationName,
			string lpKeyName,
			string lpDefault,
			StringBuilder lpReturnedstring,
			int nSize,
			string lpFileName );

		[DllImport( "kernel32.dll" )]
		private static extern int WritePrivateProfileString(
			string lpApplicationName,
			string lpKeyName,
			string lpstring,
			string lpFileName );



		readonly string _filePath;
		readonly int    _buffersize;


		public IniFile( string filePath = "", int buffersize = 1024 )
		{
			this._filePath = ( filePath == "" )
				? Path.ChangeExtension( Application.ExecutablePath, ".ini" )
				: filePath ;

			_buffersize = buffersize;

		}


		public string this[ string section, string key ]
		{
			set
			{
				WritePrivateProfileString( section, key, value, _filePath );
			}
			get
			{
				StringBuilder sb = new StringBuilder( _buffersize );
				GetPrivateProfileString( section, key, string.Empty, sb, sb.Capacity, _filePath );
				return sb.ToString( ); // or ""
			}
		}

	}
}
