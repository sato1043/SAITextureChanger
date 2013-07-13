namespace TextureChanger
{
	partial class TextureChangerForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFile,
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
            // TextureChangerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 588);
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
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem mniFile;
		private System.Windows.Forms.ToolStripMenuItem mniView;
		private System.Windows.Forms.ToolStripMenuItem mniOption;
		private System.Windows.Forms.ToolStripMenuItem mniHelp;
		private System.Windows.Forms.ToolStripMenuItem mniExit;
		private System.Windows.Forms.ToolStripMenuItem mniAbout;
        private System.Windows.Forms.ToolStripMenuItem mniRequestSaiFolder;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mniFirstExpandingFolder;
        private System.Windows.Forms.ToolStripMenuItem mniFirstExpandingRecentFolder;
        private System.Windows.Forms.ToolStripMenuItem mniFirstExpandingFixedFolder;
        private System.Windows.Forms.ToolStripMenuItem mniOthers;
        private System.Windows.Forms.ToolStripMenuItem mniPromptToExitProgram;
	}
}

