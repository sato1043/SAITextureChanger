using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

// based on Enhanced BrowseForFolder styled TreeView by Chris Richner, 28 May 2013
// http://www.codeproject.com/Articles/4472/Enhanced-BrowseForFolder-styled-TreeView

namespace Win32
{
	#region システムイメージリストで使えるサイズ
	public enum SystemImageListSize : uint
	{
		/// <summary>
		/// System Large Icon Size (typically 32x32)
		/// </summary>
		LargeIcons = 0x0,

		/// <summary>
		/// System Small Icon Size (typically 16x16)
		/// </summary>
		SmallIcons = 0x1,

		/// <summary>
		/// System Extra Large Icon Size (typically 48x48).
		/// Only available under XP; under other OS the
		/// Large Icon ImageList is returned.
		/// </summary>
		ExtraLargeIcons = 0x2
	}
	#endregion


	#region イメージリストの描画方法に関するフラグ
	[Flags]
	public enum ImageListDrawItemConstants : int
	{
		/// <summary>
		/// Draw item normally.
		/// </summary>
		ILD_NORMAL = 0x0,

		/// <summary>
		/// Draw item transparently.
		/// </summary>
		ILD_TRANSPARENT = 0x1,

		/// <summary>
		/// Draw item blended with 25% of the specified foreground colour
		/// or the Highlight colour if no foreground colour specified.
		/// </summary>
		ILD_BLEND25 = 0x2,

		/// <summary>
		/// Draw item blended with 50% of the specified foreground colour
		/// or the Highlight colour if no foreground colour specified.
		/// </summary>
		ILD_SELECTED = 0x4,

		/// <summary>
		/// Draw the icon's mask
		/// </summary>
		ILD_MASK = 0x10,

		/// <summary>
		/// Draw the icon image without using the mask
		/// </summary>
		ILD_IMAGE = 0x20,

		/// <summary>
		/// Draw the icon using the ROP specified.
		/// </summary>
		ILD_ROP = 0x40,

		/// <summary>
		/// Preserves the alpha channel in dest. XP only.
		/// </summary>
		ILD_PRESERVEALPHA = 0x1000,

		/// <summary>
		/// Scale the image to cx, cy instead of clipping it.  XP only.
		/// </summary>
		ILD_SCALE = 0x2000,

		/// <summary>
		/// Scale the image to the current DPI of the display. XP only.
		/// </summary>
		ILD_DPISCALE = 0x4000
	}
	#endregion


	#region XP イメージリスト(?)描画状態オプション一覧
	/// <summary>
	/// Enumeration containing XP ImageList Draw State options
	/// </summary>
	[Flags]
	public enum ImageListDrawStateConstants : int
	{
		/// <summary>
		/// The image state is not modified. 
		/// </summary>
		ILS_NORMAL = (0x00000000),

		/// <summary>
		/// Adds a glow effect to the icon, which causes the icon to appear to glow 
		/// with a given color around the edges. (Note: does not appear to be
		/// implemented)
		/// </summary>
		ILS_GLOW = (0x00000001),
		//The color for the glow effect is passed to the IImageList::Draw method in the crEffect member of IMAGELISTDRAWPARAMS. 

		/// <summary>
		/// Adds a drop shadow effect to the icon. (Note: does not appear to be
		/// implemented)
		/// </summary>
		ILS_SHADOW = (0x00000002),
		//The color for the drop shadow effect is passed to the IImageList::Draw method in the crEffect member of IMAGELISTDRAWPARAMS. 

		/// <summary>
		/// Saturates the icon by increasing each color component 
		/// of the RGB triplet for each pixel in the icon. (Note: only ever appears
		/// to result in a completely unsaturated icon)
		/// </summary>
		ILS_SATURATE = (0x00000004),
		// The amount to increase is indicated by the frame member in the IMAGELISTDRAWPARAMS method. 

		/// <summary>
		/// Alpha blends the icon. Alpha blending controls the transparency 
		/// level of an icon, according to the value of its alpha channel. 
		/// (Note: does not appear to be implemented).
		/// </summary>
		ILS_ALPHA = (0x00000008)
		//The value of the alpha channel is indicated by the frame member in the IMAGELISTDRAWPARAMS method. The alpha channel can be from 0 to 255, with 0 being completely transparent, and 255 being completely opaque. 
	}
	#endregion


	#region シェルで描画するためのアイコン状態を指定するフラグ
	[Flags]
	public enum ShellIconStateConstants : uint
	{
		ShellIconStateNormal = SH.SHGFI.ICON,
		ShellIconStateLinkOverlay = SH.SHGFI.LINKOVERLAY,
		ShellIconStateSelected = SH.SHGFI.SELECTED,
		ShellIconStateOpen = SH.SHGFI.OPENICON,
		ShellIconAddOverlays = SH.SHGFI.ADDOVERLAYS,
	}
	#endregion


	#region IMAGEINFO
	[StructLayout( LayoutKind.Sequential )]
	public struct IMAGEINFO
	{
		public IntPtr hbmImage;
		public IntPtr hbmMask;
		public int Unused1;
		public int Unused2;
		public RECT rcImage;
	}
	#endregion


	#region IMAGELISTDRAWPARAMS
	[StructLayout( LayoutKind.Sequential )]
	public struct IMAGELISTDRAWPARAMS
	{
		public int cbSize;
		public IntPtr himl;
		public int i;
		public IntPtr hdcDst;
		public int x;
		public int y;
		public int cx;
		public int cy;
		public int xBitmap; // x offest from the upperleft of bitmap
		public int yBitmap; // y offset from the upperleft of bitmap
		public int rgbBk;
		public int rgbFg;
		public int fStyle;
		public int dwRop;
		public int fState;
		public int Frame;
		public int crEffect;
	}
	#endregion


	#region IImageList : Private ImageList COM Interop (XP)
	[ComImport( )]
	[Guid( "46EB5926-582E-4017-9FDF-E8998DAA0950" )]
	[InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
	//helpstring("Image List"),
	public interface IImageList
	{
		[PreserveSig]
		int Add(
			IntPtr hbmImage,
			IntPtr hbmMask,
			ref int pi );

		[PreserveSig]
		int ReplaceIcon(
			int i,
			IntPtr hicon,
			ref int pi );

		[PreserveSig]
		int SetOverlayImage(
			int iImage,
			int iOverlay );

		[PreserveSig]
		int Replace(
			int i,
			IntPtr hbmImage,
			IntPtr hbmMask );

		[PreserveSig]
		int AddMasked(
			IntPtr hbmImage,
			int crMask,
			ref int pi );

		[PreserveSig]
		int Draw(
			ref IMAGELISTDRAWPARAMS pimldp );

		[PreserveSig]
		int Remove(
			int i );

		[PreserveSig]
		int GetIcon(
			int i,
			int flags,
			ref IntPtr picon );

		[PreserveSig]
		int GetImageInfo(
			int i,
			ref IMAGEINFO pImageInfo );

		[PreserveSig]
		int Copy(
			int iDst,
			IImageList punkSrc,
			int iSrc,
			int uFlags );

		[PreserveSig]
		int Merge(
			int i1,
			IImageList punk2,
			int i2,
			int dx,
			int dy,
			ref Guid riid,
			ref IntPtr ppv );

		[PreserveSig]
		int Clone(
			ref Guid riid,
			ref IntPtr ppv );

		[PreserveSig]
		int GetImageRect(
			int i,
			ref RECT prc );

		[PreserveSig]
		int GetIconSize(
			ref int cx,
			ref int cy );

		[PreserveSig]
		int SetIconSize(
			int cx,
			int cy );

		[PreserveSig]
		int GetImageCount(
			ref int pi );

		[PreserveSig]
		int SetImageCount(
			int uNewCount );

		[PreserveSig]
		int SetBkColor(
			int clrBk,
			ref int pclr );

		[PreserveSig]
		int GetBkColor(
			ref int pclr );

		[PreserveSig]
		int BeginDrag(
			int iTrack,
			int dxHotspot,
			int dyHotspot );

		[PreserveSig]
		int EndDrag( );

		[PreserveSig]
		int DragEnter(
			IntPtr hwndLock,
			int x,
			int y );

		[PreserveSig]
		int DragLeave(
			IntPtr hwndLock );

		[PreserveSig]
		int DragMove(
			int x,
			int y );

		[PreserveSig]
		int SetDragCursorImage(
			ref IImageList punk,
			int iDrag,
			int dxHotspot,
			int dyHotspot );

		[PreserveSig]
		int DragShowNolock(
			int fShow );

		[PreserveSig]
		int GetDragImage(
			ref POINT ppt,
			ref POINT pptHotspot,
			ref Guid riid,
			ref IntPtr ppv );

		[PreserveSig]
		int GetItemFlags(
			int i,
			ref int dwFlags );

		[PreserveSig]
		int GetOverlayImage(
			int iOverlay,
			ref int piIndex );
	};
	#endregion


	#region SystemImageList class
	public class SystemImageList : IDisposable
	{
		private IntPtr _hIml = IntPtr.Zero;
		private IImageList _iImageList = null;
		private SystemImageListSize _size = SystemImageListSize.SmallIcons;
		private bool _disposed = false;

		#region コンストラクタ
		public SystemImageList(SystemImageListSize size = SystemImageListSize.SmallIcons)
		{
			_size = size;
			Create();
		}
		#endregion

		#region デストラクタ
		~SystemImageList()
		{
			Dispose(false);
		}
		#endregion

		#region システムのイメージリストを取得する

		#region WindowsXPかどうか
		/// <summary>
		/// Determines if the system is running Windows XP
		/// or above
		/// </summary>
		/// <returns>True if system is running XP or above, False otherwise</returns>
		private bool isXpOrAbove( )
		{
			bool ret = false;
			if( Environment.OSVersion.Version.Major > 5 )
			{
				ret = true;
			}
			else if( ( Environment.OSVersion.Version.Major == 5 ) &&
					 ( Environment.OSVersion.Version.Minor >= 1 ) )
			{
				ret = true;
			}
			return ret;
			//return false;
		}
		#endregion

		private void Create( )
		{
			// forget last image list if any:
			_hIml = IntPtr.Zero;

			if( isXpOrAbove( ) )
			{
				// Get the System IImageList object from the Shell:
				Guid iidImageList = new Guid( "46EB5926-582E-4017-9FDF-E8998DAA0950" );
				int ret = SH.SHGetImageList(
					(int)_size,
					ref iidImageList,
					ref _iImageList
					);
				// the image list handle is the IUnknown pointer, but 
				// using Marshal.GetIUnknownForObject doesn't return
				// the right value.  It really doesn't hurt to make
				// a second call to get the handle:
				SH.SHGetImageListHandle( (int)_size, ref iidImageList, ref _hIml );
			}
			else
			{
				// Prepare flags:
				SH.SHGFI dwFlags = SH.SHGFI.USEFILEATTRIBUTES | SH.SHGFI.SYSICONINDEX;
				if( _size == SystemImageListSize.SmallIcons )
				{
					dwFlags |= SH.SHGFI.SMALLICON;
				}
				// Get image list
				SH.SHFILEINFO shfi = new SH.SHFILEINFO( );
				uint shfiSize = (uint)Marshal.SizeOf( shfi.GetType( ) );

				// Call SHGetFileInfo to get the image list handle
				// using an arbitrary file:
				_hIml = SH.SHGetFileInfo(
					".txt",
					SH.FILE_ATTRIBUTE_NORMAL,
					ref shfi,
					shfiSize,
					dwFlags );
				Debug.Assert( ( _hIml != IntPtr.Zero ), "Failed to create Image List" );
			}
		}

		#endregion

		#region システムのイメージリストを開放する（IDisposeインターフェイスの実装）
		/// <summary>
		/// Clears up any resources associated with the SystemImageList
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Clears up any resources associated with the SystemImageList
		/// when disposing is true.
		/// </summary>
		/// <param name="disposing">Whether the object is being disposed</param>
		public virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing) 
				{
					if (_iImageList != null)
					{
						Marshal.ReleaseComObject(_iImageList);
					}
					_iImageList = null;
				}
			}
			_disposed = true;
		}
	
		#endregion

		#region APIs

		[DllImport("comctl32")]
		private static extern int ImageList_Draw(
			IntPtr hIml,
			int i,
			IntPtr hdcDst,
			int x,
			int y,
			int fStyle);

		[DllImport("comctl32")]
		private static extern int ImageList_DrawIndirect(
			ref IMAGELISTDRAWPARAMS pimldp);

		[DllImport("comctl32")]
		private static extern int ImageList_GetIconSize(
			IntPtr himl,
			ref int cx,
			ref int cy);

		[DllImport("comctl32")]
		private static extern IntPtr ImageList_GetIcon(
			IntPtr himl,
			int i,
			int flags);

		#endregion

		#region Properties
		/// <summary>
		/// Gets the hImageList handle
		/// </summary>
		public IntPtr Handle
		{
			get { return _hIml; }
		}

		/// <summary>
		/// Gets/sets the size of System Image List to retrieve.
		/// </summary>
		public SystemImageListSize ImageListSize
		{
			get { return _size; }
			set { _size = value; Create(); }
		}

		/// <summary>
		/// Returns the size of the Image List Icons.
		/// </summary>
		public Size Size
		{
			get
			{
				int cx = 0; 
				int cy = 0;
				
				if (_iImageList == null)
					ImageList_GetIconSize(_hIml, ref cx, ref cy);
				else
					_iImageList.GetIconSize(ref cx, ref cy);

				Size sz = new Size(cx, cy);

				return sz;
			}
		}
		#endregion

		#region Methods

		#region アイコンインデクスとアイコンの取得

		#region イメージリストからアイコンのGDI+コピーを得る（インデクス指定）
		public Icon Icon(int index)
		{
			Icon icon = null;

			IntPtr hIcon = IntPtr.Zero;
			if (_iImageList == null)
			{
				hIcon = ImageList_GetIcon(
					_hIml,
					index,
					(int) ImageListDrawItemConstants.ILD_TRANSPARENT);

			}
			else
			{
				_iImageList.GetIcon(
					index,
					(int) ImageListDrawItemConstants.ILD_TRANSPARENT,
					ref hIcon);
			}

			if (hIcon != IntPtr.Zero)
			{
				icon = System.Drawing.Icon.FromHandle(hIcon);
			}
			return icon;
		}
		#endregion

		#region 指定ファイルのアイコンのインデクスを返す（常にキャッシュを得る）
		public int IconIndex(string fileName)
		{
			return IconIndex(fileName, false);
		}
		#endregion

		#region 指定ファイルのアイコンのインデクスを返す（再読み込み指定可能）
		public int IconIndex(string fileName, bool forceLoadFromDisk)
		{
			return IconIndex(
				fileName,
				forceLoadFromDisk,
				ShellIconStateConstants.ShellIconStateNormal);
		}
		#endregion

		#region 指定ファイルのアイコンのインデクスを返す
		/// <summary>
		/// Returns the index of the icon for the specified file
		/// </summary>
		/// <param name="fileName">Filename to get icon for</param>
		/// <param name="forceLoadFromDisk">If True, then hit the disk to get the icon,
		/// otherwise only hit the disk if no cached icon is available.</param>
		/// <param name="iconState">Flags specifying the state of the icon returned.</param>
		/// <returns>Index of the icon</returns>
		public int IconIndex( string fileName, bool forceLoadFromDisk, ShellIconStateConstants iconState )
		{
			uint dwAttr = 0;
		
			SH.SHGFI dwFlags = SH.SHGFI.SYSICONINDEX;
			if (_size == SystemImageListSize.SmallIcons)
			{
				dwFlags |= SH.SHGFI.SMALLICON;
			}

			// ディスクアクセスするか選択できます。
			// ディスクをヒットしないなら、アイコンがキャッシュされていないとおかしなアイコンを得ます。
			// これはファイルについてだけ機能します。
			if (!forceLoadFromDisk)
			{
				dwAttr = SH.FILE_ATTRIBUTE_NORMAL;
				dwFlags |= SH.SHGFI.USEFILEATTRIBUTES;
			}

			// sFileSpec can be any file. You can specify a
			// file that does not exist and still get the
			// icon, for example sFileSpec = "C:\PANTS.DOC"
			SH.SHFILEINFO shfi = new SH.SHFILEINFO();
			uint shfiSize = (uint) Marshal.SizeOf(shfi.GetType());
			IntPtr retVal = SH.SHGetFileInfo(
				fileName, dwAttr, ref shfi, shfiSize,
				(SH.SHGFI)((uint) (dwFlags) | (uint) iconState));

			if (retVal.Equals(IntPtr.Zero))
			{
				Debug.Assert((!retVal.Equals(IntPtr.Zero)), "Failed to get icon index");
				return 0;
			}
			else
			{
				return shfi.iIcon;
			}
		}
		#endregion

		#endregion

		#region デバイスコンテキストへのアイコンの描画
		/// <summary>
		/// Draws an image
		/// </summary>
		/// <param name="hdc">Device context to draw to</param>
		/// <param name="index">Index of image to draw</param>
		/// <param name="x">X Position to draw at</param>
		/// <param name="y">Y Position to draw at</param>
		public void DrawImage( 
			IntPtr hdc,
			int index, 
			int x,
			int y
			)
		{
			DrawImage(hdc, index, x, y, ImageListDrawItemConstants.ILD_TRANSPARENT);
		}

		/// <summary>
		/// Draws an image using the specified flags
		/// </summary>
		/// <param name="hdc">Device context to draw to</param>
		/// <param name="index">Index of image to draw</param>
		/// <param name="x">X Position to draw at</param>
		/// <param name="y">Y Position to draw at</param>
		/// <param name="flags">Drawing flags</param>
		public void DrawImage(
			IntPtr hdc,
			int index,
			int x,
			int y,
			ImageListDrawItemConstants flags
			)
		{
			if (_iImageList == null)
			{
				int ret = ImageList_Draw(
					_hIml, 
					index, 
					hdc, 
					x, 
					y, 
					(int)flags);
			}
			else
			{
				IMAGELISTDRAWPARAMS pimldp = new IMAGELISTDRAWPARAMS();
				pimldp.hdcDst = hdc;
				pimldp.cbSize = Marshal.SizeOf(pimldp.GetType());
				pimldp.i = index;
				pimldp.x = x;
				pimldp.y = y;
				pimldp.rgbFg = -1;
				pimldp.fStyle = (int)flags;
				_iImageList.Draw(ref pimldp);
			}
			
		}

		/// <summary>
		/// Draws an image using the specified flags and specifies
		/// the size to clip to (or to stretch to if ILD_SCALE
		/// is provided).
		/// </summary>
		/// <param name="hdc">Device context to draw to</param>
		/// <param name="index">Index of image to draw</param>
		/// <param name="x">X Position to draw at</param>
		/// <param name="y">Y Position to draw at</param>
		/// <param name="flags">Drawing flags</param>
		/// <param name="cx">Width to draw</param>
		/// <param name="cy">Height to draw</param>
		public void DrawImage(
			IntPtr hdc,
			int index,
			int x,
			int y,
			ImageListDrawItemConstants flags,
			int cx,
			int cy
			)
		{
			IMAGELISTDRAWPARAMS pimldp = new IMAGELISTDRAWPARAMS();
			pimldp.hdcDst = hdc;
			pimldp.cbSize = Marshal.SizeOf(pimldp.GetType());
			pimldp.i = index;
			pimldp.x = x;
			pimldp.y = y;
			pimldp.cx = cx;
			pimldp.cy = cy;
			pimldp.fStyle = (int)flags;
			if (_iImageList == null)
			{
				pimldp.himl = _hIml;
				int ret = ImageList_DrawIndirect(ref pimldp);
			}
			else
			{

				_iImageList.Draw(ref pimldp);
			}
		}

		/// <summary>
		/// Draws an image using the specified flags and state on XP systems.
		/// </summary>
		/// <param name="hdc">Device context to draw to</param>
		/// <param name="index">Index of image to draw</param>
		/// <param name="x">X Position to draw at</param>
		/// <param name="y">Y Position to draw at</param>
		/// <param name="flags">Drawing flags</param>
		/// <param name="cx">Width to draw</param>
		/// <param name="cy">Height to draw</param>
		/// <param name="foreColor">Fore colour to blend with when using the 
		/// ILD_SELECTED or ILD_BLEND25 flags</param>
		/// <param name="stateFlags">State flags</param>
		/// <param name="glowOrShadowColor">If stateFlags include ILS_GLOW, then
		/// the colour to use for the glow effect.  Otherwise if stateFlags includes 
		/// ILS_SHADOW, then the colour to use for the shadow.</param>
		/// <param name="saturateColorOrAlpha">If stateFlags includes ILS_ALPHA,
		/// then the alpha component is applied to the icon. Otherwise if 
		/// ILS_SATURATE is included, then the (R,G,B) components are used
		/// to saturate the image.</param>
		public void DrawImage(
			IntPtr hdc,
			int index,
			int x,
			int y,
			ImageListDrawItemConstants flags,
			int cx,
			int cy,
			Color foreColor,
			ImageListDrawStateConstants stateFlags,
			Color saturateColorOrAlpha,
			Color glowOrShadowColor
			)
		{
			IMAGELISTDRAWPARAMS pimldp = new IMAGELISTDRAWPARAMS();
			pimldp.hdcDst = hdc;
			pimldp.cbSize = Marshal.SizeOf(pimldp.GetType());
			pimldp.i = index;
			pimldp.x = x;
			pimldp.y = y;
			pimldp.cx = cx;
			pimldp.cy = cy;
			pimldp.rgbFg = Color.FromArgb(0, 
				foreColor.R, foreColor.G, foreColor.B).ToArgb();
			Console.WriteLine("{0}", pimldp.rgbFg);
			pimldp.fStyle = (int)flags;
			pimldp.fState = (int)stateFlags;
			if ((stateFlags & ImageListDrawStateConstants.ILS_ALPHA) ==
				ImageListDrawStateConstants.ILS_ALPHA)
			{
				// Set the alpha:
				pimldp.Frame = (int)saturateColorOrAlpha.A;
			}
			else if ((stateFlags & ImageListDrawStateConstants.ILS_SATURATE) ==
				ImageListDrawStateConstants.ILS_SATURATE)
			{
				// discard alpha channel:
				saturateColorOrAlpha = Color.FromArgb(0, 
					saturateColorOrAlpha.R, 
					saturateColorOrAlpha.G, 
					saturateColorOrAlpha.B);
				// set the saturate color
				pimldp.Frame = saturateColorOrAlpha.ToArgb();
			}
			glowOrShadowColor = Color.FromArgb(0, 
				glowOrShadowColor.R, 
				glowOrShadowColor.G, 
				glowOrShadowColor.B);
			pimldp.crEffect = glowOrShadowColor.ToArgb();
			if (_iImageList == null)
			{
				pimldp.himl = _hIml;
				int ret = ImageList_DrawIndirect(ref pimldp);
			}
			else
			{

				_iImageList.Draw(ref pimldp);
			}
		}		
		#endregion

		#endregion
	}
	#endregion

	#region SystemImageListHelper class
	/// <summary>
	/// ヘルパーメソッド郡（SystemImageListをコモンコントロールに結びつけるための）
	/// </summary>
	public class SystemImageListHelper
	{
		#region Constants
		private const int LVM_FIRST = 0x1000;
		private const int LVM_SETIMAGELIST = ( LVM_FIRST + 3 );

		private const int LVSIL_NORMAL = 0;
		private const int LVSIL_SMALL = 1;
		private const int LVSIL_STATE = 2;

		private const int TV_FIRST = 0x1100;
		private const int TVM_SETIMAGELIST = ( TV_FIRST + 9 );

		private const int TVSIL_NORMAL = 0;
		private const int TVSIL_STATE = 2;
		#endregion

		#region Methods
		/// <summary>
		/// SysImageListをリストビューにに関連付ける
		/// </summary>
		/// <param name="listView">ListView control to associate ImageList with</param>
		/// <param name="sysImageList">System Image List to associate</param>
		/// <param name="forStateImages">Whether to add ImageList as StateImageList</param>
		public static void SetListViewImageList(
			ListView listView,
			SystemImageList sysImageList,
			bool forStateImages
			)
		{
			IntPtr wParam = (IntPtr)LVSIL_NORMAL;
			if( sysImageList.ImageListSize == SystemImageListSize.SmallIcons )
			{
				wParam = (IntPtr)LVSIL_SMALL;
			}
			if( forStateImages )
			{
				wParam = (IntPtr)LVSIL_STATE;
			}
			Api.SendMessage(
				listView.Handle,
				LVM_SETIMAGELIST,
				wParam,
				sysImageList.Handle );
		}

		/// <summary>
		/// SysImageListをツリービューにに関連付ける
		/// </summary>
		/// <param name="treeView">TreeView control to associated ImageList with</param>
		/// <param name="sysImageList">System Image List to associate</param>
		/// <param name="forStateImages">Whether to add ImageList as StateImageList</param>
		public static void SetTreeViewImageList(
			TreeView treeView,
			SystemImageList sysImageList,
			bool forStateImages
			)
		{
			IntPtr wParam = (IntPtr)TVSIL_NORMAL;
			if( forStateImages )
			{
				wParam = (IntPtr)TVSIL_STATE;
			}
			Api.SendMessage(
				treeView.Handle,
				TVM_SETIMAGELIST,
				wParam,
				sysImageList.Handle );
		}

		#endregion
	}
	#endregion
}
