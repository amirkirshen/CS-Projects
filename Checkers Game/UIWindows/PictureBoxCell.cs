using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace UI
{
    public class PictureBoxCell : PictureBox
    {
        private Point m_PointLocation;

        private static readonly int sr_DeltaX = 0;
        private static readonly int sr_DeltaY = 0;

        public PictureBoxCell(int i_X, int i_Y)
        {
            m_PointLocation = new Point(i_X, i_Y);
            this.Size = new Size(50, 50);
            this.Location = new Point(this.Size.Width * i_X + sr_DeltaX, this.Size.Height * i_Y + sr_DeltaY);

            if ((i_X + i_Y) % 2 == 0)
            {
                this.Enabled = false;
                this.Image = Image.FromFile("EmptyWhiteCell.png");
            }
            else
            {
                this.Enabled = true;
                this.Image = Image.FromFile("EmptyBrownCell.jpg");
            }
        }
    }
}
