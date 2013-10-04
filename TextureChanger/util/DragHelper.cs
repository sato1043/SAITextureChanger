using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TextureChanger.util
{
	public class DragHelper
	{
		public enum CommonControls : uint
		{
			ICC_LISTVIEW_CLASSES = 0x00000001, // listview, header
			ICC_TREEVIEW_CLASSES = 0x00000002, // treeview, tooltips
			ICC_BAR_CLASSES = 0x00000004, // toolbar, statusbar, trackbar, tooltips
			ICC_TAB_CLASSES = 0x00000008, // tab, tooltips
			ICC_UPDOWN_CLASS = 0x00000010, // updown
			ICC_PROGRESS_CLASS = 0x00000020, // progress
			ICC_HOTKEY_CLASS = 0x00000040, // hotkey
			ICC_ANIMATE_CLASS = 0x00000080, // animate
			ICC_WIN95_CLASSES = 0x000000FF,
			ICC_DATE_CLASSES = 0x00000100, // month picker, date picker, time picker, updown
			ICC_USEREX_CLASSES = 0x00000200, // comboex
			ICC_COOL_CLASSES = 0x00000400, // rebar (coolbar) control
			ICC_INTERNET_CLASSES = 0x00000800,
			ICC_PAGESCROLLER_CLASS = 0x00001000,  // page scroller
			ICC_NATIVEFNTCTL_CLASS = 0x00002000,  // native font control
			ICC_STANDARD_CLASSES = 0x00004000,
			ICC_LINK_CLASS = 0x00008000
		}

		public struct INITCOMMONCONTROLSEX
		{
			private int dwSize;
			public uint dwICC;

			public INITCOMMONCONTROLSEX( uint dwICC )
				: this( )
			{
				dwSize = Marshal.SizeOf( typeof( INITCOMMONCONTROLSEX ) );
				this.dwICC = dwICC;
			}

			public INITCOMMONCONTROLSEX( CommonControls ICC )
				: this( (uint)ICC )
			{
			}

			public CommonControls ICC
			{
				get
				{
					return (CommonControls)dwICC;
				}
				set
				{
					dwICC = (uint)value;
				}
			}
		}

		[DllImport( "comctl32.dll", EntryPoint = "InitCommonControlsEx", CallingConvention = CallingConvention.StdCall )]
		static extern bool InitCommonControlsEx( ref INITCOMMONCONTROLSEX iccex );


		static DragHelper( )
		{
			//InitCommonControlsEx( new INITCOMMONCONTROLSEX( CommonControls ICC ) );
		}

		/// <summary>
		/// Begins dragging an image.
		/// </summary>
		/// <param name="himlTrack">Handle to the image list.</param>
		/// <param name="iTrack">Index of the image to drag.</param>
		/// <param name="dxHotspot">x-coordinate of the location of the drag position relative to the upper-left corner of the image.</param>
		/// <param name="dyHotspot">y-coordinate of the location of the drag position relative to the upper-left corner of the image.</param>
		/// <returns>Returns nonzero if successful, or zero otherwise.</returns>
		/// ImageList_BeginDragはドラッグ中に表示されるイメージにおける相対座標を指定
		[DllImport( "comctl32.dll", CharSet = CharSet.Auto )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool ImageList_BeginDrag( IntPtr himlTrack, int iTrack, int dxHotspot, int dyHotspot );


		/// <summary>
		/// Moves the image that is being dragged during a drag-and-drop operation. 
		/// This function is typically called in response to a WM_MOUSEMOVE message.
		/// </summary>
		/// <param name="x">X-coordinate at which to display the drag image. 
		/// The coordinate is relative to the upper-left corner of the window, not the client area.</param>
		/// <param name="y">Y-coordinate at which to display the drag image. 
		/// The coordinate is relative to the upper-left corner of the window, not the client area.</param>
		/// <returns>Returns nonzero if successful, or zero otherwise.</returns>
		/// ImageList_DragMove、ImageList_DragEnterに指定する座標はWindowの左上からの相対座標を渡す
		[DllImport( "comctl32.dll", CharSet = CharSet.Auto )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool ImageList_DragMove( int x, int y );


		/// <summary>
		/// Ends a drag operation.
		/// </summary>
		[DllImport( "comctl32.dll", CharSet = CharSet.Auto )]
		public static extern void ImageList_EndDrag( );


		/// <summary>
		/// Displays the drag image at the specified position within the window.
		/// </summary>
		/// <param name="hwndLock">Handle to the window that owns the drag image.</param>
		/// <param name="x">X-coordinate at which to display the drag image. 
		/// The coordinate is relative to the upper-left corner of the window, not the client area.</param>
		/// <param name="y">Y-coordinate at which to display the drag image. 
		/// The coordinate is relative to the upper-left corner of the window, not the client area.</param>
		/// <returns>Returns nonzero if successful, or zero otherwise.</returns>
		/// ImageList_DragMove、ImageList_DragEnterに指定する座標はWindowの左上からの相対座標を渡す
		// ImageList_DragEnterにはクライアント領域における相対座標ではなく 
		// タイトルバーなどの非クライアント領域を含むWindowにおける相対座標を指定する 
		// ドラッグ中は半透明イメージを表示し続けたいのでImageList_DragEnterには 
		// ListBoxのHandleを渡すのでなく、FormのHandleを渡す
		[DllImport( "comctl32.dll", CharSet = CharSet.Auto )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool ImageList_DragEnter( IntPtr hwndLock, int x, int y );


		/// <summary>
		/// Unlocks the specified window and hides the drag image, allowing the window to be updated.
		/// </summary>
		/// <param name="hwndLock">Handle to the window that owns the drag image.</param>
		/// <returns>Returns nonzero if successful, or zero otherwise.</returns>
		[DllImport( "comctl32.dll", CharSet = CharSet.Auto )]
		[return: MarshalAs( UnmanagedType.Bool )]
		internal static extern bool ImageList_DragLeave( IntPtr hwndLock );


		/// <summary>
		/// Shows or hides the image being dragged.
		/// </summary>
		/// <param name="fShow">Value specifying whether to show or hide the image being dragged. 
		/// Specify <see langword="true"/> to show the image or <see langword="false"/> to hide the image.</param>
		/// <returns>Returns nonzero if successful, or zero otherwise. </returns>
		[DllImport( "comctl32.dll", CharSet = CharSet.Auto )]
		[return: MarshalAs( UnmanagedType.Bool )]
		internal static extern bool ImageList_DragShowNolock( [MarshalAs( UnmanagedType.Bool )] bool fShow );
	}
}
