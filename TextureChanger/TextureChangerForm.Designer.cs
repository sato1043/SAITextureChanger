using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TextureChanger
{
	partial class TextureChangerForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose( );
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent( )
		{
			this.components = new System.ComponentModel.Container( );
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( TextureChangerForm ) );
			this.mnuMainMenu = new System.Windows.Forms.MenuStrip( );
			this.mniFile = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniBackup = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniRestore = new System.Windows.Forms.ToolStripMenuItem( );
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator( );
			this.mniExit = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniEdit = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniEditBlotmap = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniEditElemap = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniEditBrushtex = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniEditPapertex = new System.Windows.Forms.ToolStripMenuItem( );
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator( );
			this.mniSelectAll = new System.Windows.Forms.ToolStripMenuItem( );
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator( );
			this.mniTextureRemove = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniView = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniFileListSelectAll = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniFileListUpdateList = new System.Windows.Forms.ToolStripMenuItem( );
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator( );
			this.mniFileListRegistToCurrent = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniFileListRegist = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniFileListRegistToBlotmap = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniFileListRegistToElemap = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniFileListRegistToBrushtex = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniFileListRegistToPapertex = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniOption = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniRequestSaiFolder = new System.Windows.Forms.ToolStripMenuItem( );
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator( );
			this.mniFirstExpandingFolder = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniFirstExpandingRecentFolder = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniFirstExpandingFixedFolder = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniOthers = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniPromptToExitProgram = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniCheckUpdateAtStartUp = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniHelp = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniCheckUpdate = new System.Windows.Forms.ToolStripMenuItem( );
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator( );
			this.mniAbout = new System.Windows.Forms.ToolStripMenuItem( );
			this.splNorthSouth = new System.Windows.Forms.SplitContainer( );
			this.splTreeList = new System.Windows.Forms.SplitContainer( );
			this.trvFolder = new TextureChanger.FolderTreeView( );
			this.lsvFileList = new System.Windows.Forms.ListView( );
			this.popFileList = new System.Windows.Forms.ContextMenuStrip( this.components );
			this.mniFileListPopupSelectAll = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniFileListPopupUpdateList = new System.Windows.Forms.ToolStripMenuItem( );
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator( );
			this.mniFileListPopupRegistToCurrent = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniFileListPopupRegist = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniFileListPopupRegistToBlotmap = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniFileListPopupRegistToElemap = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniFileListPopupRegistToBrushtex = new System.Windows.Forms.ToolStripMenuItem( );
			this.mniFileListPopupRegistToPapertex = new System.Windows.Forms.ToolStripMenuItem( );
			this.ilsFileList = new System.Windows.Forms.ImageList( this.components );
			this.stsStatus = new System.Windows.Forms.StatusStrip( );
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel( );
			this.lblTextureImages = new System.Windows.Forms.Label( );
			this.btnTextureRemove = new System.Windows.Forms.Button( );
			this.lsvTextureImages = new System.Windows.Forms.ListView( );
			this.popTextureImage = new System.Windows.Forms.ContextMenuStrip( this.components );
			this.mniTextureRemovePopup = new System.Windows.Forms.ToolStripMenuItem( );
			this.ilsTextureImage = new System.Windows.Forms.ImageList( this.components );
			this.grpEditTexture = new System.Windows.Forms.GroupBox( );
			this.rdoEditPapertex = new System.Windows.Forms.RadioButton( );
			this.rdoEditBrushtex = new System.Windows.Forms.RadioButton( );
			this.rdoEditElemap = new System.Windows.Forms.RadioButton( );
			this.rdoEditBlotmap = new System.Windows.Forms.RadioButton( );
			this.mnuMainMenu.SuspendLayout( );
			( (System.ComponentModel.ISupportInitialize)( this.splNorthSouth ) ).BeginInit( );
			this.splNorthSouth.Panel1.SuspendLayout( );
			this.splNorthSouth.Panel2.SuspendLayout( );
			this.splNorthSouth.SuspendLayout( );
			( (System.ComponentModel.ISupportInitialize)( this.splTreeList ) ).BeginInit( );
			this.splTreeList.Panel1.SuspendLayout( );
			this.splTreeList.Panel2.SuspendLayout( );
			this.splTreeList.SuspendLayout( );
			this.popFileList.SuspendLayout( );
			this.stsStatus.SuspendLayout( );
			this.popTextureImage.SuspendLayout( );
			this.grpEditTexture.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// mnuMainMenu
			// 
			this.mnuMainMenu.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.mniFile,
            this.mniEdit,
            this.mniView,
            this.mniOption,
            this.mniHelp} );
			this.mnuMainMenu.Location = new System.Drawing.Point( 0, 0 );
			this.mnuMainMenu.Name = "mnuMainMenu";
			this.mnuMainMenu.Size = new System.Drawing.Size( 990, 26 );
			this.mnuMainMenu.TabIndex = 0;
			this.mnuMainMenu.Text = "menuStrip1";
			// 
			// mniFile
			// 
			this.mniFile.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.mniBackup,
            this.mniRestore,
            this.toolStripMenuItem6,
            this.mniExit} );
			this.mniFile.Name = "mniFile";
			this.mniFile.Size = new System.Drawing.Size( 40, 22 );
			this.mniFile.Text = "&File";
			// 
			// mniBackup
			// 
			this.mniBackup.Name = "mniBackup";
			this.mniBackup.Size = new System.Drawing.Size( 208, 22 );
			this.mniBackup.Text = "現状をバックアップ(&B)";
			this.mniBackup.Click += new System.EventHandler( this.mniBackup_Click );
			// 
			// mniRestore
			// 
			this.mniRestore.Name = "mniRestore";
			this.mniRestore.Size = new System.Drawing.Size( 208, 22 );
			this.mniRestore.Text = "バックアップ状態へ復元";
			this.mniRestore.Click += new System.EventHandler( this.mniRestore_Click );
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size( 205, 6 );
			// 
			// mniExit
			// 
			this.mniExit.Name = "mniExit";
			this.mniExit.ShortcutKeys = ( (System.Windows.Forms.Keys)( ( System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Q ) ) );
			this.mniExit.Size = new System.Drawing.Size( 208, 22 );
			this.mniExit.Text = "終了(&X)";
			this.mniExit.Click += new System.EventHandler( this.mniExit_Click );
			// 
			// mniEdit
			// 
			this.mniEdit.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.mniEditBlotmap,
            this.mniEditElemap,
            this.mniEditBrushtex,
            this.mniEditPapertex,
            this.toolStripMenuItem2,
            this.mniSelectAll,
            this.toolStripMenuItem3,
            this.mniTextureRemove} );
			this.mniEdit.Name = "mniEdit";
			this.mniEdit.Size = new System.Drawing.Size( 42, 22 );
			this.mniEdit.Text = "&Edit";
			// 
			// mniEditBlotmap
			// 
			this.mniEditBlotmap.Checked = true;
			this.mniEditBlotmap.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mniEditBlotmap.Name = "mniEditBlotmap";
			this.mniEditBlotmap.Size = new System.Drawing.Size( 244, 22 );
			this.mniEditBlotmap.Text = "にじみ形状(blotmap)";
			this.mniEditBlotmap.Click += new System.EventHandler( this.mniEditTexture_Click );
			// 
			// mniEditElemap
			// 
			this.mniEditElemap.Name = "mniEditElemap";
			this.mniEditElemap.Size = new System.Drawing.Size( 244, 22 );
			this.mniEditElemap.Text = "筆形状(elemap)";
			this.mniEditElemap.Click += new System.EventHandler( this.mniEditTexture_Click );
			// 
			// mniEditBrushtex
			// 
			this.mniEditBrushtex.Name = "mniEditBrushtex";
			this.mniEditBrushtex.Size = new System.Drawing.Size( 244, 22 );
			this.mniEditBrushtex.Text = "テクスチャ(brushtex)";
			this.mniEditBrushtex.Click += new System.EventHandler( this.mniEditTexture_Click );
			// 
			// mniEditPapertex
			// 
			this.mniEditPapertex.Name = "mniEditPapertex";
			this.mniEditPapertex.Size = new System.Drawing.Size( 244, 22 );
			this.mniEditPapertex.Text = "用紙質感(papertex)";
			this.mniEditPapertex.Click += new System.EventHandler( this.mniEditTexture_Click );
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size( 241, 6 );
			// 
			// mniSelectAll
			// 
			this.mniSelectAll.Name = "mniSelectAll";
			this.mniSelectAll.Size = new System.Drawing.Size( 244, 22 );
			this.mniSelectAll.Text = "すべてのテクスチャを選択";
			this.mniSelectAll.Click += new System.EventHandler( this.mniSelectAll_Click );
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size( 241, 6 );
			// 
			// mniTextureRemove
			// 
			this.mniTextureRemove.Name = "mniTextureRemove";
			this.mniTextureRemove.Size = new System.Drawing.Size( 244, 22 );
			this.mniTextureRemove.Text = "選択したテクスチャをゴミ箱へ";
			this.mniTextureRemove.Click += new System.EventHandler( this.mniTextureRemove_Click );
			// 
			// mniView
			// 
			this.mniView.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.mniFileListSelectAll,
            this.mniFileListUpdateList,
            this.toolStripMenuItem5,
            this.mniFileListRegistToCurrent,
            this.mniFileListRegist} );
			this.mniView.Name = "mniView";
			this.mniView.Size = new System.Drawing.Size( 48, 22 );
			this.mniView.Text = "&View";
			// 
			// mniFileListSelectAll
			// 
			this.mniFileListSelectAll.Name = "mniFileListSelectAll";
			this.mniFileListSelectAll.Size = new System.Drawing.Size( 304, 22 );
			this.mniFileListSelectAll.Text = "すべてのファイルを選択";
			this.mniFileListSelectAll.Click += new System.EventHandler( this.mniFileListSelectAll_Click );
			// 
			// mniFileListUpdateList
			// 
			this.mniFileListUpdateList.Name = "mniFileListUpdateList";
			this.mniFileListUpdateList.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.mniFileListUpdateList.Size = new System.Drawing.Size( 304, 22 );
			this.mniFileListUpdateList.Text = "最新の情報に更新";
			this.mniFileListUpdateList.Click += new System.EventHandler( this.mniFileListUpdateList_Click );
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size( 301, 6 );
			// 
			// mniFileListRegistToCurrent
			// 
			this.mniFileListRegistToCurrent.Name = "mniFileListRegistToCurrent";
			this.mniFileListRegistToCurrent.Size = new System.Drawing.Size( 304, 22 );
			this.mniFileListRegistToCurrent.Text = "選択ファイルを編集中のテクスチャへ登録";
			this.mniFileListRegistToCurrent.Click += new System.EventHandler( this.mniFileListRegistToCurrent_Click );
			// 
			// mniFileListRegist
			// 
			this.mniFileListRegist.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.mniFileListRegistToBlotmap,
            this.mniFileListRegistToElemap,
            this.mniFileListRegistToBrushtex,
            this.mniFileListRegistToPapertex} );
			this.mniFileListRegist.Name = "mniFileListRegist";
			this.mniFileListRegist.Size = new System.Drawing.Size( 304, 22 );
			this.mniFileListRegist.Text = "選択ファイルを登録";
			// 
			// mniFileListRegistToBlotmap
			// 
			this.mniFileListRegistToBlotmap.Name = "mniFileListRegistToBlotmap";
			this.mniFileListRegistToBlotmap.Size = new System.Drawing.Size( 196, 22 );
			this.mniFileListRegistToBlotmap.Text = "「にじみ」へ登録";
			this.mniFileListRegistToBlotmap.Click += new System.EventHandler( this.mniFileListRegistToBlotmap_Click );
			// 
			// mniFileListRegistToElemap
			// 
			this.mniFileListRegistToElemap.Name = "mniFileListRegistToElemap";
			this.mniFileListRegistToElemap.Size = new System.Drawing.Size( 196, 22 );
			this.mniFileListRegistToElemap.Text = "「筆」へ登録";
			this.mniFileListRegistToElemap.Click += new System.EventHandler( this.mniFileListRegistToElemap_Click );
			// 
			// mniFileListRegistToBrushtex
			// 
			this.mniFileListRegistToBrushtex.Name = "mniFileListRegistToBrushtex";
			this.mniFileListRegistToBrushtex.Size = new System.Drawing.Size( 196, 22 );
			this.mniFileListRegistToBrushtex.Text = "「テクスチャ」へ登録";
			this.mniFileListRegistToBrushtex.Click += new System.EventHandler( this.mniFileListRegistToBrushtex_Click );
			// 
			// mniFileListRegistToPapertex
			// 
			this.mniFileListRegistToPapertex.Name = "mniFileListRegistToPapertex";
			this.mniFileListRegistToPapertex.Size = new System.Drawing.Size( 196, 22 );
			this.mniFileListRegistToPapertex.Text = "「用紙質感」へ登録";
			this.mniFileListRegistToPapertex.Click += new System.EventHandler( this.mniFileListRegistToPapertex_Click );
			// 
			// mniOption
			// 
			this.mniOption.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.mniRequestSaiFolder,
            this.toolStripMenuItem1,
            this.mniFirstExpandingFolder,
            this.mniOthers} );
			this.mniOption.Name = "mniOption";
			this.mniOption.Size = new System.Drawing.Size( 58, 22 );
			this.mniOption.Text = "&Option";
			// 
			// mniRequestSaiFolder
			// 
			this.mniRequestSaiFolder.Name = "mniRequestSaiFolder";
			this.mniRequestSaiFolder.Size = new System.Drawing.Size( 220, 22 );
			this.mniRequestSaiFolder.Text = "&SAIフォルダを指定...";
			this.mniRequestSaiFolder.Click += new System.EventHandler( this.mniRequestSaiFolder_Click );
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size( 217, 6 );
			// 
			// mniFirstExpandingFolder
			// 
			this.mniFirstExpandingFolder.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.mniFirstExpandingRecentFolder,
            this.mniFirstExpandingFixedFolder} );
			this.mniFirstExpandingFolder.Name = "mniFirstExpandingFolder";
			this.mniFirstExpandingFolder.Size = new System.Drawing.Size( 220, 22 );
			this.mniFirstExpandingFolder.Text = "起動時に表示するフォルダ";
			// 
			// mniFirstExpandingRecentFolder
			// 
			this.mniFirstExpandingRecentFolder.Checked = true;
			this.mniFirstExpandingRecentFolder.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mniFirstExpandingRecentFolder.Name = "mniFirstExpandingRecentFolder";
			this.mniFirstExpandingRecentFolder.Size = new System.Drawing.Size( 208, 22 );
			this.mniFirstExpandingRecentFolder.Text = "前回終了時のフォルダ";
			this.mniFirstExpandingRecentFolder.Click += new System.EventHandler( this.mniFirstExpandingRecentFolder_Click );
			// 
			// mniFirstExpandingFixedFolder
			// 
			this.mniFirstExpandingFixedFolder.Name = "mniFirstExpandingFixedFolder";
			this.mniFirstExpandingFixedFolder.Size = new System.Drawing.Size( 208, 22 );
			this.mniFirstExpandingFixedFolder.Text = "固定のフォルダを選択...";
			this.mniFirstExpandingFixedFolder.Click += new System.EventHandler( this.mniFirstExpandingFixedFolder_Click );
			// 
			// mniOthers
			// 
			this.mniOthers.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.mniPromptToExitProgram,
            this.mniCheckUpdateAtStartUp} );
			this.mniOthers.Name = "mniOthers";
			this.mniOthers.Size = new System.Drawing.Size( 220, 22 );
			this.mniOthers.Text = "その他";
			// 
			// mniPromptToExitProgram
			// 
			this.mniPromptToExitProgram.Checked = true;
			this.mniPromptToExitProgram.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mniPromptToExitProgram.Name = "mniPromptToExitProgram";
			this.mniPromptToExitProgram.Size = new System.Drawing.Size( 208, 22 );
			this.mniPromptToExitProgram.Text = "終了前に問い合わせる";
			this.mniPromptToExitProgram.Click += new System.EventHandler( this.mniPromptToExitProgram_Click );
			// 
			// mniCheckUpdateAtStartUp
			// 
			this.mniCheckUpdateAtStartUp.Checked = true;
			this.mniCheckUpdateAtStartUp.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mniCheckUpdateAtStartUp.Name = "mniCheckUpdateAtStartUp";
			this.mniCheckUpdateAtStartUp.Size = new System.Drawing.Size( 208, 22 );
			this.mniCheckUpdateAtStartUp.Text = "起動時に更新を確認する";
			this.mniCheckUpdateAtStartUp.Click += new System.EventHandler( this.mniCheckUpdateAtStartUp_Click );
			// 
			// mniHelp
			// 
			this.mniHelp.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.mniCheckUpdate,
            this.toolStripMenuItem7,
            this.mniAbout} );
			this.mniHelp.Name = "mniHelp";
			this.mniHelp.Size = new System.Drawing.Size( 46, 22 );
			this.mniHelp.Text = "&Help";
			// 
			// mniCheckUpdate
			// 
			this.mniCheckUpdate.Name = "mniCheckUpdate";
			this.mniCheckUpdate.Size = new System.Drawing.Size( 167, 22 );
			this.mniCheckUpdate.Text = "更新を確認(&U)...";
			this.mniCheckUpdate.Click += new System.EventHandler( this.mniCheckUpdate_Click );
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size( 164, 6 );
			// 
			// mniAbout
			// 
			this.mniAbout.Name = "mniAbout";
			this.mniAbout.Size = new System.Drawing.Size( 167, 22 );
			this.mniAbout.Text = "&about";
			this.mniAbout.Click += new System.EventHandler( this.mniAbout_Click );
			// 
			// splNorthSouth
			// 
			this.splNorthSouth.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.splNorthSouth.BackColor = System.Drawing.SystemColors.ControlLight;
			this.splNorthSouth.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splNorthSouth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.splNorthSouth.Location = new System.Drawing.Point( 0, 27 );
			this.splNorthSouth.Name = "splNorthSouth";
			this.splNorthSouth.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splNorthSouth.Panel1
			// 
			this.splNorthSouth.Panel1.BackColor = System.Drawing.SystemColors.Desktop;
			this.splNorthSouth.Panel1.Controls.Add( this.splTreeList );
			// 
			// splNorthSouth.Panel2
			// 
			this.splNorthSouth.Panel2.BackColor = System.Drawing.SystemColors.ControlLight;
			this.splNorthSouth.Panel2.Controls.Add( this.stsStatus );
			this.splNorthSouth.Panel2.Controls.Add( this.lblTextureImages );
			this.splNorthSouth.Panel2.Controls.Add( this.btnTextureRemove );
			this.splNorthSouth.Panel2.Controls.Add( this.lsvTextureImages );
			this.splNorthSouth.Panel2.Controls.Add( this.grpEditTexture );
			this.splNorthSouth.Size = new System.Drawing.Size( 990, 557 );
			this.splNorthSouth.SplitterDistance = 270;
			this.splNorthSouth.SplitterWidth = 8;
			this.splNorthSouth.TabIndex = 1;
			this.splNorthSouth.TabStop = false;
			this.splNorthSouth.Paint += new System.Windows.Forms.PaintEventHandler( this.splSplitContainer_Paint );
			// 
			// splTreeList
			// 
			this.splTreeList.BackColor = System.Drawing.SystemColors.ControlLight;
			this.splTreeList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splTreeList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splTreeList.Location = new System.Drawing.Point( 0, 0 );
			this.splTreeList.Name = "splTreeList";
			// 
			// splTreeList.Panel1
			// 
			this.splTreeList.Panel1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.splTreeList.Panel1.Controls.Add( this.trvFolder );
			// 
			// splTreeList.Panel2
			// 
			this.splTreeList.Panel2.BackColor = System.Drawing.SystemColors.Desktop;
			this.splTreeList.Panel2.Controls.Add( this.lsvFileList );
			this.splTreeList.Size = new System.Drawing.Size( 990, 270 );
			this.splTreeList.SplitterDistance = 330;
			this.splTreeList.SplitterWidth = 6;
			this.splTreeList.TabIndex = 0;
			this.splTreeList.TabStop = false;
			this.splTreeList.Paint += new System.Windows.Forms.PaintEventHandler( this.splSplitContainer_Paint );
			// 
			// trvFolder
			// 
			this.trvFolder.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.trvFolder.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.trvFolder.Location = new System.Drawing.Point( 0, 0 );
			this.trvFolder.Name = "trvFolder";
			this.trvFolder.Size = new System.Drawing.Size( 326, 265 );
			this.trvFolder.TabIndex = 0;
			this.trvFolder.AfterSelect += new System.Windows.Forms.TreeViewEventHandler( this.trvFolder_AfterSelect );
			// 
			// lsvFileList
			// 
			this.lsvFileList.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.lsvFileList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lsvFileList.ContextMenuStrip = this.popFileList;
			this.lsvFileList.LargeImageList = this.ilsFileList;
			this.lsvFileList.Location = new System.Drawing.Point( 0, 0 );
			this.lsvFileList.Margin = new System.Windows.Forms.Padding( 0 );
			this.lsvFileList.Name = "lsvFileList";
			this.lsvFileList.Size = new System.Drawing.Size( 644, 265 );
			this.lsvFileList.TabIndex = 0;
			this.lsvFileList.UseCompatibleStateImageBehavior = false;
			// 
			// popFileList
			// 
			this.popFileList.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.mniFileListPopupSelectAll,
            this.mniFileListPopupUpdateList,
            this.toolStripMenuItem4,
            this.mniFileListPopupRegistToCurrent,
            this.mniFileListPopupRegist} );
			this.popFileList.Name = "popFileList";
			this.popFileList.Size = new System.Drawing.Size( 305, 98 );
			// 
			// mniFileListPopupSelectAll
			// 
			this.mniFileListPopupSelectAll.Name = "mniFileListPopupSelectAll";
			this.mniFileListPopupSelectAll.Size = new System.Drawing.Size( 304, 22 );
			this.mniFileListPopupSelectAll.Text = "すべて選択";
			this.mniFileListPopupSelectAll.Click += new System.EventHandler( this.mniFileListPopupSelectAll_Click );
			// 
			// mniFileListPopupUpdateList
			// 
			this.mniFileListPopupUpdateList.Name = "mniFileListPopupUpdateList";
			this.mniFileListPopupUpdateList.Size = new System.Drawing.Size( 304, 22 );
			this.mniFileListPopupUpdateList.Text = "最新の情報に更新";
			this.mniFileListPopupUpdateList.Click += new System.EventHandler( this.mniFileListPopupUpdateList_Click );
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size( 301, 6 );
			// 
			// mniFileListPopupRegistToCurrent
			// 
			this.mniFileListPopupRegistToCurrent.Name = "mniFileListPopupRegistToCurrent";
			this.mniFileListPopupRegistToCurrent.Size = new System.Drawing.Size( 304, 22 );
			this.mniFileListPopupRegistToCurrent.Text = "選択ファイルを編集中のテクスチャへ登録";
			this.mniFileListPopupRegistToCurrent.Click += new System.EventHandler( this.mniFileListPopupRegistToCurrent_Click );
			// 
			// mniFileListPopupRegist
			// 
			this.mniFileListPopupRegist.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.mniFileListPopupRegistToBlotmap,
            this.mniFileListPopupRegistToElemap,
            this.mniFileListPopupRegistToBrushtex,
            this.mniFileListPopupRegistToPapertex} );
			this.mniFileListPopupRegist.Name = "mniFileListPopupRegist";
			this.mniFileListPopupRegist.Size = new System.Drawing.Size( 304, 22 );
			this.mniFileListPopupRegist.Text = "選択ファイルを登録";
			// 
			// mniFileListPopupRegistToBlotmap
			// 
			this.mniFileListPopupRegistToBlotmap.Name = "mniFileListPopupRegistToBlotmap";
			this.mniFileListPopupRegistToBlotmap.Size = new System.Drawing.Size( 196, 22 );
			this.mniFileListPopupRegistToBlotmap.Text = "「にじみ」へ登録";
			this.mniFileListPopupRegistToBlotmap.Click += new System.EventHandler( this.mniFileListPopupRegistToBlotmap_Click );
			// 
			// mniFileListPopupRegistToElemap
			// 
			this.mniFileListPopupRegistToElemap.Name = "mniFileListPopupRegistToElemap";
			this.mniFileListPopupRegistToElemap.Size = new System.Drawing.Size( 196, 22 );
			this.mniFileListPopupRegistToElemap.Text = "「筆」へ登録";
			this.mniFileListPopupRegistToElemap.Click += new System.EventHandler( this.mniFileListPopupRegistToElemap_Click );
			// 
			// mniFileListPopupRegistToBrushtex
			// 
			this.mniFileListPopupRegistToBrushtex.Name = "mniFileListPopupRegistToBrushtex";
			this.mniFileListPopupRegistToBrushtex.Size = new System.Drawing.Size( 196, 22 );
			this.mniFileListPopupRegistToBrushtex.Text = "「テクスチャ」に登録";
			this.mniFileListPopupRegistToBrushtex.Click += new System.EventHandler( this.mniFileListPopupRegistToBrushtex_Click );
			// 
			// mniFileListPopupRegistToPapertex
			// 
			this.mniFileListPopupRegistToPapertex.Name = "mniFileListPopupRegistToPapertex";
			this.mniFileListPopupRegistToPapertex.Size = new System.Drawing.Size( 196, 22 );
			this.mniFileListPopupRegistToPapertex.Text = "「用紙質感」に登録";
			this.mniFileListPopupRegistToPapertex.Click += new System.EventHandler( this.mniFileListPopupRegistToPapertex_Click );
			// 
			// ilsFileList
			// 
			this.ilsFileList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.ilsFileList.ImageSize = new System.Drawing.Size( 256, 256 );
			this.ilsFileList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// stsStatus
			// 
			this.stsStatus.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus} );
			this.stsStatus.Location = new System.Drawing.Point( 0, 253 );
			this.stsStatus.Name = "stsStatus";
			this.stsStatus.Size = new System.Drawing.Size( 986, 22 );
			this.stsStatus.TabIndex = 6;
			// 
			// lblStatus
			// 
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size( 0, 17 );
			// 
			// lblTextureImages
			// 
			this.lblTextureImages.AutoSize = true;
			this.lblTextureImages.Font = new System.Drawing.Font( "MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 128 ) ) );
			this.lblTextureImages.Location = new System.Drawing.Point( 3, 22 );
			this.lblTextureImages.Name = "lblTextureImages";
			this.lblTextureImages.Size = new System.Drawing.Size( 151, 16 );
			this.lblTextureImages.TabIndex = 5;
			this.lblTextureImages.Text = "↓SAI登録中の画像";
			// 
			// btnTextureRemove
			// 
			this.btnTextureRemove.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.btnTextureRemove.Location = new System.Drawing.Point( 817, 20 );
			this.btnTextureRemove.Name = "btnTextureRemove";
			this.btnTextureRemove.Size = new System.Drawing.Size( 150, 24 );
			this.btnTextureRemove.TabIndex = 4;
			this.btnTextureRemove.Text = "↓選択テクスチャをゴミ箱へ";
			this.btnTextureRemove.UseVisualStyleBackColor = true;
			this.btnTextureRemove.Click += new System.EventHandler( this.btnTextureRemove_Click );
			// 
			// lsvTextureImages
			// 
			this.lsvTextureImages.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.lsvTextureImages.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lsvTextureImages.ContextMenuStrip = this.popTextureImage;
			this.lsvTextureImages.LargeImageList = this.ilsTextureImage;
			this.lsvTextureImages.Location = new System.Drawing.Point( 0, 54 );
			this.lsvTextureImages.Margin = new System.Windows.Forms.Padding( 0 );
			this.lsvTextureImages.Name = "lsvTextureImages";
			this.lsvTextureImages.Size = new System.Drawing.Size( 986, 187 );
			this.lsvTextureImages.TabIndex = 3;
			this.lsvTextureImages.UseCompatibleStateImageBehavior = false;
			// 
			// popTextureImage
			// 
			this.popTextureImage.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.mniTextureRemovePopup} );
			this.popTextureImage.Name = "popTextureImage";
			this.popTextureImage.Size = new System.Drawing.Size( 245, 26 );
			// 
			// mniTextureRemovePopup
			// 
			this.mniTextureRemovePopup.Name = "mniTextureRemovePopup";
			this.mniTextureRemovePopup.Size = new System.Drawing.Size( 244, 22 );
			this.mniTextureRemovePopup.Text = "選択したテクスチャをゴミ箱へ";
			this.mniTextureRemovePopup.Click += new System.EventHandler( this.mniTextureRemovePopup_Click );
			// 
			// ilsTextureImage
			// 
			this.ilsTextureImage.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.ilsTextureImage.ImageSize = new System.Drawing.Size( 200, 200 );
			this.ilsTextureImage.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// grpEditTexture
			// 
			this.grpEditTexture.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.grpEditTexture.Controls.Add( this.rdoEditPapertex );
			this.grpEditTexture.Controls.Add( this.rdoEditBrushtex );
			this.grpEditTexture.Controls.Add( this.rdoEditElemap );
			this.grpEditTexture.Controls.Add( this.rdoEditBlotmap );
			this.grpEditTexture.Location = new System.Drawing.Point( 167, 3 );
			this.grpEditTexture.MinimumSize = new System.Drawing.Size( 635, 48 );
			this.grpEditTexture.Name = "grpEditTexture";
			this.grpEditTexture.Padding = new System.Windows.Forms.Padding( 3, 6, 3, 3 );
			this.grpEditTexture.Size = new System.Drawing.Size( 635, 48 );
			this.grpEditTexture.TabIndex = 2;
			this.grpEditTexture.TabStop = false;
			this.grpEditTexture.Text = "編集中の種別の選択";
			// 
			// rdoEditPapertex
			// 
			this.rdoEditPapertex.Appearance = System.Windows.Forms.Appearance.Button;
			this.rdoEditPapertex.AutoEllipsis = true;
			this.rdoEditPapertex.Location = new System.Drawing.Point( 471, 17 );
			this.rdoEditPapertex.Margin = new System.Windows.Forms.Padding( 2 );
			this.rdoEditPapertex.Name = "rdoEditPapertex";
			this.rdoEditPapertex.Size = new System.Drawing.Size( 150, 24 );
			this.rdoEditPapertex.TabIndex = 3;
			this.rdoEditPapertex.TabStop = true;
			this.rdoEditPapertex.Tag = "用紙質感(papertex)";
			this.rdoEditPapertex.Text = "用紙質感(papertex)";
			this.rdoEditPapertex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rdoEditPapertex.UseVisualStyleBackColor = true;
			this.rdoEditPapertex.Click += new System.EventHandler( this.rdoEditTexture_Click );
			// 
			// rdoEditBrushtex
			// 
			this.rdoEditBrushtex.Appearance = System.Windows.Forms.Appearance.Button;
			this.rdoEditBrushtex.AutoEllipsis = true;
			this.rdoEditBrushtex.Location = new System.Drawing.Point( 317, 17 );
			this.rdoEditBrushtex.Margin = new System.Windows.Forms.Padding( 2 );
			this.rdoEditBrushtex.Name = "rdoEditBrushtex";
			this.rdoEditBrushtex.Size = new System.Drawing.Size( 150, 24 );
			this.rdoEditBrushtex.TabIndex = 2;
			this.rdoEditBrushtex.TabStop = true;
			this.rdoEditBrushtex.Tag = "テクスチャ(brushtex)";
			this.rdoEditBrushtex.Text = "テクスチャ(brushtex)";
			this.rdoEditBrushtex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rdoEditBrushtex.UseVisualStyleBackColor = true;
			this.rdoEditBrushtex.Click += new System.EventHandler( this.rdoEditTexture_Click );
			// 
			// rdoEditElemap
			// 
			this.rdoEditElemap.Appearance = System.Windows.Forms.Appearance.Button;
			this.rdoEditElemap.AutoEllipsis = true;
			this.rdoEditElemap.Location = new System.Drawing.Point( 163, 17 );
			this.rdoEditElemap.Margin = new System.Windows.Forms.Padding( 2 );
			this.rdoEditElemap.Name = "rdoEditElemap";
			this.rdoEditElemap.Size = new System.Drawing.Size( 150, 24 );
			this.rdoEditElemap.TabIndex = 1;
			this.rdoEditElemap.TabStop = true;
			this.rdoEditElemap.Tag = "筆形状(elemap)";
			this.rdoEditElemap.Text = "筆形状(elemap)";
			this.rdoEditElemap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rdoEditElemap.UseVisualStyleBackColor = true;
			this.rdoEditElemap.Click += new System.EventHandler( this.rdoEditTexture_Click );
			// 
			// rdoEditBlotmap
			// 
			this.rdoEditBlotmap.Appearance = System.Windows.Forms.Appearance.Button;
			this.rdoEditBlotmap.AutoEllipsis = true;
			this.rdoEditBlotmap.Checked = true;
			this.rdoEditBlotmap.Location = new System.Drawing.Point( 6, 17 );
			this.rdoEditBlotmap.Margin = new System.Windows.Forms.Padding( 2 );
			this.rdoEditBlotmap.Name = "rdoEditBlotmap";
			this.rdoEditBlotmap.Size = new System.Drawing.Size( 150, 24 );
			this.rdoEditBlotmap.TabIndex = 0;
			this.rdoEditBlotmap.TabStop = true;
			this.rdoEditBlotmap.Tag = "にじみ形状(blotmap)";
			this.rdoEditBlotmap.Text = "にじみ形状(blotmap)";
			this.rdoEditBlotmap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rdoEditBlotmap.UseVisualStyleBackColor = true;
			this.rdoEditBlotmap.Click += new System.EventHandler( this.rdoEditTexture_Click );
			// 
			// TextureChangerForm
			// 
			this.ClientSize = new System.Drawing.Size( 990, 588 );
			this.Controls.Add( this.splNorthSouth );
			this.Controls.Add( this.mnuMainMenu );
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.DoubleBuffered = true;
			this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
			this.MainMenuStrip = this.mnuMainMenu;
			this.MinimumSize = new System.Drawing.Size( 998, 615 );
			this.Name = "TextureChangerForm";
			this.Text = "Texture Changer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.TextureChangerForm_FormClosing );
			this.Load += new System.EventHandler( this.TextureChangerForm_Load );
			this.mnuMainMenu.ResumeLayout( false );
			this.mnuMainMenu.PerformLayout( );
			this.splNorthSouth.Panel1.ResumeLayout( false );
			this.splNorthSouth.Panel2.ResumeLayout( false );
			this.splNorthSouth.Panel2.PerformLayout( );
			( (System.ComponentModel.ISupportInitialize)( this.splNorthSouth ) ).EndInit( );
			this.splNorthSouth.ResumeLayout( false );
			this.splTreeList.Panel1.ResumeLayout( false );
			this.splTreeList.Panel2.ResumeLayout( false );
			( (System.ComponentModel.ISupportInitialize)( this.splTreeList ) ).EndInit( );
			this.splTreeList.ResumeLayout( false );
			this.popFileList.ResumeLayout( false );
			this.stsStatus.ResumeLayout( false );
			this.stsStatus.PerformLayout( );
			this.popTextureImage.ResumeLayout( false );
			this.grpEditTexture.ResumeLayout( false );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private MenuStrip mnuMainMenu;
		private ToolStripMenuItem mniFile;
		private ToolStripMenuItem mniView;
		private ToolStripMenuItem mniOption;
		private ToolStripMenuItem mniHelp;
		private ToolStripMenuItem mniExit;
		private ToolStripMenuItem mniAbout;
        private ToolStripMenuItem mniRequestSaiFolder;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem mniFirstExpandingFolder;
        private ToolStripMenuItem mniFirstExpandingRecentFolder;
        private ToolStripMenuItem mniFirstExpandingFixedFolder;
        private ToolStripMenuItem mniOthers;
        private ToolStripMenuItem mniPromptToExitProgram;
		private ToolStripMenuItem mniEdit;
		private ToolStripMenuItem mniEditBlotmap;
		private ToolStripMenuItem mniEditElemap;
		private ToolStripMenuItem mniEditBrushtex;
		private ToolStripMenuItem mniEditPapertex;
		private ToolStripSeparator toolStripMenuItem2;
		private SplitContainer splNorthSouth;
		private SplitContainer splTreeList;
		private GroupBox grpEditTexture;
		private RadioButton rdoEditPapertex;
		private RadioButton rdoEditBrushtex;
		private RadioButton rdoEditElemap;
		private RadioButton rdoEditBlotmap;
		private ListView lsvTextureImages;
		private ImageList ilsTextureImage;
		private ToolStripMenuItem mniTextureRemove;
		private ToolStripSeparator toolStripMenuItem3;
		private ToolStripMenuItem mniSelectAll;
		private ContextMenuStrip popTextureImage;
		private ToolStripMenuItem mniTextureRemovePopup;
		private Button btnTextureRemove;
		private FolderTreeView trvFolder;
		private ListView lsvFileList;
		private ImageList ilsFileList;
		private ContextMenuStrip popFileList;
		private ToolStripMenuItem mniFileListPopupSelectAll;
		private ToolStripSeparator toolStripMenuItem4;
		private ToolStripMenuItem mniFileListPopupRegist;
		private ToolStripMenuItem mniFileListPopupRegistToBlotmap;
		private ToolStripMenuItem mniFileListPopupRegistToElemap;
		private ToolStripMenuItem mniFileListPopupRegistToBrushtex;
		private ToolStripMenuItem mniFileListPopupRegistToPapertex;
		private ToolStripMenuItem mniFileListSelectAll;
		private ToolStripSeparator toolStripMenuItem5;
		private ToolStripMenuItem mniFileListRegist;
		private ToolStripMenuItem mniFileListRegistToBlotmap;
		private ToolStripMenuItem mniFileListRegistToElemap;
		private ToolStripMenuItem mniFileListRegistToBrushtex;
		private ToolStripMenuItem mniFileListRegistToPapertex;
		private ToolStripMenuItem mniFileListRegistToCurrent;
		private ToolStripMenuItem mniFileListPopupRegistToCurrent;
        private ToolStripMenuItem mniFileListUpdateList;
        private ToolStripMenuItem mniFileListPopupUpdateList;
        private Label lblTextureImages;
        private StatusStrip stsStatus;
        private ToolStripStatusLabel lblStatus;
        private ToolStripMenuItem mniBackup;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripMenuItem mniRestore;
		private ToolStripMenuItem mniCheckUpdateAtStartUp;
		private ToolStripMenuItem mniCheckUpdate;
		private ToolStripSeparator toolStripMenuItem7;
	}
}

