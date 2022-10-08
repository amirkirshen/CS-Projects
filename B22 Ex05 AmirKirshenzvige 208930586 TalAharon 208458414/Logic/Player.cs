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
        private readonly short m_NumOfTools;
        private char m_Shape;
        public static readonly char sr_Player1Soldier = 'X';
        public static readonly char sr_Player1King = 'K';
        public static readonly char sr_Player2Soldier = 'O';
        public static readonly char sr_Player2King = 'U';

        public Player(eTypeOfBoard i_SizeOfBoard, eOwnerPlayer i_PlayerNum, ePlayerType i_PlayerType = ePlayerType.Computer, string i_Name = "Comp")
        {
            m_NumOfTools = numOfTools(i_SizeOfBoard);
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

        public eOwnerPlayer PlayerOwner
        {
            get { return m_PlayerNum; }
            set { m_PlayerNum = value; }
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

        private short numOfTools(eTypeOfBoard i_SizeOfBoard)
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

        /// <summary>
        /// Checks if there is valid non-eating move.
        /// If there is, it is been done. 
        /// Otherwise returns NotValid in moveDialog.
        /// </summary>
        /// <param name="i_Board"></param>
        /// <param name="i_MovePair"></param>
        /// <returns></returns>
        public eMoveDialogs MakeNonEatMove(BoardUtilities i_Board, MoveOption i_MovePair)
        {
            bool isMoveHappend = false;
            List<MoveOption> moveOptions = new List<MoveOption>();
            eMoveDialogs MoveDialogs = new eMoveDialogs();

            // Getting all possible moves without eating.
            moveOptions.Clear();
            i_Board.SetPossibleMovesWithoutEatFromPlayersTools(ref moveOptions, m_PlayerTools, m_PlayerNum);

            if (moveOptions.Count != 0) // Has valid moves to do
            {
                if (m_Type == ePlayerType.Computer)
                {
                    computerMakeRandomMoveFromList(moveOptions, ref i_MovePair);
                }

                if (isMoveExistInList(moveOptions, i_MovePair))
                {
                    move(i_Board, i_MovePair, false); // Sending with 0 eat moves.
                    isMoveHappend = true;
                }
            }
            else
            {
                MoveDialogs = eMoveDialogs.NoMoreMoves;
            }

            if (MoveDialogs != eMoveDialogs.NoMoreMoves)
            {
                MoveDialogs = (isMoveHappend) ? eMoveDialogs.Succeeded : eMoveDialogs.NotValid;
            }

            return MoveDialogs;
        }

        private bool isMoveExistInList(List<MoveOption> i_MoveOptionsList, MoveOption i_MovePair)
        {
            bool isMoveFromList = false;

            foreach (MoveOption move in i_MoveOptionsList)
            {
                if (move.FromPosition.Col == i_MovePair.FromPosition.Col &&
                    move.FromPosition.Row == i_MovePair.FromPosition.Row &&
                    move.ToPosition.Col == i_MovePair.ToPosition.Col &&
                    move.ToPosition.Row == i_MovePair.ToPosition.Row)
                {
                    isMoveFromList = true;
                }
            }

            return isMoveFromList;
        }

        public eMoveDialogs MakeEatMoveIfHas(BoardUtilities i_Board, MoveOption i_MovePair)
        {
            eMoveDialogs moveDialogs;
            GameTool singleGameTool = null;
            List<MoveOption> moveOptions = new List<MoveOption>();

            moveDialogs = eMoveDialogs.NoMoreMoves;
            i_Board.SetPossibleEatMovesListFromPlayersTools(ref moveOptions, m_PlayerTools, m_PlayerNum, singleGameTool);
            // Check if there is a eat move steps --> YES --> Choose only between them!
            if (moveOptions.Count > 0)
            {
                // Choose the eat move from the eatMovesList:
                if (m_Type == ePlayerType.Computer)
                {
                    computerMakeRandomMoveFromList(moveOptions, ref i_MovePair);
                }

                if (isMoveExistInList(moveOptions, i_MovePair) && i_MovePair.FromPosition != null)
                {
                    move(i_Board, i_MovePair, true);
                    moveDialogs = eMoveDialogs.Succeeded;
                }
                else
                {
                    moveDialogs = eMoveDialogs.NotEatingMoveChosen;
                }

                moveOptions.Clear();
                i_Board.SetPossibleEatMovesListFromPlayersTools(ref moveOptions, m_PlayerTools, m_PlayerNum, singleGameTool);
                moveOptions = resetEatMovesByLastEatingTool(moveOptions, i_MovePair, moveDialogs);
                moveDialogs = (moveOptions.Count > 0 && moveDialogs != eMoveDialogs.NotEatingMoveChosen) ? eMoveDialogs.HaveAnotherEatMove : moveDialogs;
            }

            return moveDialogs;
        }

        private List<MoveOption> resetEatMovesByLastEatingTool(List<MoveOption> i_MoveOptions, MoveOption i_MovePair, eMoveDialogs i_MoveDialogs)
        {
            List<MoveOption> newOptionsList = new List<MoveOption>();

            if (i_MoveDialogs == eMoveDialogs.Succeeded)
            {
                foreach (MoveOption moveOption in i_MoveOptions)
                {
                    if (moveOption.FromPosition.Col == i_MovePair.ToPosition.Col && moveOption.FromPosition.Row == i_MovePair.ToPosition.Row)
                    {
                        newOptionsList.Add(moveOption);
                    }
                }
            }

            return newOptionsList;
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

        private void move(BoardUtilities i_Board, MoveOption i_MoveOption, bool i_IsEatMove)
        {
            GameTool toTool;
            GameTool fromTool = i_Board.GetGameToolByCell(i_MoveOption.FromPosition);
            Position eatenToolPos;
            short indexSaver;

            if (i_IsEatMove)
            {
                eatenToolPos = i_Board.GetTheEatenToolPosFromMoveOption(i_MoveOption);
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
        }

        /// <summary>
        /// Counts how many soldiers/kings still lives.
        /// </summary>
        /// <returns></returns>
        public short NumOfLiveTools()
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

        private void computerMakeRandomMoveFromList(List<MoveOption> i_MoveList, ref MoveOption io_MoveOption)
        {
            Random rand = new Random();
            int randomIndex;

            if (io_MoveOption.FromPosition != null)
            {
                // If a tool made eating move, only it can continue to eat
                i_MoveList = resetEatMovesByLastEatingTool(i_MoveList, io_MoveOption, eMoveDialogs.HaveAnotherEatMove);
            }

            randomIndex = rand.Next(0, i_MoveList.Count - 1);
            io_MoveOption = i_MoveList[randomIndex];
        }
    }
}