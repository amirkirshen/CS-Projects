using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex02.Logic
{
    public class HandleInputs
    {
        public static readonly string sr_lowerExitInput = "q";
        public static readonly string sr_CapitalExitInput = "Q";
        public enum ePositionInLetter { BigLetter1, SmallLetter1, Arrow, BigLetter2, SmallLetter2 };

        //the func doesn't return from! it returns to only
        public static void GetMovePositions(out MoveOption o_MoveOption, string i_PlayerName, char i_Shape, out bool o_DidExitGame)
        //ask the usr for next step
        //checks if valid
        {
            string fromToPositionsString;

            UI.UserInteraction.Print_AskPosition(i_PlayerName, i_Shape);
            fromToPositionsString = UI.UserInteraction.Get_MovePositions();
            o_MoveOption = new MoveOption(null, null);
            while (!ExitGameByInput(fromToPositionsString) && !IsValidMoveInput(fromToPositionsString)) // If in board bounds.
            {
                //Print Eror - UI
                UI.UserInteraction.Print_InvalidInput();
                fromToPositionsString = UI.UserInteraction.Get_MovePositions();
            }

            if (ExitGameByInput(fromToPositionsString))
            {
                o_DidExitGame = true;
            }
            else
            {
                o_DidExitGame = false;
                ConvertValidStringPositionToPositionType(fromToPositionsString, out o_MoveOption);
            }
        }

        public static bool IsValidMoveInput(string i_PositionInput)
        {
            char bigLetter1 = '.', smallLetter1 = '.', bigLetter2 = '.', smallLetter2 = '.', arrow = '.';
            bool result;

            result = (i_PositionInput.Length == 5) ? true : false;
            if (result)
            //checks that input include only 5 letters
            {
                InitializeFiveLettersFromNextStepInput(i_PositionInput, out bigLetter1, out smallLetter1, out arrow, out bigLetter2, out smallLetter2);
            }

            if (result && (bigLetter1 < 'A' || bigLetter1 > 'Z' || smallLetter1 < 'a' || smallLetter1 > 'z') &&
                (bigLetter2 < 'A' || bigLetter2 > 'Z' || smallLetter2 < 'a' || smallLetter2 > 'z') &&
                arrow == '>')
            {
                result = false;
            }

            return result;
        }

        private static void InitializeFiveLettersFromNextStepInput(string i_Input, out char o_BigLetter1, out char o_SmallLetter1, out char o_Arrow, out char o_BigLetter2, out char o_SmallLetter2)
        {
            o_BigLetter1 = i_Input[(int)ePositionInLetter.BigLetter1];
            o_SmallLetter1 = i_Input[(int)ePositionInLetter.SmallLetter1];
            o_Arrow = i_Input[(int)ePositionInLetter.Arrow];
            o_BigLetter2 = i_Input[(int)ePositionInLetter.BigLetter2];
            o_SmallLetter2 = i_Input[(int)ePositionInLetter.SmallLetter2];
        }

        public static void ConvertValidStringPositionToPositionType(string i_PositionInString, out MoveOption o_MoveOption)
        //Gets a string with 5 letters (Aa>Bb) and returns 2 positions - o_From, o_To
        {
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
            StringBuilder moveString = new StringBuilder();

            moveString.Append((char)(i_MovePair.FromPosition.Col + 'A'));
            moveString.Append((char)(i_MovePair.FromPosition.Row + 'a'));
            moveString.Append('>');
            moveString.Append((char)(i_MovePair.ToPosition.Col + 'A'));
            moveString.Append((char)(i_MovePair.ToPosition.Row + 'a'));

            return moveString.ToString();
        }

        public static bool GetIfSingleMode()
        {
            // Returns true if is a single game mode.
            bool isSingleMode;

            UI.UserInteraction.Print_SingleModeQuestion();
            isSingleMode = (YesNoQuestion()) ? false : true;
            Ex02.ConsoleUtils.Screen.Clear();

            return isSingleMode;
        }

        public static eTypeOfBoard GetBoardType()
        {
            string userChoiceInput;
            int userChoiceSize;
            eTypeOfBoard typeOfBoardResult;

            UI.UserInteraction.Print_BoardTypeQuestion();
            userChoiceInput = UI.UserInteraction.Get_Input();
            while (userChoiceInput.Length != 1 || !int.TryParse(userChoiceInput, out userChoiceSize) || userChoiceSize <= 0 || userChoiceSize > Enum.GetValues(typeof(eTypeOfBoard)).Length)
            {
                UI.UserInteraction.Print_InvalidInput();
                userChoiceInput = UI.UserInteraction.Get_Input();
            }

            if (userChoiceSize == 3)
            {
                typeOfBoardResult = eTypeOfBoard.Big;
            }
            else if (userChoiceSize == 2)
            {
                typeOfBoardResult = eTypeOfBoard.Medium;
            }
            else
            {
                typeOfBoardResult = eTypeOfBoard.Small;
            }

            return typeOfBoardResult;
        }

        public static bool CheckForAnotherGame()
        //Checks if the user wants another game
        {
            bool isAnotherGame;

            UI.UserInteraction.Print_IsAnotherGame();
            isAnotherGame = YesNoQuestion();

            return isAnotherGame;
        }

        private static bool YesNoQuestion()
        //Asks the user for an answer for yes-no question
        {
            string userChoiceInput = UI.UserInteraction.Get_Input();
            bool isYes;

            while (userChoiceInput.Length != 1 || (userChoiceInput != "y" && userChoiceInput != "n"))
            {
                //don't ask the user if he wants a SingleMode but gets an input
                UI.UserInteraction.Print_InvalidInput();
                userChoiceInput = UI.UserInteraction.Get_IsSingleMode();
            }

            isYes = (userChoiceInput == "y") ? true : false;

            return isYes;
        }

        public static MoveOption GetEatMoveFromList(List<MoveOption> i_EatingOptions, out bool o_DidExitGame)
        {
            short index = 0;
            Nullable<short> userChoice = null;
            MoveOption returnMove = new MoveOption(null, null);
            bool isLigelCoordinate = false;

            o_DidExitGame = false;
            UI.UserInteraction.Print_ChooseMoveFromList();
            foreach (MoveOption pair in i_EatingOptions)
            {
                index++;
                UI.UserInteraction.Print_IndexInList(index);
                UI.UserInteraction.Print_MoveOption(ConvertValidMovePositionsToString(pair));
            }

            userChoice = GetNumberOrCoordinate(out o_DidExitGame, out returnMove);
            isLigelCoordinate = i_EatingOptions.Contains(returnMove);
            while (!o_DidExitGame && index < userChoice || ((userChoice <= 0 || userChoice == null )&& !isLigelCoordinate))
            {
                UI.UserInteraction.Print_InvalidInput();
                userChoice = GetNumberOrCoordinate(out o_DidExitGame, out returnMove);
                isLigelCoordinate = IsMoveInList(i_EatingOptions, returnMove);
            }

            if(userChoice != null)
            {
                returnMove = i_EatingOptions[(short)userChoice - 1];
            }

            return returnMove;
        }

        private static bool IsMoveInList(List<MoveOption> i_EatingOptions, MoveOption i_Move)
        {
            bool isMoveInLise = false;

            foreach(MoveOption move in i_EatingOptions)
            {
                if(move.FromPosition.IsEqual(i_Move.FromPosition) && move.ToPosition.IsEqual(i_Move.ToPosition))
                {
                    isMoveInLise = true;
                }
            }

            return isMoveInLise;
        }

        public static string GetPlayerName(bool i_NumOfPlayer)
        {
            string userChoiceInput;

            UI.UserInteraction.Print_PlayerNameQuestion(i_NumOfPlayer);
            userChoiceInput = UI.UserInteraction.Get_PlayerName();
            while (userChoiceInput.Length > 20 || userChoiceInput.Length == 0 || userChoiceInput.Contains(" "))
            {
                UI.UserInteraction.Print_InvalidInput();
                userChoiceInput = UI.UserInteraction.Get_PlayerName();
            }

            Ex02.ConsoleUtils.Screen.Clear();
            return userChoiceInput;
        }

        private static Nullable<short> GetNumberOrCoordinate(out bool o_DidExitGame, out MoveOption o_MoveOption)
        {
            string input = UI.UserInteraction.Get_Input();
            Nullable<short> result = null;
            short userInput = -1;
            bool isCoordinate;
            o_MoveOption = new MoveOption(null, null);

            o_DidExitGame = (ExitGameByInput(input)) ? true : false;
            isCoordinate = (IsValidMoveInput(input)) ? true : false;
            while (!o_DidExitGame && !short.TryParse(input.ToLower(), out userInput) && !isCoordinate)
            {
                UI.UserInteraction.Print_InvalidInput();
                input = UI.UserInteraction.Get_Input();
                o_DidExitGame = (ExitGameByInput(input)) ? true : false;
                isCoordinate = (IsValidMoveInput(input)) ? true : false;
            }

            if (short.TryParse(input, out userInput))
            {
                result = userInput;
            }
            else if(isCoordinate)
            {
                ConvertValidStringPositionToPositionType(input, out o_MoveOption);
            }

            return result;
        }

        public static bool ExitGameByInput(string input)
        {
            bool res = false;

            if (input.Length == 1 && (input == sr_lowerExitInput || input == sr_CapitalExitInput))
            {
                res = true;
            }

            return res;
        }
    }
}