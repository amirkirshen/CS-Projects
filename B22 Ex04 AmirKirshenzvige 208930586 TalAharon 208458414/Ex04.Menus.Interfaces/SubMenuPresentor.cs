using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex04.Menus.Interfaces
{
    /// <summary>
    /// Responsible for showing the intermediate sub menu
    /// </summary>
    public class SubMenuPresentor : IClickListener
    {
        Dictionary<int, MenuItem> m_SubMenu;
        public SubMenuPresentor(Dictionary<int, MenuItem> i_SubMenu)
        {
            m_SubMenu = i_SubMenu;
        }

        void IClickListener.OnClick()
        {
            bool backEntered = false;
            int menuOptionKey;

            while (!backEntered)
            {
                MainMenu.ShowMenuOptions(m_SubMenu);
                menuOptionKey = MainMenu.GetValidMenuOptionKeyFromUser(m_SubMenu.Count - 1);
                Console.Clear();
                if (menuOptionKey == 0)
                {
                    backEntered = true;
                }
                else
                {
                    m_SubMenu[menuOptionKey].ClickListener.OnClick();
                }
            }

        }
    }
}
