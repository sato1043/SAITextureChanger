using System;
using System.Windows.Forms;
using TextureChanger.util;

namespace TextureChanger
{
	public partial class TextureChangerForm : Form
	{
		public TextureChangerForm( )
		{
			InitializeComponent( );
		}

		private void TextureChangerForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			DialogResult result = CenteredMessageBox.Show( this
				, "終了しますか？" , "確認"
				, MessageBoxButtons.YesNo, MessageBoxIcon.Question );

			if( result == DialogResult.No )  // [いいえ] の場合
			{
				e.Cancel = true;  // 終了処理を中止
			}
		}

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Close( );
		}

		private void aboutToolStripMenuItem_Click( object sender, EventArgs e )
		{
			DialogResult showDialog = new AboutForm().ShowDialog();
		}
	}
}
