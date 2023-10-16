namespace Ex05.GameLogic
{
    using System;
    using System.Drawing;

    internal class Cell
    {
        private const string k_ValueIfHidden = "";
        private readonly string r_Value;
        private Action<string, bool, Color?> m_StateChangeAction;
        private Color? m_AssignedColor;
        private bool m_IsRevealed;

        public Cell(string i_Value)
        {
            this.r_Value = i_Value;
            this.m_IsRevealed = false;
            this.m_AssignedColor = null;
        }

        public static bool operator ==(Cell i_CellA, Cell i_CellB)
        {
            bool equalsResult = false;

            if (!(i_CellA is null) && !(i_CellB is null))
            {
                equalsResult = i_CellA.GetSymbol().Equals(i_CellB.GetSymbol()) && i_CellA.IsHidden() == i_CellB.IsHidden();
            }

            return equalsResult;
        }

        public static bool operator !=(Cell i_CellA, Cell i_CellB)
        {
            return !(i_CellA == i_CellB);
        }

        public bool IsHidden()
        {
            return !this.m_IsRevealed;
        }

        public override string ToString()
        {
            return this.IsHidden() ? k_ValueIfHidden : this.r_Value;
        }

        public override bool Equals(object i_ObjectOther)
        {
            return i_ObjectOther is Cell cellOther && this == cellOther;
        }

        public void Reveal(Color i_NewAssignedColor)
        {
            this.m_AssignedColor = i_NewAssignedColor;
            this.m_IsRevealed = true;
        }

        public void SubscribeToVisualUpdateAction(Action<string, bool, Color?> i_ActionToSubscribeTo)
        {
            this.m_StateChangeAction += i_ActionToSubscribeTo;
        }

        public void VisualUpdate()
        {
            this.reRender(false, null);
        }

        public void TemporarilyReveal(Color i_ColorToShowIn)
        {
            this.reRender(true, i_ColorToShowIn);
        }

        public string GetSymbol()
        {
            return this.r_Value;
        }

        private void reRender(bool i_OverrideIsSymbolHidden, Color? i_ColorToOverride)
        {
            string stringToShow = i_OverrideIsSymbolHidden ? this.GetSymbol() : this.ToString();
            Color? colorToShow = i_ColorToOverride ?? this.getColorOrNull();

            this.m_StateChangeAction.Invoke(stringToShow, this.IsHidden(), colorToShow);
        }

        private Color? getColorOrNull()
        {
            return this.m_AssignedColor;
        }
    }
}
