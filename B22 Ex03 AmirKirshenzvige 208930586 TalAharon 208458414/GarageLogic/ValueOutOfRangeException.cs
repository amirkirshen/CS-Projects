using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        float m_MinValue;
        float m_MaxValue;

        public ValueOutOfRangeException(string i_Message) : base(i_Message) { }
        public ValueOutOfRangeException(float i_OneSizeOptional, string i_Message) : base(new StringBuilder("Not valid input! ").Append(i_Message).ToString())
        {
            m_MaxValue = m_MinValue = i_OneSizeOptional;
        }

        public ValueOutOfRangeException(float i_MinValue, float i_MaxValue, string i_Massage) : base(new StringBuilder("Not valid input! ").Append(i_Massage).ToString())
        {
            m_MinValue = i_MinValue;
            m_MaxValue = i_MaxValue;
        }

        public float MinValue
        {
            get { return m_MinValue; }
            set { m_MinValue = value; }
        }
        public float MaxValue
        {
            get { return m_MaxValue; }
            set { m_MaxValue = value; }
        }

        public bool IsInRange(float i_Num)
        {
            return (i_Num >= m_MinValue && i_Num <= m_MaxValue);
        }
    }
}
