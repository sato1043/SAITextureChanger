using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Win32
{
    public class SH
    {
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
            public IntPtr lParam;
            public int iImage;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHGetMalloc(out IMalloc ppMalloc);

        public static Win32.IMalloc GetMalloc()
        {
            Win32.IMalloc malloc;
            SHGetMalloc(out malloc);
            return malloc;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int SHGetSpecialFolderLocation(IntPtr hwndOwner, int nFolder, out IntPtr ppidl);
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int SHGetPathFromIDList(IntPtr pidl, StringBuilder Path);
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

        public enum SHGFI : uint
        {
            LARGEICON = 0x0, // 大きいアイコン
            SMALLICON = 0x1, // 小さいアイコン
            ICON = 0x100, // アイコン・リソースの取得
        };

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);




    }

}
