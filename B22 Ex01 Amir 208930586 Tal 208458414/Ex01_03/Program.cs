using System;

namespace Ex01_03
{
    class Program
    {
        public static void Main()
        {
            int hourGlassHeight;
            
            GetHourGlassHeightFromUser(out hourGlassHeight);
            Ex01_02.Program.PrintHourGlass(hourGlassHeight);
        }

        public static void GetHourGlassHeightFromUser(out int o_hourGlassHeight)
        {
            string heightString;

            Console.WriteLine("Please enter your desired hourglass height");
            Console.WriteLine("(in case of even number the height will be your choice +1)");

            heightString = Console.ReadLine();

            // Trying to parse the user's choice to int, asking for re-enter the height in case of unsuccess.
            while (int.TryParse(heightString, out o_hourGlassHeight) == false && o_hourGlassHeight > 0)
            {
                Console.WriteLine("Wrong choice, try again...");
                heightString = Console.ReadLine();
            }

            // Adding +1 to the height if it's even.
            if (o_hourGlassHeight % 2 == 0)
            {
                o_hourGlassHeight += 1;
            }
        }
    }
}
