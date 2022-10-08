using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex01_01
{
    class Program
    {
        public const int k_NumOfNumbers = 3, k_NumOfDigits = 8;
        static void Main()
        {
            RunApp();
        }

        public static void RunApp()
        {
            //runs the statistics app

            int[] decimalArr = new int[k_NumOfNumbers];
            int[] binaryArr = new int[k_NumOfNumbers];

            Console.WriteLine("Please enter {0} numbers with {1} digits each in binary format:", k_NumOfNumbers, k_NumOfDigits);
            ReadNumbersFromUser(decimalArr, binaryArr);

            ShowStatistics(decimalArr, binaryArr);
        }

        public static void ReadNumbersFromUser(int[] o_NumbersArray, int[] o_BinaryArray)
        {
            //Read numbers in string format from user and insert to o_NumbersArray in decimal format and to o_BinaryArray in binary format

            bool isBinaryNum;
            string outputString = "Ileagl input! ", input;

            for (int index = 0; index < k_NumOfNumbers; index++)
            {
                //Insert k_NumOfNumbers(a number) numbers to the arrays
                //Checks for each input that is it as required
                input = Console.ReadLine();
                while (!IsLegalInput(input, out isBinaryNum))
                {
                    //Illegal input message
                    if (isBinaryNum)
                    {
                        outputString = string.Format("must be {0} digits", k_NumOfDigits);
                    }
                    else
                    {
                        outputString = string.Format("not a binary number", k_NumOfDigits);
                    }
                    Console.WriteLine(outputString);
                    input = Console.ReadLine();
                }

                //insert the numbers to the array by using other functions
                o_BinaryArray[index] = StringNumberToInt(input);
                o_NumbersArray[index] = BinaryNumberUsersInputToDecimal(input);
            }
        }

        public static int BinaryNumberUsersInputToDecimal(string input)
        {
            //convert binary number in string format to decimal number in int format
            return BinaryToDecimal(StringNumberToInt(input));
        }

        public static bool IsLegalInput(string i_InputString, out bool io_IsBinaryNum)
        {
            //Checks that the input is valid - has k_NumOfDigits (number) digits and in binary format

            bool returnValue = true, isBinary = true;

            returnValue = i_InputString.Length == k_NumOfDigits;
            for (int index = 0; index < i_InputString.Length && returnValue; index++)
            {
                if (i_InputString[index] != '0' && i_InputString[index] != '1')
                {
                    returnValue = false;
                    isBinary = false;
                    break;
                }
            }

            io_IsBinaryNum = isBinary;
            return returnValue;
        }

        public static int BinaryToDecimal(int binaryNumber)
        {
            //convert a binary number to decimal

            int power = 1;
            int decimalNumber = 0;

            while (binaryNumber > 0)
            {
                if (binaryNumber % 2 != 0)
                {
                    decimalNumber += power;
                }
                power *= 2;
                binaryNumber /= 10;
            }
            return decimalNumber;
        }

        public static int StringNumberToInt(string i_InputString)
        {
            //convert a number to string
            int binaryNumber = Int32.Parse(i_InputString);

            return binaryNumber;
        }

        public static string GetArreyValueInString(int[] i_Array)
        {
            //Returns a string which contains the array members

            StringBuilder outputString = new StringBuilder();
            outputString.Append("The numbers in decimal are ");
            for (int index = 0; index < i_Array.Length; index++)
            {
                outputString.AppendFormat("{0}", i_Array[index]);
                if (index != i_Array.Length - 1)
                {
                    outputString.Append(' ');
                }
            }

            outputString.Append('.');

            return outputString.ToString();
        }

        public static void CountsOnesAndZeros(int i_Num, ref float io_NumOfOnes, ref float io_NumOfZeros)
        {
            //Gets a number in binary format and inserts to io_NumOfOnes the number of 1 and to io_NumOfZeros number of 0

            for (int index = 0; index < k_NumOfDigits; index++)
            {
                if (i_Num % 2 == 0)
                {
                    io_NumOfZeros++;
                }
                else
                {
                    io_NumOfOnes++;
                }
                i_Num /= 10;
            }
        }

        public static string CountsAverageOnesAndZeros(int[] i_NumberArray)
        {
            //Gets an array of binary numbers and return a string which contains the average og 1 and 0

            float averageZerosInInput = 0, averageOnesInInput = 0;
            string outputString;

            for (int index = 0; index < k_NumOfNumbers; index++)
            {
                CountsOnesAndZeros(i_NumberArray[index], ref averageOnesInInput, ref averageZerosInInput);
            }

            outputString = string.Format("The average num of zeros is: {0} and the average num of Ones is: {1}", averageZerosInInput / k_NumOfNumbers, averageOnesInInput / k_NumOfNumbers);
            return outputString;
        }

        public static bool CheckIfPalindrome(int i_Number)
        {
            //Gets a decimal number and return a true if palindrome or else otherwise

            string numberInString = i_Number.ToString();
            int beginIndex = 0, endIndex = numberInString.Length - 1;
            bool isPalindrome = true;

            while (beginIndex < endIndex && isPalindrome)
            {
                isPalindrome = !(numberInString[beginIndex] != numberInString[endIndex]);
                beginIndex++;
                endIndex--;
            }

            return isPalindrome;
        }

        public static string CountsNumOfPalindroms(int[] i_NumberArray)
        {
            //Gets an array of decimal numbers and return a string that contains the number of palindrome un the array

            int numOfPalindroms = 0;
            string outputString;

            for (int index = 0; index < k_NumOfNumbers; index++)
            {
                if (CheckIfPalindrome(i_NumberArray[index]))
                {
                    numOfPalindroms++;
                }
            }

            if (numOfPalindroms == 0 || numOfPalindroms == 1)
            {
                outputString = string.Format("{0} of the numbers is a palindrome", numOfPalindroms);
            }
            else
            {
                outputString = string.Format("{0} of the numbers are palindrome", numOfPalindroms);
            }

            return outputString;
        }

        public static bool IsPowerOfTwo(double i_Numer)
        {
            //Checks if a number is a power of 2

            bool isPower = true;

            while (i_Numer > 2)
            {
                i_Numer /= 2;
            }

            isPower = i_Numer == 2;

            return isPower;
        }

        public static string CountsHowManyNumbersArePowersOfTwo(int[] i_NumbersArray)
        {
            //Gets an array od decimal numbers and returns a string which contains the amount

            int numberOfInputsWhichPowerOfTwo = 0;
            string outputString;

            for (int index = 0; index < k_NumOfNumbers; index++)
            {
                if (IsPowerOfTwo((double)i_NumbersArray[index]))
                {
                    numberOfInputsWhichPowerOfTwo++;
                }
            }

            if (numberOfInputsWhichPowerOfTwo == 0)
            {
                outputString = string.Format("{0} of the numbers is power of two", numberOfInputsWhichPowerOfTwo);
            }
            else if (numberOfInputsWhichPowerOfTwo == 1)
            {
                outputString = string.Format("There is {0} number that is power of two", numberOfInputsWhichPowerOfTwo);
            }
            else
            {
                outputString = string.Format("There are {0} numbers that are power of two", numberOfInputsWhichPowerOfTwo);
            }

            return outputString;
        }

        public static string FindTheMaxAndMinNumbersInArray(int[] i_ArrayOfNumber)
        {
            //Gets an array and returns a string which contains the maximum and minimum numbers

            int maxNumber = i_ArrayOfNumber[0], minNunber = i_ArrayOfNumber[0];

            for (int index = 1; index < k_NumOfNumbers; index++)
            {
                if (maxNumber < i_ArrayOfNumber[index])
                {
                    maxNumber = i_ArrayOfNumber[index];
                }

                if (minNunber > i_ArrayOfNumber[index])
                {
                    minNunber = i_ArrayOfNumber[index];
                }
            }

            return string.Format("The biggest number is {0} and the smallest is {1}", maxNumber, minNunber);
        }

        public static void ShowStatistics(int[] i_ArrayOfDecimalNumbers, int[] i_ArrayOfBinaryNumbers)
        {
            //Get 2 arrays that contains the same numbers - one in decimal format and the other in binary format
            //Shows a statistics as requird

            StringBuilder outputString = new StringBuilder();
            outputString.Append(GetArreyValueInString(i_ArrayOfDecimalNumbers)).Append('\n');
            outputString.Append(CountsAverageOnesAndZeros(i_ArrayOfBinaryNumbers)).Append('\n');
            outputString.Append(CountsHowManyNumbersArePowersOfTwo(i_ArrayOfDecimalNumbers)).Append('\n');
            outputString.Append(CountsNumOfPalindroms(i_ArrayOfDecimalNumbers)).Append('\n');
            outputString.Append(FindTheMaxAndMinNumbersInArray(i_ArrayOfDecimalNumbers));
            Console.WriteLine(outputString);
        }
    }
}
