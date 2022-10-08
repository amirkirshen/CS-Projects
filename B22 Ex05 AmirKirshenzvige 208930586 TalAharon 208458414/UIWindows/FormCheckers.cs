using System;
using System.Windows.Forms;

namespace Checkers.UI
{
    public partial class FormCheckers : Form
    {
        public FormCheckers()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            FormGameSettings formGameSetting = new FormGameSettings();
            FormGameBoard formGameBoard = new FormGameBoard(formGameSetting.FirstPlayerName, formGameSetting.SecondPlayerName, formGameSetting.IsSingleGameMode, (short)formGameSetting.BoardType);

            if (!formGameSetting.FormWasExit)
            {
                this.Hide();
                formGameBoard.Run();
                this.Show();
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
