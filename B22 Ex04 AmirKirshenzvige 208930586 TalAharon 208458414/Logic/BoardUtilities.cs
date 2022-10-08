using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Checkers.UI;

namespace Checkers.Logic
{
    public enum eTypeOfBoard { Small = 6, Medium = 8, Big = 10 }
    public enum eDeltaBySteps { OneStep = 1, TwoSteps = 2 }

    public class BoardUtilities
    {
        private GameTool[,] m_Board;
        private eTypeOfBoard m_BoardSize;

        public eTypeOfBoard BoardSize
        {
            get { return m_BoardSize; }
            set { m_BoardSize = (eTypeOfBoard)value; }
        }

        internal GameTool[,] Board
        {
            get { return m_Board; }
            set { m_Board = value; }
        }

        public BoardUtilities(eTypeOfBoard i_Size)
        {
            short numOfRowsWithTools = SetBoard(i_Size);

            m_BoardSize = i_Size;
            for (short i = 0; i < (short)i_Size; i++)
            {
                for (short j = 0; j < (short)i_Size; j++)
                {
                    m_Board[i, j] = new GameTool(null, eOwnerPlayer.None, 0);
                }
            }

            InitializeTools(numOfRowsWithTools);
        }

        private short SetBoard(eTypeOfBoard i_Size)
        {
            short numOfUserToolsRows = (short)Math.Ceiling((short)i_Size / 3.0);

            m_BoardSize = i_Size;
            m_Board = new GameTool[(short)i_Size, (short)i_Size];
            return numOfUserToolsRows;
        }

        private void InitializeTools(short i_NumOfUserToolsRows)
        {
            InitializeOneSideTools(i_NumOfUserToolsRows, true);
            InitializeOneSideTools(i_NumOfUserToolsRows, false);
        }

        private void InitializeOneSideTools(short i_NumOfUserToolsRows, bool i_InitalizeUpperTools)
        {
            short numOfItration = 0, row;
            short col;

            row = (short)(i_InitalizeUpperTools ? 0 : (m_BoardSize - 1));
            for (; numOfItration < i_NumOfUserToolsRows; numOfItration++)
            {
                col = (short)((row % 2 == 0) ? 1 : 0);
                for (; col < (short)m_BoardSize; col += 2)
                {
                    Position pos = new Position(row, col);
                    eOwnerPlayer owner = i_InitalizeUpperTools ? eOwnerPlayer.Second : eOwnerPlayer.First;
                    if (m_Board[row, col].Owner == eOwnerPlayer.None)
                    {
                        m_Board[row, col].Position = pos;
                        m_Board[row, col].Owner = owner;
                    }
                }

                row = (short)(i_InitalizeUpperTools ? row + 1 : row - 1);
            }
        }

        public GameTool GetGameToolByCell(Position i_Pos)
        {
            return m_Board[i_Pos.Row, i_Pos.Col];
        }

        public void SetGameToolByCell(Position i_Pos, eOwnerPlayer i_Owner, eToolType i_ToolType, short i_Index)
        {
            m_Board[i_Pos.Row, i_Pos.Col] = new GameTool(i_Pos, i_Owner, i_ToolType, i_Index);
        }

        public void ResetCell(Position i_Pos)
        {
            m_Board[i_Pos.Row, i_Pos.Col].Owner = eOwnerPlayer.None;
        }

        public void Show()
        {
            short boardSize = ((short)m_BoardSize);
            StringBuilder boardInString = new StringBuilder();

            // Display columns names:
            boardInString.Append(GetColumnNamesInString((short)m_BoardSize));
            boardInString.Append(Environment.NewLine);
            // Display rows names, frame and game tools:
            boardInString.Append(GetBoardDataFrameInString());
            boardInString.Append(GetDashedLineInString((short)(boardSize)));
            UI.Board.Show(boardInString.ToString());
        }

        private static string GetDashedLineInString(short i_LineLength)
        {
            StringBuilder dashLineString = new StringBuilder();

            dashLineString.Append("  ");
            for (short index = 0; index < i_LineLength; index++)
            {
                dashLineString.Append("====");
            }

            dashLineString.Append(Environment.NewLine);

            return dashLineString.ToString();
        }

        private static string GetColumnNamesInString(short i_BoardSize)
        {
            StringBuilder columnNames = new StringBuilder();

            // Display columns names:
            columnNames.Append("  ");
            for (short index = 0; index < i_BoardSize; index++)
            {
                columnNames.Append($" {(char)('A' + index)}  ");
            }

            return columnNames.ToString();
        }

        private string GetBoardDataFrameInString()
        {
            StringBuilder boardInString = new StringBuilder();
            Position tempPos;
            GameTool currTool;

            for (short row = 0; row < (short)m_BoardSize; row++)
            {
                boardInString.Append(GetDashedLineInString((short)m_BoardSize));
                for (short col = 0; col < (short)m_BoardSize; col++)
                {
                    tempPos = new Position(row, col);
                    currTool = GetGameToolByCell(tempPos);
                    if (col == 0)
                    {
                        boardInString.Append($"{(char)('a' + row)}|");
                    }

                    if (currTool.Owner == eOwnerPlayer.None)
                    {
                        boardInString.Append("   |");
                    }
                    else
                    {
                        boardInString.Append(GetPlayerToolInString(currTool.Owner, currTool.Type));
                    }
                }

                boardInString.Append(Environment.NewLine);
            }

            return boardInString.ToString();
        }


        private char[,] GetBoardDataFrameInMatrix()
        {
            Position tempPos;
            GameTool currTool;
            char[,] charMatrix = new char[(short)BoardSize, (short)BoardSize];

            for (short row = 0; row < (short)m_BoardSize; row++)
            {
                for (short col = 0; col < (short)m_BoardSize; col++)
                {
                    tempPos = new Position(row, col);
                    currTool = GetGameToolByCell(tempPos);
                    switch (currTool.Owner)
                    {
                        case eOwnerPlayer.None:
                            charMatrix[row, col] = ' ';
                            break;
                        case eOwnerPlayer.First:
                            if (currTool.Type == eToolType.Soldier)
                            {
                                charMatrix[row, col] = Player.sr_Player1Soldier;
                            }
                            else
                            {
                                charMatrix[row, col] = Player.sr_Player1King;
                            }

                            break;
                        case eOwnerPlayer.Second:
                            if(currTool.Type == eToolType.Soldier)
                            {
                                charMatrix[row, col] = Player.sr_Player2Soldier;
                            }
                            else
                            {
                                charMatrix[row, col] = Player.sr_Player2King;
                            }

                            break;
                    }
                }
            }

            return charMatrix;
        }

        private static string GetPlayerToolInString(eOwnerPlayer i_PlayerNum, eToolType i_ToolType)
        {
            StringBuilder playerTool = new StringBuilder();

            if (i_PlayerNum == eOwnerPlayer.First)
            {
                if (i_ToolType == eToolType.Soldier)
                    playerTool.Append($" {Logic.Player.sr_Player1Soldier} |");
                else
                    playerTool.Append($" {Logic.Player.sr_Player1King} |");
            }
            else // Player 2
            {
                if (i_ToolType == eToolType.Soldier)
                    playerTool.Append($" {Logic.Player.sr_Player2Soldier} |");
                else
                    playerTool.Append($" {Logic.Player.sr_Player2King} |");
            }

            return playerTool.ToString();
        }

        public bool IsMoveWithoutEatingValids(MoveOption i_MoveOption, eOwnerPlayer i_NumOfPlayer)
        {
            // Checking directed path from i_From position to i_To position (NOT an eat move).
            // Gets the two tools from, to and checks if their positions are in the same diagonal according to the player.
            bool isValidMove = true;
            GameTool fromTool = (!IsPositionInBounds(i_MoveOption.FromPosition)) ? null : GetGameToolByCell(i_MoveOption.FromPosition);
            GameTool toTool = (!IsPositionInBounds(i_MoveOption.ToPosition)) ? null : GetGameToolByCell(i_MoveOption.ToPosition);
            Position moveOptionPos1, moveOptionPos2, moveOptionPos3, moveOptionPos4;
            Position potentialMove;

            if (fromTool == null || toTool == null)
            {
                isValidMove = false;
            }
            else if (fromTool.Owner == i_NumOfPlayer)
            {
                BoardUtilities.InitFourMovingOptionsFromPlayerPositionByDelta(fromTool.Owner, fromTool.Position, out moveOptionPos1, out moveOptionPos2,
                    out moveOptionPos3, out moveOptionPos4, eDeltaBySteps.OneStep);

                potentialMove = WhichDirectionPositionsMatchToDestinationTool(i_MoveOption.ToPosition, fromTool.Type, moveOptionPos1, moveOptionPos2, moveOptionPos3, moveOptionPos4);

                if (potentialMove == null || !IsPositionInBounds(potentialMove) || GetGameToolByCell(potentialMove).Owner != eOwnerPlayer.None)
                {
                    isValidMove = false;
                }
            }
            else
            {
                isValidMove = false;
            }

            return isValidMove;
        }

        private Position WhichDirectionPositionsMatchToDestinationTool(Position i_DestionationPos, eToolType i_ToolType,
            Position i_ForwardPos1, Position i_ForwardPos2, Position i_BackwardPos1, Position i_BackwardPos2)
        {
            Position potentialTool = null;

            if (i_DestionationPos.IsEqual(i_ForwardPos1))
            {
                potentialTool = i_ForwardPos1;
            }
            else if (i_DestionationPos.IsEqual(i_ForwardPos2))
            {
                potentialTool = i_ForwardPos2;
            }
            else if (i_ToolType == eToolType.King)
            {
                if (i_DestionationPos.IsEqual(i_BackwardPos1))
                {
                    potentialTool = i_BackwardPos1;
                }
                else if (i_DestionationPos.IsEqual(i_BackwardPos2))
                {
                    potentialTool = i_BackwardPos2;
                }
            }

            return potentialTool;
        }

        public void SetPossibleEatMovesListFromPlayersTools(ref List<MoveOption> io_EatingOptions, GameTool[] i_PlayerTools,
            eOwnerPlayer i_NumOfPlayer, GameTool i_SingleTool = null)
        {
            // Moves throgh all player's tools and check for valid eat move.
            // If there is some valid moves --> Adding them to the eating options list.
            Position firstMoveOptionPos1, firstMoveOptionPos2, firstMoveOptionPos3, firstMoveOptionPos4;
            Position secondMoveOptionPos1, secondMoveOptionPos2, secondMoveOptionPos3, secondMoveOptionPos4;
            eOwnerPlayer rivalPlayer = (i_NumOfPlayer == eOwnerPlayer.First) ? eOwnerPlayer.Second : eOwnerPlayer.First;

            foreach (GameTool tool in i_PlayerTools)
            {
                if (tool.Owner != eOwnerPlayer.None && (i_SingleTool == null || tool == i_SingleTool))
                {
                    //initializing first and second layers of positions by tool position
                    //send it to "InsertValidEatsMoveToList" wich insert to "io_EatingOptions" if valid eat move
                    InitFourMovingOptionsFromPlayerPositionByDelta(i_NumOfPlayer, tool.Position, out firstMoveOptionPos1, out firstMoveOptionPos2, out firstMoveOptionPos3, out firstMoveOptionPos4, eDeltaBySteps.OneStep);
                    InitFourMovingOptionsFromPlayerPositionByDelta(i_NumOfPlayer, tool.Position, out secondMoveOptionPos1, out secondMoveOptionPos2, out secondMoveOptionPos3, out secondMoveOptionPos4, eDeltaBySteps.TwoSteps);
                    InsertValidEatMovesToList(tool.Position, firstMoveOptionPos1, secondMoveOptionPos1, rivalPlayer, ref io_EatingOptions, i_NumOfPlayer);
                    InsertValidEatMovesToList(tool.Position, firstMoveOptionPos2, secondMoveOptionPos2, rivalPlayer, ref io_EatingOptions, i_NumOfPlayer);
                    if (tool.Type == eToolType.King)
                    {
                        InsertValidEatMovesToList(tool.Position, firstMoveOptionPos3, secondMoveOptionPos3, rivalPlayer, ref io_EatingOptions, i_NumOfPlayer);
                        InsertValidEatMovesToList(tool.Position, firstMoveOptionPos4, secondMoveOptionPos4, rivalPlayer, ref io_EatingOptions, i_NumOfPlayer);
                    }
                }
            }
        }

        public void SetPossibleMovesWithoutEatFromPlayersTools(ref List<MoveOption> io_MovingOptions, GameTool[] i_PlayerTools, eOwnerPlayer i_NumOfPlayer)
        {
            // Moves throgh all player's tools and check for valid non-eat move.
            // If there is some valid moves --> Adding them to the options list.
            Position firstMoveOptionPos1, firstMoveOptionPos2, firstMoveOptionPos3, firstMoveOptionPos4;
            eOwnerPlayer rivalPlayer = (i_NumOfPlayer == eOwnerPlayer.First) ? eOwnerPlayer.Second : eOwnerPlayer.First;

            foreach (GameTool tool in i_PlayerTools)
            {
                if (tool.Owner != eOwnerPlayer.None)
                {
                    //initializing first layers of positions by tool position
                    //send it to "InsertValidWithoutEatsMoveToList" wich insert to "io_EatingOptions" if valid eat move
                    InitFourMovingOptionsFromPlayerPositionByDelta(i_NumOfPlayer, tool.Position, out firstMoveOptionPos1, out firstMoveOptionPos2, out firstMoveOptionPos3, out firstMoveOptionPos4, eDeltaBySteps.OneStep);
                    InsertValidWithoutEatsMoveToList(tool.Position, firstMoveOptionPos1, rivalPlayer, ref io_MovingOptions, i_NumOfPlayer);
                    InsertValidWithoutEatsMoveToList(tool.Position, firstMoveOptionPos2, rivalPlayer, ref io_MovingOptions, i_NumOfPlayer);
                    if (tool.Type == eToolType.King)
                    {
                        InsertValidWithoutEatsMoveToList(tool.Position, firstMoveOptionPos3, rivalPlayer, ref io_MovingOptions, i_NumOfPlayer);
                        InsertValidWithoutEatsMoveToList(tool.Position, firstMoveOptionPos4, rivalPlayer, ref io_MovingOptions, i_NumOfPlayer);
                    }
                }
            }
        }

        private void InsertValidEatMovesToList(Position i_ToolPosition, Position i_FirstMoveOptionPos, Position i_SecondMoveOptionPos, eOwnerPlayer i_RivalPlayer,
            ref List<MoveOption> io_EatingOptions, eOwnerPlayer i_NumOfPlayer)
        {
            MoveOption eatingOption;

            if (IsValidEatMove(i_ToolPosition, i_FirstMoveOptionPos, i_SecondMoveOptionPos, i_RivalPlayer, i_NumOfPlayer))
            {
                eatingOption = new MoveOption(i_ToolPosition, i_SecondMoveOptionPos);
                io_EatingOptions.Add(eatingOption);
            }
        }

        public bool IsValidEatMove(Position i_ToolPosition, Position i_FirstMoveOptionPos, Position i_SecondMoveOptionPos, eOwnerPlayer i_RivalPlayer, eOwnerPlayer i_NumOfPlayer)
        {
            bool isValidMove = false;

            if (IsPositionInBounds(i_FirstMoveOptionPos) && GetGameToolByCell(i_FirstMoveOptionPos).Owner != eOwnerPlayer.None && GetGameToolByCell(i_FirstMoveOptionPos).Owner == i_RivalPlayer)
            {
                if (IsPositionInBounds(i_SecondMoveOptionPos) && GetGameToolByCell(i_SecondMoveOptionPos).Owner == eOwnerPlayer.None)
                //checks if in the non adjust position the cell is empty
                {
                    isValidMove = true;
                }
            }

            return isValidMove;
        }

        private void InsertValidWithoutEatsMoveToList(Position i_ToolPosition, Position i_FirstMoveOptionPos, eOwnerPlayer i_RivalPlayer,
            ref List<MoveOption> io_MovingOptions, eOwnerPlayer i_NumOfPlayer)
        {
            MoveOption movingOption;

            if (IsPositionInBounds(i_FirstMoveOptionPos) && GetGameToolByCell(i_FirstMoveOptionPos).Owner == eOwnerPlayer.None)
            {
                movingOption = new MoveOption(i_ToolPosition, i_FirstMoveOptionPos);
                io_MovingOptions.Add(movingOption);
            }
        }

        public bool IsAToolOfPlayerKing(GameTool i_PlayerTool)
        {
            // Checking if the incoming tool is a king according to the owner player and tool position.
            bool isAKing = false;
            Position toolPos = i_PlayerTool.Position;

            if ((i_PlayerTool.Owner == eOwnerPlayer.First && toolPos.Row == 0) // Change to enum !!!!!!
                || (i_PlayerTool.Owner == eOwnerPlayer.Second && toolPos.Row == (short)(m_BoardSize - 1)))
            {
                isAKing = true;
            }

            return isAKing;
        }

        public bool IsMoveToolsPositionsValids(Position i_FromPosition, Position i_ToPosition)
        {
            // Gets the two tools from, to and checks if their positions are in the same diagonal according to the player.
            bool isValidPositions = true;
            Position moveOptionPos1, moveOptionPos2, moveOptionPos3, moveOptionPos4;

            if (i_FromPosition == null || i_ToPosition == null || !IsPositionInBounds(i_ToPosition) || !IsPositionInBounds(i_FromPosition) ||
                GetGameToolByCell(i_FromPosition).Owner == eOwnerPlayer.None || GetGameToolByCell(i_ToPosition).Owner != eOwnerPlayer.None)
            // Checks if destination is valid as input (location in boundray/ destination is free)
            {
                isValidPositions = false;
            }
            else
            // Checks if destination is legal
            {
                InitFourMovingOptionsFromPlayerPositionByDelta(GetGameToolByCell(i_FromPosition).Owner, GetGameToolByCell(i_FromPosition).Position, out moveOptionPos1, out moveOptionPos2, out moveOptionPos3, out moveOptionPos4, eDeltaBySteps.OneStep);
                if (((!IsPositionInBounds(moveOptionPos1) || moveOptionPos1 != i_ToPosition) &&
                    (!IsPositionInBounds(moveOptionPos2) || moveOptionPos2 != i_ToPosition)) &&
                    ((!IsPositionInBounds(moveOptionPos3) || moveOptionPos3 != i_ToPosition) &&
                    (!IsPositionInBounds(moveOptionPos4) || moveOptionPos4 != i_ToPosition) && GetGameToolByCell(i_FromPosition).Type == eToolType.King))
                {
                    isValidPositions = false;
                }
            }

            return isValidPositions;
        }

        public static void InitFourMovingOptionsFromPlayerPositionByDelta(eOwnerPlayer i_NumOfPlayer, Position i_Position, out Position io_FirstMovingOption, out Position io_SecondMovingOption, out Position io_ThirdMovingOption, out Position io_FourthMovingOption, eDeltaBySteps i_Delta)
        {
            //Return second layer(X) of current position(O)                              cur       ---  return: x-x  /cur       -----  return: x---x
            //Soldier type - only FirstMovingOption and SecondMovingOption are relevant  position: -o-          ---  /position: -----          -----
            //King - all of them are relevant                                            delta=1   ---          x-x  /delta=2   --o--          -----
            //                                                                                                       /          -----          -----
            //                                                                                                       /          -----          x---x
            //Delta = 1 or 2
            if (i_NumOfPlayer == eOwnerPlayer.First) // X player -> moves only up.
            {
                io_FirstMovingOption = new Position((short)(i_Position.Row - i_Delta), (short)(i_Position.Col + i_Delta));  // UP RIGHT
                io_SecondMovingOption = new Position((short)(i_Position.Row - i_Delta), (short)(i_Position.Col - i_Delta)); // UP LEFT
                io_ThirdMovingOption = new Position((short)(i_Position.Row + i_Delta), (short)(i_Position.Col + i_Delta));  // DOWN RIGHT
                io_FourthMovingOption = new Position((short)(i_Position.Row + i_Delta), (short)(i_Position.Col - i_Delta)); // DOWN LEFT
            }
            else // O player -> moves only up.
            {
                io_FirstMovingOption = new Position((short)(i_Position.Row + i_Delta), (short)(i_Position.Col - i_Delta)); // DOWN LEFT
                io_SecondMovingOption = new Position((short)(i_Position.Row + i_Delta), (short)(i_Position.Col + i_Delta));  // DOWN RIGHT
                io_ThirdMovingOption = new Position((short)(i_Position.Row - i_Delta), (short)(i_Position.Col - i_Delta)); // UP LEFT
                io_FourthMovingOption = new Position((short)(i_Position.Row - i_Delta), (short)(i_Position.Col + i_Delta));  // UP RIGHT 
            }
        }

        private bool IsPositionInBounds(Position i_Position)
        {
            return ((i_Position.Col >= 0 && i_Position.Col < (short)m_BoardSize) && ((i_Position.Row >= 0 && i_Position.Row < (short)m_BoardSize)));
        }

        public Position GetTheEatenToolPosFromPositionsByPlayerNum(MoveOption i_MoveOption)
        {
            Position eatenToolPos;

            //"Short if" is possible but not readable
            if (i_MoveOption.FromPosition.Col < i_MoveOption.ToPosition.Col)
            {                                                                                                                             //x--   /---
                eatenToolPos = (i_MoveOption.FromPosition.Row < i_MoveOption.ToPosition.Row) ? new Position((short)(i_MoveOption.FromPosition.Row + 1),
                    (short)(i_MoveOption.FromPosition.Col + 1)) :     //-y-   /-y-
                    new Position((short)(i_MoveOption.FromPosition.Row - 1), (short)(i_MoveOption.FromPosition.Col + 1));                                                 //---   /x--
            }
            else
            {                                                                                                                            //--x   /---
                eatenToolPos = (i_MoveOption.FromPosition.Row < i_MoveOption.ToPosition.Row) ? new Position((short)(i_MoveOption.FromPosition.Row + 1),
                    (short)(i_MoveOption.FromPosition.Col - 1)) :    //-y-   /-y-
                    new Position((short)(i_MoveOption.FromPosition.Row - 1), (short)(i_MoveOption.FromPosition.Col - 1));                                                //---   /--x
            }

            return eatenToolPos;
        }
    }
}