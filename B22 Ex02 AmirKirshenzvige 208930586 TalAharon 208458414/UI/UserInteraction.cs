using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers.UI
{
    public class UserInteraction
    {
        private static readonly short sr_MaxLenghOfUserName = 20;


        public static string GetIsSingleMode()
        //asks the user for game mode (single player or multiplayer) and return player's input
        {
            string userChoiceInput;

            userChoiceInput = Console.ReadLine().ToLower();

            return userChoiceInput;
        }

        public static void ShowUserSingleModeQuestionMessage()
        {
            Console.WriteLine("Would you like to compete against another player? (press y/n)");
        }

        public static string GetInputFromUser()
        //asks the user for input
        {
            string userChoiceInput;

            userChoiceInput = Console.ReadLine();

            return userChoiceInput;
        }

        public static void ShowUserWelcomeToGameMessage()
        {
            Console.WriteLine("####### Checkers Game #######");
        }

        public static void ShowWhichBoardTypeQuestionMessage(string i_BoardTypesListInString)
        {
            Console.WriteLine(i_BoardTypesListInString);
        }

        public static string GetPlayerName()
        //asks the user for a name to player number "i_NumOfPlayer"
        {
            string userChoiceInput = Console.ReadLine();

            return userChoiceInput;
        }

        public static void ShowPlayerNameQuestionMessage(bool i_IsFirstPlayer)
        {
            if (i_IsFirstPlayer)
            {
                Console.WriteLine($"Write first player's name (up to {sr_MaxLenghOfUserName} chars without spaces): ");
            }
            else
            {
                Console.WriteLine($"Write second player's name (up to {sr_MaxLenghOfUserName} chars without spaces): ");
            }
        }

        public static void ShowMoveOptionMessage(string i_MoveString)
        {
            Console.WriteLine(i_MoveString);
        }

        public static void ShowIndexInListMessage(short i_Index)
        {
            Console.Write($"{i_Index}. ");
        }

        public static string GetMovePositions()
        {
            string userChoiceInput = Console.ReadLine();
            return userChoiceInput;
        }

        public static void ShowNamePlayerWithoutNewLineMessage(string i_Name)
        {
            Console.Write(i_Name);
        }

        public static void ShowAskPositionMessage(string name, char i_Shape)
        {
            Console.WriteLine($"{name}'s turn ({i_Shape}). Enter valid move (From>To):");
        }

        public static void ShowInvalidInputMessage()
        {
            Console.WriteLine("Invalid input, try again:");
        }

        public static void ShowPlayerMovedMessage(string i_PlayerName, char i_PlayerShape, string i_MoveString)
        {
            Console.Write($"{i_PlayerName}'s move was ({i_PlayerShape}): ");
            ShowMoveOptionMessage(i_MoveString);
        }

        public static void DhowSingleGameFinishedWithATieMessage()
        {
            Console.WriteLine("Game finished with a tie! There is no valid moves for both players!");
        }

        public static void PShowSingleGameWinnerMessage(string i_WinnerPlayerName, int i_SingleGameScore)
        {
            Console.Write($"The winner in current game is: ");
            ShowPlayerScoreInSingleGameMessage(i_WinnerPlayerName, i_SingleGameScore);
        }

        public static void ShowSingleGameLoserMessage(string i_LoserPlayerName, int i_SingleGameScore)
        {
            Console.Write($"The loser in current game is: ");
            ShowPlayerScoreInSingleGameMessage(i_LoserPlayerName, i_SingleGameScore);
        }

        public static void ShowAllGameWinnerMessage(string i_WinnerPlayerName, int i_AllGameScore)
        {
            Console.Write($"The winner is: ");
            ShowPlayerScoreInAllGameMessage(i_WinnerPlayerName, i_AllGameScore);
        }

        public static void ShowAllGameLoserMessage(string i_LoserPlayerName, int i_AllGameScore)
        {
            Console.Write($"The loser is: ");
            ShowPlayerScoreInAllGameMessage(i_LoserPlayerName, i_AllGameScore);
        }
        public static void ShowAllGameFinishedWithATieMessage()
        {
            Console.WriteLine("Game finished with a tie!");
        }

        public static void ShowPlayersTotalScoreMessage(string i_Player1Name, string i_Player2Name, int i_Player1Score, int i_Player2Score)
        {
            Console.WriteLine("Player's total game scores are:");
            ShowPlayerScoreInAllGameMessage(i_Player1Name, i_Player1Score);
            ShowPlayerScoreInAllGameMessage(i_Player2Name, i_Player2Score);
        }

        public static void ShowPlayerScoreInAllGameMessage(string i_PlayerName, int i_PlayerScore)
        {
            Console.WriteLine($"{i_PlayerName} with total score: {i_PlayerScore} ");
        }

        private static void ShowPlayerScoreInSingleGameMessage(string i_PlayerName, int i_PlayerScore)
        {
            Console.WriteLine($"{i_PlayerName} with score: {i_PlayerScore} ");
        }

        public static void ShowOneEatingOptionMoveIsDoneMessage()
        {
            Console.WriteLine("You have an eat move!");
            Console.Write("Eating move is done for you: ");
        }

        public static void ShowAllGameFinishedMessage()
        {
            Console.WriteLine();
            Console.WriteLine("######### GAME OVER #########");
        }

        public static void ShowExitGameMessage()
        {
            Console.WriteLine("######### GOODBYE #########");
        }

        public static void ShowInvalidMoveMessage()
        {
            Console.WriteLine("Invalid move!");
        }

        public static void ShowSingleGameFinishedMessage()
        {
            Console.WriteLine();
            Console.WriteLine("######### SINGLE GAME FINISHED #########");
        }

        public static void ShowIsAnotherGameMessage()
        {
            Console.WriteLine();
            Console.WriteLine("Would you like to play another game?");
        }

        public static void ShowChooseMoveFromListMessage()
        {
            Console.WriteLine("You have to eat, So peek one of the following eating moves:");
        }
    }
}