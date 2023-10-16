namespace Ex05.GameUI
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class GuessingButton : Button
    {
        private readonly int r_RepresentsColumn;
        private readonly int r_RepresentsRow;

        private readonly Color r_DefaultBackColor;

        public GuessingButton(int i_Column, int i_Row)
        {
            this.r_RepresentsColumn = i_Column;
            this.r_RepresentsRow = i_Row;
            this.r_DefaultBackColor = this.BackColor;
        }

        public int GetRepresentedColumn()
        {
            return this.r_RepresentsColumn;
        }

        public int GetRepresentedRow()
        {
            return this.r_RepresentsRow;
        }

        public Action<string, bool, Color?> GetUpdateVisualsAction()
        {
            return this.updateVisuals;
        }

        private void updateVisuals(string i_UpdatedTextToShow, bool i_UpdatedValue, Color? i_UpdatedColor)
        {
            this.Text = i_UpdatedTextToShow;
            this.Enabled = i_UpdatedValue;
            this.BackColor = i_UpdatedColor ?? this.r_DefaultBackColor;
        }
    }
}
