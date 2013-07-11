using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TextureChanger
{
	/// <summary>
	/// Win API
	/// </summary>
	public class Win32Api
	{
		[DllImport( "user32.dll" )]
		public static extern IntPtr GetWindowLong( IntPtr hWnd, int nIndex );
		[DllImport( "kernel32.dll" )]
		public static extern IntPtr GetCurrentThreadId( );
		[DllImport( "user32.dll" )]
		public static extern IntPtr SetWindowsHookEx( int idHook, HOOKPROC lpfn, IntPtr hInstance, IntPtr threadId );
		[DllImport( "user32.dll" )]
		public static extern bool UnhookWindowsHookEx( IntPtr hHook );
		[DllImport( "user32.dll" )]
		public static extern IntPtr CallNextHookEx( IntPtr hHook, int nCode, IntPtr wParam, IntPtr lParam );
		[DllImport( "user32.dll" )]
		public static extern bool SetWindowPos( IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags );
		[DllImport( "user32.dll" )]
		public static extern bool GetWindowRect( IntPtr hWnd, out RECT lpRect );

		public delegate IntPtr HOOKPROC( int nCode, IntPtr wParam, IntPtr lParam );

		public const int GWL_HINSTANCE = ( -6 );
		public const int WH_CBT = 5;
		public const int HCBT_ACTIVATE = 5;

		public const int SWP_NOSIZE = 0x0001;
		public const int SWP_NOZORDER = 0x0004;
		public const int SWP_NOACTIVATE = 0x0010;

        public const int MAX_PATH = 260;


        public struct RECT
		{
			public RECT( int left, int top, int right, int bottom )
			{
				Left = left;
				Top = top;
				Right = right;
				Bottom = bottom;
			}

			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}
		
		
		#region アイコン取得用のWin32API

		// SHGetFileInfo関数
		[DllImport( "shell32.dll" )]
		public static extern IntPtr SHGetFileInfo( string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags );

		// SHGetFileInfo関数で使用するフラグ
		public const uint SHGFI_ICON = 0x100; // アイコン・リソースの取得
		public const uint SHGFI_LARGEICON = 0x0; // 大きいアイコン
		public const uint SHGFI_SMALLICON = 0x1; // 小さいアイコン

		// SHGetFileInfo関数で使用する構造体
		public struct SHFILEINFO
		{
			public IntPtr hIcon;
			public IntPtr iIcon;
			public uint dwAttributes;
			[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 260 )]
			public string szDisplayName;
			[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 80 )]
			public string szTypeName;
		};

		#endregion



        // C# representation of the IMalloc interface.
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
           Guid("00000002-0000-0000-C000-000000000046")]
        public interface IMalloc
        {
            [PreserveSig]
            IntPtr Alloc([In] int cb);
            [PreserveSig]
            IntPtr Realloc([In] IntPtr pv, [In] int cb);
            [PreserveSig]
            void Free([In] IntPtr pv);
            [PreserveSig]
            int GetSize([In] IntPtr pv);
            [PreserveSig]
            int DidAlloc(IntPtr pv);
            [PreserveSig]
            void HeapMinimize();
        }

        [DllImport("User32.DLL")]
        public static extern IntPtr GetActiveWindow();


		#region Shell32

		public class Shell32
        {
            // Styles used in the BROWSEINFO.ulFlags field.
            [Flags]
            public enum BffStyles
            {
                RestrictToFilesystem = 0x0001, // BIF_RETURNONLYFSDIRS
                RestrictToDomain = 0x0002, // BIF_DONTGOBELOWDOMAIN
                RestrictToSubfolders = 0x0008, // BIF_RETURNFSANCESTORS
                ShowTextBox = 0x0010, // BIF_EDITBOX
                ValidateSelection = 0x0020, // BIF_VALIDATE
                NewDialogStyle = 0x0040, // BIF_NEWDIALOGSTYLE
                BrowseForComputer = 0x1000, // BIF_BROWSEFORCOMPUTER
                BrowseForPrinter = 0x2000, // BIF_BROWSEFORPRINTER
                BrowseForEverything = 0x4000, // BIF_BROWSEINCLUDEFILES
            }

            // Delegate type used in BROWSEINFO.lpfn field.
            public delegate int BFFCALLBACK(IntPtr hwnd, uint uMsg, IntPtr lParam, IntPtr lpData);

            [StructLayout(LayoutKind.Sequential, Pack = 8)]
            public struct BROWSEINFO
            {
                public IntPtr hwndOwner;
                public IntPtr pidlRoot;
                public IntPtr pszDisplayName;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string lpszTitle;
                public int ulFlags;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public BFFCALLBACK lpfn;
                public IntPtr lParam;
                public int iImage;
            }

            [DllImport("Shell32.DLL")]
            public static extern int SHGetMalloc(out IMalloc ppMalloc);

			public static Win32Api.IMalloc GetSHMalloc( )
			{
				Win32Api.IMalloc malloc;
				Win32Api.Shell32.SHGetMalloc( out malloc );
				return malloc;
			}

			[DllImport( "Shell32.DLL" )]
            public static extern int SHGetSpecialFolderLocation(
                        IntPtr hwndOwner, int nFolder, out IntPtr ppidl);

            [DllImport("Shell32.DLL")]
            public static extern int SHGetPathFromIDList(
                        IntPtr pidl, StringBuilder Path);

            [DllImport("Shell32.DLL", CharSet = CharSet.Auto)]
            public static extern IntPtr SHBrowseForFolder(ref BROWSEINFO bi);
			
			
		}
        #endregion


    }
}