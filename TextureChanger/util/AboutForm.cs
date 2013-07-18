using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TextureChanger.util
{
	public partial class AboutForm : Form
	{
		public AboutForm( )
		{
			InitializeComponent( );
		}

		private void AboutForm_Click( object sender, EventArgs e )
		{
			this.Close( );
		}

		private void AboutForm_Load( object sender, EventArgs e )
		{
			#region アプリケーション・アイコンを表示
			var shinfo = new Win32.SH.SHFILEINFO();
            IntPtr hSuccess = Win32.SH.SHGetFileInfo(
				Assembly.GetEntryAssembly( ).Location, 0,
                ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SH.SHGFI.ICON | Win32.SH.SHGFI.LARGEICON);

			Icon appIcon;
			if( hSuccess != IntPtr.Zero )
			{
				appIcon = Icon.FromHandle(shinfo.hIcon);
			}
			else
			{
				appIcon = SystemIcons.Application;
			}
			pctAppIcon.Image = appIcon.ToBitmap();
			#endregion


			#region ラベルテキストを表示
			lblAppName.Text = Application.CompanyName + " " + Application.ProductName;
			lblAppVers.Text = "Version " + Application.ProductVersion;

			object[] CopyrightArray =
				Assembly.GetEntryAssembly( ).GetCustomAttributes(
				typeof( AssemblyCopyrightAttribute ), false );
			if ((CopyrightArray != null) && (CopyrightArray.Length > 0))
			{
				lblAppLicense.Text =
					((AssemblyCopyrightAttribute) CopyrightArray[0]).Copyright;
			}
			else
			{
				lblAppLicense.Text = "";
			}
			#endregion

		}
	}
}
