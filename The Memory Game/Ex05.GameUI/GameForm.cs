namespace Ex05.GameUI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using Ex05.GameLogic;

    internal sealed class GameForm : Form
    {
        private const string k_WindowTitle = "The Memory Game - by Eli";

        private const int k_GuessingButtonSizeX = 65;
        private const int k_GuessingButtonSizeY = 65;

        private const int k_GuessingButtonPaddingX = 12;
        private const int k_GuessingButtonPaddingY = 12;

        private const int k_AdjustmentForWindowBarY = 30;
        private const int k_SpaceForLowerInformationDisplay = 120;

        private readonly GameLogic r_Gamelogic;

        private InfoLabel m_CurrentPlayerLabel;

        public GameForm(ref GameLogic i_GameLogic)
        {
            this.r_Gamelogic = i_GameLogic;

            this.setUpDynamicallyChaningUiElements();
            this.setUpConstantUiElements();
            this.setUpAditionalFunctionality();
        }

        protected override void OnShown(EventArgs i_EventArguments)
        {
            base.OnShown(i_EventArguments);
        }

        private void setUpDynamicallyChaningUiElements()
        {
            this.placeButtons();
            this.setUpCurrentPlayerLabel();
            this.setUpScoreboard();
        }

        private void setUpConstantUiElements()
        {
            this.setWindowSize(this.getColumnCount(), this.getRowCount());
            this.Text = k_WindowTitle;
        }

        private void setUpAditionalFunctionality()
        {
            this.r_Gamelogic.SubscribeToGameOver(this.gameOver);
        }

        private void placeButtons()
        {
            for (int currentRow = 0; currentRow < this.getRowCount(); currentRow++)
            {
                for (int currentColumn = 0; currentColumn < this.getColumnCount(); currentColumn++)
                {
                    GuessingButton currentButton = new GuessingButton(currentColumn, currentRow);

                    currentButton.Location = new Point((currentColumn * k_GuessingButtonSizeX) + ((currentColumn + 1) * k_GuessingButtonPaddingX), (currentRow * k_GuessingButtonSizeY) + ((currentRow + 1) * k_GuessingButtonPaddingY));
                    currentButton.Size = new Size(k_GuessingButtonSizeX, k_GuessingButtonSizeY);

                    this.r_Gamelogic.SubscribeToCellVisualUpdate(currentRow, currentColumn, currentButton.GetUpdateVisualsAction());

                    currentButton.Click += this.respondToGuessingButtonClick;

                    this.Controls.Add(currentButton);
                }
            }
        }

        private void respondToGuessingButtonClick(object i_Sender, EventArgs i_EventArguments)
        {
            if (i_Sender is GuessingButton clickedButton)
            {
                this.r_Gamelogic.RespondToSelection(clickedButton.GetRepresentedColumn(), clickedButton.GetRepresentedRow());
            }
        }

        private void setUpCurrentPlayerLabel()
        {
            this.m_CurrentPlayerLabel = new InfoLabel();
            this.m_CurrentPlayerLabel.Location = new System.Drawing.Point(
                x: k_GuessingButtonPaddingX,
                (this.getRowCount() * (k_GuessingButtonSizeY + k_GuessingButtonPaddingY)) + k_AdjustmentForWindowBarY);
            this.r_Gamelogic.SubscribeToCurrentPlayer(this.m_CurrentPlayerLabel.GetUpdateVisualsAction());
            this.Controls.Add(this.m_CurrentPlayerLabel);
        }

        private void setUpScoreboard()
        {
            int playerCount = this.r_Gamelogic.GetPlayerCount();

            List<Action<string, Color?>> actionsToSub = new List<Action<string, Color?>>();

            for (int i = 0; i < playerCount; i++)
            {
                this.m_CurrentPlayerLabel = new InfoLabel();
                this.m_CurrentPlayerLabel.Location = new System.Drawing.Point(
                    x: k_GuessingButtonPaddingX,
                    (this.getRowCount() * (k_GuessingButtonSizeY + k_GuessingButtonPaddingY)) + ((k_GuessingButtonPaddingY * ((2 * i) + 3)) + k_AdjustmentForWindowBarY));
                actionsToSub.Add(this.m_CurrentPlayerLabel.GetUpdateVisualsAction());
                this.Controls.Add(this.m_CurrentPlayerLabel);
            }

            this.r_Gamelogic.SubscribeToPlayerScoreboard(actionsToSub);
        }

        private void setWindowSize(int i_ColumnCount, int i_RowCount)
        {
            this.Size = new System.Drawing.Size(
                                                (i_ColumnCount * k_GuessingButtonSizeX) + ((i_ColumnCount + 2) * k_GuessingButtonPaddingX),
                                                (i_RowCount * k_GuessingButtonSizeY) + ((i_RowCount + 2) * k_GuessingButtonPaddingY) + k_AdjustmentForWindowBarY + k_SpaceForLowerInformationDisplay);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private int getRowCount()
        {
            return this.r_Gamelogic.GetRowCount();
        }

        private int getColumnCount()
        {
            return this.r_Gamelogic.GetColumnCount();
        }

        private void gameOver()
        {
            this.Close();
        }
    }
}
