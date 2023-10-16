namespace Ex05.GameUI
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class BoardSizeChangeButton : Button
    {
        private const int k_ButtonSizeX = 100;
        private const int k_ButtonSizeY = 70;

        private readonly Color r_DefaultButtonColor = Color.LightSkyBlue;

        private readonly SelectedBoardSize[] r_PossibleSizes =
            {
                                                                       new SelectedBoardSize(4, 4), new SelectedBoardSize(4, 5),
                                                                       new SelectedBoardSize(4, 6), new SelectedBoardSize(5, 4),
                                                                       new SelectedBoardSize(5, 6), new SelectedBoardSize(6, 4),
                                                                       new SelectedBoardSize(6, 5), new SelectedBoardSize(6, 6),
            };

        private int m_CurrentPossibleSizesIndex;

        public BoardSizeChangeButton()
        {
            this.Size = new Size(k_ButtonSizeX, k_ButtonSizeY);
            this.BackColor = this.r_DefaultButtonColor;
            this.m_CurrentPossibleSizesIndex = 0;
            this.Click += this.handleClick;
            this.reRender();
        }

        public int GetColumnCount()
        {
            SelectedBoardSize selectedSize = this.getSelectedSize();
            return selectedSize.GetColumns();
        }

        public int GetRowCount()
        {
            SelectedBoardSize selectedSize = this.getSelectedSize();
            return selectedSize.GetRows();
        }

        private void handleClick(object i_Sender, EventArgs i_EventArguments)
        {
            this.advanceSizeOption();
            this.reRender();
        }

        private void advanceSizeOption()
        {
            this.m_CurrentPossibleSizesIndex++;
            if (this.m_CurrentPossibleSizesIndex >= this.r_PossibleSizes.Length)
            {
                this.m_CurrentPossibleSizesIndex = 0;
            }
        }

        private SelectedBoardSize getSelectedSize()
        {
            return this.r_PossibleSizes[this.m_CurrentPossibleSizesIndex];
        }

        private void reRender()
        {
            this.Text = $"{this.GetColumnCount()}x{this.GetRowCount()}";
        }

        internal readonly struct SelectedBoardSize
        {
            private readonly int r_Column;
            private readonly int r_Row;

            public SelectedBoardSize(int i_Column, int i_Row)
            {
                this.r_Column = i_Column;
                this.r_Row = i_Row;
            }

            public int GetColumns()
            {
                return this.r_Column;
            }

            public int GetRows()
            {
                return this.r_Row;
            }
        }
    }
}
