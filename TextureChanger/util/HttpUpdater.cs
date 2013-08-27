using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace TextureChanger.util
{
	class HttpUpdater
	{
		private const string UpdateSetupUri
			= "https://github.com/sato1043/SAITextureChanger/blob/master/TextureChangerSetup/Release/TextureChangerSetup.msi";

		private const string AppConfigUri
			= "https://raw.github.com/sato1043/SAITextureChanger/master/TextureChanger/bin/Release/TextureChanger.exe.config";

		private const int DefaultTimeoutMs = 10000;

		private const int HttpStreamTempBufferSize = 8000;

		#region メッセージボックスのオーナーウィンドウ
		private IWin32Window _owner = null;

		public IWin32Window Owner
		{
			get
			{
				return _owner;
			}
		}
		#endregion

		public HttpUpdater(IWin32Window owner)
		{
			this._owner = owner;
		}

		public void CheckUpdate( IWin32Window owner )
		{
			bool needUpdate = false;
			try
			{
				needUpdate = NeedUpdate();
				if( needUpdate == false )
					return;
			}
			catch
			{
			}

			DialogResult res = CenteredMessageBox.Show( owner
				, "TextureChangerの更新プログラムが存在します。\n" +
					"ダウンロード・インストールしますか？"
				, "TexureChanger起動確認"
				, MessageBoxButtons.YesNo, MessageBoxIcon.Information );
			if( res == DialogResult.No )
				return;
			
			try
			{
				GetOnlineFile(owner);
			}
			catch (Exception)
			{
				CenteredMessageBox.Show( owner
					, "申し訳ありません。\n" +
 					  "TextureChangerのアップデートに失敗してしまいした。"
					, "TexureChanger起動確認"
					, MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
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

					while( xr.Read( ) )
					{
						if (xr.LocalName != "add")
							continue;

						if (xr.HasAttributes == false)
							continue;

						for (int i = 0; i < xr.AttributeCount; i++)
						{
							xr.MoveToAttribute( i );
							if( xr.Name( "key" ) == "AppVers" )
							{
								//xr.MoveToNextAttribute( );

								string value = xr.GetAttribute( "value" );

								Console.WriteLine( value );


								//自分自身のバージョン情報を取得する
								System.Diagnostics.FileVersionInfo ver =
									System.Diagnostics.FileVersionInfo.GetVersionInfo(
									System.Reflection.Assembly.GetExecutingAssembly( ).Location );
								//結果を表示
								Console.WriteLine( ver );
							}
						}


						//xr.MoveToFirstAttribute();

						
					}
				}
			}
			catch
			{
				// エラーが起きても捨て置く
			}
		}

		public bool NeedUpdate(  )
		{
			FileInfo localFile = GetExeFileInfo( GetExeFullPath() );

			bool result = true;
			WebRequest req = WebRequest.Create( UpdateSetupUri );
			req.Method = "HEAD";
			req.Timeout = DefaultTimeoutMs;
			req.PreAuthenticate = false;
			//req.Credentials = CredentialCache.DefaultCredentials;

			WebResponse resp = null;
			try
			{
				resp = req.GetResponse();
				if (localFile.Exists)
				{
					string date = resp.Headers["Date"];//["Last-Modified"];
					DateTime remoteDate = DateTime.Parse(date);
					DateTime localDate = localFile.LastWriteTime;
					result = remoteDate > localDate;
				}
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
			return result;
		}

		public void GetOnlineFile( IWin32Window owner )
		{
			WebRequest req = WebRequest.Create( UpdateSetupUri );
			req.Method = "GET"; // 規定値なので設定は不要
			req.Timeout = DefaultTimeoutMs;
			req.PreAuthenticate = false;
			//req.Credentials = CredentialCache.DefaultCredentials;

			WebResponse resp = null;
			string tempf = null;
			string lastModified = null;
			try
			{
				resp = req.GetResponse();
				Stream stm = resp.GetResponseStream();
				tempf = Path.GetTempFileName();
				using (FileStream dest = new FileStream(tempf, FileMode.Create))
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
		}


	}
}
