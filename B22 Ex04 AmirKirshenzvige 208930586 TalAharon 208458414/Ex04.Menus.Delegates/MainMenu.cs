using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex04.Menus.Delegates
{
    public class MainMenu
    {
        private MenuItem m_MainSubMenu;
        private bool m_ExitEntered = false;

        public MenuItem CurrentMenuItem
        {
            get { return m_MainSubMenu; }
            set { m_MainSubMenu = value; }
        }

        public MainMenu(MenuItem i_MenuItem)
        {
            m_MainSubMenu = i_MenuItem;
        }

        /// <summary>
        /// Starting the main menu
        /// </summary>
        public void Show()
        {
            m_MainSubMenu.OnClicked();
        }

        /// <summary>
        /// Asks user for picking an option from menu until user picks valid choice
        /// Returns user valid choice
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
