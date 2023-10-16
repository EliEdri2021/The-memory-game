namespace Ex05.GameUI
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class InfoLabel : Label
    {
        private readonly string r_LabelDefaultWhileLoading = "Loading";
        private readonly Color r_DefaultBackColor;

        public InfoLabel()
        {
            this.Text = this.r_LabelDefaultWhileLoading;
            this.r_DefaultBackColor = this.BackColor;
        }

        public Action<string, Color?> GetUpdateVisualsAction()
        {
            return this.updateVisuals;
        }

        private void updateVisuals(string i_UpdatedValue, Color? i_UpdatedColor)
        {
            this.BackColor = i_UpdatedColor ?? this.r_DefaultBackColor;
            this.Text = i_UpdatedValue;
        }
    }
}