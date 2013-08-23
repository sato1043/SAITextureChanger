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
using TextureChanger.util;
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
		public IntPtr pidlRelative;
		public SH.SFGAO attributes;
		public string path;
	}

	//TODO COMを使うところのエラー処理を追加する

	public class FolderTreeView : System.Windows.Forms.TreeView
	{
		private ImageList _folderTreeViewImageList = new ImageList( );
		private readonly System.Globalization.CultureInfo _cultureInfo = System.Globalization.CultureInfo.CurrentCulture;

		private static IntPtr _pidlDesktop = IntPtr.Zero;


		#region コンストラクタと初期化処理

		private void TreeViewBeforeExpand( object sender, System.Windows.Forms.TreeViewCancelEventArgs e )
		{
			this.BeginUpdate( );
			ExpandBranch( this, (TreeView)sender, e.Node, this.ImageList );
			this.EndUpdate( );
		}

		public FolderTreeView()
		{
			BeforeExpand += new TreeViewCancelEventHandler(TreeViewBeforeExpand);
		}

		public void InitFolderTreeView(IWin32Window owner)
		{
			InitImageList();
			PopulateTree( owner, this, _folderTreeViewImageList );
			if(Nodes.Count > 0)
			{
				Nodes[0].Expand();
			}
		}

		public static void PopulateTree( IWin32Window owner, TreeView tree, ImageList imageList )
		{
			int imageCount = imageList.Images.Count - 1;
			tree.Nodes.Clear( );
			AddRootNode( owner, tree, ref imageCount, imageList, true );
			if( tree.Nodes.Count > 1 )
			{
				tree.SelectedNode = tree.Nodes[ 1 ];
				ExpandBranch( owner, tree, tree.Nodes[ 1 ], imageList );
			}
		}

		#region システムイメージリストの登録 リストビューにイメージリストを登録する
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

		#region ツリーノードを展開表示する
		public static void ExpandBranch( IWin32Window owner, TreeView tree, TreeNode tn, ImageList imageList )
		{
			// if there's a dummy node present, clear it and replace with actual contents
			if( tn.Nodes.Count != 1 )
				return;

			TreeViewTag tnTag = (TreeViewTag)tn.Nodes[ 0 ].Tag;


			if( tnTag.isDummy )
			{
				tn.Nodes.Clear( );

				int imageCount = imageList.Images.Count - 1;
				FillSubDirectories( owner, tree, tn, ref imageCount, imageList, true );
			}
		}
		#endregion

		#region ノードのパスを返すプロパティ郡

		#region 選択中ノードのパスを返すプロパティ
		public string GetSelectedNodePath( )
		{
			return GetFilePath( SelectedNode );
		}
		#endregion

		#region 指定ノードのファイルパスを返すプロパティ（ファイルシステムにマップできないときは空文字列）
		public static string GetFilePath( TreeNode tn )
		{
			try
			{
				TreeViewTag tag = (TreeViewTag)tn.Tag;
				StringBuilder strDisplay = new StringBuilder( (int)MAX.PATH );
				SH.SHGetPathFromIDList( tag.pidlAbsolute, strDisplay );
				if( Directory.Exists( strDisplay.ToString( ) ) )
					return strDisplay.ToString( );
			}
			catch
			{
			}
			return "";
		}
		#endregion

		#endregion

		#region ツリービューのノードの展開を行うメソッド郡

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
					string tnPath = GetFilePath(tn).ToUpper(_cultureInfo);
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

		#region デスクトップ（初期ノード）を追加する
		private static void AddRootNode( IWin32Window owner, TreeView tree, ref int imageCount, ImageList imageList, bool getIcons)
		{
			SH.IShellFolder sfDesktop = null;
			IntPtr pidlRoot = IntPtr.Zero;
			SH.SFGAO attributesToRetrieve =
							SH.SFGAO.CAPABILITYMASK
						  | SH.SFGAO.HASSUBFOLDER
						  | SH.SFGAO.SHARE
						  | SH.SFGAO.FOLDER
						  | SH.SFGAO.FILESYSTEM;
			string strDisplay = "";
			try
			{
				sfDesktop = SH.GetDesktopFolder();
				if( sfDesktop == null )
				{
					throw new ArgumentNullException( );
				}

				SH.SHGetSpecialFolderLocation(
					IntPtr.Zero,
					SH.CSIDL.DESKTOP,
					ref _pidlDesktop
				);

				pidlRoot = SH.ILClone(_pidlDesktop);
				if (pidlRoot == IntPtr.Zero)
				{
					throw new ArgumentNullException();
				}

				IntPtr[] pidlList = { pidlRoot, IntPtr.Zero, IntPtr.Zero };
				sfDesktop.GetAttributesOf(
					  1  //ひと組のIDLについて
					, pidlList
					, attributesToRetrieve
					);

				strDisplay = SH.ExtractDisplayName( sfDesktop, pidlRoot, pidlRoot );

			}
			catch
			{
				if (pidlRoot != IntPtr.Zero)
				{
					SH.ILFree(pidlRoot);
				}
				if( sfDesktop != null )
				{
					Marshal.ReleaseComObject( sfDesktop );
				}
				CenteredMessageBox.Show( owner
					, "フォルダツリーの作成に失敗しました。\n" +
					  "プログラムを再起動してみてください。"
					, "TexureChanger内部処理エラー"
					, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			
			tree.Nodes.Clear();
			
			TreeNode desktop = new TreeNode( strDisplay, 0, 0 );

			desktop.Tag = new TreeViewTag()
			{
				isDummy = false,
				shellFolder = sfDesktop,
				pidlAbsolute = pidlRoot,
				pidlRelative = pidlRoot,
				attributes = attributesToRetrieve,
				path = strDisplay,
			};

			tree.Nodes.Add(desktop);

			FillSubDirectories( owner, tree, desktop, ref imageCount, imageList, getIcons );

		}
		#endregion

		#region フォルダの子ノードを列挙してツリーに追加する
		private static void FillSubDirectories( IWin32Window owner, TreeView tree, TreeNode tn, ref int imageCount, ImageList imageList, bool getIcons )
		{
			TreeViewTag tnTag = (TreeViewTag)tn.Tag;
			try
			{
				if (tnTag.shellFolder == null
				    || tnTag.pidlAbsolute == IntPtr.Zero
				    || tnTag.pidlRelative == IntPtr.Zero)
				{
					throw new ArgumentNullException();
				}
			}
			catch
			{
				CenteredMessageBox.Show( owner
					, "フォルダツリーの子ノードが不正な値でした。\n" +
					  "プログラムを再起動してみてください。"
					, "TexureChanger内部処理エラー"
					, MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			IntPtr hWndMain = Api.GetMainWindow( tree.Handle );

			try
			{
				IntPtr pidlChildRelative = IntPtr.Zero;
				IntPtr notUsed;

				IntPtr ppEnumIDList = IntPtr.Zero;
				tnTag.shellFolder.EnumObjects(
					hWndMain
				  , SH.SHCONTF.SHCONTF_FOLDERS //| SH.SHCONTF.SHCONTF_INCLUDEHIDDEN
				  , out ppEnumIDList );
				if( ppEnumIDList == IntPtr.Zero )
				{
					throw new ArgumentNullException( );
				}
				SH.IEnumIDList enumIDList= (SH.IEnumIDList)Marshal.GetTypedObjectForIUnknown( ppEnumIDList, typeof( SH.IEnumIDList ) );
				if( enumIDList == null )
				{
					throw new ArgumentNullException( );
				}

				while( enumIDList.Next( 1, out pidlChildRelative, out notUsed ) == (uint)ErrNo.S_OK )
				{
					if (pidlChildRelative == IntPtr.Zero)
					{
						continue;
					}

					SH.SFGAO attributesToRetrieve =
						SH.SFGAO.CAPABILITYMASK
					  | SH.SFGAO.HASSUBFOLDER
					  | SH.SFGAO.SHARE
					  | SH.SFGAO.FOLDER
					  | SH.SFGAO.FILESYSTEM
					  | SH.SFGAO.STREAM;
					IntPtr pidlAbsolute = IntPtr.Zero;
					string strDisplay = "";
					SH.IShellFolder childShellFolder = null;

					try
					{
						//子PIDLのファイル属性を取得
						IntPtr[] pidlList = { pidlChildRelative, IntPtr.Zero, IntPtr.Zero };
						tnTag.shellFolder.GetAttributesOf(
							  1  //ひと組のIDLについて
							, pidlList
							, attributesToRetrieve
						);
						if( ( (ulong)attributesToRetrieve & (ulong)SH.SFGAO.FILESYSTEM ) != 0
							&& ( (ulong)attributesToRetrieve & (ulong)SH.SFGAO.FOLDER ) != 0
							//&& ( (ulong)attributesToRetrieve & (ulong)SH.SFGAO.STREAM ) == 0
							&& ( (ulong)attributesToRetrieve & (ulong)SH.SFGAO.BROWSABLE ) == 0
						)
						{
							;
						}
						else
						{
							Api.CoTaskMemFree( pidlChildRelative );
							continue;
						}

						//相対PIDLを親の絶対PIDLと連結して絶対PIDLに加工する
						pidlAbsolute = SH.ILClone( tnTag.pidlAbsolute );
						if( pidlAbsolute == IntPtr.Zero )
						{
							throw new ArgumentNullException( );
						}
						pidlAbsolute = SH.ILCombine( pidlAbsolute, pidlChildRelative );
						if( pidlAbsolute == IntPtr.Zero )
						{
							throw new ArgumentNullException( );
						}

						//表示名を取得
						strDisplay = SH.ExtractDisplayName( tnTag.shellFolder, pidlChildRelative, pidlAbsolute );
						if( String.Compare( Path.GetExtension( strDisplay ), ".zip", true ) == 0 )
						{
							Api.CoTaskMemFree( pidlChildRelative );
							SH.ILFree( pidlAbsolute );
							continue;
						}

						//子PIDLのIShellFolderオブジェクトを取得
						IntPtr ppv;
						tnTag.shellFolder.BindToObject(
							pidlChildRelative
							, IntPtr.Zero
							, SH.Guid_IShellFolder.IID_IShellFolder
							, out ppv );
						if (ppv == IntPtr.Zero)
						{
							throw new ArgumentNullException();
						}
						childShellFolder = (SH.IShellFolder)Marshal.GetTypedObjectForIUnknown( ppv, typeof( SH.IShellFolder ) );
						if (childShellFolder == null)
						{
							throw new ArgumentNullException();
						}

						//子ノードを生成して親ノードに追加
						TreeNode babyNode = AddTreeNode(
							new TreeViewTag( )
							{
								isDummy = false,
								shellFolder = childShellFolder,
								pidlAbsolute = pidlAbsolute,
								pidlRelative = pidlChildRelative,
								attributes = attributesToRetrieve,
								path = strDisplay,
							}
							, ref imageCount, imageList, getIcons );

						tn.Nodes.Add( babyNode );
						CheckForSubDirs( owner, tree, babyNode );
					}
					catch
					{
						if( pidlAbsolute != IntPtr.Zero )
						{
							SH.ILFree( pidlAbsolute );
						}
						if( childShellFolder != null )
						{
							Marshal.ReleaseComObject( childShellFolder );
						}
						CenteredMessageBox.Show( owner
							, "フォルダツリーの子ノードを列挙中に不正な処理をした要素が存在しました。\n" +
							  "プログラムを再起動してみてください。"
							, "TexureChanger内部処理エラー"
							, MessageBoxButtons.OK, MessageBoxIcon.Error );
					}
				}

				Marshal.ReleaseComObject( enumIDList );

			}
			catch
			{
				CenteredMessageBox.Show( owner
					, "フォルダツリーの子ノードを列挙に失敗しました。\n" +
					  "プログラムを再起動してみてください。"
					, "TexureChanger内部処理エラー"
					, MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
		}
		#endregion

		#region サブディレクトリが存在する場合にノード＋マークをつけるためにダミーを差し込んでおく（フォルダリストを遅延読み込みするため）
		private static void CheckForSubDirs( IWin32Window owner, TreeView tree, TreeNode tn )
		{
			if (tn.Nodes.Count != 0)
				return;

			TreeViewTag tnTag = (TreeViewTag)tn.Tag;
			try
			{
				if( tnTag.shellFolder == null
					|| tnTag.pidlAbsolute == IntPtr.Zero
					|| tnTag.pidlRelative == IntPtr.Zero )
				{
					throw new ArgumentNullException( );
				}
			}
			catch
			{
				CenteredMessageBox.Show( owner
					, "フォルダツリーの子ノードが不正な値でした。\n" +
					  "プログラムを再起動してみてください。"
					, "TexureChanger内部処理エラー"
					, MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			IntPtr hWndMain = Api.GetMainWindow( tn.Handle );

			bool hasFolders = false;

			try
			{
				IntPtr ppEnumIDList;
				tnTag.shellFolder.EnumObjects(
					hWndMain
					, SH.SHCONTF.SHCONTF_FOLDERS //| SH.SHCONTF.SHCONTF_INCLUDEHIDDEN
					, out ppEnumIDList);
				if (ppEnumIDList == IntPtr.Zero)
				{
					throw new ArgumentNullException();
				}

				SH.IEnumIDList enumIDList =
					(SH.IEnumIDList) Marshal.GetTypedObjectForIUnknown(ppEnumIDList, typeof (SH.IEnumIDList));
				if (enumIDList == null)
				{
					throw new ArgumentNullException( );
				}

				IntPtr pidlChildRelative = IntPtr.Zero;
				IntPtr notUsed;
				while (enumIDList.Next(1, out pidlChildRelative, out notUsed) == (uint) ErrNo.S_OK)
				{
					if (pidlChildRelative == IntPtr.Zero)
					{
						continue;
					}

					SH.SFGAO attributesToRetrieve =
						SH.SFGAO.CAPABILITYMASK
						| SH.SFGAO.HASSUBFOLDER
						| SH.SFGAO.SHARE
						| SH.SFGAO.FOLDER
						| SH.SFGAO.FILESYSTEM;
					try
					{
						//絶対PIDLのファイル属性を取得
						IntPtr[] pidlList = { pidlChildRelative, IntPtr.Zero, IntPtr.Zero };
						tnTag.shellFolder.GetAttributesOf(
							1 //ひと組のIDLについて
							, pidlList
							, attributesToRetrieve
							);
						if( ( (ulong)attributesToRetrieve & (ulong)SH.SFGAO.FILESYSTEM ) != 0
							&& ( (ulong)attributesToRetrieve & (ulong)SH.SFGAO.FOLDER ) != 0
							&& ( (ulong)attributesToRetrieve & (ulong)SH.SFGAO.BROWSABLE ) == 0 )
						{
							hasFolders = true;
							Api.CoTaskMemFree( pidlChildRelative );
							break;
						}
					}
					catch
					{
					}
					Api.CoTaskMemFree( pidlChildRelative );
				}

				Marshal.ReleaseComObject(enumIDList);
			}
			catch
			{
				CenteredMessageBox.Show( owner
					, "フォルダツリーの子フォルダの検査に失敗しました。\n" +
					  "プログラムを再起動してみてください。"
					, "TexureChanger内部処理エラー"
					, MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
			
			if( hasFolders )
			{
				TreeNode babyNode = new TreeNode( );
				babyNode.Tag = new TreeViewTag( )
				{
					isDummy = true,
				};
				tn.Nodes.Add( babyNode );
			}
		}
		#endregion

		#region ツリーにノードを追加する
		private static TreeNode AddTreeNode(TreeViewTag tag, ref int imageCount, ImageList imageList, bool getIcons)
		{
			TreeNode tn = new TreeNode();
			tn.Text = tag.path;
			tn.Tag = tag;

			if(getIcons)
			{
				try
				{
					tn.ImageIndex         = ++imageCount;
					tn.SelectedImageIndex = ++imageCount;
					imageList.Images.Add( ExtractIcons.GetIcon( tag.pidlAbsolute, false, imageList ) ); // normal icon
					imageList.Images.Add( ExtractIcons.GetIcon( tag.pidlAbsolute, true , imageList ) ); // selected icon
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

			Icon icon = null;
			try
			{
				SH.SHGetFileInfo( pidl, 0, ref shfi, (uint)cbFileInfo, flags );
				if( shfi.hIcon == IntPtr.Zero )
					return null;

				icon = (Icon)Icon.FromHandle( shfi.hIcon ).Clone( );
				Api.DestroyIcon( shfi.hIcon );
			}
			catch
			{
			}
			return icon;
		}
		#endregion
		
		#region Get Desktop Icon
		// Retreive the desktop icon from Shell32.dll - it always appears at index 34 in all shell32 versions.
		// This is probably NOT the best way to retreive this icon, but it works - if you have a better way
		// by all means let me know..
		public static Icon GetDesktopIcon()
		{
			try
			{
				IntPtr[] handlesIconLarge = new IntPtr[ 1 ];
				IntPtr[] handlesIconSmall = new IntPtr[ 1 ];
				uint i = Api.ExtractIconEx( Environment.SystemDirectory + "\\shell32.dll", 34,
					handlesIconLarge, handlesIconSmall, 1 );

				return Icon.FromHandle( handlesIconSmall[ 0 ] );
			}
			catch
			{
			}
			return null;
		}
		#endregion

	}
	#endregion


}

