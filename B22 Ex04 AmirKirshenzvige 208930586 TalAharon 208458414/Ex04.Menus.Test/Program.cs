using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ex04.Menus.Delegates;

namespace Ex04.Menus.Test
{
    public class Program
    {
        public static void Main()
        {
            MenusInterfacesManager menusInterfacesManager = new MenusInterfacesManager();
            menusInterfacesManager.Run();
            MenusDelegatesManager menuDelegatesManager = new MenusDelegatesManager();
            menuDelegatesManager.Run();
        }
    }
}
