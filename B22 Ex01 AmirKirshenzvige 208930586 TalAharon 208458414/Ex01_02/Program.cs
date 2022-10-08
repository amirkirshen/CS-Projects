using System;

namespace Ex01_02
{
    public class Program
    {
        public static void Main()
        {
            PrintHourGlass();
        }

        public static void PrintHourGlass(int i_Height = 5, int i_NumOfSpaces = 0)
        {
            // Recursive method for printing the Hourglass by it's height.
            // inputs: i_Height - the hourglass height
            //         i_NumOfSpaces - the distance from the side of the console (optional)

            PrintSingleHourGlassRow(i_Height, i_NumOfSpaces);
            if (i_Height > 1)
            {
                PrintHourGlass(i_Height - 2, i_NumOfSpaces + 1);
                PrintSingleHourGlassRow(i_Height, i_NumOfSpaces);
            }
        }

        private static void PrintSingleHourGlassRow(int i_NumOfStars, int i_NumOfSpaces)
        {
            // Printing Single row in the pattern : (Spaces, Stars)
            // inputs: i_NumOfStars - num of stars in a single row
            //         i_NumOfSpaces - the distance from the side of the console (optional)

            for (int index = 0; index < i_NumOfSpaces; index++)
            {
                Console.Write(" ");
            }

            for (int index = 0; index < i_NumOfStars; index++)
            {
                Console.Write("*");
            }
            Console.WriteLine();
        }
    }
}
