using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers.Logic
{
    public enum eGameStatus { FirstPlayerWin, SecondPlayerWin, Tie, Exit, StillInGame }

    public class Game
    {
        private BoardUtilities m_GameBoard;
        private Player m_Player1;
        private Player m_Player2;
        private bool m_IsSingleGameFinished;
        private bool m_IsAllGameFinished;
        private bool m_IsFirstPlayerTurn;

        public bool IsAllGameFinished
        {
            get { return m_IsAllGameFinished; }
            set { m_IsAllGameFinished = value; }
        }

        public bool IsFirstPlayerTurn
        {
            get { return m_IsFirstPlayerTurn; }
            set { m_IsFirstPlayerTurn = value; }
        }

        public bool IsSingleGameFinished
        {
            get { return m_IsSingleGameFinished; }
            set { m_IsSingleGameFinished = value; }
        }

        public Game()
        {
            m_IsSingleGameFinished = false;
            m_IsAllGameFinished = false;
            m_IsFirstPlayerTurn = true;
        }

        public void SetGame(string i_FirstPlayerName, string i_SecondPlayerName, bool i_IsSingleGameMode, eTypeOfBoard i_BoardType)
        {
            m_Player1 = new Player(i_BoardType, eOwnerPlayer.First, ePlayerType.Human, i_FirstPlayerName);
            m_Player2 = new Player(i_BoardType, eOwnerPlayer.Second, ePlayerType.Computer, i_SecondPlayerName); // Automaticly computer player.
            if (!i_IsSingleGameMode) // Two human players in the game!
            {
                m_Player2.PlayerType = ePlayerType.Human;
            }

            ResetBoard(i_BoardType);
        }

        public void ResetBoard(eTypeOfBoard i_BoardSize)
        {
            // Re-arraging the tools players to starting point. 
            m_GameBoard = new BoardUtilities(i_BoardSize);
            m_Player1.SetToolsForPlayer(m_GameBoard.Board, eOwnerPlayer.First);
            m_Player2.SetToolsForPlayer(m_GameBoard.Board, eOwnerPlayer.Second);
        }

        /// <summary>
        /// Handling the move progress by current player's turn.
        /// Returns the move dialog to represent the move status.
        /// Returns in output param the game status.
        /// </summary>
        /// <param name="i_MovePair"></param>
        /// <param name="o_GameStatus"></param>
        /// <returns></returns>
        public eMoveDialogs HandleMoveProgress(MoveOption i_MovePair, out eGameStatus o_GameStatus)
        {
            eMoveDialogs moveDialog;

            if (m_IsFirstPlayerTurn)
            {
                if ((moveDialog = m_Player1.MakeEatMoveIfHas(m_GameBoard, i_MovePair)) == eMoveDialogs.NoMoreMoves)
                {
                    moveDialog = m_Player1.MakeNonEatMove(m_GameBoard, i_MovePair);
                }
            }
            else
            {
                if ((moveDialog = m_Player2.MakeEatMoveIfHas(m_GameBoard, i_MovePair)) == eMoveDialogs.NoMoreMoves)
                {
                    moveDialog = m_Player2.MakeNonEatMove(m_GameBoard, i_MovePair);
                }
            }

            if (!m_IsFirstPlayerTurn && m_Player2.PlayerType == ePlayerType.Computer)
            {
                System.Threading.Thread.Sleep(600);
            }

            m_IsFirstPlayerTurn = (moveDialog == eMoveDialogs.NotValid || moveDialog == eMoveDialogs.NotEatingMoveChosen || moveDialog == eMoveDialogs.HaveAnotherEatMove) ? m_IsFirstPlayerTurn : changePlayerAndrResetMovePair(m_IsFirstPlayerTurn, ref i_MovePair);
            o_GameStatus = IsGameFinished(moveDialog);

            return moveDialog;
        }

        private bool changePlayerAndrResetMovePair(bool i_IsFirstPlayerTurn, ref MoveOption io_MovePair)
        {
            i_IsFirstPlayerTurn = !i_IsFirstPlayerTurn;
            io_MovePair.FromPosition = null;
            io_MovePair.ToPosition = null;

            return i_IsFirstPlayerTurn;
        }

        public eGameStatus IsGameFinished(eMoveDialogs i_MoveDialog)
        {
            eGameStatus gameStauts = eGameStatus.StillInGame;

            switch (i_MoveDialog)
            {
                case eMoveDialogs.Succeeded:
                    if (!rivalPlayerHasMoves()) // If the rival has no moves to do:
                    {
                        m_IsFirstPlayerTurn = !m_IsFirstPlayerTurn;
                        gameStauts = (m_IsFirstPlayerTurn) ? eGameStatus.FirstPlayerWin : eGameStatus.SecondPlayerWin;
                        UpdateScore();
                        m_IsFirstPlayerTurn = true;
                    }
                    break;
                case eMoveDialogs.NotValid:
                    break;
                case eMoveDialogs.NoMoreMoves:
                    m_IsFirstPlayerTurn = !m_IsFirstPlayerTurn;
                    gameStauts = (!rivalPlayerHasMoves()) ? eGameStatus.Tie : eGameStatus.SecondPlayerWin;
                    UpdateScore();
                    m_IsFirstPlayerTurn = true;
                    break;
                default:
                    break;
            }

            return gameStauts;
        }

        public string GetCurrentPlayerName()
        {
            return (m_IsFirstPlayerTurn) ? m_Player1.Name : m_Player2.Name;
        }

        public GameTool[,] GetCurrentBoardStatus()
        {
            return m_GameBoard.Board;
        }

        private bool rivalPlayerHasMoves()
        {
            List<MoveOption> eatMoveOptions = new List<MoveOption>();
            List<MoveOption> nonEatMoveOptions = new List<MoveOption>();
            Player currPlayer = (m_IsFirstPlayerTurn) ? m_Player1 : m_Player2;
            bool isHaveMoves;

            m_GameBoard.SetPossibleEatMovesListFromPlayersTools(ref eatMoveOptions, currPlayer.PlayerToolsArray, currPlayer.PlayerOwner);
            m_GameBoard.SetPossibleMovesWithoutEatFromPlayersTools(ref nonEatMoveOptions, currPlayer.PlayerToolsArray, currPlayer.PlayerOwner);
            isHaveMoves = (eatMoveOptions.Count > 0 || nonEatMoveOptions.Count > 0) ? true : false;

            return isHaveMoves;
        }

        public void UpdateScore()
        {
            short numOfPlayer1Tools = m_Player1.NumOfLiveTools();
            short numOfPlayer2Tools = m_Player2.NumOfLiveTools();
            int player1SingleGameScore = m_Player1.CalculateScore();
            int player2SingleGameScore = m_Player2.CalculateScore();

            if (numOfPlayer1Tools > numOfPlayer2Tools)
            {
                m_Player1.Score += player1SingleGameScore - player2SingleGameScore;
            }
            else
            {
                m_Player2.Score += player2SingleGameScore - player1SingleGameScore;
            }
        }

        public int GetFirstPlayerScore()
        {
            return m_Player1.Score;
        }

        public int GetSecondPlayerScore()
        {
            return m_Player2.Score;
        }

        public string GetRivalPlayerName()
        {
            return (m_IsFirstPlayerTurn) ? m_Player2.Name : m_Player1.Name;
        }

        /// <summary>
        /// Checks if "from_Position" and "to_Position" are both user's tools.
        /// Checks if "from_Position" is an empty cell and "to_Position" is user's tool.
        /// If one of the above is true, it sets "from_Position"'s value to be as "to_Position"
        /// and sets "to_Position"'s value to be null.
        /// </summary>
        /// <param name="i_MoveOption"></param>
        /// <returns></returns>
        public bool SwapFromPositionAndToPositionIfSameOwner(ref MoveOption i_MoveOption)
        {
            bool isSwapes = false;

            if (m_GameBoard.GetGameToolByCell(i_MoveOption.FromPosition).Owner
                == m_GameBoard.GetGameToolByCell(i_MoveOption.ToPosition).Owner &&
                ((m_GameBoard.GetGameToolByCell(i_MoveOption.FromPosition).Owner == eOwnerPlayer.First &&
                    m_IsFirstPlayerTurn) ||
                    (!m_IsFirstPlayerTurn && m_GameBoard.GetGameToolByCell(i_MoveOption.FromPosition).Owner == eOwnerPlayer.Second)) ||
                    (m_GameBoard.GetGameToolByCell(i_MoveOption.FromPosition).Owner == eOwnerPlayer.None
                && m_GameBoard.GetGameToolByCell(i_MoveOption.ToPosition).Owner != eOwnerPlayer.None &&
                ((m_GameBoard.GetGameToolByCell(i_MoveOption.ToPosition).Owner == eOwnerPlayer.First
                    && m_IsFirstPlayerTurn) ||
                    (m_GameBoard.GetGameToolByCell(i_MoveOption.ToPosition).Owner == eOwnerPlayer.Second
                    && !m_IsFirstPlayerTurn))))
            {
                i_MoveOption.FromPosition = i_MoveOption.ToPosition;
                i_MoveOption.ToPosition = null;
                isSwapes = true;
            }

            return isSwapes;
        }
    }
}
