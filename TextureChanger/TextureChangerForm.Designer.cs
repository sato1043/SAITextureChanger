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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextureChangerForm));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mniExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mniEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.mniEditBlotmap = new System.Windows.Forms.ToolStripMenuItem();
			this.mniEditElemap = new System.Windows.Forms.ToolStripMenuItem();
			this.mniEditBrushtex = new System.Windows.Forms.ToolStripMenuItem();
			this.mniEditPapertex = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mniView = new System.Windows.Forms.ToolStripMenuItem();
			this.mniOption = new System.Windows.Forms.ToolStripMenuItem();
			this.mniRequestSaiFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mniFirstExpandingFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.mniFirstExpandingRecentFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.mniFirstExpandingFixedFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.mniOthers = new System.Windows.Forms.ToolStripMenuItem();
			this.mniPromptToExitProgram = new System.Windows.Forms.ToolStripMenuItem();
			this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mniAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.splNorthSouth = new System.Windows.Forms.SplitContainer();
			this.splTreeList = new System.Windows.Forms.SplitContainer();
			this.splTextureImage = new System.Windows.Forms.SplitContainer();
			this.menuStrip1.SuspendLayout();
			this.splNorthSouth.Panel1.SuspendLayout();
			this.splNorthSouth.Panel2.SuspendLayout();
			this.splNorthSouth.SuspendLayout();
			this.splTreeList.SuspendLayout();
			this.splTextureImage.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFile,
            this.mniEdit,
            this.mniView,
            this.mniOption,
            this.mniHelp});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(990, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// mniFile
			// 
			this.mniFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniExit});
			this.mniFile.Name = "mniFile";
			this.mniFile.Size = new System.Drawing.Size(36, 20);
			this.mniFile.Text = "&File";
			// 
			// mniExit
			// 
			this.mniExit.Name = "mniExit";
			this.mniExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Q)));
			this.mniExit.Size = new System.Drawing.Size(124, 22);
			this.mniExit.Text = "E&xit";
			this.mniExit.Click += new System.EventHandler(this.mniExit_Click);
			// 
			// mniEdit
			// 
			this.mniEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniEditBlotmap,
            this.mniEditElemap,
            this.mniEditBrushtex,
            this.mniEditPapertex,
            this.toolStripMenuItem2});
			this.mniEdit.Name = "mniEdit";
			this.mniEdit.Size = new System.Drawing.Size(37, 20);
			this.mniEdit.Text = "&Edit";
			// 
			// mniEditBlotmap
			// 
			this.mniEditBlotmap.Checked = true;
			this.mniEditBlotmap.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mniEditBlotmap.Name = "mniEditBlotmap";
			this.mniEditBlotmap.Size = new System.Drawing.Size(171, 22);
			this.mniEditBlotmap.Text = "にじみ形状(blotmap)";
			this.mniEditBlotmap.Click += new System.EventHandler(this.mniEditTexture_Click);
			// 
			// mniEditElemap
			// 
			this.mniEditElemap.Name = "mniEditElemap";
			this.mniEditElemap.Size = new System.Drawing.Size(171, 22);
			this.mniEditElemap.Text = "筆形状(elemap)";
			this.mniEditElemap.Click += new System.EventHandler(this.mniEditTexture_Click);
			// 
			// mniEditBrushtex
			// 
			this.mniEditBrushtex.Name = "mniEditBrushtex";
			this.mniEditBrushtex.Size = new System.Drawing.Size(171, 22);
			this.mniEditBrushtex.Text = "テクスチャ(brushtex)";
			this.mniEditBrushtex.Click += new System.EventHandler(this.mniEditTexture_Click);
			// 
			// mniEditPapertex
			// 
			this.mniEditPapertex.Name = "mniEditPapertex";
			this.mniEditPapertex.Size = new System.Drawing.Size(171, 22);
			this.mniEditPapertex.Text = "用紙質感(papertex)";
			this.mniEditPapertex.Click += new System.EventHandler(this.mniEditTexture_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(168, 6);
			// 
			// mniView
			// 
			this.mniView.Name = "mniView";
			this.mniView.Size = new System.Drawing.Size(42, 20);
			this.mniView.Text = "&View";
			// 
			// mniOption
			// 
			this.mniOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniRequestSaiFolder,
            this.toolStripMenuItem1,
            this.mniFirstExpandingFolder,
            this.mniOthers});
			this.mniOption.Name = "mniOption";
			this.mniOption.Size = new System.Drawing.Size(50, 20);
			this.mniOption.Text = "&Option";
			// 
			// mniRequestSaiFolder
			// 
			this.mniRequestSaiFolder.Name = "mniRequestSaiFolder";
			this.mniRequestSaiFolder.Size = new System.Drawing.Size(193, 22);
			this.mniRequestSaiFolder.Text = "&SAIフォルダを指定...";
			this.mniRequestSaiFolder.Click += new System.EventHandler(this.mniRequestSaiFolder_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(190, 6);
			// 
			// mniFirstExpandingFolder
			// 
			this.mniFirstExpandingFolder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFirstExpandingRecentFolder,
            this.mniFirstExpandingFixedFolder});
			this.mniFirstExpandingFolder.Name = "mniFirstExpandingFolder";
			this.mniFirstExpandingFolder.Size = new System.Drawing.Size(193, 22);
			this.mniFirstExpandingFolder.Text = "起動時に表示するフォルダ";
			// 
			// mniFirstExpandingRecentFolder
			// 
			this.mniFirstExpandingRecentFolder.Checked = true;
			this.mniFirstExpandingRecentFolder.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mniFirstExpandingRecentFolder.Name = "mniFirstExpandingRecentFolder";
			this.mniFirstExpandingRecentFolder.Size = new System.Drawing.Size(178, 22);
			this.mniFirstExpandingRecentFolder.Text = "前回終了時のフォルダ";
			this.mniFirstExpandingRecentFolder.Click += new System.EventHandler(this.mniFirstExpandingRecentFolder_Click);
			// 
			// mniFirstExpandingFixedFolder
			// 
			this.mniFirstExpandingFixedFolder.Name = "mniFirstExpandingFixedFolder";
			this.mniFirstExpandingFixedFolder.Size = new System.Drawing.Size(178, 22);
			this.mniFirstExpandingFixedFolder.Text = "固定のフォルダを選択...";
			this.mniFirstExpandingFixedFolder.Click += new System.EventHandler(this.mniFirstExpandingFixedFolder_Click);
			// 
			// mniOthers
			// 
			this.mniOthers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniPromptToExitProgram});
			this.mniOthers.Name = "mniOthers";
			this.mniOthers.Size = new System.Drawing.Size(193, 22);
			this.mniOthers.Text = "その他";
			// 
			// mniPromptToExitProgram
			// 
			this.mniPromptToExitProgram.Checked = true;
			this.mniPromptToExitProgram.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mniPromptToExitProgram.Name = "mniPromptToExitProgram";
			this.mniPromptToExitProgram.Size = new System.Drawing.Size(178, 22);
			this.mniPromptToExitProgram.Text = "終了前に問い合わせる";
			this.mniPromptToExitProgram.Click += new System.EventHandler(this.mniPromptToExitProgram_Click);
			// 
			// mniHelp
			// 
			this.mniHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniAbout});
			this.mniHelp.Name = "mniHelp";
			this.mniHelp.Size = new System.Drawing.Size(40, 20);
			this.mniHelp.Text = "&Help";
			// 
			// mniAbout
			// 
			this.mniAbout.Name = "mniAbout";
			this.mniAbout.Size = new System.Drawing.Size(98, 22);
			this.mniAbout.Text = "&about";
			this.mniAbout.Click += new System.EventHandler(this.mniAbout_Click);
			// 
			// splNorthSouth
			// 
			this.splNorthSouth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splNorthSouth.BackColor = System.Drawing.SystemColors.ControlLight;
			this.splNorthSouth.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splNorthSouth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.splNorthSouth.Location = new System.Drawing.Point(0, 27);
			this.splNorthSouth.Name = "splNorthSouth";
			this.splNorthSouth.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splNorthSouth.Panel1
			// 
			this.splNorthSouth.Panel1.BackColor = System.Drawing.SystemColors.Desktop;
			this.splNorthSouth.Panel1.Controls.Add(this.splTreeList);
			// 
			// splNorthSouth.Panel2
			// 
			this.splNorthSouth.Panel2.BackColor = System.Drawing.SystemColors.Desktop;
			this.splNorthSouth.Panel2.Controls.Add(this.splTextureImage);
			this.splNorthSouth.Size = new System.Drawing.Size(990, 557);
			this.splNorthSouth.SplitterDistance = 270;
			this.splNorthSouth.SplitterWidth = 8;
			this.splNorthSouth.TabIndex = 1;
			this.splNorthSouth.TabStop = false;
			this.splNorthSouth.Paint += new System.Windows.Forms.PaintEventHandler(this.splSplitContainer_Paint);
			// 
			// splTreeList
			// 
			this.splTreeList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splTreeList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splTreeList.Location = new System.Drawing.Point(0, 0);
			this.splTreeList.Name = "splTreeList";
			this.splTreeList.Size = new System.Drawing.Size(990, 270);
			this.splTreeList.SplitterDistance = 330;
			this.splTreeList.SplitterWidth = 6;
			this.splTreeList.TabIndex = 0;
			this.splTreeList.TabStop = false;
			this.splTreeList.Paint += new System.Windows.Forms.PaintEventHandler(this.splSplitContainer_Paint);
			// 
			// splTextureImage
			// 
			this.splTextureImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splTextureImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splTextureImage.Location = new System.Drawing.Point(0, 0);
			this.splTextureImage.Name = "splTextureImage";
			this.splTextureImage.Size = new System.Drawing.Size(990, 279);
			this.splTextureImage.SplitterDistance = 330;
			this.splTextureImage.SplitterWidth = 6;
			this.splTextureImage.TabIndex = 0;
			this.splTextureImage.TabStop = false;
			this.splTextureImage.Paint += new System.Windows.Forms.PaintEventHandler(this.splSplitContainer_Paint);
			// 
			// TextureChangerForm
			// 
			this.ClientSize = new System.Drawing.Size(990, 588);
			this.Controls.Add(this.splNorthSouth);
			this.Controls.Add(this.menuStrip1);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(998, 615);
			this.Name = "TextureChangerForm";
			this.Text = "Texture Changer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TextureChangerForm_FormClosing);
			this.Load += new System.EventHandler(this.TextureChangerForm_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.splNorthSouth.Panel1.ResumeLayout(false);
			this.splNorthSouth.Panel2.ResumeLayout(false);
			this.splNorthSouth.ResumeLayout(false);
			this.splTreeList.ResumeLayout(false);
			this.splTextureImage.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MenuStrip menuStrip1;
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
		private SplitContainer splTextureImage;
	}
}

