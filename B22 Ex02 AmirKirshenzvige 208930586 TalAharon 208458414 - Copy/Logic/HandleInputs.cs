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

        public static void GetMovePositions(out MoveOption o_MoveOption, string i_PlayerName, char i_Shape, out bool o_DidExitGame)
        {
            // Getting position string input from user. Checks if the string is valid and return the outputted move option variable. 
            string fromToPositionsString;

            UI.UserInteraction.ShowAskPositionMessage(i_PlayerName, i_Shape);
            fromToPositionsString = UI.UserInteraction.GetMovePositions();
            o_MoveOption = new MoveOption(null, null);
            while (!IsUserExitGame(fromToPositionsString) && !IsValidMoveInput(fromToPositionsString)) // If in board bounds.
            {
                UI.UserInteraction.ShowInvalidInputMessage();
                fromToPositionsString = UI.UserInteraction.GetMovePositions();
            }

            if (IsUserExitGame(fromToPositionsString)) // If user chose to exit game.
            {
                o_DidExitGame = true;
            }
            else // Building the move option from string.
            {
                o_DidExitGame = false;
                ConvertValidStringPositionToPositionType(fromToPositionsString, out o_MoveOption);
            }
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
                InitializeFiveLettersFromNextStepInput(i_PositionInput, out bigLetter1, out smallLetter1, out arrow, out bigLetter2, out smallLetter2);
            }

            if (!result || (bigLetter1 < 'A' || bigLetter1 > 'Z' || smallLetter1 < 'a' || smallLetter1 > 'z') ||
                (bigLetter2 < 'A' || bigLetter2 > 'Z' || smallLetter2 < 'a' || smallLetter2 > 'z') ||
                arrow != '>')
            {
                result = false;
            }

            return result;
        }

        private static void InitializeFiveLettersFromNextStepInput(string i_Input, out char o_BigLetter1, out char o_SmallLetter1, out char o_Arrow, out char o_BigLetter2, out char o_SmallLetter2)
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

        public static bool GetIfSingleMode()
        {
            // Returns true if is a single game mode.
            bool isSingleMode;

            UI.UserInteraction.ShowUserSingleModeQuestionMessage();
            isSingleMode = (YesNoQuestion()) ? false : true;
            Ex02.ConsoleUtils.Screen.Clear();

            return isSingleMode;
        }

        public static eTypeOfBoard GetBoardType()
        {
            // Showing the board sizes to user and getting valid board size from him.
            string userChoiceInput;
            int userChoiceSize;
            eTypeOfBoard typeOfBoardResult;

            ShowBoardTypesList();
            userChoiceInput = UI.UserInteraction.GetInputFromUser();
            while (userChoiceInput.Length != 1 || !int.TryParse(userChoiceInput, out userChoiceSize) || userChoiceSize <= 0 || userChoiceSize > Enum.GetValues(typeof(eTypeOfBoard)).Length)
            {
                UI.UserInteraction.ShowInvalidInputMessage();
                userChoiceInput = UI.UserInteraction.GetInputFromUser();
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

            UI.UserInteraction.ShowIsAnotherGameMessage();
            isAnotherGame = YesNoQuestion();

            return isAnotherGame;
        }

        private static bool YesNoQuestion()
        //Asks the user for an answer for yes-no question
        {
            string userChoiceInput = UI.UserInteraction.GetInputFromUser();
            bool isYes;

            while (userChoiceInput.Length != 1 || (userChoiceInput != "y" && userChoiceInput != "n"))
            {
                //don't ask the user if he wants a SingleMode but gets an input
                UI.UserInteraction.ShowInvalidInputMessage();
                userChoiceInput = UI.UserInteraction.GetIsSingleMode();
            }

            isYes = (userChoiceInput == "y") ? true : false;

            return isYes;
        }

        public static MoveOption GetEatMoveFromList(List<MoveOption> i_EatingOptions, out bool o_DidExitGame)
        {
            // Getting a move option from the user which shown on the list.
            // The move option from the list can be submitted as index from the moves list or move string.
            short index = 0;
            Nullable<short> userChoice = null;
            MoveOption returnMove = new MoveOption(null, null);
            bool isLigelCoordinate = false;

            o_DidExitGame = false;
            UI.UserInteraction.ShowChooseMoveFromListMessage();
            foreach (MoveOption pair in i_EatingOptions)
            {
                index++;
                UI.UserInteraction.ShowIndexInListMessage(index);
                UI.UserInteraction.ShowMoveOptionMessage(ConvertValidMovePositionsToString(pair));
            }

            userChoice = GetNumberOrCoordinate(out o_DidExitGame, out returnMove);
            isLigelCoordinate = (userChoice == -1) ? IsMoveInList(i_EatingOptions, returnMove) : false;
            while (!o_DidExitGame && index < userChoice || ((userChoice <= 0 || userChoice == null) && !isLigelCoordinate))
            {
                UI.UserInteraction.ShowInvalidInputMessage();
                userChoice = GetNumberOrCoordinate(out o_DidExitGame, out returnMove);
                isLigelCoordinate = IsMoveInList(i_EatingOptions, returnMove);
            }

            if (userChoice != null)
            {
                returnMove = i_EatingOptions[(short)userChoice - 1];
            }

            return returnMove;
        }

        public static bool IsMoveInList(List<MoveOption> i_EatingOptions, MoveOption i_Move)
        {
            // Checking if i_Move is in i_EatingOptions list.
            bool isMoveInLise = false;
            foreach (MoveOption move in i_EatingOptions)
            {
                if (move.FromPosition.IsEqual(i_Move.FromPosition) && move.ToPosition.IsEqual(i_Move.ToPosition))
                {
                    isMoveInLise = true;
                }
            }

            return isMoveInLise;
        }

        public static string GetPlayerName(bool i_NumOfPlayer)
        {
            // Requesting a name from the player.
            string userChoiceInput;

            UI.UserInteraction.ShowPlayerNameQuestionMessage(i_NumOfPlayer);
            userChoiceInput = UI.UserInteraction.GetPlayerName();
            while (userChoiceInput.Length > 20 || userChoiceInput.Length == 0 || userChoiceInput.Contains(" "))
            {
                UI.UserInteraction.ShowInvalidInputMessage();
                userChoiceInput = UI.UserInteraction.GetPlayerName();
            }

            Ex02.ConsoleUtils.Screen.Clear();
            return userChoiceInput;
        }

        private static Nullable<short> GetNumberOrCoordinate(out bool o_DidExitGame, out MoveOption o_MoveOption)
        {
            // Force the user to bring a coordinate pattern or number (Represents index in moves options list).
            string input = UI.UserInteraction.GetInputFromUser();
            Nullable<short> result = null;
            short userInput = -1;
            bool isCoordinate;
            o_MoveOption = new MoveOption(null, null);

            o_DidExitGame = (IsUserExitGame(input)) ? true : false;
            isCoordinate = (IsValidMoveInput(input)) ? true : false;
            while (!o_DidExitGame && !short.TryParse(input.ToLower(), out userInput) && !isCoordinate)
            {
                UI.UserInteraction.ShowInvalidInputMessage();
                input = UI.UserInteraction.GetInputFromUser();
                o_DidExitGame = (IsUserExitGame(input)) ? true : false;
                isCoordinate = (IsValidMoveInput(input)) ? true : false;
            }

            if (short.TryParse(input, out userInput))
            {
                result = userInput;
            }
            else if (isCoordinate)
            {
                ConvertValidStringPositionToPositionType(input, out o_MoveOption);
            }

            return result;
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

        public static void ShowBoardTypesList()
        {
            // Show the board's sizes options to user.
            int index = 1;
            StringBuilder printBoardList = new StringBuilder("Choose game board size:\n");

            foreach (eTypeOfBoard size in Enum.GetValues(typeof(eTypeOfBoard)))
            {
                printBoardList.Append(index).Append(". ").Append(size).Append("\n");
                index++;
            }

            UI.UserInteraction.ShowWhichBoardTypeQuestionMessage(printBoardList.ToString());
        }
    }
}