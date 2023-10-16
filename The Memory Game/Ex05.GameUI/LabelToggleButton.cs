namespace Ex05.GameUI
{
    using System;
    using System.Windows.Forms;

    internal class LabelToggleButton : Button
    {
        private const string k_TextForDisabledTextBox = "- computer -";
        private const string k_TextForDisableLabel = "Against Computer";
        private const string k_TextForEnableLabel = "Against a Friend";
        private const string k_TextForEnabledTextBox = "";
        private const int k_ButtonSizeX = 100;
        private readonly TextBox r_LabelToToggle;

        public LabelToggleButton(TextBox i_LabelToToggle)
        {
            this.r_LabelToToggle = i_LabelToToggle;
            this.Click += this.handleClick;
            this.Width = k_ButtonSizeX;
            this.Text = k_TextForDisableLabel;
        }

        public bool GetIsAiMode()
        {
            return !this.isTextBoxEnabled();
        }

        private void handleClick(object i_Sender, EventArgs i_EventArguments)
        {
            this.stateChange();
        }

        private void stateChange()
        {
            this.toggleTextBox();
            this.reRender();
        }

        private void reRender()
        {
            this.setTextBoxText(this.isTextBoxEnabled() ? k_TextForEnabledTextBox : k_TextForDisabledTextBox);
            this.Text = this.isTextBoxEnabled() ? k_TextForDisableLabel : k_TextForEnableLabel;
        }

        private bool isTextBoxEnabled()
        {
            return this.r_LabelToToggle.Enabled;
        }

        private void toggleTextBox()
        {
            this.r_LabelToToggle.Enabled = !this.r_LabelToToggle.Enabled;
        }

        private void setTextBoxText(string i_UpdatedText)
        {
            this.r_LabelToToggle.Text = i_UpdatedText;
        }
    }
}
