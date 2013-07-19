using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Win32
{
    public class SH
    {
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
            USENEWUI = (EDITBOX | NEWDIALOGSTYLE),
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

            SETSTATUSTEXTA = ((uint)Win32.WM.USER + 100),
            SETSTATUSTEXTW = ((uint)Win32.WM.USER + 104),
            ENABLEOK = ((uint)Win32.WM.USER + 101),
            SETSELECTION = ((uint)Win32.WM.USER + 102),
        };


        public delegate int BFFCALLBACK(IntPtr hwnd, UInt32 uMsg, IntPtr lParam, IntPtr lpData);

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
            public UInt32 lParam;
            public int iImage;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHGetMalloc(out IMalloc ppMalloc);

        public static IMalloc GetMalloc()
        {
            IMalloc malloc;
            SHGetMalloc(out malloc);
            return malloc;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
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




        #region CSIDL
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

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SHGetSpecialFolderLocation(IntPtr hwndOwner, CSIDL nFolder, ref IntPtr ppidl);





        /// <summary>
        /// Converts an item identifier list to a file system path. (Note: SHGetPathFromIDList calls the ANSI version, must call SHGetPathFromIDListW for .NET)
        /// </summary>
        /// <param name="pidl">Address of an item identifier list that specifies a file or directory location relative to the root of the namespace (the desktop).</param>
        /// <param name="pszPath">Address of a buffer to receive the file system path. This buffer must be at least MAX_PATH characters in size.</param>
        /// <returns>Returns TRUE if successful, or FALSE otherwise. </returns>
        [DllImport("shell32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SHGetPathFromIDListW(IntPtr pidl, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder pszPath);


        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SHBrowseForFolder(ref BROWSEINFO bi);

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

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SHGetFileInfo( string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags );

		[DllImport( "shell32.dll", CharSet = CharSet.Auto )]
		public static extern IntPtr SHGetFileInfo( IntPtr pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags );

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int SHGetDesktopFolder(ref IShellFolder ppshf);

        #region IShellFolder

        /// <summary>
        ///  managed equivalent of IShellFolder interface
        ///  Pinvoke.net / Mod by Arik Poznanski - pooya parsa
        ///  Msdn:      http://msdn.microsoft.com/en-us/library/windows/desktop/bb775075(v=vs.85).aspx
        ///  Pinvoke:   http://pinvoke.net/default.aspx/Interfaces/IShellFolder.html
        /// </summary>
        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214E6-0000-0000-C000-000000000046")]
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
            void ParseDisplayName(IntPtr hwnd, IntPtr pbc, String pszDisplayName, ref UInt32 pchEaten, out IntPtr ppidl,
                ref UInt32 pdwAttributes);

            /// <summary>
            ///Allows a client to determine the contents of a folder by creating an item identifier enumeration object and returning its IEnumIDList interface. 
            ///Return value: error code, if any
            /// </summary>
            /// <param name="hwnd">If user input is required to perform the enumeration, this window handle should be used by the enumeration object as the parent window to take user input.</param>
            /// <param name="grfFlags">Flags indicating which items to include in the  enumeration. For a list of possible values, see the SHCONTF enum. </param>
            /// <param name="ppenumIDList">Address that receives a pointer to the IEnumIDList interface of the enumeration object created by this method. </param>
            void EnumObjects(IntPtr hwnd, ESHCONTF grfFlags, out IntPtr ppenumIDList);

            /// <summary>
            ///Retrieves an IShellFolder object for a subfolder.
            // Return value: error code, if any
            /// </summary>
            /// <param name="pidl">Address of an ITEMIDLIST structure (PIDL) that identifies the subfolder.</param>
            /// <param name="pbc">Optional address of an IBindCtx interface on a bind context object to be used during this operation.</param>
            /// <param name="riid">Identifier of the interface to return. </param>
            /// <param name="ppv">Address that receives the interface pointer.</param>
            void BindToObject(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, out IntPtr ppv);

            /// <summary>
            /// Requests a pointer to an object's storage interface. 
            /// Return value: error code, if any
            /// </summary>
            /// <param name="pidl">Address of an ITEMIDLIST structure that identifies the subfolder relative to its parent folder. </param>
            /// <param name="pbc">Optional address of an IBindCtx interface on a bind context object to be  used during this operation.</param>
            /// <param name="riid">Interface identifier (IID) of the requested storage interface.</param>
            /// <param name="ppv"> Address that receives the interface pointer specified by riid.</param>
            void BindToStorage(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, out IntPtr ppv);

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
            Int32 CompareIDs(Int32 lParam, IntPtr pidl1, IntPtr pidl2);

            /// <summary>
            /// Requests an object that can be used to obtain information from or interact
            /// with a folder object.
            /// Return value: error code, if any
            /// </summary>
            /// <param name="hwndOwner">Handle to the owner window.</param>
            /// <param name="riid">Identifier of the requested interface.</param>
            /// <param name="ppv">Address of a pointer to the requested interface. </param>
            void CreateViewObject(IntPtr hwndOwner, [In] ref Guid riid, out IntPtr ppv);

            /// <summary>
            /// Retrieves the attributes of one or more file objects or subfolders. 
            /// Return value: error code, if any
            /// </summary>
            /// <param name="cidl">Number of file objects from which to retrieve attributes. </param>
            /// <param name="apidl">Address of an array of pointers to ITEMIDLIST structures, each of which  uniquely identifies a file object relative to the parent folder.</param>
            /// <param name="rgfInOut">Address of a single ULONG value that, on entry contains the attributes that the caller is 
            /// requesting. On exit, this value contains the requested attributes that are common to all of the specified objects. this value can be from the SFGAO enum
            /// </param>
            void GetAttributesOf(UInt32 cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IntPtr[] apidl,
                ref ESFGAO rgfInOut);

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
            void GetUIObjectOf(IntPtr hwndOwner, UInt32 cidl,
                [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IntPtr[] apidl, [In] ref Guid riid,
                UInt32 rgfReserved, out IntPtr ppv);

            /// <summary>
            /// Retrieves the display name for the specified file object or subfolder. 
            /// Return value: error code, if any
            /// </summary>
            /// <param name="pidl">Address of an ITEMIDLIST structure (PIDL)  that uniquely identifies the file  object or subfolder relative to the parent  folder. </param>
            /// <param name="uFlags">Flags used to request the type of display name to return. For a list of possible values. </param>
            /// <param name="pName"> Address of a STRRET structure in which to return the display name.</param>
            void GetDisplayNameOf(IntPtr pidl, ESHGDN uFlags, out STRRET pName);

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
            void SetNameOf(IntPtr hwnd, IntPtr pidl, String pszName, ESHCONTF uFlags, out IntPtr ppidlOut);
        }

        public enum ESFGAO : uint
        {
            SFGAO_CANCOPY = 0x00000001,
            SFGAO_CANMOVE = 0x00000002,
            SFGAO_CANLINK = 0x00000004,
            SFGAO_LINK = 0x00010000,
            SFGAO_SHARE = 0x00020000,
            SFGAO_READONLY = 0x00040000,
            SFGAO_HIDDEN = 0x00080000,
            SFGAO_FOLDER = 0x20000000,
            SFGAO_FILESYSTEM = 0x40000000,
            SFGAO_HASSUBFOLDER = 0x80000000,
        }

        public enum ESHCONTF
        {
            SHCONTF_FOLDERS = 0x0020,
            SHCONTF_NONFOLDERS = 0x0040,
            SHCONTF_INCLUDEHIDDEN = 0x0080,
            SHCONTF_INIT_ON_FIRST_NEXT = 0x0100,
            SHCONTF_NETPRINTERSRCH = 0x0200,
            SHCONTF_SHAREABLE = 0x0400,
            SHCONTF_STORAGE = 0x0800
        }

        public enum ESHGDN
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
        [StructLayout(LayoutKind.Explicit, Size = 520)]
        public struct STRRETinternal
        {
            [FieldOffset(0)] public IntPtr pOleStr;

            [FieldOffset(0)] public IntPtr pStr; // LPSTR pStr;   NOT USED

            [FieldOffset(0)] public uint uOffset;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STRRET
        {
            public uint uType;
            public STRRETinternal data;
        }

        public class Guid_IShellFolder
        {
            public static Guid IID_IShellFolder = new Guid("{000214E6-0000-0000-C000-000000000046}");
        }

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
	}

}
