/*

	Windows Forms Folder Tree View control for .Net
	Version 1.1, posted 20-Oct-2002
	(c)Copyright 2002 Furty (furty74@yahoo.com). All rights reserved.
	Free for any use, so long as copyright is acknowledged.
	
	This is an all-new version of the FolderTreeView control I posted here at CP some weeks ago.
	The control now starts in the Desktop namespace, and a new DrillToFolder method has been added
	so the startup folder can be specified. Please note that this control is not intended to have 
	all of the functionality of the actual Windows Explorer TreeView - it is a light-weight control 
	designed for use in projects where you want to supply a treeview for folder navigation, without supporting
	windows shell extensions. If you are looking for a control that supports shell extensions
	you should be looking at the excellent ﾋxplorerTreeControl submitted by Carlos H Perez at the CP website.
	
	The 3 classes that make up the control have been merged into the one file here for ease of
	integration into your own projects. The reason for separate classes is that this code has been
	extracted from a much larger project I'm working on, and the code that is not required for this
	control has been removed.	
	
	Acknowledgments:
	Substantial portions of the ShellOperations and ExtractIcons classes were borrowed from the 
	FTPCom article written by Jerome Lacaille, available on the www.codeproject.com website.
	
	If you improve this control, please email me the updated source, and if you have any 
	comments or suggestions, please post your thoughts in the feedback section on the 
	codeproject.com page for this control.
	
	Version 1.11 Changes:
	Updated the GetDesktopIcon method so that the small (16x16) desktop icon is returned instead of the large version
	Added code to give the Desktop root node a FolderItem object tag equal to the DesktopDirectory SpecialFolder,
	this ensures that the desktop node returns a file path.
 
 */

using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Win32;
using System.Text;

namespace TextureChanger
{
	#region FolderTreeView Class

	public class TreeViewTag
	{
		public bool isDummy;
		public SH.IShellFolder shellFolder;
		public IntPtr pidlAbsolute;
		public SH.SFGAO attributes;
		public string path;
	}

	public class FolderTreeView : System.Windows.Forms.TreeView
	{
		private ImageList _folderTreeViewImageList;
		private readonly System.Globalization.CultureInfo _cultureInfo = System.Globalization.CultureInfo.CurrentCulture;
		private IntPtr _himlSystemSmall;

		#region Constructors

		public FolderTreeView()
		{
			BeforeExpand += new TreeViewCancelEventHandler(TreeViewBeforeExpand);
		}

		public void InitFolderTreeView()
		{
			//InitFolderTreeImageLists();
			InitImageList();
			ShellOperations.PopulateTree(this, ImageList);
			if(Nodes.Count > 0)
			{
				Nodes[0].Expand();
			}
		}

		#region システムイメージリストの登録 リストビューにイメージリストを登録する
		void InitFolderTreeImageLists()
		{
			var shfi = new SH.SHFILEINFO();

			//次のようにして、イメージリストのハンドルを得る
			_himlSystemSmall = SH.SHGetFileInfo(
				"C:\\"
				, 0
				, ref shfi
				, (uint) Marshal.SizeOf(shfi)
				, SH.SHGFI.SYSICONINDEX | SH.SHGFI.SMALLICON
				);
			if (_himlSystemSmall == IntPtr.Zero)
			{
				throw new NullReferenceException();
			}

			_folderTreeViewImageList = new ImageList
			{
				ColorDepth = ColorDepth.Depth32Bit,
				ImageSize = new Size(16, 16),
				TransparentColor = Color.Transparent
			};

			var nIcons = Api.ImageList_GetImageCount(_himlSystemSmall);

			for (int i = 0; i < nIcons; ++i)
			{
				var hIcon = Api.ImageList_GetIcon(_himlSystemSmall, i, ILD_FLAGS.NORMAL);
				if (hIcon == IntPtr.Zero)
				{
					var bmp = new Bitmap(16, 16);
					var img = (Image)bmp;
					_folderTreeViewImageList.Images.Add((Image)img.Clone());
					bmp.Dispose();
				}
				else
				{
					var icon = (Icon)Icon.FromHandle(hIcon).Clone();
					_folderTreeViewImageList.Images.Add(icon);
					Api.DestroyIcon(hIcon);
				}
			}

			ImageList = _folderTreeViewImageList;
		}


		private void InitImageList()
		{
			// setup the image list to hold the folder icons
			_folderTreeViewImageList = new ImageList();
			_folderTreeViewImageList.ColorDepth = ColorDepth.Depth32Bit;
			_folderTreeViewImageList.ImageSize = new Size(16, 16);
			_folderTreeViewImageList.TransparentColor = Color.Transparent;

			// add the Desktop icon to the image list
			try
			{
				_folderTreeViewImageList.Images.Add(ExtractIcons.GetDesktopIcon());
			}
			catch
			{
				// Create a blank icon if the desktop icon fails for some reason
				Bitmap bmp = new Bitmap(16,16);
				Image img = (Image)bmp;
				_folderTreeViewImageList.Images.Add((Image)img.Clone());
				bmp.Dispose();
			}
			ImageList = _folderTreeViewImageList;
		}
		#endregion

		#endregion

		#region Event Handlers

		private void TreeViewBeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			this.BeginUpdate();
			ShellOperations.ExpandBranch(e.Node, this.ImageList);
			this.EndUpdate();
		}

		#endregion

		#region Furty.Windows.Forms.FolderTreeView Properties & Methods

		public string GetSelectedNodePath()
		{
			return ShellOperations.GetFilePath(SelectedNode);
		}

		public bool DrillToFolder(string folderPath)
		{
			bool folderFound = false;
			if(Directory.Exists(folderPath)) // don't bother drilling unless the directory exists
			{
				this.BeginUpdate();
				// if there's a trailing \ on the folderPath, remove it unless it's a drive letter
				if(folderPath.Length > 3 && folderPath.LastIndexOf("\\") == folderPath.Length -1)
					folderPath = folderPath.Substring(0, folderPath.Length -1);
				//Start drilling the tree
				DrillTree(this.Nodes[0].Nodes, folderPath.ToUpper(_cultureInfo), ref folderFound);
				this.EndUpdate();
			}
			if(!folderFound)
				this.SelectedNode = this.Nodes[0];
			return folderFound;
		}

		private void DrillTree(TreeNodeCollection tnc, string path, ref bool folderFound)
		{
			foreach(TreeNode tn in tnc)
			{
				if(!folderFound)
				{
					this.SelectedNode = tn;
					string tnPath = ShellOperations.GetFilePath(tn).ToUpper(_cultureInfo);
					if(path == tnPath && !folderFound)
					{
						this.SelectedNode = tn;
						tn.EnsureVisible();
						folderFound = true;
						break;
					}
					else if(path.IndexOf(tnPath) > -1 && !folderFound)
					{
						tn.Expand();
						DrillTree(tn.Nodes, path, ref folderFound);
					}
				}
			}
		}


		#endregion
        
		#region System.Windows.Forms.TreeView Properties

        public override System.Drawing.Color BackColor
        {
            get
            { return base.BackColor; }
            set
            { base.BackColor = value; }
        }
        
        public override System.Drawing.Image BackgroundImage
        {
            get
            { return base.BackgroundImage; }
            set
            { base.BackgroundImage = value; }
        }
        
        public override System.Drawing.Color ForeColor
        {
            get
            { return base.ForeColor; }
            set
            { base.ForeColor = value; }
        }
        
        public override string Text
        {
            get
            { return base.Text; }
            set
            { base.Text = value; }
        }
        
        public override bool AllowDrop
        {
            get
            { return base.AllowDrop; }
            set
            { base.AllowDrop = value; }
        }
        
        public override System.Windows.Forms.AnchorStyles Anchor
        {
            get
            { return base.Anchor; }
            set
            { base.Anchor = value; }
        }
        
        public override System.Windows.Forms.BindingContext BindingContext
        {
            get
            { return base.BindingContext; }
            set
            { base.BindingContext = value; }
        }
        
        public override System.Windows.Forms.ContextMenu ContextMenu
        {
            get
            { return base.ContextMenu; }
            set
            { base.ContextMenu = value; }
        }
        
        public override System.Windows.Forms.Cursor Cursor
        {
            get
            {  return base.Cursor; }
            set
            {  base.Cursor = value; }
        }
        
        public override System.Drawing.Rectangle DisplayRectangle
        {
            get
            { return base.DisplayRectangle; }
        }
        
        public override System.Windows.Forms.DockStyle Dock
        {
            get
            { return base.Dock;  }
            set
            { base.Dock = value; }
        }
        
        public override bool Focused
        {
            get
            { return base.Focused; }
        }
        
        public override System.Drawing.Font Font
        {
            get
            { return base.Font; }
            set
            { base.Font = value; }
        }
        
        public override System.Windows.Forms.RightToLeft RightToLeft
        {
            get
            { return base.RightToLeft; }
            set
            { base.RightToLeft = value; }
        }
        
        public override System.ComponentModel.ISite Site
        {
            get
            { return base.Site; }
            set
            { base.Site = value; }
        }

		#endregion

		#region System.Windows.Forms.TreeView Overrides
 
        public override void ResetText()
        {
            base.ResetText();
        }
        
        public override void Refresh()
        {
            base.Refresh();
        }
        
        public override void ResetRightToLeft()
        {
            base.ResetRightToLeft();
        }
        
        public override void ResetForeColor()
        {
            base.ResetForeColor();
        }
        
        public override void ResetFont()
        {
            base.ResetFont();
        }
        
        public override void ResetCursor()
        {
            base.ResetCursor();
        }
        
        public override void ResetBackColor()
        {
            base.ResetBackColor();
        }
        
        public override bool PreProcessMessage(ref System.Windows.Forms.Message msg)
        {
            return base.PreProcessMessage(ref msg);
        }
        
        public override System.Runtime.Remoting.ObjRef CreateObjRef(System.Type requestedType)
        {
            return base.CreateObjRef(requestedType);
        }
        
        public override object InitializeLifetimeService()
        {
            return base.InitializeLifetimeService();
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        
        public override string ToString()
        {
            return base.ToString();
        }

		#endregion

    }

	#endregion

	#region ShellOperations Class

	public class ShellOperations
	{

		#region ShellFolder Enums
		// Enums for standard Windows shell folders
		public enum ShellFolder
		{
			Desktop = Shell32.ShellSpecialFolderConstants.ssfDESKTOP, 
			DesktopDirectory = Shell32.ShellSpecialFolderConstants.ssfDESKTOPDIRECTORY,
			MyComputer = Shell32.ShellSpecialFolderConstants.ssfDRIVES,
			MyDocuments = Shell32.ShellSpecialFolderConstants.ssfPERSONAL,
			MyPictures = Shell32.ShellSpecialFolderConstants.ssfMYPICTURES,
			History = Shell32.ShellSpecialFolderConstants.ssfHISTORY,
			Favorites = Shell32.ShellSpecialFolderConstants.ssfFAVORITES,
			Fonts = Shell32.ShellSpecialFolderConstants.ssfFONTS,
			ControlPanel = Shell32.ShellSpecialFolderConstants.ssfCONTROLS,
			TemporaryInternetFiles = Shell32.ShellSpecialFolderConstants.ssfINTERNETCACHE,
			MyNetworkPlaces = Shell32.ShellSpecialFolderConstants.ssfNETHOOD,
			NetworkNeighborhood = Shell32.ShellSpecialFolderConstants.ssfNETWORK,
			ProgramFiles = Shell32.ShellSpecialFolderConstants.ssfPROGRAMFILES,
			RecentFiles = Shell32.ShellSpecialFolderConstants.ssfRECENT,
			StartMenu = Shell32.ShellSpecialFolderConstants.ssfSTARTMENU,
			Windows = Shell32.ShellSpecialFolderConstants.ssfWINDOWS,
			Printers = Shell32.ShellSpecialFolderConstants.ssfPRINTERS,
			RecycleBin = Shell32.ShellSpecialFolderConstants.ssfBITBUCKET,
			Cookies = Shell32.ShellSpecialFolderConstants.ssfCOOKIES,
			ApplicationData = Shell32.ShellSpecialFolderConstants.ssfAPPDATA,
			SendTo = Shell32.ShellSpecialFolderConstants.ssfSENDTO,
			StartUp = Shell32.ShellSpecialFolderConstants.ssfSTARTUP
		}
		#endregion
		
		#region FolderTreeView Methods

		#region GetFilePath
		public static string GetFilePath(TreeNode tn)
		{
			try
			{
				TreeViewTag tag = (TreeViewTag)tn.Tag;

				SH.STRRET ptrString;
				tag.shellFolder.GetDisplayNameOf(tag.pidlAbsolute,
					(uint)SH.SHGDN.SHGDN_NORMAL, out ptrString);

				StringBuilder strDisplay = new StringBuilder(256);
				SH.StrRetToBuf(ref ptrString, tag.pidlAbsolute, strDisplay,
					(uint)strDisplay.Capacity);

				//Shell32.FolderItem folderItem = (Shell32.FolderItem)tn.Tag;
				string folderPath = strDisplay.ToString();// folderItem.Path;
				if(Directory.Exists(folderPath))
					return folderPath;
				else
					return "";
			}
			catch
			{
				return "";
			}
		}
		#endregion

		#region Populate Tree
		public static void PopulateTree(TreeView tree, ImageList imageList)
		{
			int imageCount = imageList.Images.Count -1;//TODO なぜ -1する？
			tree.Nodes.Clear();
			AddRootNode(tree, ref imageCount, imageList, true);
			if(tree.Nodes.Count > 1)
			{
				tree.SelectedNode = tree.Nodes[1];
				ExpandBranch(tree.Nodes[1], imageList);
			}
		}
		#endregion

		#region Add Root Node
		private static void AddRootNode(TreeView tree, ref int imageCount, ImageList imageList, bool getIcons)
		{
			SH.IShellFolder sfDesktop = SH.GetDesktopFolder();

			IntPtr pidlDesktop;
			SH.SHGetFolderLocation(
				IntPtr.Zero,
				(short)SH.CSIDL.DESKTOP,
				IntPtr.Zero,
				0,
				out pidlDesktop
			);

			IntPtr pidlRoot = SH.ILClone(pidlDesktop);

			IntPtr[] pidlList = { pidlRoot };
			SH.SFGAO attributesToRetrieve =
				SH.SFGAO.CAPABILITYMASK
			  | SH.SFGAO.HASSUBFOLDER
			  | SH.SFGAO.SHARE
			  | SH.SFGAO.FOLDER
			  | SH.SFGAO.FILESYSTEM;
			sfDesktop.GetAttributesOf(
				  1  //ひと組のIDLについて
				, pidlList
				, attributesToRetrieve
				);
//			if (!((ulong)attributesToRetrieve & (ulong)SH.SFGAO.FOLDER))
//				return hPrev;
			//フォルダでなかったらサブフォルダの列挙が不要なので
			//ツリーへのアイテム挿入位置hPrevをそのままにして返してしまう

			SH.STRRET ptrString;
			sfDesktop.GetDisplayNameOf(pidlRoot,
				(uint)SH.SHGDN.SHGDN_NORMAL, out ptrString);
			StringBuilder strDisplay = new StringBuilder(256);
			SH.StrRetToBuf(ref ptrString ,pidlRoot,strDisplay,
				(uint)strDisplay.Capacity);
			
			tree.Nodes.Clear();
			
			TreeNode desktop = new TreeNode( strDisplay.ToString(), 0, 0 );

			desktop.Tag = new TreeViewTag()
			{
				isDummy = false,
				shellFolder = sfDesktop,
				pidlAbsolute = pidlRoot,
				attributes = attributesToRetrieve,
				path = strDisplay.ToString(),
			};

			tree.Nodes.Add(desktop);

			FillSubDirectories(desktop, ref imageCount, imageList, getIcons);

		}
		#endregion

		#region フォルダの子ノードを列挙してツリーに追加する
		private static void FillSubDirectories(TreeNode tn, ref int imageCount, ImageList imageList, bool getIcons)
		{
			TreeViewTag tnTag = (TreeViewTag)tn.Tag;

			IntPtr hWndMain = Api.GetMainWindow(tn.Handle);

			IntPtr ppEnumIDList;
			tnTag.shellFolder.EnumObjects(
				hWndMain
			  , SH.SHCONTF.SHCONTF_FOLDERS //| SH.SHCONTF.SHCONTF_INCLUDEHIDDEN
			  , out ppEnumIDList);

			Object obj = Marshal.GetTypedObjectForIUnknown(ppEnumIDList, typeof(SH.IEnumIDList));
			SH.IEnumIDList enumIDList = (SH.IEnumIDList)obj;

			IntPtr pidlChildRelative;
			IntPtr notUsed;
			while (enumIDList.Next(1, out pidlChildRelative, out notUsed) == (uint)ErrNo.S_OK)
			{
				//相対PIDLを親の絶対PIDLと連結して絶対PIDLに加工する
				IntPtr pidlAbsolute;
				pidlAbsolute = SH.ILClone(tnTag.pidlAbsolute);
				pidlAbsolute = SH.ILCombine(pidlAbsolute, pidlChildRelative);
				Api.CoTaskMemFree(pidlChildRelative);

				//絶対PIDLのファイル属性を取得
				IntPtr[] pidlList = { pidlAbsolute };
				SH.SFGAO attributesToRetrieve =
					SH.SFGAO.CAPABILITYMASK
				  | SH.SFGAO.HASSUBFOLDER
				  | SH.SFGAO.SHARE
				  | SH.SFGAO.FOLDER
				  | SH.SFGAO.FILESYSTEM;
				tnTag.shellFolder.GetAttributesOf(
					  1  //ひと組のIDLについて
					, pidlList
					, attributesToRetrieve
				);
				if (((ulong)attributesToRetrieve & (ulong)SH.SFGAO.FILESYSTEM) != 0
					&& ((ulong)attributesToRetrieve & (ulong)SH.SFGAO.FOLDER) != 0
					&& ((ulong)attributesToRetrieve & (ulong)SH.SFGAO.BROWSABLE) == 0)
				{
					;
				}
				else
				{
					continue;
				}

				//同じく絶対PIDLの表示名を取得
				SH.STRRET ptrString;
				tnTag.shellFolder.GetDisplayNameOf(pidlAbsolute,
					(uint)SH.SHGDN.SHGDN_NORMAL, out ptrString);
				StringBuilder strDisplay = new StringBuilder(256);
				SH.StrRetToBuf(ref ptrString, pidlAbsolute, strDisplay,
					(uint)strDisplay.Capacity);

				//同じく絶対PIDLのIShellFolderオブジェクトを取得
				IntPtr ppv;
				tnTag.shellFolder.BindToObject(
					pidlAbsolute
					, IntPtr.Zero
					, SH.Guid_IShellFolder.IID_IShellFolder
					, out ppv);
				Object objsf = Marshal.GetTypedObjectForIUnknown(ppv, typeof(SH.IShellFolder));
				
				//子ノードを生成して親ノードに追加
				TreeNode babyNode = AddTreeNode(
					new TreeViewTag()
					{
						isDummy = false,
						shellFolder = (SH.IShellFolder)objsf,
						pidlAbsolute = pidlAbsolute,
						attributes = attributesToRetrieve,
						path = strDisplay.ToString(),
					}
					, ref imageCount, imageList, getIcons);
				tn.Nodes.Add(babyNode);
				CheckForSubDirs(babyNode);
			}
		}
		#endregion

		#region サブディレクトリが存在する場合にノード＋マークをつけるためにダミーを差し込んでおく（フォルダリストを遅延読み込みするため）
		private static void CheckForSubDirs(TreeNode tn)
		{
			if (tn.Nodes.Count != 0)
				return;
			try
			{
				bool hasFolders = false;

				TreeViewTag tnTag = (TreeViewTag)tn.Tag;
				
				IntPtr hWndMain = Api.GetMainWindow(tn.Handle);

				IntPtr ppEnumIDList;
				tnTag.shellFolder.EnumObjects(
					hWndMain
					, SH.SHCONTF.SHCONTF_FOLDERS //| SH.SHCONTF.SHCONTF_INCLUDEHIDDEN
					, out ppEnumIDList);

				Object obj = Marshal.GetTypedObjectForIUnknown(ppEnumIDList, typeof(SH.IEnumIDList));
				SH.IEnumIDList enumIDList = (SH.IEnumIDList)obj;

				IntPtr pidlChildRelative;
				IntPtr notUsed;
				while (enumIDList.Next(1, out pidlChildRelative, out notUsed) == (uint)ErrNo.S_OK)
				{
					//相対PIDLを親の絶対PIDLと連結して絶対PIDLに加工する
					IntPtr pidlAbsolute;
					pidlAbsolute = SH.ILClone(tnTag.pidlAbsolute);
					pidlAbsolute = SH.ILCombine(pidlAbsolute, pidlChildRelative);
					Api.CoTaskMemFree(pidlChildRelative);

					//絶対PIDLのファイル属性を取得
					IntPtr[] pidlList = { pidlAbsolute };
					SH.SFGAO attributesToRetrieve =
						SH.SFGAO.CAPABILITYMASK
						| SH.SFGAO.HASSUBFOLDER
						| SH.SFGAO.SHARE
						| SH.SFGAO.FOLDER
						| SH.SFGAO.FILESYSTEM;
					tnTag.shellFolder.GetAttributesOf(
							1  //ひと組のIDLについて
						, pidlList
						, attributesToRetrieve
					);
					if ( ((ulong)attributesToRetrieve & (ulong)SH.SFGAO.FILESYSTEM) != 0
						&& ((ulong)attributesToRetrieve & (ulong)SH.SFGAO.FOLDER) != 0
						&& ((ulong)attributesToRetrieve & (ulong)SH.SFGAO.BROWSABLE) == 0 )
					{
						hasFolders = true;
						break;
					}
					if (hasFolders)
					{
						TreeNode babyNode = new TreeNode();
						babyNode.Tag = new TreeViewTag()
						{
							isDummy = true,
						}; 
						tn.Nodes.Add(babyNode);
					}
				}
			}
			catch {}
		}
		#endregion

		#region Expand Branch
		public static void ExpandBranch(TreeNode tn, ImageList imageList)
		{
			// if there's a dummy node present, clear it and replace with actual contents
			if(tn.Nodes.Count != 1)
				return;

			TreeViewTag tnTag = (TreeViewTag)tn.Nodes[0].Tag;


			if(tnTag.isDummy)
			{
				tn.Nodes.Clear();

				int imageCount = imageList.Images.Count - 1;
				FillSubDirectories(tn, ref imageCount, imageList, true);
			}
		}
		#endregion

		#region Add Tree Node
		private static TreeNode AddTreeNode(TreeViewTag tag, ref int imageCount, ImageList imageList, bool getIcons)
		{
			TreeNode tn = new TreeNode();
			tn.Text = tag.path;
			tn.Tag = tag;

			if(getIcons)
			{
				try
				{
					imageCount++;
					tn.ImageIndex = imageCount;
					imageCount++;
					tn.SelectedImageIndex = imageCount;

					IntPtr pidl = SH.ILClone(tag.pidlAbsolute);


					imageList.Images.Add(ExtractIcons.GetIcon(pidl, false, imageList)); // normal icon
					imageList.Images.Add(ExtractIcons.GetIcon(pidl, true, imageList)); // selected icon

					SH.ILFree(pidl);
				}
				catch // use default 
				{
					tn.ImageIndex = 1;
					tn.SelectedImageIndex = 2;
				}
			}
			else // use default
			{
				tn.ImageIndex = 1;
				tn.SelectedImageIndex = 2;
			}
			return tn;
		}

		#endregion

		#endregion
	}

	#endregion

	#region パスからアイコンを得るstaticメソッドを持つのみのクラス
	public class ExtractIcons
	{
		#region GetIcon
		public static Icon GetIcon( IntPtr pidl, bool selected, ImageList imageList )
		{
			SH.SHFILEINFO shfi = new SH.SHFILEINFO();
			int cbFileInfo = Marshal.SizeOf(shfi);

			SH.SHGFI flags = SH.SHGFI.PIDL;

			if (!selected)
				flags = flags | SH.SHGFI.ICON | SH.SHGFI.SMALLICON;
			else
				flags = flags | SH.SHGFI.ICON | SH.SHGFI.SMALLICON | SH.SHGFI.OPENICON;

			SH.SHGetFileInfo( pidl, 0, ref shfi, (uint)cbFileInfo, flags );
			Icon.FromHandle( shfi.hIcon );

			if (pidl != IntPtr.Zero)
			{
				IMalloc pMalloc = SH.GetMalloc();
				pMalloc.Free(pidl);
				Marshal.ReleaseComObject(pMalloc);
			}

			var icon = (Icon)Icon.FromHandle( shfi.hIcon ).Clone( );
			Api.DestroyIcon( shfi.hIcon );

			return icon;
		}
		#endregion
		
		#region Get Desktop Icon
		// Retreive the desktop icon from Shell32.dll - it always appears at index 34 in all shell32 versions.
		// This is probably NOT the best way to retreive this icon, but it works - if you have a better way
		// by all means let me know..
		public static Icon GetDesktopIcon()
		{
			IntPtr[] handlesIconLarge = new IntPtr[1];
			IntPtr[] handlesIconSmall = new IntPtr[1];
			uint i = Api.ExtractIconEx(Environment.SystemDirectory + "\\shell32.dll", 34, 
				handlesIconLarge, handlesIconSmall, 1);

			return Icon.FromHandle(handlesIconSmall[0]);
		}
		#endregion

	}
	#endregion


}

