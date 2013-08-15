using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Win32
{
    public class SH
    {
		#region SHGetMalloc

		/*
		 * http://www.codeproject.com/Articles/3551/C-does-Shell-Part-1
		 * 
			// Get IMalloc interface
			IntPtr ptrRet;	
			SHGetMalloc(out ptrRet);

			System.Type mallocType = System.Type.GetType("IMalloc");
			Object obj = Marshal.GetTypedObjectForIUnknown(ptrRet,mallocType);
			IMalloc pMalloc = (IMalloc)obj;
 
			// Get a PIDL
			IntPtr pidlRoot;
			SHGetFolderLocation(IntPtr.zero,CSIDL_WINDOWS,IntPtr.Zero,0,out pidlRoot);
 
			// Use the IMalloc object to free PIDL
			if (pidlRoot != IntPtr.Zero)
				pMalloc.Free(pidlRoot);

			// Free the IMalloc object
			System.Runtime.InteropServices.Marshal.ReleaseComObject(pMalloc);
		 */

		[DllImport( "shell32.dll", CharSet = CharSet.Auto )]
		private static extern int SHGetMalloc( out IMalloc ppMalloc );

	    public static IMalloc GetMalloc()
	    {
		    IMalloc malloc;
		    SHGetMalloc(out malloc);
		    return malloc;
	    }

	    public static void FreeMalloc(IMalloc malloc)
	    {
			Marshal.ReleaseComObject(malloc);
	    }

	    #endregion


		#region Path <-> PIDL

		#region SHGetPathFromIDList : Converts an PIDL to a file system path.
		/*
			IntPtr pidlRoot;
			SHGetFolderLocation(IntPtr.zero,CSIDL_WINDOWS,IntPtr.Zero,0,out pidlRoot);

			System.Text.StringBuilder path = new System.Text.StringBuilder(MAX_PATH);
			SHGetPathFromIDList(pidlRoot,path); 
		 */
		[DllImport( "shell32.dll" )]
		public static extern Int32 SHGetPathFromIDList(
			IntPtr pidl,                // Address of an item identifier list that
			// specifies a file or directory location
			// relative to the root of the namespace (the desktop). 
			StringBuilder pszPath );     // Address of a buffer to receive the file system path.
		#endregion

		#region SHParseDisplayName : Translates a Shell namespace object's display name into an PIDL

		// Translates a Shell namespace object's display name into an item 
		// identifier list and returns the attributes of the object. This function is 
		// the preferred method to convert a string to a pointer to an item identifier 
		// list (PIDL). 
		[DllImport( "shell32.dll" )]
		public static extern Int32 SHParseDisplayName(
			[MarshalAs( UnmanagedType.LPWStr )]
			String pszName,					// Pointer to a zero-terminated wide string that
			// contains the display name to parse. 
			IntPtr pbc,						// Optional bind context that controls the parsing
			// operation. This parameter 
			// is normally set to NULL.
			out IntPtr ppidl,				// Address of a pointer to a variable of type
			// ITEMIDLIST that receives the item
			// identifier list for the object.
			UInt32 sfgaoIn,					// ULONG value that specifies the attributes to query.
			out UInt32 psfgaoOut );			// Pointer to a ULONG. On return, those attributes
		// that are true for the 
		// object and were requested in sfgaoIn will be set. 
		/*
			ShellLib.IMalloc pMalloc;
			pMalloc = ShellLib.ShellFunctions.GetMalloc();

			IntPtr pidlRoot;
			String sPath = @"c:\temp\divx";
			uint iAttribute;

			ShellLib.ShellApi.SHParseDisplayName(sPath,IntPtr.Zero,out pidlRoot,0,
				out iAttribute);

			if (pidlRoot != IntPtr.Zero)
				pMalloc.Free(pidlRoot);

			System.Runtime.InteropServices.Marshal.ReleaseComObject(pMalloc);
		 * 
		 * Explaining the code: Suppose you want a PIDL of the my documents folder,
		 * we already seen how this is done, we got a function called SHGetFolderLocation
		 * which return us all the PIDL's of the special folders. What if I want a PIDL 
		 * which represents C:\temp\Divx? in this case we will use the SHParseDisplayName 
		 * function. the example is quite simple, I set a string with the folder I want 
		 * and call the SHParseDisplayName, the result is return in the pidlRoot variable.
		 * and finally I'm not forgetting to free the PIDL memory when I finish using it.
		 */
		#endregion

		#endregion

		#region CSIDL & PIDL or Path : Takes the CSIDL of a folder and returns PIDL or PATH.

		#region CSIDL : const shell IDL
		/// <summary>
		/// <para>
		/// CSIDL values provide a unique system-independent way to identify special folders used frequently by applications, 
		/// but which may not have the same name or location on any given system. For example, the system folder may be 
		/// "C:\Windows" on one system and "C:\Winnt" on another. These constants are defined in Shlobj.h and Shfolder.h.
		/// </para>
		/// </summary>
		/// <remarks>
		/// <para>
		/// These values supersede the use of environment variables for this purpose.
		/// A CSIDL is used in conjunction with one of four Shell functions, SHGetFolderLocation, SHGetFolderPath, 
		/// SHGetSpecialFolderLocation, and SHGetSpecialFolderPath, to retrieve a special folder's path or pointer 
		/// to an item identifier list (PIDL).
		/// Combine CSIDL_FLAG_CREATE with any of the other CSIDLs to force the creation of the associated folder. 
		/// The remaining CSIDLs correspond to either file system folders or virtual folders. Where the CSIDL 
		/// identifies a file system folder, a commonly used path is given as an example. Other paths may be used. 
		/// Some CSIDLs can be mapped to an equivalent %VariableName% environment variable. CSIDLs are more reliable, 
		/// however, and should be used if possible.
		/// </para>
		/// </remarks>
		[Flags]
		public enum CSIDL : int
		{
			/// <summary>
			/// <para>
			/// Version 5.0. Combine this CSIDL with any of the following CSIDLs to force the creation of the associated folder. 
			/// </para>
			///</summary>
			FLAG_CREATE = 0x8000,

			/// <summary>
			/// <para>
			/// Version 5.0. The file system directory that is used to store administrative tools for an individual user. 
			/// The Microsoft Management Console (MMC) will save customized consoles to this directory, and it will roam with the user.
			/// </para>
			///</summary>
			ADMINTOOLS = 0x0030,

			/// <summary>
			/// <para>
			/// The file system directory that corresponds to the user's nonlocalized Startup program group.
			/// </para>
			///</summary>
			ALTSTARTUP = 0x001d,

			/// <summary>
			/// <para>
			/// Version 4.71. The file system directory that serves as a common repository for application-specific data. 
			/// A typical path is C:\Documents and Settings\username\Application Data. This CSIDL is supported by the 
			/// redistributable Shfolder.dll for systems that do not have the Microsoft Internet Explorer 4.0 integrated Shell installed.
			/// </para>
			///</summary>
			APPDATA = 0x001a,

			BITBUCKET = 0x000a, // The virtual folder containing the objects in the user's Recycle Bin.

			/// <summary>
			/// <para>
			/// Version 6.0. The file system directory acting as a staging area for files waiting to be written to CD. 
			/// A typical path is C:\Documents and Settings\username\Local Settings\Application Data\Microsoft\CD Burning.
			/// </para>
			///</summary>
			CDBURN_AREA = 0x003b,

			/// <summary>
			/// <para>
			/// Version 5.0. The file system directory containing administrative tools for all users of the computer.
			/// </para>
			///</summary>
			COMMON_ADMINTOOLS = 0x002f,

			/// <summary>
			/// <para>
			/// The file system directory that corresponds to the nonlocalized Startup program group for all users. 
			/// Valid only for Microsoft Windows NT systems.
			/// </para>
			///</summary>
			COMMON_ALTSTARTUP = 0x001e,

			/// <summary>
			/// <para>
			/// Version 5.0. The file system directory containing application data for all users. A typical path is 
			/// C:\Documents and Settings\All Users\Application Data.
			/// </para>
			///</summary>
			COMMON_APPDATA = 0x0023,

			/// <summary>
			/// <para>
			/// The file system directory that contains files and folders that appear on the desktop for all users. 
			/// A typical path is C:\Documents and Settings\All Users\Desktop. Valid only for Windows NT systems.
			/// </para>
			///</summary>
			COMMON_DESKTOPDIRECTORY = 0x0019,

			/// <summary>
			/// <para>
			/// The file system directory that contains documents that are common to all users. A typical paths is 
			/// C:\Documents and Settings\All Users\Documents. Valid for Windows NT systems and Microsoft Windows 95 
			/// and Windows 98 systems with Shfolder.dll installed.
			/// </para>
			///</summary>
			COMMON_DOCUMENTS = 0x002e,

			/// <summary>
			/// <para>
			/// The file system directory that serves as a common repository for favorite items common to all users. 
			/// Valid only for Windows NT systems.
			/// </para>
			///</summary>
			COMMON_FAVORITES = 0x001f,

			/// <summary>
			/// <para>
			/// Version 6.0. The file system directory that serves as a repository for music files common to all users. 
			/// A typical path is C:\Documents and Settings\All Users\Documents\My Music.
			/// </para>
			///</summary>
			COMMON_MUSIC = 0x0035,

			/// <summary>
			/// <para>
			/// Version 6.0. The file system directory that serves as a repository for image files common to all users. 
			/// A typical path is C:\Documents and Settings\All Users\Documents\My Pictures.
			/// </para>
			///</summary>
			COMMON_PICTURES = 0x0036,

			/// <summary>
			/// <para>
			/// The file system directory that contains the directories for the common program groups that appear on the 
			/// Start menu for all users. A typical path is C:\Documents and Settings\All Users\Start Menu\Programs. 
			/// Valid only for Windows NT systems.
			/// </para>
			///</summary>
			COMMON_PROGRAMS = 0x0017,

			/// <summary>
			/// <para>
			/// The file system directory that contains the programs and folders that appear on the Start menu for all users. 
			/// A typical path is C:\Documents and Settings\All Users\Start Menu. Valid only for Windows NT systems.
			/// </para>
			///</summary>
			COMMON_STARTMENU = 0x0016,

			/// <summary>
			/// <para>
			/// The file system directory that contains the programs that appear in the Startup folder for all users. 
			/// A typical path is C:\Documents and Settings\All Users\Start Menu\Programs\Startup. Valid only for Windows NT systems.
			/// </para>
			///</summary>
			COMMON_STARTUP = 0x0018,

			/// <summary>
			/// <para>
			/// The file system directory that contains the templates that are available to all users. A typical path is 
			/// C:\Documents and Settings\All Users\Templates. Valid only for Windows NT systems.
			/// </para>
			///</summary>
			COMMON_TEMPLATES = 0x002d,

			/// <summary>
			/// <para>
			/// Version 6.0. The file system directory that serves as a repository for video files common to all users. 
			/// A typical path is C:\Documents and Settings\All Users\Documents\My Videos.
			/// </para>
			///</summary>
			COMMON_VIDEO = 0x0037,

			CONTROLS = 0x0003, // The virtual folder containing icons for the Control Panel applications.

			/// <summary>
			/// <para>
			/// The file system directory that serves as a common repository for Internet cookies. A typical path is 
			/// C:\Documents and Settings\username\Cookies.
			/// </para>
			///</summary>
			COOKIES = 0x0021,

			DESKTOP = 0x0000, // The virtual folder representing the Windows desktop, the root of the namespace.

			/// <summary>
			/// <para>
			/// The file system directory used to physically store file objects on the desktop (not to be confused with 
			/// the desktop folder itself). A typical path is C:\Documents and Settings\username\Desktop.
			/// </para>
			///</summary>
			DESKTOPDIRECTORY = 0x0010,

			/// <summary>
			/// <para>
			/// The virtual folder representing My Computer, containing everything on the local computer: storage devices, 
			/// printers, and Control Panel. The folder may also contain mapped network drives.
			/// </para>
			///</summary>
			DRIVES = 0x0011,

			/// <summary>
			/// <para>
			/// The file system directory that serves as a common repository for the user's favorite items. A typical path is 
			/// C:\Documents and Settings\username\Favorites.
			/// </para>
			///</summary>
			FAVORITES = 0x0006,

			FONTS = 0x0014, // A virtual folder containing fonts. A typical path is C:\Windows\Fonts.

			HISTORY = 0x0022, // The file system directory that serves as a common repository for Internet history items.

			INTERNET = 0x0001, // A virtual folder representing the Internet.

			/// <summary>
			/// <para>
			/// Version 4.72. The file system directory that serves as a common repository for temporary Internet files. 
			/// A typical path is C:\Documents and Settings\username\Local Settings\Temporary Internet Files.
			/// </para>
			///</summary>
			INTERNET_CACHE = 0x0020,

			/// <summary>
			/// <para>
			/// Version 5.0. The file system directory that serves as a data repository for local (nonroaming) applications. 
			/// A typical path is C:\Documents and Settings\username\Local Settings\Application Data.
			/// </para>
			///</summary>
			LOCAL_APPDATA = 0x001c,

			MYDOCUMENTS = 0x000c, // Version 6.0. The virtual folder representing the My Documents desktop item.

			/// <summary>
			/// <para>
			/// The file system directory that serves as a common repository for music files. A typical path is 
			/// C:\Documents and Settings\User\My Documents\My Music.
			/// </para>
			///</summary>
			MYMUSIC = 0x000d,

			/// <summary>
			/// <para>
			/// Version 5.0. The file system directory that serves as a common repository for image files. 
			/// A typical path is C:\Documents and Settings\username\My Documents\My Pictures.
			/// </para>
			///</summary>
			MYPICTURES = 0x0027,

			/// <summary>
			/// <para>
			/// Version 6.0. The file system directory that serves as a common repository for video files. 
			/// A typical path is C:\Documents and Settings\username\My Documents\My Videos.
			/// </para>
			///</summary>
			MYVIDEO = 0x000e,

			/// <summary>
			/// <para>
			/// A file system directory containing the link objects that may exist in the My Network Places virtual folder. 
			/// It is not the same as CSIDL_NETWORK, which represents the network namespace root. 
			/// A typical path is C:\Documents and Settings\username\NetHood.
			/// </para>
			///</summary>
			NETHOOD = 0x0013,

			/// <summary>
			/// <para>
			/// A virtual folder representing Network Neighborhood, the root of the network namespace hierarchy.
			/// </para>
			///</summary>
			NETWORK = 0x0012,

			/// <summary>
			/// <para>
			/// Version 6.0. The virtual folder representing the My Documents desktop item. This is equivalent to CSIDL_MYDOCUMENTS. 
			/// Previous to Version 6.0. The file system directory used to physically store a user's common repository of documents. 
			/// A typical path is C:\Documents and Settings\username\My Documents. This should be distinguished from the virtual 
			/// My Documents folder in the namespace. To access that virtual folder, use SHGetFolderLocation, which returns the 
			/// ITEMIDLIST for the virtual location, or refer to the technique described in Managing the File System.
			/// </para>
			///</summary>
			PERSONAL = 0x0005,

			PRINTERS = 0x0004, // The virtual folder containing installed printers.

			/// <summary>
			/// <para>
			/// The file system directory that contains the link objects that can exist in the Printers virtual folder. 
			/// A typical path is C:\Documents and Settings\username\PrintHood.
			/// </para>
			///</summary>
			PRINTHOOD = 0x001b,

			/// <summary>
			/// <para>
			/// Version 5.0. The user's profile folder. A typical path is C:\Documents and Settings\username. Applications should 
			/// not create files or folders at this level; they should put their data under the locations referred to by 
			/// CSIDL_APPDATA or CSIDL_LOCAL_APPDATA.
			/// </para>
			///</summary>
			PROFILE = 0x0028,

			/// <summary>
			/// <para>
			/// Version 6.0. The file system directory containing user profile folders. A typical path is C:\Documents and Settings.
			/// </para>
			///</summary>
			PROFILES = 0x003e,

			PROGRAM_FILES = 0x0026, // Version 5.0. The Program Files folder. A typical path is C:\Program Files.

			/// <summary>
			/// <para>
			/// Version 5.0. A folder for components that are shared across applications. A typical path is C:\Program Files\Common. 
			/// Valid only for Windows NT, Windows 2000, and Windows XP systems. Not valid for Windows Millennium Edition (Windows Me).
			/// </para>
			///</summary>
			PROGRAM_FILES_COMMON = 0x002b,

			/// <summary>
			/// <para>
			/// The file system directory that contains the user's program groups (which are themselves file system directories). 
			/// A typical path is C:\Documents and Settings\username\Start Menu\Programs.
			/// </para>
			///</summary>
			PROGRAMS = 0x0002,

			/// <summary>
			/// <para>
			/// The file system directory that contains shortcuts to the user's most recently used documents. A typical path is 
			/// C:\Documents and Settings\username\My Recent Documents. To create a shortcut in this folder, use SHAddToRecentDocs. 
			/// In addition to creating the shortcut, this function updates the Shell's list of recent documents and adds the shortcut 
			/// to the My Recent Documents submenu of the Start menu.
			/// </para>
			///</summary>
			RECENT = 0x0008,

			/// <summary>
			/// <para>
			/// The file system directory that contains Send To menu items. A typical path is C:\Documents and Settings\username\SendTo.
			/// </para>
			///</summary>
			SENDTO = 0x0009,

			/// <summary>
			/// <para>
			/// The file system directory containing Start menu items. A typical path is C:\Documents and Settings\username\Start Menu.
			/// </para>
			///</summary>
			STARTMENU = 0x000b,

			/// <summary>
			/// <para>
			/// The file system directory that corresponds to the user's Startup program group. The system starts these programs 
			/// whenever any user logs onto Windows NT or starts Windows 95. 
			/// A typical path is C:\Documents and Settings\username\Start Menu\Programs\Startup.
			/// </para>
			///</summary>
			STARTUP = 0x0007,

			SYSTEM = 0x0025, // Version 5.0. The Windows System folder. A typical path is C:\Windows\System32.

			/// <summary>
			/// <para>
			/// The file system directory that serves as a common repository for document templates. A typical path is 
			/// C:\Documents and Settings\username\Templates.
			/// </para>
			///</summary>
			TEMPLATES = 0x0015,

			/// <summary>
			/// <para>
			/// Version 5.0. The Windows directory or SYSROOT. This corresponds to the %windir% or %SYSTEMROOT% environment variables. 
			/// A typical path is C:\Windows.
			/// </para>
			///</summary>
			WINDOWS = 0x0024,
		};
		#endregion

		#region SHGetFolderLocation : Takes the CSIDL of a folder and retrieves as an PIDL.

		[DllImport( "shell32.dll" )]
		public static extern Int32 SHGetFolderLocation(
			IntPtr hwndOwner,       // Handle to the owner window.
			Int32 nFolder,          // A CSIDL value that identifies the folder to be located.
			IntPtr hToken,          // Token that can be used to represent a particular user.
			UInt32 dwReserved,
			out IntPtr ppidl        // Address of a pointer to an item identifier list structure
		);							// specifying the folder's location relative to the
		// root of the namespace (the desktop).
		/*
			// Get a PIDL
			IntPtr pidlRoot;
			SHGetFolderLocation(IntPtr.zero,CSIDL_WINDOWS,IntPtr.Zero,0,out pidlRoot);
		*/
		#endregion

		#region SHGetFolderPath : Takes the CSIDL of a folder and returns the pathname.

		[DllImport( "shell32.dll" )]
		public static extern Int32 SHGetFolderPath(
			IntPtr hwndOwner,				// Handle to an owner window.
			Int32 nFolder,					// A CSIDL value that identifies the folder whose
											// path is to be retrieved.
			IntPtr hToken,					// An access token that can be used to represent
											// a particular user.
			UInt32 dwFlags,					// Flags to specify which path is to be returned.
												// It is used for cases where 
											// the folder associated with a CSIDL may be moved
											// or renamed by the user. 
			StringBuilder pszPath );        // Pointer to a null-terminated string which will
											// receive the path.
		/*
			System.Text.StringBuilder path = new System.Text.StringBuilder(MAX_PATH);
			SHGetFolderPath(IntPtr.Zero,CSIDL_WINDOWS,IntPtr.Zero,SHGFP_TYPE_CURRENT,path);
		 */
		#endregion

		#endregion

		#region IShellFolder

		/// <summary>
		///  managed equivalent of IShellFolder interface
		///  Pinvoke.net / Mod by Arik Poznanski - pooya parsa
		///  Msdn:      http://msdn.microsoft.com/en-us/library/windows/desktop/bb775075(v=vs.85).aspx
		///  Pinvoke:   http://pinvoke.net/default.aspx/Interfaces/IShellFolder.html
		/// </summary>
		[ComImport]
		[InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
		[Guid( "000214E6-0000-0000-C000-000000000046" )]
		public interface IShellFolder
		{
			/// <summary>
			/// Translates a file object's or folder's display name into an item identifier list.
			/// Return value: error code, if any
			/// </summary>
			/// <param name="hwnd">Optional window handle</param>
			/// <param name="pbc">Optional bind context that controls the parsing operation. This parameter is normally set to NULL. </param>
			/// <param name="pszDisplayName">Null-terminated UNICODE string with the display name</param>
			/// <param name="pchEaten">Pointer to a ULONG value that receives the number of characters of the display name that was parsed.</param>
			/// <param name="ppidl"> Pointer to an ITEMIDLIST pointer that receives the item identifier list for the object.</param>
			/// <param name="pdwAttributes">Optional parameter that can be used to query for file attributes.this can be values from the SFGAO enum</param>
			void ParseDisplayName( IntPtr hwnd, IntPtr pbc, String pszDisplayName, ref UInt32 pchEaten, out IntPtr ppidl,
				ref UInt32 pdwAttributes );

			/// <summary>
			///Allows a client to determine the contents of a folder by creating an item identifier enumeration object and returning its IEnumIDList interface. 
			///Return value: error code, if any
			/// </summary>
			/// <param name="hwnd">If user input is required to perform the enumeration, this window handle should be used by the enumeration object as the parent window to take user input.</param>
			/// <param name="grfFlags">Flags indicating which items to include in the  enumeration. For a list of possible values, see the SHCONTF enum. </param>
			/// <param name="ppenumIDList">Address that receives a pointer to the IEnumIDList interface of the enumeration object created by this method. </param>
			void EnumObjects( IntPtr hwnd, SHCONTF grfFlags, out IntPtr ppenumIDList );

			/// <summary>
			///Retrieves an IShellFolder object for a subfolder.
			// Return value: error code, if any
			/// </summary>
			/// <param name="pidl">Address of an ITEMIDLIST structure (PIDL) that identifies the subfolder.</param>
			/// <param name="pbc">Optional address of an IBindCtx interface on a bind context object to be used during this operation.</param>
			/// <param name="riid">Identifier of the interface to return. </param>
			/// <param name="ppv">Address that receives the interface pointer.</param>
			void BindToObject( IntPtr pidl, IntPtr pbc, [In] ref Guid riid, out IntPtr ppv );

			/// <summary>
			/// Requests a pointer to an object's storage interface. 
			/// Return value: error code, if any
			/// </summary>
			/// <param name="pidl">Address of an ITEMIDLIST structure that identifies the subfolder relative to its parent folder. </param>
			/// <param name="pbc">Optional address of an IBindCtx interface on a bind context object to be  used during this operation.</param>
			/// <param name="riid">Interface identifier (IID) of the requested storage interface.</param>
			/// <param name="ppv"> Address that receives the interface pointer specified by riid.</param>
			void BindToStorage( IntPtr pidl, IntPtr pbc, [In] ref Guid riid, out IntPtr ppv );

			/// <summary>
			/// Determines the relative order of two file objects or folders, given 
			/// their item identifier lists. Return value: If this method is 
			/// successful, the CODE field of the HRESULT contains one of the 
			/// following values (the code can be retrived using the helper function
			/// GetHResultCode): Negative A negative return value indicates that the first item should precede the second (pidl1 < pidl2). 
			//// 
			///Positive A positive return value indicates that the first item should
			///follow the second (pidl1 > pidl2).  Zero A return value of zero
			///indicates that the two items are the same (pidl1 = pidl2). 
			/// </summary>
			/// <param name="lParam">Value that specifies how the comparison  should be performed. The lower Sixteen bits of lParam define the sorting  rule. 
			///  The upper sixteen bits of lParam are used for flags that modify the sorting rule. values can be from  the SHCIDS enum
			/// </param>
			/// <param name="pidl1">Pointer to the first item's ITEMIDLIST structure.</param>
			/// <param name="pidl2"> Pointer to the second item's ITEMIDLIST structure.</param>
			/// <returns></returns>
			[PreserveSig]
			Int32 CompareIDs( Int32 lParam, IntPtr pidl1, IntPtr pidl2 );

			/// <summary>
			/// Requests an object that can be used to obtain information from or interact
			/// with a folder object.
			/// Return value: error code, if any
			/// </summary>
			/// <param name="hwndOwner">Handle to the owner window.</param>
			/// <param name="riid">Identifier of the requested interface.</param>
			/// <param name="ppv">Address of a pointer to the requested interface. </param>
			void CreateViewObject( IntPtr hwndOwner, [In] ref Guid riid, out IntPtr ppv );

			/// <summary>
			/// Retrieves the attributes of one or more file objects or subfolders. 
			/// Return value: error code, if any
			/// </summary>
			/// <param name="cidl">Number of file objects from which to retrieve attributes. </param>
			/// <param name="apidl">Address of an array of pointers to ITEMIDLIST structures, each of which  uniquely identifies a file object relative to the parent folder.</param>
			/// <param name="rgfInOut">Address of a single ULONG value that, on entry contains the attributes that the caller is 
			/// requesting. On exit, this value contains the requested attributes that are common to all of the specified objects. this value can be from the SFGAO enum
			/// </param>
			void GetAttributesOf( UInt32 cidl, [MarshalAs( UnmanagedType.LPArray, SizeParamIndex = 0 )] IntPtr[] apidl,
				 ref SFGAO rgfInOut );

			/// <summary>
			/// Retrieves an OLE interface that can be used to carry out actions on the 
			/// specified file objects or folders. Return value: error code, if any
			/// </summary>
			/// <param name="hwndOwner">Handle to the owner window that the client should specify if it displays a dialog box or message box.</param>
			/// <param name="cidl">Number of file objects or subfolders specified in the apidl parameter. </param>
			/// <param name="apidl">Address of an array of pointers to ITEMIDLIST  structures, each of which  uniquely identifies a file object or subfolder relative to the parent folder.</param>
			/// <param name="riid">Identifier of the COM interface object to return.</param>
			/// <param name="rgfReserved"> Reserved. </param>
			/// <param name="ppv">Pointer to the requested interface.</param>
			void GetUIObjectOf( IntPtr hwndOwner, UInt32 cidl,
				[MarshalAs( UnmanagedType.LPArray, SizeParamIndex = 1 )] IntPtr[] apidl, [In] ref Guid riid,
				  UInt32 rgfReserved, out IntPtr ppv );

			/// <summary>
			/// Retrieves the display name for the specified file object or subfolder. 
			/// Return value: error code, if any
			/// </summary>
			/// <param name="pidl">Address of an ITEMIDLIST structure (PIDL)  that uniquely identifies the file  object or subfolder relative to the parent  folder. </param>
			/// <param name="uFlags">Flags used to request the type of display name to return. For a list of possible values. </param>
			/// <param name="pName"> Address of a STRRET structure in which to return the display name.</param>
			void GetDisplayNameOf( IntPtr pidl, SHGDN uFlags, out STRRET pName );

			/// <summary>
			/// Sets the display name of a file object or subfolder, changing the item
			/// identifier in the process.
			/// Return value: error code, if any
			/// </summary>
			/// <param name="hwnd"> Handle to the owner window of any dialog or message boxes that the client displays.</param>
			/// <param name="pidl"> Pointer to an ITEMIDLIST structure that uniquely identifies the file object or subfolder relative to the parent folder. </param>
			/// <param name="pszName"> Pointer to a null-terminated string that specifies the new display name.</param>
			/// <param name="uFlags">Flags indicating the type of name specified by  the lpszName parameter. For a list of possible values, see the description of the SHGNO enum.</param>
			/// <param name="ppidlOut"></param>
			void SetNameOf( IntPtr hwnd, IntPtr pidl, String pszName, SHCONTF uFlags, out IntPtr ppidlOut );
		}

		public enum SFGAO : uint
		{

			CANCOPY = 0x1,                // Objects can be copied    (DROPEFFECT_COPY)
			CANMOVE = 0x2,                // Objects can be moved     (DROPEFFECT_MOVE)
			CANLINK = 0x4,                // Objects can be linked    (DROPEFFECT_LINK)
			STORAGE = 0x00000008,         // supports BindToObject(IID_IStorage)
			CANRENAME = 0x00000010,         // Objects can be renamed
			CANDELETE = 0x00000020,         // Objects can be deleted
			HASPROPSHEET = 0x00000040,         // Objects have property sheets
			DROPTARGET = 0x00000100,         // Objects are drop target
			CAPABILITYMASK = 0x00000177,
			ENCRYPTED = 0x00002000,         // object is encrypted (use alt color)
			ISSLOW = 0x00004000,         // 'slow' object
			GHOSTED = 0x00008000,         // ghosted icon
			LINK = 0x00010000,         // Shortcut (link)
			SHARE = 0x00020000,         // shared
			READONLY = 0x00040000,         // read-only
			HIDDEN = 0x00080000,         // hidden object
			DISPLAYATTRMASK = 0x000FC000,
			FILESYSANCESTOR = 0x10000000,         // may contain children with FILESYSTEM
			FOLDER = 0x20000000,         // support BindToObject(IID_IShellFolder)
			FILESYSTEM = 0x40000000,         // is a win32 file system object (file/folder/root)
			HASSUBFOLDER = 0x80000000,         // may contain children with FOLDER
			CONTENTSMASK = 0x80000000,
			VALIDATE = 0x01000000,         // invalidate cached information
			REMOVABLE = 0x02000000,         // is this removeable media?
			COMPRESSED = 0x04000000,         // Object is compressed (use alt color)
			BROWSABLE = 0x08000000,         // supports IShellFolder, but only implements CreateViewObject() (non-folder view)
			NONENUMERATED = 0x00100000,         // is a non-enumerated object
			NEWCONTENT = 0x00200000,         // should show bold in explorer tree
			CANMONIKER = 0x00400000,         // defunct
			HASSTORAGE = 0x00400000,         // defunct
			STREAM = 0x00400000,         // supports BindToObject(IID_IStream)
			STORAGEANCESTOR = 0x00800000,         // may contain children with STORAGE or STREAM
			STORAGECAPMASK = 0x70C50008,         // for determining storage capabilities, ie for open/save semantics


		}

		public enum SHCONTF
		{
			SHCONTF_FOLDERS = 0x0020,
			SHCONTF_NONFOLDERS = 0x0040,
			SHCONTF_INCLUDEHIDDEN = 0x0080,
			SHCONTF_INIT_ON_FIRST_NEXT = 0x0100,
			SHCONTF_NETPRINTERSRCH = 0x0200,
			SHCONTF_SHAREABLE = 0x0400,
			SHCONTF_STORAGE = 0x0800
		}

		public enum SHGDN
		{
			SHGDN_NORMAL = 0x0000,
			SHGDN_INFOLDER = 0x0001,
			SHGDN_FOREDITING = 0x1000,
			SHGDN_FORADDRESSBAR = 0x4000,
			SHGDN_FORPARSING = 0x8000,
		}

		public enum ESTRRET : int
		{
			eeRRET_WSTR = 0x0000, // Use STRRET.pOleStr
			STRRET_OFFSET = 0x0001, // Use STRRET.uOffset to Ansi
			STRRET_CSTR = 0x0002 // Use STRRET.cStr
		}

		// this works too...from Unions.cs
		[StructLayout( LayoutKind.Explicit, Size = 520 )]
		public struct STRRETinternal
		{
			[FieldOffset( 0 )]
			public IntPtr pOleStr;

			[FieldOffset( 0 )]
			public IntPtr pStr; // LPSTR pStr;   NOT USED

			[FieldOffset( 0 )]
			public uint uOffset;
		}

		[StructLayout( LayoutKind.Sequential )]
		public struct STRRET
		{
			public uint uType;
			public STRRETinternal data;
		}

		public class Guid_IShellFolder
		{
			public static Guid IID_IShellFolder = new Guid( "{000214E6-0000-0000-C000-000000000046}" );
		}

		/*
			int retVal;

			ShellLib.IMalloc pMalloc;
			pMalloc = ShellLib.ShellFunctions.GetMalloc();

			IntPtr pidlSystem;
			retVal = ShellLib.ShellApi.SHGetFolderLocation(
							IntPtr.Zero,
							(int)ShellLib.ShellApi.CSIDL.CSIDL_SYSTEM,
							IntPtr.Zero,
							0,
							out pidlSystem);

			IntPtr ptrParent;
			IntPtr pidlRelative = IntPtr.Zero;
			retVal = ShellLib.ShellApi.SHBindToParent(
							pidlSystem,
							ShellLib.ShellGUIDs.IID_IShellFolder,
							out ptrParent,
							ref pidlRelative);

			System.Type shellFolderType = ShellLib.ShellFunctions.GetShellFolderType();
			Object obj = System.Runtime.InteropServices.Marshal.GetTypedObjectForIUnknown(
				ptrParent,shellFolderType);
			ShellLib.IShellFolder ishellParent = (ShellLib.IShellFolder)obj;

			ShellLib.ShellApi.STRRET ptrString;
			retVal = ishellParent.GetDisplayNameOf(pidlRelative,
				(uint)ShellLib.ShellApi.SHGNO.SHGDN_NORMAL, out ptrString);

			System.Text.StringBuilder strDisplay = new System.Text.StringBuilder(256);
			retVal = ShellLib.ShellApi.StrRetToBuf(ref ptrString ,pidlSystem,strDisplay,
				(uint)strDisplay.Capacity);

			System.Runtime.InteropServices.Marshal.ReleaseComObject(ishellParent);
			pMalloc.Free(pidlSystem);
			System.Runtime.InteropServices.Marshal.ReleaseComObject(pMalloc); 		
		 */
		#endregion

		#region SHGetDesktopFolder : Retrieves the IShellFolder I/F for the desktop

		// Retrieves the IShellFolder interface for the desktop folder,
		// which is the root of the Shell's namespace. 
		[DllImport( "shell32.dll", CharSet = CharSet.Auto )]
		public static extern Int32 SHGetDesktopFolder(
			out IntPtr ppshf );			// Address that receives an IShellFolder interface
										// pointer for the desktop folder.

		//[DllImport("shell32.dll", CharSet = CharSet.Auto)]
		//public static extern int SHGetDesktopFolder(ref IShellFolder ppshf);

		public static IShellFolder GetDesktopFolder( )
		{

			IntPtr ptrRet;
			SH.SHGetDesktopFolder( out ptrRet );

			Object obj = Marshal.GetTypedObjectForIUnknown( ptrRet, typeof( SH.IShellFolder ) );
			IShellFolder ishellFolder = (IShellFolder)obj;

			return ishellFolder;

		}

		/*
			public static IShellFolder GetDesktopFolder()
			{    
				IntPtr ptrRet;
				ShellApi.SHGetDesktopFolder(out ptrRet);

				System.Type shellFolderType = System.Type.GetType("ShellLib.IShellFolder");
				Object obj = Marshal.GetTypedObjectForIUnknown(ptrRet,shellFolderType);
				IShellFolder ishellFolder = (IShellFolder)obj;

				return ishellFolder;
			}

			{ 
				... 

				ShellLib.IShellFolder pShellFolder;
				pShellFolder = ShellLib.ShellFunctions.GetDesktopFolder();

				IntPtr pidlRoot;
				ShellLib.ShellApi.SHGetFolderLocation(
											IntPtr.Zero,
											(short)ShellLib.ShellApi.CSIDL.CSIDL_SYSTEM,
											IntPtr.Zero,
											0,
											out pidlRoot);

				ShellLib.ShellApi.STRRET ptrDisplayName;
				pShellFolder.GetDisplayNameOf(
									pidlRoot,
									(uint)ShellLib.ShellApi.SHGNO.SHGDN_NORMAL 
									| (uint)ShellLib.ShellApi.SHGNO.SHGDN_FORPARSING,
									out ptrDisplayName);

				String sDisplay;
				ShellLib.ShellApi.StrRetToBSTR(ref ptrDisplayName,pidlRoot,out sDisplay);

				System.Runtime.InteropServices.Marshal.ReleaseComObject(pShellFolder);
			}
		 */
		#endregion

		#region SHBindToParent
		// This function takes the fully-qualified pointer to an item
		// identifier list (PIDL) of a namespace object, and returns a specified
		// interface pointer on the parent object.
		[DllImport( "shell32.dll" )]
		public static extern Int32 SHBindToParent(
			IntPtr pidl,            // The item's PIDL. 
			[MarshalAs( UnmanagedType.LPStruct )]
		    Guid riid,                  // The REFIID of one of the interfaces exposed by
										// the item's parent object. 
			out IntPtr ppv,				// A pointer to the interface specified by riid. You
										// must release the object when 
										// you are finished. 
			ref IntPtr ppidlLast );		// The item's PIDL relative to the parent folder. This
										// PIDL can be used with many
										// of the methods supported by the parent folder's
										// interfaces. If you set ppidlLast 
										// to NULL, the PIDL will not be returned. 
		#endregion

		#region StrRetToBSTR
		// Accepts a STRRET structure returned by
		// ShellFolder::GetDisplayNameOf that contains or points to a string, and then
		// returns that string as a BSTR.
		[DllImport( "shlwapi.dll" )]
		public static extern Int32 StrRetToBSTR(
			ref STRRET pstr,		// Pointer to a STRRET structure.
			IntPtr pidl,			// Pointer to an ITEMIDLIST uniquely identifying a file
									// object or subfolder relative
									// to the parent folder.
			[MarshalAs( UnmanagedType.BStr )]
			out String pbstr );		// Pointer to a variable of type BSTR that contains the
									// converted string.
		#endregion

		#region StrRetToBuf
		// Takes a STRRET structure returned by IShellFolder::GetDisplayNameOf,
		// converts it to a string, and places the result in a buffer. 
		[DllImport( "shlwapi.dll" )]
		public static extern Int32 StrRetToBuf(
			ref STRRET pstr,		// Pointer to the STRRET structure. When the function
									// returns, this pointer will no
									// longer be valid.
			IntPtr pidl,			// Pointer to the item's ITEMIDLIST structure.
			StringBuilder pszBuf,	// Buffer to hold the display name. It will be returned
									// as a null-terminated
									// string. If cchBuf is too small, the name will be
									// truncated to fit. 
			UInt32 cchBuf );        // Size of pszBuf, in characters. If cchBuf is too small,
									// the string will be 
									// truncated to fit. 
		#endregion




		#region SHGetSpecialFolderLocation

		[DllImport( "shell32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		public static extern int SHGetSpecialFolderLocation( IntPtr hwndOwner, CSIDL nFolder, ref IntPtr ppidl );

		#endregion





		#region BrowseForFolderDialog

		[Flags]
		public enum BIF : uint
		{
			/// <summary>
			/// Only return file system directories. 
			/// 
			/// If the user selects folders that are not part of the file system, 
			/// the OK button is grayed. 
			/// </summary>
			RETURNONLYFSDIRS = 0x00000001,
			/// <summary>
			/// Do not include network folders below the domain level in the dialog box's tree view control.
			/// </summary>
			DONTGOBELOWDOMAIN = 0x00000002,
			/// <summary>
			/// Include a status area in the dialog box. 
			/// The callback function can set the status text by sending messages to the dialog box. 
			/// This flag is not supported when <bold>BIF_NEWDIALOGSTYLE</bold> is specified
			/// </summary>
			STATUSTEXT = 0x00000004,
			/// <summary>
			/// Only return file system ancestors. 
			/// An ancestor is a subfolder that is beneath the root folder in the namespace hierarchy. 
			/// If the user selects an ancestor of the root folder that is not part of the file system, the OK button is grayed
			/// </summary>
			RETURNFSANCESTORS = 0x00000008,
			/// <summary>
			/// Include an edit control in the browse dialog box that allows the user to type the name of an item.
			/// </summary>
			EDITBOX = 0x00000010,
			/// <summary>
			/// If the user types an invalid name into the edit box, the browse dialog box calls the application's BrowseCallbackProc with the BFFM_VALIDATEFAILED message. 
			/// This flag is ignored if <bold>BIF_EDITBOX</bold> is not specified.
			/// </summary>
			VALIDATE = 0x00000020,
			/// <summary>
			/// Use the new user interface. 
			/// Setting this flag provides the user with a larger dialog box that can be resized. 
			/// The dialog box has several new capabilities, including: drag-and-drop capability within the 
			/// dialog box, reordering, shortcut menus, new folders, delete, and other shortcut menu commands. 
			/// </summary>
			NEWDIALOGSTYLE = 0x00000040,
			/// <summary>
			/// The browse dialog box can display URLs. The <bold>BIF_USENEWUI</bold> and <bold>BIF_BROWSEINCLUDEFILES</bold> flags must also be set. 
			/// If any of these three flags are not set, the browser dialog box rejects URLs.
			/// </summary>
			BROWSEINCLUDEURLS = 0x00000080,
			/// <summary>
			/// Use the new user interface, including an edit box. This flag is equivalent to <bold>BIF_EDITBOX | BIF_NEWDIALOGSTYLE</bold>
			/// </summary>
			USENEWUI = ( EDITBOX | NEWDIALOGSTYLE ),
			/// <summary>
			/// hen combined with <bold>BIF_NEWDIALOGSTYLE</bold>, adds a usage hint to the dialog box, in place of the edit box. <bold>BIF_EDITBOX</bold> overrides this flag.
			/// </summary>
			UAHINT = 0x00000100,
			/// <summary>
			/// Do not include the New Folder button in the browse dialog box.
			/// </summary>
			NONEWFOLDERBUTTON = 0x00000200,
			/// <summary>
			/// When the selected item is a shortcut, return the PIDL of the shortcut itself rather than its target.
			/// </summary>
			NOTRANSLATETARGETS = 0x00000400,
			/// <summary>
			/// Only return computers. If the user selects anything other than a computer, the OK button is grayed.
			/// </summary>
			BROWSEFORCOMPUTER = 0x00001000,
			/// <summary>
			/// Only allow the selection of printers. If the user selects anything other than a printer, the OK button is grayed
			/// </summary>
			BROWSEFORPRINTER = 0x00002000,
			/// <summary>
			/// The browse dialog box displays files as well as folders.
			/// </summary>
			BROWSEINCLUDEFILES = 0x00004000,
			/// <summary>
			/// The browse dialog box can display shareable resources on remote systems.
			/// </summary>
			SHAREABLE = 0x00008000,
			/// <summary>
			/// Allow folder junctions such as a library or a compressed file with a .zip file name extension to be browsed.
			/// </summary>
			BROWSEFILEJUNCTIONS = 0x00010000,
		};

		public enum BFFM : uint
		{
			INITIALIZED = 1,
			SELCHANGED = 2,
			VALIDATEFAILED = 3,

			SETSTATUSTEXTA = ( (uint)Win32.WM.USER + 100 ),
			SETSTATUSTEXTW = ( (uint)Win32.WM.USER + 104 ),
			ENABLEOK = ( (uint)Win32.WM.USER + 101 ),
			SETSELECTION = ( (uint)Win32.WM.USER + 102 ),
		};

		public delegate int BFFCALLBACK( IntPtr hwnd, UInt32 uMsg, IntPtr lParam, IntPtr lpData );

		[StructLayout( LayoutKind.Sequential, Pack = 8 )]
		public struct BROWSEINFO
		{
			public IntPtr hwndOwner;
			public IntPtr pidlRoot;
			public IntPtr pszDisplayName;
			[MarshalAs( UnmanagedType.LPTStr )]
			public string lpszTitle;
			public int ulFlags;
			[MarshalAs( UnmanagedType.FunctionPtr )]
			public BFFCALLBACK lpfn;
			public UInt32 lParam;
			public int iImage;
		}

		[DllImport( "shell32.dll", CharSet = CharSet.Auto )]
		public static extern IntPtr SHBrowseForFolder( ref BROWSEINFO bi );

		#endregion



		[StructLayout( LayoutKind.Sequential, CharSet = CharSet.Unicode )]
        public struct SHITEMID
        {
              public ushort cb;
              public byte[] abID;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
        public struct ITEMIDLIST
        {
            [MarshalAs(UnmanagedType.Struct)]
            public SHITEMID mkid;
        }








        /// <summary>
        /// Converts an item identifier list to a file system path. (Note: SHGetPathFromIDList calls the ANSI version, must call SHGetPathFromIDListW for .NET)
        /// </summary>
        /// <param name="pidl">Address of an item identifier list that specifies a file or directory location relative to the root of the namespace (the desktop).</param>
        /// <param name="pszPath">Address of a buffer to receive the file system path. This buffer must be at least MAX_PATH characters in size.</param>
        /// <returns>Returns TRUE if successful, or FALSE otherwise. </returns>
        [DllImport("shell32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SHGetPathFromIDListW(IntPtr pidl, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder pszPath);





		#region SHGetFileInfo

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

	    [Flags]
		public enum SHGFI : uint
	    {
		    ICON = 0x000000100,
		    DISPLAYNAME = 0x000000200,
		    TYPENAME = 0x000000400,
		    ATTRIBUTES = 0x000000800,
		    ICONLOCATION = 0x000001000,
		    EXETYPE = 0x000002000,
		    SYSICONINDEX = 0x000004000,
		    LINKOVERLAY = 0x000008000,
		    SELECTED = 0x000010000,
		    ATTR_SPECIFIED = 0x000020000,
		    LARGEICON = 0x000000000,
		    SMALLICON = 0x000000001,
		    OPENICON = 0x000000002,
		    SHELLICONSIZE = 0x000000004,
		    PIDL = 0x000000008,
		    USEFILEATTRIBUTES = 0x000000010,
		    ADDOVERLAYS = 0x000000020,
		    OVERLAYINDEX = 0x000000040
	    };

		public const uint FILE_ATTRIBUTE_NORMAL = 0x80;
		public const uint FILE_ATTRIBUTE_DIRECTORY = 0x10;

		[DllImport( "shell32.dll", CharSet = CharSet.Auto )]
		public static extern IntPtr SHGetFileInfo( string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags );

		[DllImport( "shell32.dll", CharSet = CharSet.Auto )]
		public static extern IntPtr SHGetFileInfo( IntPtr pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags );

		#endregion


		#region SHFileOperation

		public enum FileFuncFlags : uint
		{
			FO_MOVE = 0x1,
			FO_COPY = 0x2,
			FO_DELETE = 0x3,
			FO_RENAME = 0x4
		}

		[Flags]
		public enum FILEOP_FLAGS : ushort
		{
			FOF_MULTIDESTFILES = 0x1,
			FOF_CONFIRMMOUSE = 0x2,
			/// <summary>
			/// Don't create progress/report
			/// </summary>
			FOF_SILENT = 0x4,
			FOF_RENAMEONCOLLISION = 0x8,
			/// <summary>
			/// Don't prompt the user.
			/// </summary>
			FOF_NOCONFIRMATION = 0x10,
			/// <summary>
			/// Fill in SHFILEOPSTRUCT.hNameMappings.
			/// Must be freed using SHFreeNameMappings
			/// </summary>
			FOF_WANTMAPPINGHANDLE = 0x20,
			FOF_ALLOWUNDO = 0x40,
			/// <summary>
			/// On *.*, do only files
			/// </summary>
			FOF_FILESONLY = 0x80,
			/// <summary>
			/// Don't show names of files
			/// </summary>
			FOF_SIMPLEPROGRESS = 0x100,
			/// <summary>
			/// Don't confirm making any needed dirs
			/// </summary>
			FOF_NOCONFIRMMKDIR = 0x200,
			/// <summary>
			/// Don't put up error UI
			/// </summary>
			FOF_NOERRORUI = 0x400,
			/// <summary>
			/// Dont copy NT file Security Attributes
			/// </summary>
			FOF_NOCOPYSECURITYATTRIBS = 0x800,
			/// <summary>
			/// Don't recurse into directories.
			/// </summary>
			FOF_NORECURSION = 0x1000,
			/// <summary>
			/// Don't operate on connected elements.
			/// </summary>
			FOF_NO_CONNECTED_ELEMENTS = 0x2000,
			/// <summary>
			/// During delete operation, 
			/// warn if nuking instead of recycling (partially overrides FOF_NOCONFIRMATION)
			/// </summary>
			FOF_WANTNUKEWARNING = 0x4000,
			/// <summary>
			/// Treat reparse points as objects, not containers
			/// </summary>
			FOF_NORECURSEREPARSE = 0x8000
		}

		//[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
		//If you use the above you may encounter an invalid memory access exception (when using ANSI
		//or see nothing (when using unicode) when you use FOF_SIMPLEPROGRESS flag.
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct SHFILEOPSTRUCT
		{
			public IntPtr hwnd;
			public FileFuncFlags wFunc;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pFrom;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pTo;
			public FILEOP_FLAGS fFlags;
			[MarshalAs(UnmanagedType.Bool)]
			public bool fAnyOperationsAborted;
			public IntPtr hNameMappings;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpszProgressTitle;
		}

		[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
		public static extern int SHFileOperation([In] ref SHFILEOPSTRUCT lpFileOp);
		#endregion


		#region SHGetImageList

		/// <summary>
	    /// SHGetImageList is not exported correctly in XP.  See KB316931
	    /// http://support.microsoft.com/default.aspx?scid=kb;EN-US;Q316931
	    /// Apparently (and hopefully) ordinal 727 isn't going to change.
	    /// </summary>
	    [DllImport("shell32.dll", EntryPoint = "#727")]
	    public static extern int SHGetImageList(
		    int iImageList,
		    ref Guid riid,
		    ref IImageList ppv
		    );

	    [DllImport("shell32.dll", EntryPoint = "#727")]
	    public static extern int SHGetImageListHandle(
		    int iImageList,
		    ref Guid riid,
		    ref IntPtr handle
		    );

	    #endregion


	}

}
