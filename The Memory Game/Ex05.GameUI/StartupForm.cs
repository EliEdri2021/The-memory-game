namespace Ex05.GameUI
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class StartupForm : Form
    {
        private const string k_FormTitle = "Memory Game - Settings";

        private const int k_SpacingAroundBorder = 15;

        private const int k_TextBoxXSpace = 125;
        private const int k_TextBoxYSpace = 30;

        private const string k_StartButtonText = "Start!";
        private const string k_FirstPlayerNameLabelText = "First Player Name:";
        private const string k_SecondPlayerNameLabelText = "Second Player Name:";
        private const string k_BoardSizeLabelText = "Board Size:";

        private readonly Color r_StartButtonColor = Color.LimeGreen;

        private BoardSizeChangeButton m_BoardSizeChangeButton;
        private LabelToggleButton m_SecondPlayerAiButton;
        private TextBox m_FirstPlayersNameTextBox;
        private TextBox m_SecondPlayersNameTextBox;

        private bool m_IsSucssesfulClose;

        public StartupForm()
        {
            this.Text = k_FormTitle;
            this.m_IsSucssesfulClose = false;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.Size = new Size((k_TextBoxYSpace * 14) - k_SpacingAroundBorder, k_TextBoxYSpace * 8);
            this.setUpVisualElements();
        }

        public bool GetIsGameStart()
        {
            return this.m_IsSucssesfulClose;
        }

        public string GetPlayerOneName()
        {
            return this.m_FirstPlayersNameTextBox.Text;
        }

        public string GetPlayerTwoName()
        {
            return this.m_SecondPlayersNameTextBox.Text;
        }

        public bool GetIsSecondPlayerAi()
        {
            return this.m_SecondPlayerAiButton.GetIsAiMode();
        }

        public int GetColumnCount()
        {
            return this.m_BoardSizeChangeButton.GetColumnCount();
        }

        public int GetRowCount()
        {
            return this.m_BoardSizeChangeButton.GetRowCount();
        }

        private void setUpVisualElements()
        {
            Label firstPlayersNameLabel = new Label();
            firstPlayersNameLabel.Location = new Point(k_SpacingAroundBorder, k_SpacingAroundBorder + 3);
            firstPlayersNameLabel.Text = k_FirstPlayerNameLabelText;

            this.m_FirstPlayersNameTextBox = new TextBox();
            this.m_FirstPlayersNameTextBox.Location = new Point(k_SpacingAroundBorder + k_TextBoxXSpace, k_SpacingAroundBorder);
            this.m_FirstPlayersNameTextBox.Width = k_TextBoxXSpace;

            Label secondPlayersNameLabel = new Label();
            secondPlayersNameLabel.Location = new Point(k_SpacingAroundBorder, k_SpacingAroundBorder + k_TextBoxYSpace + 3);
            secondPlayersNameLabel.Width = k_TextBoxXSpace;
            secondPlayersNameLabel.Text = k_SecondPlayerNameLabelText;

            this.m_SecondPlayersNameTextBox = new TextBox();
            this.m_SecondPlayersNameTextBox.Location = new Point(k_SpacingAroundBorder + k_TextBoxXSpace, k_SpacingAroundBorder + k_TextBoxYSpace);
            this.m_SecondPlayersNameTextBox.Width = k_TextBoxXSpace;

            this.m_SecondPlayerAiButton = new LabelToggleButton(this.m_SecondPlayersNameTextBox);
            this.m_SecondPlayerAiButton.Location = new Point((k_SpacingAroundBorder + k_TextBoxXSpace) * 2, k_SpacingAroundBorder + k_TextBoxYSpace);
            this.m_SecondPlayersNameTextBox.Width = k_TextBoxXSpace;

            Label boardSizeChangeLabel = new Label();
            boardSizeChangeLabel.Location = new Point(k_SpacingAroundBorder, (k_SpacingAroundBorder + k_TextBoxYSpace) * 2);
            boardSizeChangeLabel.Text = k_BoardSizeLabelText;

            this.m_BoardSizeChangeButton = new BoardSizeChangeButton();
            this.m_BoardSizeChangeButton.Location = new Point(k_SpacingAroundBorder, k_TextBoxYSpace * 4);

            Button startButton = new Button();
            startButton.Location = new Point(k_TextBoxYSpace * 10, (k_TextBoxYSpace * 6) - 10);
            startButton.BackColor = this.r_StartButtonColor;
            startButton.Text = k_StartButtonText;
            startButton.Click += this.startButtonClick;

            this.Controls.Add(firstPlayersNameLabel);
            this.Controls.Add(this.m_FirstPlayersNameTextBox);
            this.Controls.Add(secondPlayersNameLabel);
            this.Controls.Add(this.m_SecondPlayersNameTextBox);
            this.Controls.Add(this.m_SecondPlayerAiButton);
            this.Controls.Add(boardSizeChangeLabel);
            this.Controls.Add(this.m_BoardSizeChangeButton);
            this.Controls.Add(startButton);
        }

        private void startButtonClick(object i_Sender, EventArgs i_EventArguments)
        {
            this.m_IsSucssesfulClose = true;
            this.Close();
        }
    }
}
