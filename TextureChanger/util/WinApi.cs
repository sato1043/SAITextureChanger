using System;
using System.Runtime.InteropServices;

namespace TextureChanger
{
	/// <summary>
	/// Win API
	/// </summary>
	public class WinApi
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

	}
}