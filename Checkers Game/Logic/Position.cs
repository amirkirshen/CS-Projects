using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers.Logic
{
    public class Position
    {
        private short m_Row;
        private short m_Col;

        public Position(short i_Row, short i_Col)
        {
            m_Row = i_Row;
            m_Col = i_Col;
        }

        public short Row
        {
            get { return m_Row; }
            set { m_Row = value; }
        }

        public short Col
        {
            get { return m_Col; }
            set { m_Col = value; }
        }

        public bool IsEqual(Position i_Pos)
        {
            return (m_Col == i_Pos.Col && m_Row == i_Pos.Row);
        }
    }
}
