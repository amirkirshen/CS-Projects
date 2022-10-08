using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex04.Menus.Interfaces
{
    public class IntermediateMenuItem : MenuItem
    {
        private Dictionary<int, MenuItem> m_SubMenu = new Dictionary<int, MenuItem>();

        public IntermediateMenuItem(string i_Text, MenuItem i_ParentMenuItem = null)
            : base(i_Text, i_ParentMenuItem)
        {
            base.ClickListener = new SubMenuPresentor(m_SubMenu);
        }

        public Dictionary<int, MenuItem> SubMenu
        {
            get { return m_SubMenu; }
            set { m_SubMenu = value; }
        }

        /// <summary>
        /// Addin a menu item to current menu items dictionary
        /// </summary>
        /// <param name="i_MenuItem"></param>
        public void AddMenuOption(MenuItem i_MenuItem)
        {
            // Assume that the first entered new operation is to exit menu (key = 0).
            m_SubMenu.Add(m_SubMenu.Count, i_MenuItem);
        }
    }
}
