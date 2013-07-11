using System;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace TextureChanger.util
{
	public class BrowseFolderDialog : Component
	{

		#region 表示時ダイアログメッセージ

		private string _dialogMessage;

		public string DialogMessage
		{
			get
			{
				return _dialogMessage;
			}
			set
			{
				_dialogMessage = value;
			}
		}
		#endregion

		#region 結果、取得できたディレクトリパス

		private string _directoryPath;

		public string DirectoryPath
		{
			get
			{
				return _directoryPath;
			}
		}
		#endregion

		#region 表示時初期ルートノード

		//special folderを示すDWORD定数
		public enum FolderId
		{
			Desktop = 0x0000,
			Printers = 0x0004,
			MyDocuments = 0x0005,
			Favorites = 0x0006,
			Recent = 0x0008,
			SendTo = 0x0009,
			StartMenu = 0x000b,
			MyComputer = 0x0011,
			NetworkNeighborhood = 0x0012,
			Templates = 0x0015,
			MyPictures = 0x0027,
			NetAndDialUpConnections = 0x0031,
		};

		private FolderId _startLocation;

		public FolderId StartLocation
		{
			get
			{
				return _startLocation;
			}
			set
			{
				new UIPermission(UIPermissionWindow.AllWindows).Demand();
				_startLocation = value;
			}
		}

		#endregion

		#region 表示時DWORD値ダイアログオプション（ファイル下部に個別設定用プロパティあり）

		private int _publicOptions =
			(int) Win32Api.Shell32.BffStyles.RestrictToFilesystem |
			(int) Win32Api.Shell32.BffStyles.RestrictToDomain     ;

		private int _privateOptions =
			(int)Win32Api.Shell32.BffStyles.NewDialogStyle; //表示時に_publicOptionsに加算されます。

		private void SetOptionField( int mask, bool turnOn )
		{
			if (turnOn)
				_publicOptions |= mask;
			else
				_publicOptions &= ~mask;
		}
		#endregion

		// TODO: BFFCALLBACKの実装

		public BrowseFolderDialog( )
		{
			_dialogMessage = "フォルダを選択してください：";
			_directoryPath = String.Empty;
			_startLocation = FolderId.Desktop;
		}

		public DialogResult ShowDialog()
		{
			return ShowDialog(null);
		}
		
		public DialogResult ShowDialog(IWin32Window owner)
		{
			IntPtr pidlRoot = IntPtr.Zero; //ルートノードのPIDLを保存する

			IntPtr hWndOwner = ( owner != null ) ? owner.Handle : Win32Api.GetActiveWindow();

			// Get the IDL for the specific startLocation.
			Win32Api.Shell32.SHGetSpecialFolderLocation( hWndOwner, (int)_startLocation, out pidlRoot );

			if (pidlRoot == IntPtr.Zero)
			{
				return DialogResult.Cancel;
			}

			int mergedOptions = (int)_publicOptions | (int)_privateOptions;

			if ((mergedOptions & (int)Win32Api.Shell32.BffStyles.NewDialogStyle) != 0)
			{
				if (System.Threading.ApartmentState.MTA == Application.OleRequired())
					mergedOptions = mergedOptions & (~(int)Win32Api.Shell32.BffStyles.NewDialogStyle);
			}

			IntPtr pidlRet = IntPtr.Zero;

			try
			{
				// Construct a BROWSEINFO.
				Win32Api.Shell32.BROWSEINFO bi = new Win32Api.Shell32.BROWSEINFO();
				IntPtr buffer = Marshal.AllocHGlobal(Win32Api.MAX_PATH);

				bi.pidlRoot       = pidlRoot;
				bi.hwndOwner      = hWndOwner;
				bi.pszDisplayName = buffer;
				bi.lpszTitle      = DialogMessage;
				bi.ulFlags        = mergedOptions;
				// The rest of the fields are initialized to zero by the constructor.
				// bi.lpfn = null;  bi.lParam = IntPtr.Zero;    bi.iImage = 0;

				// Show the dialog.
				pidlRet = Win32Api.Shell32.SHBrowseForFolder(ref bi);

				// Free the buffer you've allocated on the global heap.
				Marshal.FreeHGlobal(buffer);

				if (pidlRet == IntPtr.Zero)
				{
					// User clicked Cancel.
					return DialogResult.Cancel;
				}

				// Then retrieve the path from the IDList.
				StringBuilder sb = new StringBuilder( Win32Api.MAX_PATH );
				if (0 == Win32Api.Shell32.SHGetPathFromIDList(pidlRet, sb))
				{
					return DialogResult.Cancel;
				}

				// Convert to a string.
				_directoryPath = sb.ToString();
			}
			finally
			{
				Win32Api.IMalloc malloc = Win32Api.Shell32.GetSHMalloc( );
				malloc.Free(pidlRoot);

				if (pidlRet != IntPtr.Zero)
				{
					malloc.Free(pidlRet);
				}
			}

			return DialogResult.OK;
		}
	
 
		public bool OnlyFilesystem
		{
			get
			{
				return ( _publicOptions & (int)Win32Api.Shell32.BffStyles.RestrictToFilesystem ) != 0;
			}
			set
			{
				SetOptionField( (int)Win32Api.Shell32.BffStyles.RestrictToFilesystem, value );
			}
		}
		public bool ShowNetworkFolders
		{
			get
			{
				return ( _publicOptions & (int)Win32Api.Shell32.BffStyles.RestrictToDomain ) != 0;
			}
			set
			{
				SetOptionField( (int)Win32Api.Shell32.BffStyles.RestrictToDomain, value );
			}
		}
		public bool OnlySubfolders
		{
			get
			{
				return ( _publicOptions & (int)Win32Api.Shell32.BffStyles.RestrictToSubfolders ) != 0;
			}
			set
			{
				SetOptionField( (int)Win32Api.Shell32.BffStyles.RestrictToSubfolders, value );
			}
		}
		public bool ShowTextBox
		{
			get
			{
				return ( _publicOptions & (int)Win32Api.Shell32.BffStyles.ShowTextBox ) != 0;
			}
			set
			{
				SetOptionField( (int)Win32Api.Shell32.BffStyles.ShowTextBox, value );
			}
		}
		public bool ValidateUserInput
		{
			get
			{
				return ( _publicOptions & (int)Win32Api.Shell32.BffStyles.ValidateSelection ) != 0;
			}
			set
			{
				SetOptionField( (int)Win32Api.Shell32.BffStyles.ValidateSelection, value );
			}
		}
		public bool SelectComputer
		{
			get
			{
				return ( _publicOptions & (int)Win32Api.Shell32.BffStyles.BrowseForComputer ) != 0;
			}
			set
			{
				SetOptionField( (int)Win32Api.Shell32.BffStyles.BrowseForComputer, value );
			}
		}
		public bool SelectPrinter
		{
			get
			{
				return ( _publicOptions & (int)Win32Api.Shell32.BffStyles.BrowseForPrinter ) != 0;
			}
			set
			{
				SetOptionField( (int)Win32Api.Shell32.BffStyles.BrowseForPrinter, value );
			}
		}
		public bool SelectFiles
		{
			get
			{
				return ( _publicOptions & (int)Win32Api.Shell32.BffStyles.BrowseForEverything ) != 0;
			}
			set
			{
				SetOptionField( (int)Win32Api.Shell32.BffStyles.BrowseForEverything, value );
			}
		}

	}

}
