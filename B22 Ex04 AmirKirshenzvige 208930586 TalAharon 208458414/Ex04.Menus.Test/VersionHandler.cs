using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ex04.Menus.Interfaces;

namespace Ex04.Menus.Test
{
    public class VersionHandler : IClickListener
    {
        public void OnClick()
        {
            string version = "22.2.4.8950";
            Console.WriteLine($"Version: {version}");
        }
    }
}
