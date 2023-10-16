namespace Ex05.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    internal class Ai : Player
    {
        private const string k_DefaultAiPlayerName = "Super Intelegent Ai";
        private static readonly Color sr_DefaultAiPlayerColor = Color.Goldenrod;
        private List<Index> m_DiscoveredIndexes;
        private readonly int r_Column;
        private readonly int r_Row;
        private Action<int, int> m_AiPlay;

        public Ai(Color? i_Color, int i_Column, int i_Row)
            : base(k_DefaultAiPlayerName, i_Color ?? sr_DefaultAiPlayerColor)
        {
            this.r_Row = i_Row;
            this.r_Column = i_Column;
            this.m_DiscoveredIndexes = new List<Index>();
        }

        public void ObserveWin(Index i_ObservedIndex)
        {
            this.m_DiscoveredIndexes.Add(i_ObservedIndex);
        }

        public void SubscribeToMakeSelection(Action<int, int> i_ActionToSubscribeTo)
        {
            this.m_AiPlay += i_ActionToSubscribeTo;
        }

        public void Play()
        {
            Index indexToSend = this.getRandomIndex();
            int column = indexToSend.GetColumn();
            int row = indexToSend.GetRow();
            this.m_AiPlay.Invoke(column, row);
        }

        private Index getRandomIndex()
        {
            Random randomGenerator = new Random();

            while (true)
            {
                int proposedColumn = randomGenerator.Next(0, this.r_Column);
                int proposedRow = randomGenerator.Next(0, this.r_Row);

                Index proporsedIndex = new Index(proposedColumn, proposedRow);

                if (!this.m_DiscoveredIndexes.Contains(proporsedIndex))
                {
                    return proporsedIndex;
                }
            }
        }
    }
}
