using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ex04.Menus.Interfaces;

namespace Ex04.Menus.Test
{
    public class SpacesCountersHandler : IClickListener
    {
        private string m_Sentence = null;
        private int m_NumOfSpaces = 0;

        public void OnClick()
        {
            StringBuilder outputString = new StringBuilder();

            Console.WriteLine("Please enter your sentence:\n");
            m_Sentence = Console.ReadLine();
            foreach (char letter in m_Sentence)
            {
                if (letter == ' ')
                    m_NumOfSpaces++;
            }

            switch (m_NumOfSpaces)
            {
                case 0:
                    outputString.Append("There are no Spaces in your sentence");
                    break;
                case 1:
                    outputString.Append("There is 1 Space in your sentence");
                    break;
                default:
                    outputString.Append($"There are {m_NumOfSpaces} Spaces in your sentence");
                    break;
            }

            Console.WriteLine(outputString);
        }
    }
}


