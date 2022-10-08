using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex04.Menus.Interfaces
{
    public abstract class MenuItem
    {
        private readonly string r_Text;
        private MenuItem m_ParentMenuItem;
        private IClickListener m_ClickListener;

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

        public IClickListener ClickListener
        {
            get { return m_ClickListener; }
            set { m_ClickListener = value; }
        }
    }
}
