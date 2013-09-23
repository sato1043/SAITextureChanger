using System;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Win32;

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

        private SH.CSIDL _startLocation;

        public SH.CSIDL StartLocation
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

        private SH.BIF _options;
        #endregion

        #region ダイアログ表示中のコールバック
        private SH.BFFCALLBACK _procedure;
 
        public SH.BFFCALLBACK Procedure
        {
            get
            {
                return _procedure;
            }
            set
            {
                _procedure = value;
            }
        }
        #endregion

        #region longパラメータ
        private UInt32 _lParam;

        public UInt32 lParam
        {
            get
            {
                return _lParam;
            }
            set
            {
                _lParam = value;
            }
        }
        #endregion

        public BrowseFolderDialog()
        {
            _dialogMessage = "フォルダを選択してください：";
            _directoryPath = String.Empty;
            _startLocation = SH.CSIDL.DESKTOP;
            _options = SH.BIF.RETURNONLYFSDIRS  | SH.BIF.DONTGOBELOWDOMAIN | SH.BIF.NEWDIALOGSTYLE ;
            _procedure = BffCallback;
            _lParam = 0;
        }

        int BffCallback(IntPtr hwnd, UInt32 uMsg, IntPtr lParam, IntPtr lpData)
        {
            switch(uMsg)
            {
                case (uint)SH.BFFM.INITIALIZED:
                    //はじめに選択されるフォルダをitemIDLでメッセージ
                    //Win32.Api.SendMessage( hwnd, (uint)Win32.SH.BFFM.SETSELECTION, IntPtr.Zero, lpData );
                    break;

                case (uint)SH.BFFM.SELCHANGED:
                    // TODO:
                    /*
                    char szPath[Win32Api.MAX_PATH+1];
                    Win32.SH.SHGetPathFromIDList((LPCITEMIDLIST)lParam,szPath);
                    Win32Api.SendMessage(hwnd,BFFM_SETSTATUSTEXT,0,(LPARAM)szPath);
                    //ユーザーがフォルダ選択を変更した時には
                    //ITEMIDLIST構造体からパス名を取り出して表示する
                     * */
                    break;
            }
            return 0;
        }

        public DialogResult ShowDialog()
        {
            return ShowDialog(null);
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            //新しいスタイルのダイアログを使うならOLEを初期化
            //初期化できなかったら新しいスタイルのダイアログを無効化
            if (this.fNewDialogStyle == true)
            {
                if (Application.OleRequired()
                     == System.Threading.ApartmentState.MTA)
                {
                    this.fNewDialogStyle = false;
                }
            }

            IntPtr hWndOwner = (owner != null) ? owner.Handle : Api.GetActiveWindow();

            IntPtr pidlRoot = IntPtr.Zero;

            SH.SHGetSpecialFolderLocation(hWndOwner, _startLocation, ref pidlRoot);
            if (pidlRoot == IntPtr.Zero)
            {
                return DialogResult.Cancel;
            }

            IntPtr pidlRet = IntPtr.Zero;

            try
            {
                SH.BROWSEINFO bi = new SH.BROWSEINFO();
                IntPtr buffer = Marshal.AllocHGlobal((int)Win32.MAX.PATH);

                bi.hwndOwner = hWndOwner;
                bi.pidlRoot = pidlRoot;
                bi.pszDisplayName = buffer;
                bi.lpszTitle = DialogMessage;
                bi.ulFlags = (int)_options;
                bi.iImage = 0;
                bi.lpfn = Procedure;
                bi.lParam = lParam;
                pidlRet = Win32.SH.SHBrowseForFolder(ref bi);
                Marshal.FreeHGlobal(buffer);

                if (pidlRet == IntPtr.Zero)
                {
                    return DialogResult.Cancel; // User clicked Cancel.
                }

                // Then retrieve the path from the IDList.
                StringBuilder sb = new StringBuilder((int)Win32.MAX.PATH);
                if (SH.SHGetPathFromIDListW(pidlRet, sb) == false)
                {
                    return DialogResult.Cancel;
                }
                _directoryPath = sb.ToString();

            }
            finally
            {
                IMalloc malloc = SH.GetMalloc();
                malloc.Free(pidlRoot);

                if (pidlRet != IntPtr.Zero)
                {
                    malloc.Free(pidlRet);
                }
            }

            return DialogResult.OK;
        }


        public void SetOptionField(Win32.SH.BIF mask, bool turnOn)
        {
            if (turnOn)
                _options |= mask;
            else
                _options &= ~mask;
        }
        public bool fReturnOnlyFsDirs
        {
            get
            {
                return (_options & SH.BIF.RETURNONLYFSDIRS) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.RETURNONLYFSDIRS, value);
            }
        }
        public bool fDontGoBelowDomain
        {
            get
            {
                return (_options & SH.BIF.DONTGOBELOWDOMAIN) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.DONTGOBELOWDOMAIN, value);
            }
        }
        public bool fStatusText
        {
            get
            {
                return (_options & SH.BIF.STATUSTEXT) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.STATUSTEXT, value);
            }
        }
        public bool fReturnFsAncestors
        {
            get
            {
                return (_options & SH.BIF.RETURNFSANCESTORS) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.RETURNFSANCESTORS, value);
            }
        }
        public bool fEditBox
        {
            get
            {
                return (_options & SH.BIF.EDITBOX) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.EDITBOX, value);
            }
        }
        public bool fValidate
        {
            get
            {
                return (_options & SH.BIF.VALIDATE) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.VALIDATE, value);
            }
        }
        public bool fNewDialogStyle
        {
            get
            {
                return (_options & SH.BIF.NEWDIALOGSTYLE) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.NEWDIALOGSTYLE, value);
            }
        }
        public bool fBrowseIncludeUrls
        {
            get
            {
                return (_options & SH.BIF.BROWSEINCLUDEURLS) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.BROWSEINCLUDEURLS, value);
            }
        }
        public bool fUseNewUi
        {
            get
            {
                return (_options & SH.BIF.USENEWUI) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.USENEWUI, value);
            }
        }
        public bool fUaHint
        {
            get
            {
                return (_options & SH.BIF.UAHINT) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.UAHINT, value);
            }
        }
        public bool fNoNewFolderButton
        {
            get
            {
                return (_options & SH.BIF.NONEWFOLDERBUTTON) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.NONEWFOLDERBUTTON, value);
            }
        }
        public bool fNoTranslateTargets
        {
            get
            {
                return (_options & SH.BIF.NOTRANSLATETARGETS) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.NOTRANSLATETARGETS, value);
            }
        }
        public bool fBrowseForComputer
        {
            get
            {
                return (_options & SH.BIF.BROWSEFORCOMPUTER) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.BROWSEFORCOMPUTER, value);
            }
        }
        public bool fBrowseForPrinter
        {
            get
            {
                return (_options & SH.BIF.BROWSEFORPRINTER) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.BROWSEFORPRINTER, value);
            }
        }
        public bool fBrowseIncludeFiles
        {
            get
            {
                return (_options & SH.BIF.BROWSEINCLUDEFILES) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.BROWSEINCLUDEFILES, value);
            }
        }
        public bool fShareable
        {
            get
            {
                return (_options & SH.BIF.SHAREABLE) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.SHAREABLE, value);
            }
        }
        public bool fBrowseFileJunctions
        {
            get
            {
                return (_options & SH.BIF.BROWSEFILEJUNCTIONS) != 0;
            }
            set
            {
                SetOptionField(SH.BIF.BROWSEFILEJUNCTIONS, value);
            }
        }

    }

}
