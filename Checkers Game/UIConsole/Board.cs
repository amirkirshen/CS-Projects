using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers.UIConsole
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
