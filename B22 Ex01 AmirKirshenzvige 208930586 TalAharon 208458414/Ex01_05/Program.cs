using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex01_05
{
    class Program
    {
        public const int k_NumOfNumbers = 1, k_NumOfDigits = 7;
        static void Main()
        {
            RunApp();
        }

        public static void RunApp()
        {
            //runs the statistics app

            int[] decimalArr;

            Console.WriteLine("Please enter a natural numbers with {0} digits:", k_NumOfDigits);
            decimalArr = ReadNumbersFromUser();

            ShowStatistics(decimalArr);
            Console.ReadLine();
        }

        public static int[] ReadNumbersFromUser()
        {
            //Read numbers in string format from user and insert to o_NumbersArray in decimal format and to o_BinaryArray in binary format

            string input;
            int inputNumber;
            int[] numberInArray = new int[k_NumOfDigits];

            //Checks for each input that is it as required
            input = Console.ReadLine();
            while (!IsLegalInput(input, out inputNumber))
            {
                Console.WriteLine("Ileagl input! try again:");
                input = Console.ReadLine();
            }

            for (int digit = 0; digit < input.Length; digit++)
            {
                numberInArray[digit] = input[digit] - '0';
            }

            return numberInArray;
        }

        public static bool IsLegalInput(string i_InputString, out int o_decimalNumber)
        {
            //Checks that the input is valid - has k_NumOfDigits (number) digits

            bool returnValue = true;

            o_decimalNumber = 0;
            if (i_InputString.Length != k_NumOfDigits || !int.TryParse(i_InputString, out o_decimalNumber) || o_decimalNumber < 0)
            {
                returnValue = false;
            }

            return returnValue;
        }

        public static string ChecksForAverageDigits(int[] i_NumberArray)
        {
            //Gets a number in array and return a string which contains the average digit

            float averageDigitsInInput = i_NumberArray[0];
            string outputString;

            for (int index = 1; index < i_NumberArray.Length; index++)
            {
                averageDigitsInInput += i_NumberArray[index];
            }

            averageDigitsInInput = averageDigitsInInput / k_NumOfDigits;
            outputString = string.Format("The average digit is: {0}", averageDigitsInInput);

            return outputString;
        }

        public static string CountsHowManyDigitsAreDevidedByTwo(int[] i_NumberArray)
        {
            //Gets an array of a decimal numbers and returns a string which contains the amount of digits which are power of two

            int numberOfInputsWhichDevidedByTwo = 0;
            string outputString;

            for (int index = 0; index < i_NumberArray.Length; index++)
            {
                if (i_NumberArray[index] % 2 == 0 && i_NumberArray[index] >= 2)
                {
                    numberOfInputsWhichDevidedByTwo++;
                }
            }

            if (numberOfInputsWhichDevidedByTwo == 0)
            {
                outputString = string.Format("{0} of the digits is devided by two", numberOfInputsWhichDevidedByTwo);
            }
            else if (numberOfInputsWhichDevidedByTwo == 1)
            {
                outputString = string.Format("There is {0} digit that is devided by two", numberOfInputsWhichDevidedByTwo);
            }
            else
            {
                outputString = string.Format("There are {0} digits that are devided by two", numberOfInputsWhichDevidedByTwo);
            }

            return outputString;
        }

        public static string FindTheMinDigitInArray(int[] i_ArrayOfANumber)
        {
            //Gets an array and returns a string which contains the minimum numbers that represent a digit

            int minNunber = i_ArrayOfANumber[0];

            for (int index = 1; index < i_ArrayOfANumber.Length; index++)
            {
                minNunber = Math.Min(i_ArrayOfANumber[index], minNunber);
            }

            return string.Format("The smallest digit is {0}", minNunber);
        }

        public static string FindSmallerThanUnityOccures(int[] i_ArrayOfANumber)
        {
            //Gets a number in array and returns a string which contains how many digits are smaller than the unity digit

            int unityDigit = i_ArrayOfANumber[i_ArrayOfANumber.Length - 1], digitsThatSmallerThanUnityDigit = 0;

            for (int index = 0; index < i_ArrayOfANumber.Length - 1; index++)
            {
                if (unityDigit > i_ArrayOfANumber[index])
                {
                    digitsThatSmallerThanUnityDigit++;
                }
            }

            return string.Format("There are {0} digits that are smaller than the unity digit", digitsThatSmallerThanUnityDigit);
        }

        public static void ShowStatistics(int[] i_ArrayOfADecimalNumber)
        {
            //Get an array that contains a number
            //Shows a statistics as requird

            StringBuilder outputString = new StringBuilder();
            outputString.Append(FindTheMinDigitInArray(i_ArrayOfADecimalNumber)).Append('\n');
            outputString.Append(ChecksForAverageDigits(i_ArrayOfADecimalNumber)).Append('\n');
            outputString.Append(CountsHowManyDigitsAreDevidedByTwo(i_ArrayOfADecimalNumber)).Append('\n');
            outputString.Append(FindSmallerThanUnityOccures(i_ArrayOfADecimalNumber)).Append('\n');
            Console.WriteLine(outputString);
        }
    }
}
