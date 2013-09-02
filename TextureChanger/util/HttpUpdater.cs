using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;

namespace TextureChanger.util
{
	class HttpUpdater
	{
		private const string UpdateSetupUri
			= "https://github.com/sato1043/SAITextureChanger/raw/master/TextureChangerSetup/Release/TextureChangerSetup.msi";

		private const string AppConfigUri
			= "https://github.com/sato1043/SAITextureChanger/raw/master/TextureChangerSetup/TextureChanger.exe.config";

		private const int DefaultTimeoutMs = 10000;

		private const int HttpStreamTempBufferSize = 8000;

		#region メッセージボックスのオーナーウィンドウ
		private TextureChangerForm _owner = null;

		public TextureChangerForm Owner
		{
			get
			{
				return _owner;
			}
		}
		#endregion

		public HttpUpdater( TextureChangerForm owner )
		{
			this._owner = owner;
		}

		public string GetExeFullPath( )
		{
			return Assembly.GetEntryAssembly( ).Location;
		}

		public FileInfo GetExeFileInfo( string fullpath )
		{
			if( string.IsNullOrEmpty( fullpath ) )
			{
				throw new ArgumentNullException( "fullpath" );
			}

			return new FileInfo( fullpath );
		}

		public void BeginAsyncCheckAppConfigUpdated( )
		{
			try
			{
				WebClient webClient = new WebClient( );
				webClient.DownloadStringCompleted
					+= new DownloadStringCompletedEventHandler(
							AppConfigNeedUpdate_DownloadStringCompleted );
				webClient.DownloadStringAsync( new Uri( AppConfigUri ) );
			}
			catch
			{
				// エラーが起きても捨て置く
			}
		}

		private void AppConfigNeedUpdate_DownloadStringCompleted( object sender, DownloadStringCompletedEventArgs e )
		{
			if (e.Error != null)
				return; // エラーが起きても捨て置く
			
			FileInfo localFile = GetExeFileInfo( GetExeFullPath( ) );
			
			try
			{
				string result = e.Result;
				StringReader sr = new StringReader( result );

				XmlReaderSettings settings = new XmlReaderSettings( );
				settings.IgnoreWhitespace = true;
				settings.IgnoreComments = true;

				using( XmlReader xr = XmlReader.Create( sr, settings ) )
				{
					xr.ReadToFollowing( "appSettings" );

					while (xr.ReadToFollowing("add"))
					{
						if (xr.GetAttribute("key") != "AppVers")
							continue;
						string value = xr.GetAttribute("value");
						
						System.Diagnostics.FileVersionInfo ver =
							System.Diagnostics.FileVersionInfo.GetVersionInfo(
							System.Reflection.Assembly.GetExecutingAssembly( ).Location );
						
						// プロジェクトプロパティのアセンブリ情報、ファイルバージョンと比較します。
						if (ver.FileVersion.CompareTo(value) < 0)
						{
							DownloadFileExisted();
							break;
						}
					}
				}
			}
			catch
			{
				// エラーが起きても捨て置く
			}
		}

		public void DownloadFileExisted()
		{
			DialogResult res = CenteredMessageBox.Show(this.Owner
				, "TextureChangerの更新プログラムが存在します。\n" +
				  "起動を中断して、更新をダウンロード・インストールしますか？\n" +
				  "  （更新はいつでもヘルプメニューから確認できます）"
				, "TexureChanger起動確認"
				, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
			if (res == DialogResult.No)
				return;

			try
			{
				Process.Start("msiexec.exe", @"/i " + UpdateSetupUri);
				//string tempfilename = GetOnlineFile();
				//Process.Start("msiexec.exe", @"/i " + tempfilename);
				this.Owner.ForceExitProgram = true;
				this.Owner.Close();
			}
			catch
			{
				CenteredMessageBox.Show(this.Owner
					, "申し訳ありません。\n" +
					  "TextureChangerのアップデートに失敗してしまいした。"
					, "TexureChanger起動確認"
					, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public string GetOnlineFile()
		{
			WebRequest req = WebRequest.Create( UpdateSetupUri );
			req.Method = "GET"; // 規定値なので設定は不要
			req.Timeout = DefaultTimeoutMs;
			req.PreAuthenticate = false;
			//req.Credentials = CredentialCache.DefaultCredentials;

			WebResponse resp = null;
			string tempfilename = null;
			string lastModified = null;
			try
			{
				resp = req.GetResponse();
				Stream stm = resp.GetResponseStream();
				tempfilename = Path.GetTempFileName();
				using (FileStream dest = new FileStream(tempfilename, FileMode.Create))
				{
					byte[] buff = new Byte[HttpStreamTempBufferSize];
					int len = 0;
					while ((len = stm.Read(buff, 0, buff.Length)) > 0)
					{
						dest.Write(buff, 0, len);
					}
				}
				lastModified = resp.Headers["Last-Modified"];
			}
			catch
			{
				throw; // needs your handler. Timeout or something.
			}
			finally
			{
				if( resp != null )
					resp.Close( );
			}
			return tempfilename;
		}


	}
}
