using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TextureChanger
{
    public partial class DragImagesForm : Form
    {
        public ListViewItem BeginDragPosItem = null;

        public DragImagesForm()
        {
            InitializeComponent();
        }
    }
}
