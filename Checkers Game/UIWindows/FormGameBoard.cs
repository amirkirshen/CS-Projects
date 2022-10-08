using System;
using System.Drawing;
using System.Windows.Forms;
using Checkers.Logic;

namespace Checkers.UI
{
    public partial class FormGameBoard : Form
    {
        private readonly Button[,] m_Board;
        private readonly Game m_Game;
        private readonly eTypeOfBoard m_BoardType;
        private MoveOption m_MovePair;
        private readonly bool m_IsSingleGameMode;
        private readonly string m_FirstPlayerName;
        private readonly string m_SecondPlayerName;
        private eMoveDialogs m_MoveDialog;

        public FormGameBoard(string i_FirstPlayerName, string i_SecondPlayerName, bool i_IsSingleGameMode, short i_BoardSize)
        {
            m_BoardType = (eTypeOfBoard)i_BoardSize;
            m_Game = new Game();
            m_Board = new Button[i_BoardSize, i_BoardSize];
            m_FirstPlayerName = i_FirstPlayerName;
            m_SecondPlayerName = i_SecondPlayerName;
            m_IsSingleGameMode = i_IsSingleGameMode;
            m_MovePair.FromPosition = null;
            m_MovePair.ToPosition = null;

            m_Game.SetGame(m_FirstPlayerName, m_SecondPlayerName, m_IsSingleGameMode, m_BoardType);
            initializeBoardButtons(i_BoardSize);
            InitializeComponent();
            adjustFormSizeByBoardSize(i_BoardSize);
        }

        public string Label1Messages
        {
            get { return label1Messages.Text; }
            set { label1Messages.Text = value; }
        }

        public string Label2Messages
        {
            get { return label2Messages.Text; }
            set { label2Messages.Text = value; }
        }

        //public string FirstPlayerName { get; }

        //public string SecondPlayerName { get; }

        //public bool IsSingleGameMode { get; }

        //public eTypeOfBoard BoardType { get; }

        public void Run()
        {
            ResetGame();
            this.ShowDialog();
        }

        /// <summary>
        /// Reset all game board tools.
        /// </summary>
        public void ResetGame()
        {
            m_Game.ResetBoard(m_BoardType);
            updateFormTools();
        }

        /// <summary>
        /// Presents MessageBox with "Game Over" to the screen.
        /// </summary>
        private void allGameFinished()
        {
            MessageBox.Show("GAME OVER!");
            m_Game.IsAllGameFinished = true;
            this.Close();
        }

        /// <summary>
        /// Adjusting the board size.
        /// </summary>
        /// <param name="i_BoardSize"></param>
        private void adjustFormSizeByBoardSize(short i_BoardSize)
        {
            int width = (int)((m_Board[0, 0].Width + 1) * i_BoardSize + 5), height = (int)((m_Board[0, 0].Height + 1) * i_BoardSize + panelLegend.Height + 20);
            this.Size = new Size(width, height);
        }

        private void initializeBoardButtons(int i_BoardSize)
        {
            for (int i = 0; i < i_BoardSize; i++)
            {
                for (int j = 0; j < i_BoardSize; j++)
                {
                    setButtonCell(out m_Board[i, j], i, j);
                }
            }
        }

        /// <summary>
        /// Setting all button's program properties.
        /// </summary>
        /// <param name="o_Button"></param>
        /// <param name="i_Row"></param>
        /// <param name="i_Col"></param>
        private void setButtonCell(out Button o_Button, int i_Row, int i_Col)
        {
            char col = (char)('A' + i_Col), row = (char)('a' + i_Row);

            o_Button = new Button();
            o_Button.Name = $"Button{col}{row}";
            o_Button.Size = new Size(69, 65);
            o_Button.Location = new Point(i_Row * 69 + 1, i_Col * 65 + 1);  // <-- You might want to tweak this
            if ((i_Row + i_Col) % 2 == 0)
            {
                o_Button.Enabled = false;
                o_Button.BackColor = Color.White;
            }
            else
            {
                o_Button.Enabled = true;
                o_Button.BackColor = Color.BurlyWood;
                o_Button.Click += Button_Click;
            }

            o_Button.BackgroundImageLayout = ImageLayout.Stretch;
            o_Button.TabStop = false;
            o_Button.Parent = panelBoard;
            o_Button.FlatStyle = FlatStyle.Flat;
            o_Button.FlatAppearance.BorderSize = 2;
            o_Button.FlatAppearance.BorderColor = Color.DarkGray;
            Controls.Add(o_Button);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            (sender as Button).FlatAppearance.BorderColor = ((sender as Button).FlatAppearance.BorderColor == Color.Blue) ?
                                                            Color.DarkGray : Color.Blue;
            if (m_MovePair.FromPosition == null)
            {
                // Prevent updating from position if the last moving tool has another eat move
                m_MovePair.FromPosition = getPositionByName((sender as Button).Name);
            }
            else if (getPositionByName((sender as Button).Name).IsEqual(m_MovePair.FromPosition))
            {
                // Determines wether the same button has been clicked twice.
                (sender as Button).FlatAppearance.BorderColor = Color.DarkGray;
                Update();
                m_MovePair.FromPosition = null;
                m_MovePair.ToPosition = null;
            }
            else
            {
                eGameStatus outGameStatus;

                m_MovePair.ToPosition = getPositionByName((sender as Button).Name);
                if (!resetFromPositionToAnotherTool(ref m_MovePair))
                {
                    m_MoveDialog = m_Game.HandleMoveProgress(m_MovePair, out outGameStatus);
                    if (m_MoveDialog == eMoveDialogs.HaveAnotherEatMove)
                    {
                        m_MoveDialog = (m_MoveDialog != eMoveDialogs.Succeeded) ? eMoveDialogs.HaveAnotherEatMove : m_MoveDialog;
                    }

                    handleMoveDialog(m_MoveDialog);
                    checkIfSingleGameFinished(outGameStatus);
                    m_MovePair.FromPosition = (m_MoveDialog == eMoveDialogs.HaveAnotherEatMove) ? m_MovePair.ToPosition : null;
                    m_MovePair.ToPosition = null;
                    Update();
                    if (m_MoveDialog == eMoveDialogs.Succeeded && outGameStatus == eGameStatus.StillInGame && !m_Game.IsFirstPlayerTurn && m_IsSingleGameMode)
                    {
                        //Move computer's tools
                        computerMoveProgress(ref outGameStatus);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a move option and checks if the user wants to change his tool selection.
        /// Checks if the user might confused between "from_position" and "to_position"
        /// </summary>
        /// <param name="m_MovePair"></param>
        /// <returns></returns>
        private bool resetFromPositionToAnotherTool(ref MoveOption m_MovePair)
        {
            bool isSwapHaveBeenMade = false;

            if (m_MoveDialog != eMoveDialogs.HaveAnotherEatMove)
            {
                isSwapHaveBeenMade = m_Game.SwapFromPositionAndToPositionIfSameOwner(ref m_MovePair);
            }

            return isSwapHaveBeenMade;
        }

        private void computerMoveProgress(ref eGameStatus o_outGameStatus)
        {
            do
            {
                //In computer mode, no button will be pressed so all double eating moves are made here
                m_MoveDialog = m_Game.HandleMoveProgress(m_MovePair, out o_outGameStatus);
                handleMoveDialog(m_MoveDialog);
                checkIfSingleGameFinished(o_outGameStatus);
                Update();
            } while (m_MoveDialog == eMoveDialogs.HaveAnotherEatMove);

            m_MovePair.FromPosition = null;
            m_MovePair.ToPosition = null;
        }

        /// <summary>
        /// Updating the labels and board buttons according to the move that has been done.
        /// </summary>
        /// <param name="i_MoveDialog"></param>
        private void handleMoveDialog(eMoveDialogs i_MoveDialog)
        {
            switch (i_MoveDialog)
            {
                case eMoveDialogs.Succeeded:
                    label2Messages.Text = $"{m_Game.GetRivalPlayerName()} has moved!";
                    updateFormTools();
                    break;
                case eMoveDialogs.NotValid:
                    label2Messages.Text = $"{m_Game.GetCurrentPlayerName()} has done invalid move!";
                    break;
                case eMoveDialogs.NoMoreMoves:
                    label2Messages.Text = $"{m_Game.GetCurrentPlayerName()} has no more moves to do!";
                    updateFormTools();
                    break;
                case eMoveDialogs.NotEatingMoveChosen:
                    label2Messages.Text = $"{m_Game.GetCurrentPlayerName()} has to do the eat move!";
                    break;
                case eMoveDialogs.HaveAnotherEatMove:
                    label2Messages.Text = $"{m_Game.GetCurrentPlayerName()} has another eat move!";
                    updateFormTools();
                    break;
                default:
                    break;
            }

            updateBoardButtons(m_Game.GetCurrentBoardStatus());
            Update();
        }

        /// <summary>
        /// Updating the game board form labels and buttons.
        /// </summary>
        private void updateFormTools()
        {
            label1Messages.Text = $"{m_FirstPlayerName}: {m_Game.GetFirstPlayerScore()} | {m_SecondPlayerName}: {m_Game.GetSecondPlayerScore()}";
            label2Messages.Text += $" It is {m_Game.GetCurrentPlayerName()}'s turn!";
            updateBoardButtons(m_Game.GetCurrentBoardStatus());
        }

        /// <summary>
        /// Updating the game board tool button according to the current game tool in board.
        /// </summary>
        /// <param name="i_GameTools"></param>
        private void updateBoardButtons(GameTool[,] i_GameTools)
        {
            foreach (GameTool gameTool in i_GameTools)
            {
                int gameToolRow = gameTool.Position.Row;
                int gameToolCol = gameTool.Position.Col;

                if (gameTool.Owner == eOwnerPlayer.First)
                {
                    if (gameTool.Type == eToolType.Soldier)
                    {
                        m_Board[gameToolCol, gameToolRow].BackgroundImage = UIWindows.Resources.BlackGameTool;
                    }
                    else
                    {
                        m_Board[gameToolCol, gameToolRow].BackgroundImage = UIWindows.Resources.BlackKingTool;
                    }
                }
                else if (gameTool.Owner == eOwnerPlayer.Second)
                {
                    if (gameTool.Type == eToolType.Soldier)
                    {
                        m_Board[gameToolCol, gameToolRow].BackgroundImage = UIWindows.Resources.BlueGameTool;
                    }
                    else
                    {
                        m_Board[gameToolCol, gameToolRow].BackgroundImage = UIWindows.Resources.BlueKingTool;

                    }
                }
                else
                {
                    m_Board[gameToolCol, gameToolRow].BackgroundImage = null;
                }

                m_Board[gameToolCol, gameToolRow].BackgroundImageLayout = ImageLayout.Stretch;
                m_Board[gameToolCol, gameToolRow].FlatAppearance.BorderColor = Color.DarkGray;
            }
        }

        /// <summary>
        /// Extract the button's row and col according to his name.
        /// </summary>
        /// <param name="i_ButtonName"></param>
        /// <returns></returns>
        private Position getPositionByName(string i_ButtonName)
        {
            Position position;
            short col, row;

            row = (short)((i_ButtonName[i_ButtonName.Length - 2]) - 'A');
            col = (short)((i_ButtonName[i_ButtonName.Length - 1]) - 'a');
            position = new Position(row, col);

            return position;
        }

        /// <summary>
        /// Presenting MessageBox to screen with the question to stop the game.
        /// Finish the game it true, otherwise return to the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormGameBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_Game.IsAllGameFinished)
            {
                DialogResult dialogResult = MessageBox.Show("Would you like to stop the game?", "Checkers Game", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    allGameFinished();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Checking if the game is over according to i_GameStatus and updating the Form if true.
        /// </summary>
        /// <param name="i_GameStatus"></param>
        private void checkIfSingleGameFinished(eGameStatus i_GameStatus)
        {
            bool gameFinished = false;

            switch (i_GameStatus)
            {
                case eGameStatus.FirstPlayerWin:
                    this.Label2Messages = $"{m_FirstPlayerName} is the WINNER !";
                    gameFinished = true;
                    break;
                case eGameStatus.SecondPlayerWin:
                    this.Label2Messages = $"{m_SecondPlayerName} is the WINNER !";
                    gameFinished = true;
                    break;
                case eGameStatus.Tie:
                    this.Label2Messages = $"This game finished with a TIE !";
                    gameFinished = true;
                    break;
                case eGameStatus.Exit:
                    break;
                case eGameStatus.StillInGame:
                    break;
                default:
                    break;
            }

            if (gameFinished)
            {
                m_MovePair.FromPosition = null;
                m_MovePair.ToPosition = null;
            }

            anotherGameCheck(gameFinished);
        }

        /// <summary>
        /// Presenting MessageBox to screen with the question to start new game.
        /// Handling the results according to the response.
        /// </summary>
        /// <param name="i_IsGameFinished"></param>
        private void anotherGameCheck(bool i_IsGameFinished)
        {
            if (i_IsGameFinished)
            {
                DialogResult dialogResult = MessageBox.Show("Would you like to play another game?", "Checkers Game", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    //this.Run();
                    ResetGame();
                }
                else
                {
                    allGameFinished();
                }
            }
        }
    }
}
