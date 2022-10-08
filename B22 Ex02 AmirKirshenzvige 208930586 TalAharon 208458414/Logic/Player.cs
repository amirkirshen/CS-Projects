using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers.Logic
{
    public enum ePlayerType { Computer, Human }
    public enum eOwnerPlayer { First, Second, None }
    public enum eScorePerTool { SoldierScore = 1, KingScore = 4 }

    public class Player
    {
        private string m_Name;
        private GameTool[] m_PlayerTools;
        private ePlayerType m_Type;
        private int m_Score;
        private eOwnerPlayer m_PlayerNum;
        private short m_NumOfTools;
        private char m_Shape;
        public static readonly char sr_Player1Soldier = 'X';
        public static readonly char sr_Player1King = 'K';
        public static readonly char sr_Player2Soldier = 'O';
        public static readonly char sr_Player2King = 'U';

        public Player(eTypeOfBoard i_SizeOfBoard, eOwnerPlayer i_PlayerNum, ePlayerType i_PlayerType = ePlayerType.Computer, string i_Name = "Comp")
        {
            m_NumOfTools = NumOfTools(i_SizeOfBoard);
            m_PlayerTools = new GameTool[m_NumOfTools];
            m_Name = i_Name;
            m_Type = i_PlayerType;
            m_Score = 0;
            m_PlayerNum = i_PlayerNum;

            switch (i_PlayerNum)
            {
                case eOwnerPlayer.First:
                    m_Shape = Logic.Player.sr_Player1Soldier;
                    break;
                case eOwnerPlayer.Second:
                    m_Shape = Logic.Player.sr_Player2Soldier;
                    break;
            }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public ePlayerType PlayerType
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        public char Shape
        {
            get { return m_Shape; }
            set { m_Shape = value; }
        }

        public void SetToolsForPlayer(GameTool[,] i_Board, eOwnerPlayer i_NumOfPlayer)
        {
            //set tools for player in array.
            short toolIndex = 0;

            foreach (GameTool tool in i_Board)
            {
                if (tool != null && tool.Owner != eOwnerPlayer.None && i_NumOfPlayer == tool.Owner)
                {
                    m_PlayerTools[toolIndex] = tool;
                    tool.Index = toolIndex;
                    toolIndex++;
                }
            }
        }

        private short NumOfTools(eTypeOfBoard i_SizeOfBoard)
        {
            //deals with exist eTypeOfBoard's types.
            //for new size - this func needs to be update
            short numOfTools;

            if (i_SizeOfBoard == eTypeOfBoard.Small)
            {
                numOfTools = 6;
            }
            else if (i_SizeOfBoard == eTypeOfBoard.Medium)
            {
                numOfTools = 12;
            }
            else
            {
                numOfTools = 20;
            }

            return numOfTools;
        }

        public GameTool[] PlayerToolsArray
        {
            get { return m_PlayerTools; }
            set { m_PlayerTools = value; }
        }

        public MoveOption MakeAMove(BoardUtilities i_Board, out bool o_DidExitGame)
        {
            bool isMoveHappend = false;
            bool anEatMove = true;
            GameTool singleGameTool = null;
            List<MoveOption> moveOptions = new List<MoveOption>(); // List of pairs: first coordinate: from position, second coordinate: to position.
            MoveOption currentMove = new MoveOption(null, null);
            MoveOption eatMove;

            o_DidExitGame = false;
            do
            {
                moveOptions.Clear();
                i_Board.SetPossibleEatMovesListFromPlayersTools(ref moveOptions, m_PlayerTools, m_PlayerNum, singleGameTool);
                // Check if there is a eat move steps --> YES --> Choose only between them!
                if (moveOptions.Count > 0)
                {
                    // Choose the eat move from the eatMovesList:
                    eatMove = SetEatingMoveFromList(moveOptions, out o_DidExitGame);
                    anEatMove = isMoveHappend = true;
                    Move(i_Board, eatMove, anEatMove, moveOptions.Count);
                    singleGameTool = i_Board.GetGameToolByCell(eatMove.ToPosition);
                    currentMove = eatMove;
                }
            }
            while (moveOptions.Count > 0); // There is eating options

            // Check for non-eating valid moves if not eating move been done.
            // If has valid move --> Asks the user for a move. 
            // Otherwise --> return parameters for none valid move!
            if (!isMoveHappend)
            {
                // Getting all possible moves without eating.
                moveOptions.Clear();
                i_Board.SetPossibleMovesWithoutEatFromPlayersTools(ref moveOptions, m_PlayerTools, m_PlayerNum);

                if (moveOptions.Count != 0) // Has valid moves to do
                {
                    currentMove = SetMoveWithoutEating(i_Board, moveOptions, out o_DidExitGame);
                    if (!o_DidExitGame)
                    {
                        // Legel Move:
                        isMoveHappend = true;
                        Move(i_Board, currentMove, anEatMove = false, 0); // Sending with 0 eat moves.
                    }
                }
            }

            if (o_DidExitGame || !isMoveHappend) // Decide to quit the game or has no valid moves to do.
            {
                isMoveHappend = false;
                currentMove = new MoveOption(null, null);
            }

            return currentMove;
        }

        private MoveOption SetEatingMoveFromList(List<MoveOption> i_EatingOptions, out bool o_DidExitGame)
        {
            MoveOption eatMove = new MoveOption(null, null);

            // Choose the eat move from the eatMovesList:
            if (i_EatingOptions.Count == 1 && m_Type == ePlayerType.Human)
            {
                UI.UserInteraction.ShowOneEatingOptionMoveIsDoneMessage();
                UI.UserInteraction.ShowMoveOptionMessage(HandleInputs.ConvertValidMovePositionsToString(i_EatingOptions[0]));
                eatMove.FromPosition = i_EatingOptions[0].FromPosition;
                eatMove.ToPosition = i_EatingOptions[0].ToPosition;
                o_DidExitGame = false;
                Game.DecideToSleep(m_Type, 1); // Sleeping for more prettier game moving.
            }
            else
            {
                if (m_Type == ePlayerType.Human)
                {
                    eatMove = HandleInputs.GetEatMoveFromList(i_EatingOptions, out o_DidExitGame);
                }
                else
                {
                    ComputerMakeRandomMoveFromList(i_EatingOptions, out eatMove);
                    o_DidExitGame = false;
                }
            }

            return eatMove;
        }

        public int CalculateScore()
        {
            int score = 0;

            foreach (GameTool tool in m_PlayerTools)
            {
                if (tool.Owner != eOwnerPlayer.None)
                {
                    if (tool.Type == eToolType.King)
                    {
                        score += (int)eScorePerTool.KingScore;
                    }
                    else
                    {
                        score += (int)eScorePerTool.SoldierScore;
                    }
                }
            }

            return score;
        }

        private MoveOption SetMoveWithoutEating(BoardUtilities i_Board, List<MoveOption> i_EatingOptions, out bool o_DidExitGame)
        {
            MoveOption currentMove;

            if (m_Type == ePlayerType.Human)
            {
                UserMakeMoveWithoutEating(i_Board, i_EatingOptions, out currentMove, out o_DidExitGame);
            }
            else
            {
                ComputerMakeRandomMoveFromList(i_EatingOptions, out currentMove);
                o_DidExitGame = false;
            }

            return currentMove;
        }

        private void UserMakeMoveWithoutEating(BoardUtilities i_Board, List<MoveOption> i_EatingOptions, out MoveOption o_MoveOption, out bool o_DidExitGame)
        {
            HandleInputs.GetMovePositions(out o_MoveOption, m_Name, m_Shape, out o_DidExitGame);
            while (!o_DidExitGame && !HandleInputs.IsMoveInList(i_EatingOptions, o_MoveOption)    /*!i_Board.IsMoveWithoutEatingValids(o_MoveOption, m_PlayerNum*/)
            {
                Ex02.ConsoleUtils.Screen.Clear();
                i_Board.Show();
                UI.UserInteraction.ShowInvalidMoveMessage();
                HandleInputs.GetMovePositions(out o_MoveOption, m_Name, m_Shape, out o_DidExitGame);
            }
        }

        private void Move(BoardUtilities i_Board, MoveOption i_MoveOption, bool i_IsEatMove, int i_NumOfEatMoves)
        {
            GameTool toTool;
            GameTool fromTool = i_Board.GetGameToolByCell(i_MoveOption.FromPosition);
            Position eatenToolPos;
            short indexSaver;

            if (i_IsEatMove)
            {
                eatenToolPos = i_Board.GetTheEatenToolPosFromPositionsByPlayerNum(i_MoveOption);
                i_Board.ResetCell(eatenToolPos);
                // Score will be update at the end of the current game.
            }

            indexSaver = m_PlayerTools[fromTool.Index].Index;
            i_Board.SetGameToolByCell(i_MoveOption.ToPosition, m_PlayerNum, m_PlayerTools[fromTool.Index].Type, indexSaver);
            m_PlayerTools[fromTool.Index] = i_Board.GetGameToolByCell(i_MoveOption.ToPosition);
            i_Board.ResetCell(i_MoveOption.FromPosition);
            toTool = m_PlayerTools[fromTool.Index];
            // Check if to_tool now changed his type to king!
            if (i_Board.IsAToolOfPlayerKing(toTool))
            {
                toTool.Type = eToolType.King;
            }

            EndOfMove(i_Board, i_NumOfEatMoves, i_MoveOption);
        }

        private void EndOfMove(BoardUtilities i_Board, int i_NumOfEatMoves, MoveOption i_MoveOption)
        {
            Game.DecideToSleep(m_Type, i_NumOfEatMoves);
            Ex02.ConsoleUtils.Screen.Clear();
            i_Board.Show();
            UI.UserInteraction.ShowPlayerMovedMessage(m_Name, m_Shape, HandleInputs.ConvertValidMovePositionsToString(i_MoveOption));
        }

        public short NumOfLiveTools()
        //Counts how many soldiers was eaten
        {
            short numOfLiveTools = 0;

            foreach (GameTool tool in m_PlayerTools)
            {
                if (tool.Owner != eOwnerPlayer.None)
                {
                    numOfLiveTools++;
                }
            }

            return numOfLiveTools;
        }

        private void ComputerMakeRandomMoveFromList(List<MoveOption> i_MoveList, out MoveOption o_MoveOption)
        {
            Random rand = new Random();
            int randomIndex = rand.Next(0, i_MoveList.Count - 1);
            o_MoveOption = i_MoveList[randomIndex];
        }
    }
}