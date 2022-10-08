using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ex04.Menus.Interfaces;

namespace Ex04.Menus.Test
{
    public class MenusInterfacesManager
    {

        public void Run()
        {
            Interfaces.MenuItem mainMenuItem = new IntermediateMenuItem("Main");
            ExitHandler exitObject = new ExitHandler();

            Interfaces.MenuItem exit = new Interfaces.FinalMenuItem("Exit", exitObject);
            Interfaces.MenuItem showDateOrTime = new Interfaces.IntermediateMenuItem("Show Date/Time", null);
            Interfaces.MenuItem showVersionOrSpaces = new Interfaces.IntermediateMenuItem("Version and Spaces");
            Interfaces.MainMenu interfaceMenu = new Ex04.Menus.Interfaces.MainMenu(mainMenuItem);

            createInterfaceMenu(mainMenuItem);
            interfaceMenu.Show();

        }

        private static void createInterfaceMenu(Interfaces.MenuItem i_InterfaceMenu)
        {
            TimeHandler timeObject = new TimeHandler();
            DateHandler dateObject = new DateHandler();
            VersionHandler versionObject = new VersionHandler();
            SpacesCountersHandler spaceObjectObject = new SpacesCountersHandler();
            ExitHandler exitObject = new ExitHandler();
            Interfaces.MenuItem exit = new Interfaces.FinalMenuItem("Exit", exitObject);
            Interfaces.MenuItem backOfVersions = new Interfaces.FinalMenuItem("Back", exitObject);
            Interfaces.MenuItem showDateOrTime = new Interfaces.IntermediateMenuItem("Show Date/Time", null);
            Interfaces.MenuItem showVersionOrSpaces = new Interfaces.IntermediateMenuItem("Version and Spaces");
            Interfaces.MenuItem timeMenuItem = new Interfaces.FinalMenuItem("Time", timeObject, showDateOrTime);
            Interfaces.MenuItem dateMenuItem = new Interfaces.FinalMenuItem("Date", dateObject, showDateOrTime);
            Interfaces.MenuItem SpaceMenuItem = new Interfaces.FinalMenuItem("Space counter", spaceObjectObject, showVersionOrSpaces);
            Interfaces.MenuItem VersionMenuItem = new Interfaces.FinalMenuItem("Show version", versionObject, showVersionOrSpaces);

            // Main menu
            (i_InterfaceMenu as IntermediateMenuItem).AddMenuOption(exit);
            (i_InterfaceMenu as IntermediateMenuItem).AddMenuOption(showDateOrTime);
            (i_InterfaceMenu as IntermediateMenuItem).AddMenuOption(showVersionOrSpaces);

            //// Sub menu
            (showDateOrTime as IntermediateMenuItem).AddMenuOption(backOfVersions);
            (showDateOrTime as IntermediateMenuItem).AddMenuOption(timeMenuItem);
            (showDateOrTime as IntermediateMenuItem).AddMenuOption(dateMenuItem);

            //// Sub menu
            (showVersionOrSpaces as IntermediateMenuItem).AddMenuOption(backOfVersions);
            (showVersionOrSpaces as IntermediateMenuItem).AddMenuOption(SpaceMenuItem);
            (showVersionOrSpaces as IntermediateMenuItem).AddMenuOption(VersionMenuItem);
        }
    }
}
