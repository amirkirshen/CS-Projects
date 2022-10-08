using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ex04.Menus.Delegates;

namespace Ex04.Menus.Test
{
    public class MenusDelegatesManager
    {

        public void Run()
        {
            Delegates.IntermediateMenuItem mainMenuItem = new IntermediateMenuItem("Main");
            ExitHandler exitObject = new ExitHandler();
            Delegates.MainMenu delgateMenu = new Ex04.Menus.Delegates.MainMenu(mainMenuItem);

            mainMenuItem.OnClick += mainMenuItem.ManageMenu;
            createDelegatesMenu(mainMenuItem);
            delgateMenu.Show();
        }

        private void createDelegatesMenu(Delegates.MenuItem i_DelegatesMenu)
        {
            Delegates.MenuItem exit = new Delegates.FinalMenuItem("Exit");
            Delegates.MenuItem backOfVersions = new Delegates.FinalMenuItem("Back");
            Delegates.IntermediateMenuItem showDateOrTime = new Delegates.IntermediateMenuItem("Show Date/Time");
            Delegates.IntermediateMenuItem showVersionOrSpaces = new Delegates.IntermediateMenuItem("Version and Spaces");
            Delegates.FinalMenuItem timeMenuItem = new Delegates.FinalMenuItem("Time", showDateOrTime ,ShowTime);
            Delegates.FinalMenuItem dateMenuItem = new Delegates.FinalMenuItem("Date", showDateOrTime, ShowDate);
            Delegates.FinalMenuItem SpaceMenuItem = new Delegates.FinalMenuItem("Space counter", showVersionOrSpaces, SpaceCounter);
            Delegates.FinalMenuItem VersionMenuItem = new Delegates.FinalMenuItem("Show version", showVersionOrSpaces, ShowVersion);

            // Main menu
            (i_DelegatesMenu as IntermediateMenuItem).AddMenuOption(exit);
            (i_DelegatesMenu as IntermediateMenuItem).AddMenuOption(showDateOrTime);
            (i_DelegatesMenu as IntermediateMenuItem).AddMenuOption(showVersionOrSpaces);
            //// Sub menu - date or time
            (showDateOrTime as IntermediateMenuItem).AddMenuOption(backOfVersions);
            (showDateOrTime as IntermediateMenuItem).AddMenuOption(timeMenuItem);
            (showDateOrTime as IntermediateMenuItem).AddMenuOption(dateMenuItem);
            //// Sub menu - versions or spaces
            (showVersionOrSpaces as IntermediateMenuItem).AddMenuOption(backOfVersions);
            (showVersionOrSpaces as IntermediateMenuItem).AddMenuOption(SpaceMenuItem);
            (showVersionOrSpaces as IntermediateMenuItem).AddMenuOption(VersionMenuItem);


            showDateOrTime.OnClick += showDateOrTime.ManageMenu;
            showVersionOrSpaces.OnClick += showVersionOrSpaces.ManageMenu;

        }

        public static void ShowVersion()
        {
            string version = "22.2.4.8950";
            Console.WriteLine($"Version: {version}");
        }

        public static void ShowTime()
        {
            DateTime currentTime = DateTime.Now;
            Console.WriteLine($"Current Date is: {currentTime.Hour}:{currentTime.Minute}:{currentTime.Second}");
        }

        public static void ShowDate()
        {
            DateTime currentTime = DateTime.Now;
            Console.WriteLine($"Current Date is: {currentTime.Month}.{currentTime.Day}.{currentTime.Year}");
        }



        public static void SpaceCounter()
        {
            StringBuilder outputString = new StringBuilder();
            string m_Sentence = null;
            int m_NumOfSpaces = 0;

            Console.WriteLine("Please enter your sentence:\n");
            m_Sentence = Console.ReadLine();
            foreach (char letter in m_Sentence)
            {
                if (letter == ' ')
                    m_NumOfSpaces++;
            }

            switch (m_NumOfSpaces)
            {
                case 0:
                    outputString.Append("There are no Spaces in your sentence");
                    break;
                case 1:
                    outputString.Append("There is 1 Space in your sentence");
                    break;
                default:
                    outputString.Append($"There are {m_NumOfSpaces} Spaces in your sentence");
                    break;
            }

            Console.WriteLine(outputString);
        }

    }
}
