using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32
{
    #region POINT
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public POINT(System.Drawing.Point pt) : this(pt.X, pt.Y) { }

        public static implicit operator System.Drawing.Point(POINT p)
        {
            return new System.Drawing.Point(p.X, p.Y);
        }

        public static implicit operator POINT(System.Drawing.Point p)
        {
            return new POINT(p.X, p.Y);
        }
    }
    #endregion

    #region RECT
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left, Top, Right, Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

        public int X
        {
            get { return Left; }
            set { Right -= (Left - value); Left = value; }
        }

        public int Y
        {
            get { return Top; }
            set { Bottom -= (Top - value); Top = value; }
        }

        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }

        public System.Drawing.Point Location
        {
            get { return new System.Drawing.Point(Left, Top); }
            set { X = value.X; Y = value.Y; }
        }

        public System.Drawing.Size Size
        {
            get { return new System.Drawing.Size(Width, Height); }
            set { Width = value.Width; Height = value.Height; }
        }

        public static implicit operator System.Drawing.Rectangle(RECT r)
        {
            return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
        }

        public static implicit operator RECT(System.Drawing.Rectangle r)
        {
            return new RECT(r);
        }

        public static bool operator ==(RECT r1, RECT r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(RECT r1, RECT r2)
        {
            return !r1.Equals(r2);
        }

        public bool Equals(RECT r)
        {
            return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
        }

        public override bool Equals(object obj)
        {
            if (obj is RECT)
            return Equals((RECT)obj);
            else if (obj is System.Drawing.Rectangle)
            return Equals(new RECT((System.Drawing.Rectangle)obj));
            return false;
        }

        public override int GetHashCode()
        {
            return ((System.Drawing.Rectangle)this).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
        }
    }
    #endregion

	#region SIZE
	[StructLayout(LayoutKind.Sequential)]
	public struct SIZE
	{
		public int cx;
		public int cy;

		public SIZE(int cx, int cy)
		{
			this.cx = cx;
			this.cy = cy;
		}
	}
	#endregion

	public enum GWL : int
    {
        WNDPROC   = (-4),
        HINSTANCE = (-6),
    }

    public enum WH : int {
	    CBT = 5,
    };

    public enum HCBT : int {
	    ACTIVATE = 5,
    };

    public enum SWP : int {
	    NOSIZE = 0x0001,
	    NOZORDER = 0x0004,
	    NOACTIVATE = 0x0010,
    };

    public enum MAX : int {
	    PATH = 260,
    };

	[Flags]
	public enum ILD_FLAGS : int
	{
		NORMAL = 0x00000000,
		TRANSPARENT = 0x00000001,
		BLEND25 = 0x00000002,
		FOCUS = 0x00000002,
		BLEND50 = 0x00000004,
		SELECTED = 0x00000004,
		BLEND = 0x00000004,
		MASK = 0x00000010,
		IMAGE = 0x00000020,
		ROP = 0x00000040,
		OVERLAYMASK = 0x00000F00,
		PRESERVEALPHA = 0x00001000,
		SCALE = 0x00002000,
		DPISCALE = 0x00004000,
		ASYNC = 0x00008000,
	};

    #region WIN32 API prototypes
    public partial class Api
    {
        #region WindowsLong
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowLong(IntPtr hwnd, GWL nIndex);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLong(IntPtr hwnd, GWL nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLong(IntPtr hwnd, GWL nIndex, HOOKPROC dwNewLong);
        #endregion

        #region WindowsPos
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, SWP uFlags);
        #endregion

        #region some window operation
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowsHookEx(WH idHook, HOOKPROC lpfn, IntPtr hInstance, IntPtr threadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, IntPtr lParam);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CallWindowProc(int lpPrevWndFunc, IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);
        #endregion

        #region thread something ;P
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetCurrentThreadId();
        #endregion

		#region icon manip
		[DllImport("Shell32.dll", CharSet = CharSet.Auto)]
		public static extern uint ExtractIconEx(
			string lpszFile, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);

		[DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool DestroyIcon( IntPtr hIcon );
		#endregion

		#region ImageList manip
		[DllImport("comctl32.dll", CharSet = CharSet.Auto, SetLastError=true)]
		public static extern IntPtr ImageList_GetIcon(IntPtr himl, int i, ILD_FLAGS flags);
		[DllImport("comctl32.dll", CharSet = CharSet.Auto)]
		public static extern int ImageList_GetImageCount(IntPtr himl);
		#endregion

		[DllImport( "kernel32", CharSet = CharSet.Auto )]
	    public extern static int GetLastError( );


		#region FormatMessage

	    public enum FORMAT_MESSAGE : uint
	    {
			ALLOCATE_BUFFER = 0x100,
			ARGUMENT_ARRAY  = 0x2000,
			FROM_HMODULE    = 0x800,
			FROM_STRING     = 0x400,
			FROM_SYSTEM     = 0x1000,
			IGNORE_INSERTS  = 0x200,
			MAX_WIDTH_MASK  = 0xFF,
	    }

		[DllImport( "kernel32", CharSet = CharSet.Auto )]
		public static extern int FormatMessage(
		    int dwFlags,
		    IntPtr lpSource,
		    int dwMessageId,
		    int dwLanguageId,
		    string lpBuffer,
		    uint nSize,
		    int argumentsLong
		);
	    #endregion




    };
    #endregion

}