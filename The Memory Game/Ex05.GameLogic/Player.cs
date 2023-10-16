namespace Ex05.GameLogic
{
    using System.Drawing;
    using System.Text;

    internal class Player
    {
        private readonly string r_PlayerName;
        private readonly Color r_DefaultColorOne = Color.DeepSkyBlue;
        private readonly Color r_SelectedColor;

        private int m_RevealedPairs;

        public Player(string i_PlayerName, Color? i_Color)
        {
            this.m_RevealedPairs = 0;
            this.r_PlayerName = i_PlayerName;
            this.r_SelectedColor = i_Color ?? this.r_DefaultColorOne;
        }

        public virtual Color GetColor()
        {
            return this.r_SelectedColor;
        }

        public int GetPairs()
        {
            return this.m_RevealedPairs;
        }

        public void AddPairReveal()
        {
            this.m_RevealedPairs++;
        }

        public string GetPlayerName()
        {
            return this.r_PlayerName;
        }

        public override string ToString()
        {
            StringBuilder amountName = new StringBuilder();

            switch (this.m_RevealedPairs)
            {
                case 0:
                    amountName.Append("No Pairs Yet");
                    break;
                case 1:
                    amountName.Append($"{this.GetPairs()} Pair(s)");
                    break;
                default:
                    amountName.Append($"{this.GetPairs()} Pairs");
                    break;
            }

            return $"{this.GetPlayerName()}: {amountName}";
        }
    }
}
