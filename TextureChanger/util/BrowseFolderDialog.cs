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
		//special folderを示すDWORD定数
        public enum FolderID
        {
            Desktop                 = 0x0000,
            Printers                = 0x0004,
            MyDocuments             = 0x0005,
            Favorites               = 0x0006,
            Recent                  = 0x0008,
            SendTo                  = 0x0009,
            StartMenu               = 0x000b,
            MyComputer              = 0x0011,
            NetworkNeighborhood     = 0x0012,
            Templates               = 0x0015,
            MyPictures              = 0x0027,
            NetAndDialUpConnections = 0x0031,
        }

        private FolderID _startLocation = FolderID.Desktop; //ルートノード初期値

        private int _publicOptions =
			(int)Win32Api.Shell32.BffStyles.RestrictToFilesystem |
            (int)Win32Api.Shell32.BffStyles.RestrictToDomain ;

        private int _privateOptions = 
			(int)Win32Api.Shell32.BffStyles.NewDialogStyle ;

		private string _descriptionText = "フォルダを選択してください：";

        private string directoryPath = String.Empty;



        /// <summary>
        /// Shows the folder browser dialog box.
        /// </summary>
        public DialogResult ShowDialog()
        {
            return ShowDialog(null);
        }
        


        /// <summary>
        /// Shows the folder browser dialog box with the specified owner window.
        /// </summary>
        public DialogResult ShowDialog(IWin32Window owner)
        {
            IntPtr pidlRoot = IntPtr.Zero;

            // Get/find an owner HWND for this dialog.
            IntPtr hWndOwner;

            if (owner != null)
            {
                hWndOwner = owner.Handle;
            }
            else
            {
                hWndOwner = Win32Api.GetActiveWindow();
            }

            // Get the IDL for the specific startLocation.
            Win32Api.Shell32.SHGetSpecialFolderLocation(hWndOwner, (int)_startLocation, out pidlRoot);

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
                bi.lpszTitle      = _descriptionText;
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
                directoryPath = sb.ToString();
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
	
 
        public bool ShowNetworkFolders;
        public bool OnlySubfolders;
        public bool ShowTextBox;
        public bool ValidateUserInput;
        public bool SelectComputer;
        public bool SelectPrinter;
        public bool SelectFiles;

        public bool Description;

        /// <summary>
        /// Helper function used to set and reset bits in the publicOptions bitfield.
        /// </summary>
        private void SetOptionField(int mask, bool turnOn)
        {
            if (turnOn)
                _publicOptions |= mask;
            else
                _publicOptions &= ~mask;
        }

        /// <summary>
        /// Only return file system directories. If the user selects folders
        /// that are not part of the file system, the OK button is unavailable.
        /// </summary>
        [Category("Navigation")]
        [Description("Only return file system directories. If the user selects folders " +
                       "that are not part of the file system, the OK button is unavailable.")]
        [DefaultValue(true)]
        public bool OnlyFilesystem
        {
            get
            {
                return (_publicOptions & (int)Win32Api.Shell32.BffStyles.RestrictToFilesystem) != 0;
            }
            set
            {
                SetOptionField((int)Win32Api.Shell32.BffStyles.RestrictToFilesystem, value);
            }
        }

        /// <summary>
        /// Location of the root folder from which to start browsing. Only the specified
        /// folder and any folders beneath it in the namespace hierarchy  appear
        /// in the dialog box.
        /// </summary>
        [Category("Navigation")]
        [Description("Location of the root folder from which to start browsing. Only the specified " +
                       "folder and any folders beneath it in the namespace hierarchy appear " +
                       "in the dialog box.")]
        [DefaultValue(typeof(FolderID), "0")]
        public FolderID StartLocation
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

        /// <summary>
        /// Full path to the folder selected by the user.
        /// </summary>
        [Category("Navigation")]
        [Description("Full path to the folder slected by the user.")]
        public string DirectoryPath
        {
            get
            {
                return directoryPath;
            }
        } 
      
    }

}
