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

		public void BeginDrag( ListView curListView, Point curPos )
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
					0, //( vertical - DragListView.LargeImageList.ImageSize.Height ) / 2,
					( horizontal - DragListView.LargeImageList.ImageSize.Width ),
					0 //( vertical - DragListView.LargeImageList.ImageSize.Height )
				);

			// 上に切れたアイテムをドラッグすると透明が描画されない

			BackgroundImage = new Bitmap( Size.Width, Size.Height );

			GraphicsPath path = new GraphicsPath( );

			using( Graphics gfx = Graphics.FromImage( BackgroundImage ) )
			{
				var brush = new SolidBrush(DragListView.ForeColor);

				foreach( int index in DragListView.SelectedIndices )
				{
					var rcIcon = DragListView.GetItemRect( index, ItemBoundsPortion.Entire );
					rcIcon.Offset( DragListViewRect.X, DragListViewRect.Y );
					rcIcon.Offset( padding.X, padding.Y );
					rcIcon.Width -= padding.Width;
					rcIcon.Height -= padding.Height;
					
					gfx.DrawImage( DragListView.LargeImageList.Images[ index ], rcIcon.Left, rcIcon.Top );

					var rcText = DragListView.GetItemRect( index, ItemBoundsPortion.Label );
					rcText.Offset( DragListViewRect.X, DragListViewRect.Y );
					rcText.Offset( 0 /*padding.X*/, padding.Y );
					rcText.Width -= padding.Width;
					rcText.Height -= padding.Height;
					
					gfx.DrawString(
						DragListView.Items[ index ].Text,
						DragListView.Font, brush,
						rcText.Left, rcText.Top
					);

					path.AddRectangle( rcIcon );
					//path.AddRectangle( rcText );
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
