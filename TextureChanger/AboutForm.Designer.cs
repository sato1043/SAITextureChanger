namespace TextureChanger
{
	partial class AboutForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose( );
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( )
		{
			this.pctAppIcon = new System.Windows.Forms.PictureBox( );
			this.lblAppName = new System.Windows.Forms.Label( );
			this.lblAppVers = new System.Windows.Forms.Label( );
			this.lblAppLicense = new System.Windows.Forms.Label( );
			( (System.ComponentModel.ISupportInitialize)( this.pctAppIcon ) ).BeginInit( );
			this.SuspendLayout( );
			// 
			// pctAppIcon
			// 
			this.pctAppIcon.Location = new System.Drawing.Point( 13, 13 );
			this.pctAppIcon.MaximumSize = new System.Drawing.Size( 64, 64 );
			this.pctAppIcon.MinimumSize = new System.Drawing.Size( 64, 64 );
			this.pctAppIcon.Name = "pctAppIcon";
			this.pctAppIcon.Size = new System.Drawing.Size( 64, 64 );
			this.pctAppIcon.TabIndex = 0;
			this.pctAppIcon.TabStop = false;
			this.pctAppIcon.Click += new System.EventHandler( this.AboutForm_Click );
			// 
			// lblAppName
			// 
			this.lblAppName.Font = new System.Drawing.Font( "メイリオ", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 128 ) ) );
			this.lblAppName.Location = new System.Drawing.Point( 96, 17 );
			this.lblAppName.Name = "lblAppName";
			this.lblAppName.Size = new System.Drawing.Size( 285, 40 );
			this.lblAppName.TabIndex = 1;
			this.lblAppName.Text = "lblAppName";
			this.lblAppName.Click += new System.EventHandler( this.AboutForm_Click );
			// 
			// lblAppVers
			// 
			this.lblAppVers.Font = new System.Drawing.Font( "メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 128 ) ) );
			this.lblAppVers.Location = new System.Drawing.Point( 96, 70 );
			this.lblAppVers.Name = "lblAppVers";
			this.lblAppVers.Size = new System.Drawing.Size( 285, 40 );
			this.lblAppVers.TabIndex = 2;
			this.lblAppVers.Text = "lblAppVers";
			this.lblAppVers.Click += new System.EventHandler( this.AboutForm_Click );
			// 
			// lblAppLicense
			// 
			this.lblAppLicense.Font = new System.Drawing.Font( "メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 128 ) ) );
			this.lblAppLicense.Location = new System.Drawing.Point( 96, 110 );
			this.lblAppLicense.Name = "lblAppLicense";
			this.lblAppLicense.Size = new System.Drawing.Size( 285, 40 );
			this.lblAppLicense.TabIndex = 3;
			this.lblAppLicense.Text = "lblAppLicense";
			this.lblAppLicense.Click += new System.EventHandler( this.AboutForm_Click );
			// 
			// AboutForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 394, 172 );
			this.ControlBox = false;
			this.Controls.Add( this.lblAppLicense );
			this.Controls.Add( this.lblAppVers );
			this.Controls.Add( this.lblAppName );
			this.Controls.Add( this.pctAppIcon );
			this.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size( 400, 200 );
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size( 400, 200 );
			this.Name = "AboutForm";
			this.Padding = new System.Windows.Forms.Padding( 10 );
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About Texture Changer...";
			this.Load += new System.EventHandler( this.AboutForm_Load );
			this.Click += new System.EventHandler( this.AboutForm_Click );
			( (System.ComponentModel.ISupportInitialize)( this.pctAppIcon ) ).EndInit( );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.PictureBox pctAppIcon;
		private System.Windows.Forms.Label lblAppName;
		private System.Windows.Forms.Label lblAppVers;
		private System.Windows.Forms.Label lblAppLicense;
	}
}