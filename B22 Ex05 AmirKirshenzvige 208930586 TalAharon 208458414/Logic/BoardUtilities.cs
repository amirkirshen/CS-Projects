using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public GameTool[,] Board
        {
            get { return m_Board; }
            set { m_Board = value; }
        }

        public BoardUtilities(eTypeOfBoard i_Size)
        {
            short numOfRowsWithTools = setBoard(i_Size);

            m_BoardSize = i_Size;
            for (short i = 0; i < (short)i_Size; i++)
            {
                for (short j = 0; j < (short)i_Size; j++)
                {
                    m_Board[i, j] = new GameTool(new Position(i, j), eOwnerPlayer.None, 0);
                }
            }

            initializeTools(numOfRowsWithTools);
        }

        private short setBoard(eTypeOfBoard i_Size)
        {
            short numOfUserToolsRows = (short)Math.Ceiling((short)i_Size / 3.0);

            m_BoardSize = i_Size;
            m_Board = new GameTool[(short)i_Size, (short)i_Size];

            return numOfUserToolsRows;
        }

        private void initializeTools(short i_NumOfUserToolsRows)
        {
            initializeOneSideTools(i_NumOfUserToolsRows, true);
            initializeOneSideTools(i_NumOfUserToolsRows, false);
        }

        private void initializeOneSideTools(short i_NumOfUserToolsRows, bool i_InitalizeUpperTools)
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
                    insertValidEatMovesToList(tool.Position, firstMoveOptionPos1, secondMoveOptionPos1, rivalPlayer, ref io_EatingOptions);
                    insertValidEatMovesToList(tool.Position, firstMoveOptionPos2, secondMoveOptionPos2, rivalPlayer, ref io_EatingOptions);
                    if (tool.Type == eToolType.King)
                    {
                        insertValidEatMovesToList(tool.Position, firstMoveOptionPos3, secondMoveOptionPos3, rivalPlayer, ref io_EatingOptions);
                        insertValidEatMovesToList(tool.Position, firstMoveOptionPos4, secondMoveOptionPos4, rivalPlayer, ref io_EatingOptions);
                    }
                }
            }
        }

        /// <summary>
        /// Moves throgh all player's tools and check for valid non-eat move.
        /// If there is some valid moves --> Adding them to the options list.
        /// </summary>
        /// <param name="io_MovingOptions"></param>
        /// <param name="i_PlayerTools"></param>
        /// <param name="i_NumOfPlayer"></param>
        public void SetPossibleMovesWithoutEatFromPlayersTools(ref List<MoveOption> io_MovingOptions, GameTool[] i_PlayerTools, eOwnerPlayer i_NumOfPlayer)
        {
            Position firstMoveOptionPos1, firstMoveOptionPos2, firstMoveOptionPos3, firstMoveOptionPos4;
            eOwnerPlayer rivalPlayer = (i_NumOfPlayer == eOwnerPlayer.First) ? eOwnerPlayer.Second : eOwnerPlayer.First;

            foreach (GameTool tool in i_PlayerTools)
            {
                if (tool.Owner != eOwnerPlayer.None)
                {
                    //initializing first layers of positions by tool position
                    //send it to "InsertValidWithoutEatsMoveToList" wich insert to "io_EatingOptions" if valid eat move
                    InitFourMovingOptionsFromPlayerPositionByDelta(i_NumOfPlayer, tool.Position, out firstMoveOptionPos1, out firstMoveOptionPos2, out firstMoveOptionPos3, out firstMoveOptionPos4, eDeltaBySteps.OneStep);
                    insertValidWithoutEatsMoveToList(tool.Position, firstMoveOptionPos1, ref io_MovingOptions);
                    insertValidWithoutEatsMoveToList(tool.Position, firstMoveOptionPos2, ref io_MovingOptions);
                    if (tool.Type == eToolType.King)
                    {
                        insertValidWithoutEatsMoveToList(tool.Position, firstMoveOptionPos3, ref io_MovingOptions);
                        insertValidWithoutEatsMoveToList(tool.Position, firstMoveOptionPos4, ref io_MovingOptions);
                    }
                }
            }
        }

        private void insertValidEatMovesToList(Position i_ToolPosition, Position i_FirstMoveOptionPos, Position i_SecondMoveOptionPos,
                                                eOwnerPlayer i_RivalPlayer, ref List<MoveOption> io_EatingOptions)
        {
            MoveOption eatingOption;

            if (IsValidEatMove(i_FirstMoveOptionPos, i_SecondMoveOptionPos, i_RivalPlayer))
            {
                eatingOption = new MoveOption(i_ToolPosition, i_SecondMoveOptionPos);
                io_EatingOptions.Add(eatingOption);
            }
        }

        public bool IsValidEatMove(Position i_FirstMoveOptionPos, Position i_SecondMoveOptionPos, eOwnerPlayer i_RivalPlayer)
        {
            bool isValidMove = false;

            if (isPositionInBounds(i_FirstMoveOptionPos) && GetGameToolByCell(i_FirstMoveOptionPos).Owner != eOwnerPlayer.None && GetGameToolByCell(i_FirstMoveOptionPos).Owner == i_RivalPlayer)
            {
                if (isPositionInBounds(i_SecondMoveOptionPos) && GetGameToolByCell(i_SecondMoveOptionPos).Owner == eOwnerPlayer.None)
                {
                    //checks if in the non adjust position the cell is empty
                    isValidMove = true;
                }
            }

            return isValidMove;
        }

        private void insertValidWithoutEatsMoveToList(Position i_ToolPosition, Position i_FirstMoveOptionPos, ref List<MoveOption> io_MovingOptions)
        {
            MoveOption movingOption;

            if (isPositionInBounds(i_FirstMoveOptionPos) && GetGameToolByCell(i_FirstMoveOptionPos).Owner == eOwnerPlayer.None)
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

            if ((i_PlayerTool.Owner == eOwnerPlayer.First && toolPos.Row == 0)
                || (i_PlayerTool.Owner == eOwnerPlayer.Second && toolPos.Row == (short)(m_BoardSize - 1)))
            {
                isAKing = true;
            }

            return isAKing;
        }

        //public bool IsMoveToolsPositionsValids(Position i_FromPosition, Position i_ToPosition)
        //{
        //    // Gets the two tools from, to and checks if their positions are in the same diagonal according to the player.
        //    bool isValidPositions = true;
        //    Position moveOptionPos1, moveOptionPos2, moveOptionPos3, moveOptionPos4;

        //    if (i_FromPosition == null || i_ToPosition == null || !isPositionInBounds(i_ToPosition) || !isPositionInBounds(i_FromPosition) ||
        //        GetGameToolByCell(i_FromPosition).Owner == eOwnerPlayer.None || GetGameToolByCell(i_ToPosition).Owner != eOwnerPlayer.None)
        //    // Checks if destination is valid as input (location in boundray/ destination is free)
        //    {
        //        isValidPositions = false;
        //    }
        //    else
        //    // Checks if destination is legal
        //    {
        //        InitFourMovingOptionsFromPlayerPositionByDelta(GetGameToolByCell(i_FromPosition).Owner, GetGameToolByCell(i_FromPosition).Position, out moveOptionPos1, out moveOptionPos2, out moveOptionPos3, out moveOptionPos4, eDeltaBySteps.OneStep);
        //        if (((!isPositionInBounds(moveOptionPos1) || moveOptionPos1 != i_ToPosition) &&
        //            (!isPositionInBounds(moveOptionPos2) || moveOptionPos2 != i_ToPosition)) &&
        //            ((!isPositionInBounds(moveOptionPos3) || moveOptionPos3 != i_ToPosition) &&
        //            (!isPositionInBounds(moveOptionPos4) || moveOptionPos4 != i_ToPosition) && GetGameToolByCell(i_FromPosition).Type == eToolType.King))
        //        {
        //            isValidPositions = false;
        //        }
        //    }

        //    return isValidPositions;
        //}

        public static void InitFourMovingOptionsFromPlayerPositionByDelta(eOwnerPlayer i_NumOfPlayer, Position i_Position, out Position o_FirstMovingOption, out Position o_SecondMovingOption, out Position o_ThirdMovingOption, out Position o_FourthMovingOption, eDeltaBySteps i_Delta)
        {
            // Return second layer(X) of current position(O)                              cur       ---  return: x-x  /cur       -----  return: x---x
            // Soldier type - only FirstMovingOption and SecondMovingOption are relevant  position: -o-          ---  /position: -----          -----
            // King - all of them are relevant                                            delta=1   ---          x-x  /delta=2   --o--          -----
            //                                                                                                        /          -----          -----
            //                                                                                                        /          -----          x---x
            //Delta = 1 or 2
            if (i_NumOfPlayer == eOwnerPlayer.First) // X player -> moves only up.
            {
                o_FirstMovingOption = new Position((short)(i_Position.Row - i_Delta), (short)(i_Position.Col + i_Delta));  // UP RIGHT
                o_SecondMovingOption = new Position((short)(i_Position.Row - i_Delta), (short)(i_Position.Col - i_Delta)); // UP LEFT
                o_ThirdMovingOption = new Position((short)(i_Position.Row + i_Delta), (short)(i_Position.Col + i_Delta));  // DOWN RIGHT
                o_FourthMovingOption = new Position((short)(i_Position.Row + i_Delta), (short)(i_Position.Col - i_Delta)); // DOWN LEFT
            }
            else // O player -> moves only up.
            {
                o_FirstMovingOption = new Position((short)(i_Position.Row + i_Delta), (short)(i_Position.Col - i_Delta)); // DOWN LEFT
                o_SecondMovingOption = new Position((short)(i_Position.Row + i_Delta), (short)(i_Position.Col + i_Delta));  // DOWN RIGHT
                o_ThirdMovingOption = new Position((short)(i_Position.Row - i_Delta), (short)(i_Position.Col - i_Delta)); // UP LEFT
                o_FourthMovingOption = new Position((short)(i_Position.Row - i_Delta), (short)(i_Position.Col + i_Delta));  // UP RIGHT 
            }
        }

        private bool isPositionInBounds(Position i_Position)
        {
            return ((i_Position.Col >= 0 && i_Position.Col < (short)m_BoardSize) && ((i_Position.Row >= 0 && i_Position.Row < (short)m_BoardSize)));
        }

        public Position GetTheEatenToolPosFromMoveOption(MoveOption i_MoveOption)
        {
            Position eatenToolPos;

            if (i_MoveOption.FromPosition.Col < i_MoveOption.ToPosition.Col)
            {
                eatenToolPos = (i_MoveOption.FromPosition.Row < i_MoveOption.ToPosition.Row) ? new Position((short)(i_MoveOption.FromPosition.Row + 1),
                    (short)(i_MoveOption.FromPosition.Col + 1)) :
                    new Position((short)(i_MoveOption.FromPosition.Row - 1), (short)(i_MoveOption.FromPosition.Col + 1));
            }
            else
            {
                eatenToolPos = (i_MoveOption.FromPosition.Row < i_MoveOption.ToPosition.Row) ? new Position((short)(i_MoveOption.FromPosition.Row + 1),
                    (short)(i_MoveOption.FromPosition.Col - 1)) :
                    new Position((short)(i_MoveOption.FromPosition.Row - 1), (short)(i_MoveOption.FromPosition.Col - 1));
            }

            return eatenToolPos;
        }
    }
}