using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Checkers.Logic;

namespace UIWindows
{
    public partial class FormCheckers : Form
    {
        private Game m_Game = new Game();
        public FormCheckers()
        {
            InitializeComponent();
        }

        private void FormCheckers_Load(object sender, EventArgs e)
        {

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            //m_Game.Run();
            FormGameBoard fgb = new FormGameBoard();
            fgb.ShowDialog();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
