using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ex02.Logic;

namespace Ex02.UI
{
    public class Board
    {


        public static void Show(string i_BoardInString)
        {
            Console.WriteLine(i_BoardInString);
        }

        public static void PrintDashedLine(short i_LineLength)
        {
            Console.Write("  ");

            for (short index = 0; index < i_LineLength; index++)
            {
                Console.Write("====");
            }
            Console.Write(Environment.NewLine);
        }
    }
}
