namespace Checkers.Logic
{
    public enum eToolType { Soldier, King }

    public class GameTool
    {
        private Position m_Position;
        private eOwnerPlayer m_Owner;
        private eToolType m_Type;
        private short m_IndexInPlayerTools;

        public GameTool(Position i_Position, eOwnerPlayer i_Owner, eToolType i_ToolType = eToolType.Soldier, short i_Index = 0)
        {
            m_Position = i_Position;
            m_Type = i_ToolType;
            m_Owner = i_Owner;
            m_IndexInPlayerTools = i_Index;
        }

        public Position Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public eOwnerPlayer Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }

        public eToolType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        public short Index
        {
            get { return m_IndexInPlayerTools; }
            set { m_IndexInPlayerTools = value; }
        }
    }
}
