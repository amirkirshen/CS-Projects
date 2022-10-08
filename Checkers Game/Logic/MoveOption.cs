using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers.Logic
{
    public struct MoveOption
    {
        private Position m_From;
        private Position m_To;

        public MoveOption(Position i_From, Position i_To)
        {
            m_From = i_From;
            m_To = i_To;
        }

        public Position FromPosition
        {
            get { return m_From; }
            set { m_From = value; }
        }

        public Position ToPosition
        {
            get { return m_To; }
            set { m_To = value; }
        }
    }
}
