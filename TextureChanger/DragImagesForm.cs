using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Win32;

namespace TextureChanger
{
    public partial class DragImagesForm : Form
    {
		private ListView DragListView = null;
		private Rectangle DragListViewRect;

		private Point DragOffset;


	    public DragImagesForm()
        {
            InitializeComponent();
        }

		private Rectangle CalculateApproximateRect( ListView lsv )
		{
			IntPtr coord = Api.SendMessage( lsv.Handle, (uint)LVM.APPROXIMATEVIEWRECT, -1, -1 );
			int w = (int)Api.LOWORD( coord );
			int h = (int)Api.HIWORD( coord );

			int ox = 0, oy = 0;
			foreach( ListViewItem item in lsv.Items )
			{
				if( item.Bounds.X < ox )
					ox = item.Bounds.X;
				if( item.Bounds.Y < oy )
					oy = item.Bounds.Y;
			}
			return new Rectangle( ox, oy, w, h );
		}

		public void BeginDrag( ListView curListView, Point curPos, ListViewItem curPosItem )
		{
			if( Region != null )
				Region.Dispose( );
			if( BackgroundImage != null )
				BackgroundImage.Dispose( );

			DragListViewRect = CalculateApproximateRect( curListView );

			DragListView = curListView;

			Size = new Size( DragListViewRect.Width, DragListViewRect.Height );
			Opacity = 0.5;

			DragOffset = DragListView.PointToClient( curPos );
			DragOffset.Offset( DragListViewRect.Left, DragListViewRect.Top );

			Location = new Point(
				curPos.X - DragOffset.X,
				curPos.Y - DragOffset.Y
			);


			IntPtr coord = Api.SendMessage( DragListView.Handle, (uint)LVM.GETITEMSPACING, 0, 0 );
			int horizontal = (int)Api.LOWORD( coord );
			int vertical = (int)Api.HIWORD( coord );

			Rectangle padding = new Rectangle(
					( horizontal - DragListView.LargeImageList.ImageSize.Width ) / 2,
					( vertical - DragListView.LargeImageList.ImageSize.Height ) / 2,
					( horizontal - DragListView.LargeImageList.ImageSize.Width ),
					( vertical - DragListView.LargeImageList.ImageSize.Height )
				);

			// 上に切れたアイテムをドラッグすると透明が描画されない

			BackgroundImage = new Bitmap( Size.Width, Size.Height );

			GraphicsPath path = new GraphicsPath( );

			using( Graphics g = Graphics.FromImage( BackgroundImage ) )
			{
				foreach( int index in DragListView.SelectedIndices )
				{
					var rc = DragListView.GetItemRect( index, ItemBoundsPortion.Entire );
					rc.Offset( DragListViewRect.X, DragListViewRect.Y );
					rc.Offset( padding.X, padding.Y );
					rc.Width -= padding.Width;
					rc.Height -= padding.Height;
					path.AddRectangle( rc );
					g.DrawImage( DragListView.LargeImageList.Images[ index ], rc.Left, rc.Top );
					//TODO draw text
				}
			}

			Region = new Region( path );

			Show( );
		}

		public void MoveDrag( Point curPos )
		{
			Location = new Point(
				curPos.X - DragOffset.X,
				curPos.Y - DragOffset.Y
			);
		}

		public void EndDrag( )
		{
			Hide( );

			if( Region != null )
				Region.Dispose( );
			if( BackgroundImage != null )
				BackgroundImage.Dispose( );
		}

    }
}
