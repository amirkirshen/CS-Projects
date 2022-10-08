using System;
using System.Text;

namespace Ex01_04
{
    class Program
    {
        const int STRING_MAX_SIZE = 8;
        static void Main()
        {
            StringBuilder userString;

            GetLegalUserString(out userString);
            StringAnalyzer(userString);
        }

        public static void StringAnalyzer(StringBuilder i_String)
        {
            // Checking and printing if the string is Palindrome, divisible by 3 (if number) and number of lowercase letters (if contains only letters)

            if (IsStringPalindrome(i_String.ToString()))
            {
                Console.WriteLine("The string is a palindrome!");
            }
            else
            {
                Console.WriteLine("The string is not a palindrome!");
            }

            if (IsADigit(i_String[0]))
            {
                int numberInString = int.Parse(i_String.ToString());
                
                if (numberInString % 3 == 0)
                {
                    Console.WriteLine("The number is divisible by 3");
                }
                else
                {
                    Console.WriteLine("The number is not divisible by 3");
                }
            }
            else // Contains only letters.
            {
                short numOfLowercaseLetters = NumOfLowercaseLettersInAString(i_String);
                
                Console.WriteLine("The number of lowercase letters in the string is: " + numOfLowercaseLetters);
            }
        }

        public static void GetLegalUserString(out StringBuilder o_userString)
        {
            Console.WriteLine("Please enter 8 English letters or numbers as your wish");

            o_userString = new StringBuilder(Console.ReadLine());

            // Checking if the input is valid. Asking for re-enter a string till it valid.
            while (!IsValidString(o_userString))
            {
                o_userString = new StringBuilder(Console.ReadLine());
            }
        }

        public static bool IsValidString(StringBuilder i_String)
        {
            //Checking if i_String is valid (length = 8 and contains only letters or only digits)

            int stringLength = i_String.Length;
            bool isValid = true;

            if (stringLength != STRING_MAX_SIZE)
            {
                Console.WriteLine("Invalid input! String length should be 8");
                isValid = false;
            }
            
            if (!IsStringContainOnlyLetters(i_String) && !IsStringContainOnlyDigits(i_String))
            {
                Console.WriteLine("Invalid input! The string is a mixing of letters and digits");
                isValid = false;
            }
            
            return isValid;
        }

        public static bool IsStringContainOnlyLetters(StringBuilder i_String)
        {
            bool isValid = true;
            
            for (short index = 0; index < STRING_MAX_SIZE; index++)
            {
                if (!IsALetter(i_String[index]))
                {
                    isValid = false;
                }
            }
            
            return isValid;
        }

        public static bool IsStringContainOnlyDigits(StringBuilder i_String)
        {
            bool isValid = true;
            
            for (short index = 0; index < STRING_MAX_SIZE; index++)
            {
                if (!IsADigit(i_String[index]))
                {
                    isValid = false;
                }
            }
            
            return isValid;
        }

        public static bool IsALetter(char i_Char)
        {
            return (IsALowercaseLetter(i_Char) || IsACapitalLetter(i_Char));
        }
        
        public static bool IsALowercaseLetter(char i_Char)
        {
            return (i_Char >= 'a' && i_Char <= 'z');
        }
        
        public static bool IsACapitalLetter(char i_Char)
        {
            return (i_Char >= 'A' && i_Char <= 'Z');
        }
        
        public static bool IsADigit(char i_Char)
        {
            return (i_Char >= '0' && i_Char <= '9');
        }
        
        public static short NumOfLowercaseLettersInAString(StringBuilder i_String)
        {
            short numOfLowercaseLetters = 0;

            for (short index = 0; index < STRING_MAX_SIZE; index++)
            {
                if (IsALowercaseLetter(i_String[index]))
                {
                    numOfLowercaseLetters++;
                }
            }
            
            return numOfLowercaseLetters;
        }
        
        public static bool IsStringPalindrome(string i_String)
        {
            int stringLength = i_String.Length;
            bool isPalindrome = true;

            if (i_String.Length == 2)
            {
                isPalindrome = (i_String[0] == i_String[1]);
            }

            isPalindrome = ((i_String[0] == i_String[stringLength - 1]) &&
                    (IsStringPalindrome(i_String.Substring(1, stringLength - 2))));
            
            return isPalindrome;
        }

    }
}
