using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex04.Menus.Delegates
{
    public class IntermediateMenuItem : MenuItem
    {
        //Holds sub menu options
        Dictionary<int, MenuItem> m_SubMenu;

        public event Action<IntermediateMenuItem> OnClick;

        public IntermediateMenuItem(string i_Text, Action<MenuItem> i_ToInvoke = null, MenuItem i_ParentMenuItem = null)
        : base(i_Text, i_ParentMenuItem)
        {
            m_SubMenu = new Dictionary<int, MenuItem>();
            if (i_ToInvoke != null)
            //Allows to add defualt function when declared (ctor)
            {
                OnClick += i_ToInvoke;
            }
        }

        public Dictionary<int, MenuItem> SubMenu
        {
            get { return m_SubMenu; }
            set { m_SubMenu = value; }
        }

        /// <summary>
        /// Adding new menu item option to the current sub menu
        /// </summary>
        /// <param name="i_MenuItem"></param>
        public void AddMenuOption(MenuItem i_MenuItem)
        {
            // Assume that the first entered new operation is to exit menu (key = 0).
            m_SubMenu.Add(m_SubMenu.Count, i_MenuItem);
        }

        /// <summary>
        /// Showing all menu items in current sub menu
        /// </summary>
        /// <param name="i_SubMenu"></param>
        public void ShowMenu(IntermediateMenuItem i_SubMenu)
        {
            foreach (KeyValuePair<int, MenuItem> menuOption in i_SubMenu.SubMenu)
            {
                if (menuOption.Key != 0)
                {
                    Console.WriteLine($"{menuOption.Key} -> {menuOption.Value.Text}");
                }
            }

            Console.WriteLine($"{0} -> {i_SubMenu.SubMenu[0].Text}");
        }

        /// <summary>
        /// Announce to all invoked functions that curr SubMenu has been clicked
        /// </summary>
        public override void OnClicked()
        {
            if (OnClick != null)
            {
                OnClick.Invoke(this);
            }
        }

        /// <summary>
        /// Shows i_SubMenuItem's menu and gets valid choics from user
        /// </summary>
        /// <param name="i_SubMenuItem"></param>
        public void ManageMenu(IntermediateMenuItem i_SubMenuItem)
        {
            bool backEntered = false;
            int menuOptionKey;

            while (!backEntered)
            {
                i_SubMenuItem.ShowMenu(i_SubMenuItem);
                menuOptionKey = MainMenu.GetValidMenuOptionKeyFromUser(i_SubMenuItem.SubMenu.Count - 1);
                Console.Clear();
                if (menuOptionKey == 0)
                {
                    backEntered = true;
                }
                else
                {
                    i_SubMenuItem.SubMenu[menuOptionKey].OnClicked();
                }
            }

        }

    }
}
