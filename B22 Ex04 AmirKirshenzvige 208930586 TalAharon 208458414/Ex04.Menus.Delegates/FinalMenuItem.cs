using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex04.Menus.Delegates
{
    public class FinalMenuItem : MenuItem
    {

        public event Action OnClick;

        public FinalMenuItem(string i_Title, MenuItem i_Parent = null, Action i_ToInvoke = null)
            : base(i_Title, i_Parent)
        {
            if(i_ToInvoke != null)
            {
                OnClick += i_ToInvoke;
            }
        }

        /// <summary>
        /// Annouce to all invoked functions that curr menu item has been clicked
        /// </summary>
        public override void OnClicked()
        {
            if (OnClick != null)
            {
                OnClick.Invoke();
            }
        }
    }
}
