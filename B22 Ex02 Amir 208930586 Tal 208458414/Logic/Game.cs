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

        public Game()
        {
            m_IsSingleGameFinished = false;
            m_IsAllGameFinished = false;
        }

        public void Run()
        {
            UI.UserInteraction.ShowUserWelcomeToGameMessage();

            SetGame(); // Should be called from the CTOR ?!
            PlayGame();
        }

        private void SetGame()
        {
            string playerName = HandleInputs.GetPlayerName(true); // True for get the main player's name.
            eTypeOfBoard boardType = HandleInputs.GetBoardType();
            m_Player1 = new Player(boardType, eOwnerPlayer.First, ePlayerType.Human, playerName);
            m_Player2 = new Player(boardType, eOwnerPlayer.Second); // Automaticly computer player.
            Ex02.ConsoleUtils.Screen.Clear();
            if (!HandleInputs.GetIfSingleMode()) // Two human players in the game!
            {
                playerName = HandleInputs.GetPlayerName(false); // False for get the second player's name.
                m_Player2.PlayerType = ePlayerType.Human;
                m_Player2.Name = playerName;
            }

            ResetBoard(boardType);
        }

        private void PlayGame()
        {
            bool firstPlayerTurn = true;
            bool player1HadMoved = true, player2HadMoved = true;
            bool didExitGame;
            eGameStatus gameStatus = eGameStatus.StillInGame;

            while (!m_IsAllGameFinished)
            {
                SingleGame(firstPlayerTurn, player1HadMoved, player2HadMoved, out didExitGame, ref gameStatus);

                // Single Game finished - Updating values to start another single game.
                UI.UserInteraction.ShowSingleGameFinishedMessage();
                UpdateScore();
                if (gameStatus != eGameStatus.Exit)
                {
                    SingleGameFinishedMessage(gameStatus);
                    m_IsSingleGameFinished = false;
                    gameStatus = eGameStatus.StillInGame;

                    // Ask for another game:
                    m_IsAllGameFinished = (HandleInputs.CheckForAnotherGame()) ? false : true; // If player chose to play another game -> game not finished.
                    if (!m_IsAllGameFinished)
                    {
                        ResetBoard(m_GameBoard.BoardSize);
                    }
                }
                else // Player chose to exit the game
                {
                    m_IsAllGameFinished = true;
                }
            }

            // ALL GAME FINISHED:
            Ex02.ConsoleUtils.Screen.Clear();
            AllGameFinishedMessage();
        }

        private void SingleGame(bool io_FirstPlayerTurn, bool o_Player1HadMoved, bool o_Player2HadMoved, out bool o_DidExitGame, ref eGameStatus io_GameStatus)
        {
            MoveOption movePair;

            o_DidExitGame = false;
            m_GameBoard.Show();
            while (!m_IsSingleGameFinished)
            {
                if (io_FirstPlayerTurn)
                {
                    movePair = m_Player1.MakeAMove(m_GameBoard, out o_DidExitGame);
                    o_Player1HadMoved = (movePair.ToPosition == null || movePair.ToPosition == null) ? false : true;
                }
                else
                {
                    movePair = m_Player2.MakeAMove(m_GameBoard, out o_DidExitGame);
                    o_Player2HadMoved = (movePair.ToPosition == null || movePair.ToPosition == null) ? false : true;
                }

                io_GameStatus = UpdateGameStatus(o_DidExitGame, io_FirstPlayerTurn, o_Player1HadMoved, o_Player2HadMoved); // Checks the game status.
                m_IsSingleGameFinished = (io_GameStatus == eGameStatus.StillInGame) ? false : true; // Changing m_IsSingleGameFinished if game finished.
                io_FirstPlayerTurn = (io_FirstPlayerTurn) ? false : true; // Changing to rival player turn.
            }

        }

        private void ResetBoard(eTypeOfBoard i_BoardSize)
        {
            // Re-arraging the tools players to starting point. 
            m_GameBoard = new BoardUtilities(i_BoardSize);

            m_Player1.SetToolsForPlayer(m_GameBoard.Board, eOwnerPlayer.First);
            m_Player2.SetToolsForPlayer(m_GameBoard.Board, eOwnerPlayer.Second);

            Ex02.ConsoleUtils.Screen.Clear();
        }

        private void AllGameFinishedMessage()
        {
            UI.UserInteraction.ShowAllGameFinishedMessage();

            if (m_Player1.Score > m_Player2.Score)
            {
                UI.UserInteraction.ShowAllGameWinnerMessage(m_Player1.Name, m_Player1.Score);
                UI.UserInteraction.ShowAllGameLoserMessage(m_Player2.Name, m_Player2.Score);
            }
            else if (m_Player1.Score < m_Player2.Score)
            {
                UI.UserInteraction.ShowAllGameWinnerMessage(m_Player2.Name, m_Player2.Score);
                UI.UserInteraction.ShowAllGameLoserMessage(m_Player1.Name, m_Player1.Score);
            }
            else // TIE
            {
                UI.UserInteraction.ShowAllGameFinishedWithATieMessage();
            }

            UI.UserInteraction.ShowExitGameMessage();
        }

        private eGameStatus UpdateGameStatus(bool i_DidExitGame, bool i_IsFirstPlayerTurn, bool i_DidPlayer1Moved, bool i_DidPlayer2Moved)
        {
            eGameStatus gameStatus = eGameStatus.StillInGame;
            bool didPlayersMoved = (i_DidPlayer1Moved || i_DidPlayer2Moved) ? true : false;

            if (i_DidExitGame)
            {
                gameStatus = eGameStatus.Exit;
            }
            else if (!didPlayersMoved)
            {
                gameStatus = eGameStatus.Tie;
            }
            else if (i_IsFirstPlayerTurn && !i_DidPlayer1Moved) // Was first player turn and he has no moves to do OR second player has no tools.
            {
                gameStatus = eGameStatus.SecondPlayerWin;
            }
            else if (!i_IsFirstPlayerTurn && !i_DidPlayer2Moved) // Was second player turn and he has no moves to do OR second player has no tools.
            {
                gameStatus = eGameStatus.FirstPlayerWin;
            }

            return gameStatus;
        }

        private void SingleGameFinishedMessage(eGameStatus i_FinishMode)
        {
            int player1SingleGameScore = m_Player1.CalculateScore();
            int player2SingleGameScore = m_Player2.CalculateScore();

            if (i_FinishMode == eGameStatus.FirstPlayerWin)
            {
                UI.UserInteraction.PShowSingleGameWinnerMessage(m_Player1.Name, player1SingleGameScore);
                UI.UserInteraction.ShowSingleGameLoserMessage(m_Player2.Name, player2SingleGameScore);
            }
            else if (i_FinishMode == eGameStatus.SecondPlayerWin)
            {
                UI.UserInteraction.PShowSingleGameWinnerMessage(m_Player2.Name, player2SingleGameScore);
                UI.UserInteraction.ShowSingleGameLoserMessage(m_Player1.Name, player1SingleGameScore);
            }
            else if (i_FinishMode == eGameStatus.Tie)
            {
                UI.UserInteraction.DhowSingleGameFinishedWithATieMessage();
            }

            UI.UserInteraction.ShowPlayersTotalScoreMessage(m_Player1.Name, m_Player2.Name, m_Player1.Score, m_Player2.Score);
        }

        private void UpdateScore()
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

        public static void DecideToSleep(ePlayerType i_CurrentPlayerType, int i_NumOfEatMoves)
        {
            if (i_CurrentPlayerType == ePlayerType.Computer || i_NumOfEatMoves == 1)
            {
                System.Threading.Thread.Sleep(3000);
            }
        }
    }
}
