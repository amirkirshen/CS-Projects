using System;
using System.Windows.Forms;
using Checkers.Logic;

namespace Checkers.UI
{
    public partial class FormGameSettings : Form
    {
        private string m_FirstPlayerName;
        private string m_SecondPlayerName;
        private bool m_IsSingleGameMode;
        private eTypeOfBoard m_BoardType;
        private bool m_FormWasExit;
        private bool m_DoneEntered;

        public FormGameSettings()
        {
            m_FirstPlayerName = null;
            m_SecondPlayerName = "Computer";
            m_IsSingleGameMode = true;
            m_FormWasExit = false;
            m_DoneEntered = false;
            m_BoardType = eTypeOfBoard.Medium;
            InitializeComponent();
            this.ShowDialog();
        }

        public string FirstPlayerName
        {
            get { return m_FirstPlayerName; }
            set { m_FirstPlayerName = value; }
        }

        public string SecondPlayerName
        {
            get { return m_SecondPlayerName; }
            set { m_SecondPlayerName = value; }
        }

        public bool IsSingleGameMode
        {
            get { return m_IsSingleGameMode; }
            set { m_IsSingleGameMode = value; }
        }

        public eTypeOfBoard BoardType
        {
            get { return m_BoardType; }
            set { m_BoardType = value; }
        }

        public bool FormWasExit
        {
            get { return m_FormWasExit; }
            set { m_FormWasExit = value; }
        }

        private void checkBoxPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPlayer2.Checked)
            {
                textBoxPlayer2.Enabled = true;
                m_IsSingleGameMode = false;
            }
            else
            {
                textBoxPlayer2.Enabled = false;
                m_IsSingleGameMode = true;
            }
        }

        private void radioButton6x6_Click(object sender, EventArgs e)
        {
            if (radioButton10x10.Checked)
            {
                radioButton10x10.Checked = false;
            }
            else if (radioButton8x8.Checked)
            {
                radioButton8x8.Checked = false;
            }

            radioButton6x6.Checked = true;
            m_BoardType = eTypeOfBoard.Small;
        }

        private void radioButton8x8_Click(object sender, EventArgs e)
        {
            if (radioButton10x10.Checked)
            {
                radioButton10x10.Checked = false;
            }
            else if (radioButton6x6.Checked)
            {
                radioButton6x6.Checked = false;
            }

            radioButton8x8.Checked = true;
            m_BoardType = eTypeOfBoard.Medium;
        }

        private void radioButton10x10_Click(object sender, EventArgs e)
        {
            if (radioButton8x8.Checked)
            {
                radioButton8x8.Checked = false;
            }
            else if (radioButton6x6.Checked)
            {
                radioButton6x6.Checked = false;
            }

            radioButton10x10.Checked = true;
            m_BoardType = eTypeOfBoard.Big;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDone_Click(object sender, EventArgs e)
        {
            m_FirstPlayerName = textBoxPlayer1.Text;
            if (m_IsSingleGameMode)
            {
                m_SecondPlayerName = "Computer";
            }
            else
            {
                m_SecondPlayerName = textBoxPlayer2.Text;
            }

            m_DoneEntered = true;
            this.Close();
        }

        private bool doesNamesValid()
        {
            return m_FirstPlayerName != "" && m_FirstPlayerName != null && m_SecondPlayerName != "" && m_SecondPlayerName != null;
        }

        /// <summary>
        /// Handling invalid inputs when pressing the "Done" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormGameSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && m_DoneEntered == false)
            {
                m_FormWasExit = true;
            }
            else
            {
                m_FormWasExit = false;
            }

            if (!m_FormWasExit)
            {
                m_DoneEntered = false;
                if (!radioButton6x6.Checked && !radioButton8x8.Checked && !radioButton10x10.Checked)
                {
                    MessageBox.Show("No board size has been selected!");
                    e.Cancel = true;
                }
                else if (!doesNamesValid())
                {
                    MessageBox.Show("There is invalid name!");
                    e.Cancel = true;
                }
            }
        }
    }
}
