using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ex02.Logic;

namespace Ex02.UI
{
    public class UserInteraction
    {
        private static readonly short sr_MaxLenghOfUserName = 20;


        public static string Get_IsSingleMode()
        //asks the user for game mode (single player or multiplayer) and return player's input
        {
            string userChoiceInput;

            userChoiceInput = Console.ReadLine().ToLower();

            return userChoiceInput;
        }

        public static void Print_SingleModeQuestion()
        {
            Console.WriteLine("Would you like to compete against another player? (press y/n)");
        }

        public static string Get_Input()
        //asks the user for input
        {
            string userChoiceInput;

            userChoiceInput = Console.ReadLine();

            return userChoiceInput;
        }

        public static void Print_WelcomeToGame()
        {
            Console.WriteLine("####### Checkers Game #######");
        }

        public static void Print_BoardTypeQuestion()
        {
            int index = 1;
            StringBuilder printBoardList = new StringBuilder("Choose game board size:\n");

            foreach (eTypeOfBoard size in Enum.GetValues(typeof(eTypeOfBoard)))
            {
                printBoardList.Append(index).Append(". ").Append(size).Append("\n");
                index++;
            }

            Console.Write(printBoardList);
        }

        public static string Get_PlayerName()
        //asks the user for a name to player number "i_NumOfPlayer"
        {
            string userChoiceInput = Console.ReadLine();

            return userChoiceInput;
        }

        public static void Print_PlayerNameQuestion(bool i_IsFirstPlayer)
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

        public static void Print_MoveOption(string i_MoveString)
        {
            Console.WriteLine(i_MoveString);
        }

        public static void Print_IndexInList(short i_Index)
        {
            Console.Write($"{i_Index}. ");
        }

        public static string Get_MovePositions()
        {
            string userChoiceInput = Console.ReadLine();
            return userChoiceInput;
        }

        public static void Print_NamePlayerWithoutNewLine(string i_Name)
        {
            Console.Write(i_Name);
        }

        public static void Print_AskPosition(string name, char i_Shape)
        {
            Console.WriteLine($"{name}'s turn ({i_Shape}). Enter valid move (From>To):");
        }

        public static void Print_InvalidInput()
        {
            Console.WriteLine("Invalid input, try again:");
        }

        public static void Print_PlayerMoved(string i_PlayerName, char i_PlayerShape, string i_MoveString)
        {
            Console.Write($"{i_PlayerName}'s move was ({i_PlayerShape}): ");
            Print_MoveOption(i_MoveString);
        }

        public static void Print_SingleGameFinishedWithATie()
        {
            Console.WriteLine("Game finished with a tie! There is no valid moves for both players!");
        }

        public static void Print_SingleGameWinner(string i_WinnerPlayerName, int i_SingleGameScore)
        {
            Console.Write($"The winner in current game is: ");
            Print_PlayerScoreInSingleGame(i_WinnerPlayerName, i_SingleGameScore);
        }

        public static void Print_SingleGameLoser(string i_LoserPlayerName, int i_SingleGameScore)
        {
            Console.Write($"The loser in current game is: ");
            Print_PlayerScoreInSingleGame(i_LoserPlayerName, i_SingleGameScore);

        }

        public static void Print_AllGameWinner(string i_WinnerPlayerName, int i_AllGameScore)
        {
            Console.Write($"The winner is: ");
            Print_PlayerScoreInAllGame(i_WinnerPlayerName, i_AllGameScore);
        }

        public static void Print_AllGameLoser(string i_LoserPlayerName, int i_AllGameScore)
        {
            Console.Write($"The loser is: ");
            Print_PlayerScoreInAllGame(i_LoserPlayerName, i_AllGameScore);
        }
        public static void Print_AllGameFinishedWithATie()
        {
            Console.WriteLine("Game finished with a tie!");
        }

        public static void Print_PlayersTotalScore(string i_Player1Name, string i_Player2Name, int i_Player1Score, int i_Player2Score)
        {
            Console.WriteLine("Player's total game scores are:");
            Print_PlayerScoreInAllGame(i_Player1Name, i_Player1Score);
            Print_PlayerScoreInAllGame(i_Player2Name, i_Player2Score);
        }

        public static void Print_PlayerScoreInAllGame(string i_PlayerName, int i_PlayerScore)
        {
            Console.WriteLine($"{i_PlayerName} with total score: {i_PlayerScore} ");
        }

        private static void Print_PlayerScoreInSingleGame(string i_PlayerName, int i_PlayerScore)
        {
            Console.WriteLine($"{i_PlayerName} with score: {i_PlayerScore} ");
        }

        public static void Print_OneEatingOptionMoveIsDone()
        {
            Console.WriteLine("You have an eat move!");
            Console.Write("Eating move is done for you: ");
        }

        public static void Print_AllGameFinished()
        {
            Console.WriteLine();
            Console.WriteLine("######### GAME OVER #########");
        }

        public static void Print_ExitGame()
        {
            Console.WriteLine("######### GOODBYE #########");
        }

        public static void Print_InvalidMoveMessage()
        {
            Console.WriteLine("Invalid move!");
        }

        public static void Print_SingleGameFinished()
        {
            Console.WriteLine();
            Console.WriteLine("######### SINGLE GAME FINISHED #########");
        }

        public static void Print_IsAnotherGame()
        {
            Console.WriteLine();
            Console.WriteLine("Would you like to play another game?");
        }

        public static void Print_ChooseMoveFromList()
        {
            Console.WriteLine("You have to eat, So peek one of the following eating moves:");
        }
    }
}