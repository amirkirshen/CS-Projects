using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers.Logic
{
    public class HandleInputs
    {
        public static readonly string sr_lowerExitInput = "q";
        public static readonly string sr_CapitalExitInput = "Q";
        public enum ePositionInLetter { BigLetter1, SmallLetter1, Arrow, BigLetter2, SmallLetter2 };

        public static bool GetMovePositions(out MoveOption o_MoveOption, string i_PlayerName, char i_Shape, string i_FromToPositionsString)
        {
            // Getting position string input from user. Checks if the string is valid and return the outputted move option variable. 
            bool isValidMove = true;

            o_MoveOption = new MoveOption(null, null);
            if (!IsUserExitGame(i_FromToPositionsString) && !IsValidMoveInput(i_FromToPositionsString)) // If in board bounds.
            {
                //UI.UserInteraction.ShowInvalidInputMessage();
                //i_FromToPositionsString = UI.UserInteraction.GetMovePositions();
                isValidMove = false;
            }
            else // Building the move option from string.
            {
                ConvertValidStringPositionToPositionType(i_FromToPositionsString, out o_MoveOption);
            }

            return isValidMove;
        }

        public static bool IsValidMoveInput(string i_PositionInput)
        {
            // Checks if the user's input string is in the pattern: e.g Ab>Cd.
            char bigLetter1 = '.', smallLetter1 = '.', bigLetter2 = '.', smallLetter2 = '.', arrow = '.';
            bool result;

            result = (i_PositionInput.Length == 5) ? true : false;
            if (result)
            // Checks if tha input include only 5 letters.
            {
                initializeFiveLettersFromNextStepInput(i_PositionInput, out bigLetter1, out smallLetter1, out arrow, out bigLetter2, out smallLetter2);
            }

            if (!result || (bigLetter1 < 'A' || bigLetter1 > 'Z' || smallLetter1 < 'a' || smallLetter1 > 'z') ||
                (bigLetter2 < 'A' || bigLetter2 > 'Z' || smallLetter2 < 'a' || smallLetter2 > 'z') ||
                arrow != '>')
            {
                result = false;
            }

            return result;
        }

        private static void initializeFiveLettersFromNextStepInput(string i_Input, out char o_BigLetter1, out char o_SmallLetter1, out char o_Arrow, out char o_BigLetter2, out char o_SmallLetter2)
        {
            // Init the five letters (move option) from string.
            o_BigLetter1 = i_Input[(int)ePositionInLetter.BigLetter1];
            o_SmallLetter1 = i_Input[(int)ePositionInLetter.SmallLetter1];
            o_Arrow = i_Input[(int)ePositionInLetter.Arrow];
            o_BigLetter2 = i_Input[(int)ePositionInLetter.BigLetter2];
            o_SmallLetter2 = i_Input[(int)ePositionInLetter.SmallLetter2];
        }

        public static void ConvertValidStringPositionToPositionType(string i_PositionInString, out MoveOption o_MoveOption)
        {
            //Gets a string with 5 letters (Aa>Bb) and returns 2 positions - o_From, o_To
            char bigLetter1 = i_PositionInString[(int)ePositionInLetter.BigLetter1];
            char bigLetter2 = i_PositionInString[(int)ePositionInLetter.BigLetter2];
            char smallLetter1 = i_PositionInString[(int)ePositionInLetter.SmallLetter1];
            char smallLetter2 = i_PositionInString[(int)ePositionInLetter.SmallLetter2];
            short pos1Row = (short)(smallLetter1 - 'a');
            short pos1Col = (short)(bigLetter1 - 'A');
            short pos2Row = (short)(smallLetter2 - 'a');
            short pos2Col = (short)(bigLetter2 - 'A');

            o_MoveOption = new MoveOption(new Position(pos1Row, pos1Col), new Position(pos2Row, pos2Col));
        }

        public static string ConvertValidMovePositionsToString(MoveOption i_MovePair)
        {
            // Converting move option to string.
            StringBuilder moveString = new StringBuilder();

            moveString.Append((char)(i_MovePair.FromPosition.Col + 'A'));
            moveString.Append((char)(i_MovePair.FromPosition.Row + 'a'));
            moveString.Append('>');
            moveString.Append((char)(i_MovePair.ToPosition.Col + 'A'));
            moveString.Append((char)(i_MovePair.ToPosition.Row + 'a'));

            return moveString.ToString();
        }

        public static bool IsUserExitGame(string input)
        {
            // Checking if the input from string is q or Q -> user want to stop the game.
            bool res = false;
            if (input.Length == 1 && (input == sr_lowerExitInput || input == sr_CapitalExitInput))
            {
                res = true;
            }

            return res;
        }
    }
}