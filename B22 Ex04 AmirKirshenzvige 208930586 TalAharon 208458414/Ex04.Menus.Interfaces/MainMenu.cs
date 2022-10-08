using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex04.Menus.Interfaces
{
    public class MainMenu
    {
        private MenuItem m_CurrentMenuItem;
        private bool m_ExitEntered = false;

        public MainMenu(MenuItem i_MenuItem)
        {
            m_CurrentMenuItem = i_MenuItem;
        }

        /// <summary>
        /// Starting the main menu
        /// </summary>
        public void Show()
        {
            m_CurrentMenuItem.ClickListener.OnClick();
        }

        public static void ShowMenuOptions(Dictionary<int, MenuItem> i_SubMenu)
        {
            foreach (KeyValuePair<int, MenuItem> menuOption in i_SubMenu)
            {
                if (menuOption.Key != 0)
                {
                    Console.WriteLine($"{menuOption.Key} -> {menuOption.Value.Text}");
                }
            }

            Console.WriteLine($"{0} -> {i_SubMenu[0].Text}");
        }

        /// <summary>
        /// Asks user for a valid menu option by askin an integer number from 0 to i_MaxValue
        /// </summary>
        /// <param name="i_MaxValue"></param>
        /// <returns></returns>
        public static int GetValidMenuOptionKeyFromUser(int i_MaxValue)
        {
            string strInput;
            int optionNumberKey;

            Console.WriteLine($"Enter your request (1 to {i_MaxValue} or press '0' to Exit)");
            strInput = Console.ReadLine();

            while (!int.TryParse(strInput, out optionNumberKey) || !isInOptionsRange(optionNumberKey, i_MaxValue))
            {
                Console.WriteLine("Invalid input, try again:\n");
                strInput = Console.ReadLine();
            }

            return optionNumberKey;
        }

        private static bool isInOptionsRange(int i_OptionNumberKey, int i_MaxValue)
        {
            return i_OptionNumberKey >= 0 && i_OptionNumberKey <= i_MaxValue;
        }

    }
}
