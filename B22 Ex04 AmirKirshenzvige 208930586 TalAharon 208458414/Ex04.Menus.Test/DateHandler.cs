using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ex04.Menus.Interfaces;

namespace Ex04.Menus.Test
{
    public class DateHandler : IClickListener
    {
        DateTime m_CurrentTime;

        public void OnClick()
        {
            m_CurrentTime = DateTime.Now;
            Console.WriteLine($"Current Date is: {m_CurrentTime.Month}.{m_CurrentTime.Day}.{m_CurrentTime.Year}");
        }
    }
}
