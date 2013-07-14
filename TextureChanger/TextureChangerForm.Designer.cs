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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TextureChangerForm));
            this.menuStrip1 = new MenuStrip();
            this.mniFile = new ToolStripMenuItem();
            this.mniExit = new ToolStripMenuItem();
            this.mniView = new ToolStripMenuItem();
            this.mniOption = new ToolStripMenuItem();
            this.mniRequestSaiFolder = new ToolStripMenuItem();
            this.toolStripMenuItem1 = new ToolStripSeparator();
            this.mniFirstExpandingFolder = new ToolStripMenuItem();
            this.mniFirstExpandingRecentFolder = new ToolStripMenuItem();
            this.mniFirstExpandingFixedFolder = new ToolStripMenuItem();
            this.mniOthers = new ToolStripMenuItem();
            this.mniPromptToExitProgram = new ToolStripMenuItem();
            this.mniHelp = new ToolStripMenuItem();
            this.mniAbout = new ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new ToolStripItem[] {
            this.mniFile,
            this.mniView,
            this.mniOption,
            this.mniHelp});
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(990, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mniFile
            // 
            this.mniFile.DropDownItems.AddRange(new ToolStripItem[] {
            this.mniExit});
            this.mniFile.Name = "mniFile";
            this.mniFile.Size = new Size(36, 20);
            this.mniFile.Text = "&File";
            // 
            // mniExit
            // 
            this.mniExit.Name = "mniExit";
            this.mniExit.ShortcutKeys = ((Keys)((Keys.Alt | Keys.Q)));
            this.mniExit.Size = new Size(124, 22);
            this.mniExit.Text = "E&xit";
            this.mniExit.Click += new EventHandler(this.mniExit_Click);
            // 
            // mniView
            // 
            this.mniView.Name = "mniView";
            this.mniView.Size = new Size(42, 20);
            this.mniView.Text = "&View";
            // 
            // mniOption
            // 
            this.mniOption.DropDownItems.AddRange(new ToolStripItem[] {
            this.mniRequestSaiFolder,
            this.toolStripMenuItem1,
            this.mniFirstExpandingFolder,
            this.mniOthers});
            this.mniOption.Name = "mniOption";
            this.mniOption.Size = new Size(50, 20);
            this.mniOption.Text = "&Option";
            // 
            // mniRequestSaiFolder
            // 
            this.mniRequestSaiFolder.Name = "mniRequestSaiFolder";
            this.mniRequestSaiFolder.Size = new Size(193, 22);
            this.mniRequestSaiFolder.Text = "&SAIフォルダを指定...";
            this.mniRequestSaiFolder.Click += new EventHandler(this.mniRequestSaiFolder_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(190, 6);
            // 
            // mniFirstExpandingFolder
            // 
            this.mniFirstExpandingFolder.DropDownItems.AddRange(new ToolStripItem[] {
            this.mniFirstExpandingRecentFolder,
            this.mniFirstExpandingFixedFolder});
            this.mniFirstExpandingFolder.Name = "mniFirstExpandingFolder";
            this.mniFirstExpandingFolder.Size = new Size(193, 22);
            this.mniFirstExpandingFolder.Text = "起動時に表示するフォルダ";
            // 
            // mniFirstExpandingRecentFolder
            // 
            this.mniFirstExpandingRecentFolder.Checked = true;
            this.mniFirstExpandingRecentFolder.CheckState = CheckState.Checked;
            this.mniFirstExpandingRecentFolder.Name = "mniFirstExpandingRecentFolder";
            this.mniFirstExpandingRecentFolder.Size = new Size(178, 22);
            this.mniFirstExpandingRecentFolder.Text = "前回終了時のフォルダ";
            this.mniFirstExpandingRecentFolder.Click += new EventHandler(this.mniFirstExpandingRecentFolder_Click);
            // 
            // mniFirstExpandingFixedFolder
            // 
            this.mniFirstExpandingFixedFolder.Name = "mniFirstExpandingFixedFolder";
            this.mniFirstExpandingFixedFolder.Size = new Size(178, 22);
            this.mniFirstExpandingFixedFolder.Text = "固定のフォルダを選択...";
            this.mniFirstExpandingFixedFolder.Click += new EventHandler(this.mniFirstExpandingFixedFolder_Click);
            // 
            // mniOthers
            // 
            this.mniOthers.DropDownItems.AddRange(new ToolStripItem[] {
            this.mniPromptToExitProgram});
            this.mniOthers.Name = "mniOthers";
            this.mniOthers.Size = new Size(193, 22);
            this.mniOthers.Text = "その他";
            // 
            // mniPromptToExitProgram
            // 
            this.mniPromptToExitProgram.Checked = true;
            this.mniPromptToExitProgram.CheckState = CheckState.Checked;
            this.mniPromptToExitProgram.Name = "mniPromptToExitProgram";
            this.mniPromptToExitProgram.Size = new Size(178, 22);
            this.mniPromptToExitProgram.Text = "終了前に問い合わせる";
            this.mniPromptToExitProgram.Click += new EventHandler(this.mniPromptToExitProgram_Click);
            // 
            // mniHelp
            // 
            this.mniHelp.DropDownItems.AddRange(new ToolStripItem[] {
            this.mniAbout});
            this.mniHelp.Name = "mniHelp";
            this.mniHelp.Size = new Size(40, 20);
            this.mniHelp.Text = "&Help";
            // 
            // mniAbout
            // 
            this.mniAbout.Name = "mniAbout";
            this.mniAbout.Size = new Size(98, 22);
            this.mniAbout.Text = "&about";
            this.mniAbout.Click += new EventHandler(this.mniAbout_Click);
            // 
            // TextureChangerForm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 12F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(990, 588);
            this.Controls.Add(this.menuStrip1);
            this.Cursor = Cursors.Default;
            this.DoubleBuffered = true;
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new Size(998, 615);
            this.Name = "TextureChangerForm";
            this.Text = "Texture Changer";
            this.FormClosing += new FormClosingEventHandler(this.TextureChangerForm_FormClosing);
            this.Load += new EventHandler(this.TextureChangerForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
	}
}

