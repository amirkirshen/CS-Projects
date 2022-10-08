using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex04.Menus.Delegates
{
    public abstract class MenuItem
    {
        private readonly string r_Text;
        private MenuItem m_ParentMenuItem;

        public MenuItem(string i_Text, MenuItem i_ParentManuItem)
        {
            r_Text = i_Text;
            m_ParentMenuItem = i_ParentManuItem;
        }

        public string Text
        {
            get { return r_Text; }
        }

        public MenuItem Parent
        {
            get { return m_ParentMenuItem; }
            set { m_ParentMenuItem = value; }
        }

        public abstract void OnClicked();
    }
}
