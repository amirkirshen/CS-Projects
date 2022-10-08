using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex04.Menus.Interfaces
{
    public class FinalMenuItem : MenuItem
    {
        public FinalMenuItem(string i_Text, IClickListener i_ClickListener, MenuItem i_ParentManuItem = null)
        : base(i_Text, i_ParentManuItem)
        {
            base.ClickListener = i_ClickListener;
        }
    }
}
