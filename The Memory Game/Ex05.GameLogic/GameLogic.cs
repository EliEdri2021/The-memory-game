using System.Linq;

namespace Ex05.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class GameLogic
    {
        private readonly Dictionary<Index, Cell> r_Gameboard;
        private readonly List<Cell> r_PreviouslyPlayedCells;
        private readonly List<Player> r_PlayerList;
        private readonly int r_TotalWinsNeeded;
        private readonly int r_ColumnCount;
        private readonly int r_RowCount;

        private Action m_GameOverAction;
        private Action<string, Color?> m_CurrentPlayerVisualUpdateAction;
        private List<Action<string, Color?>> m_PlayerScoreBoardUpdateActionList;

        private Index? m_CurrentRoundPreviousSelection;
        private int m_CurrentActivePlayerIndex;
        private bool m_IsThisTheFirstTurnInARound;
        private int m_CurrentTotalWins;

        private readonly int? r_IndexAi;

        public GameLogic(int i_ColumnCount, int i_RowCount, string i_FirstPlayerName, bool i_IsSecondPlayerAi, string i_SecondPlayerName, Color? i_SecondPlayerColor)
        {
            this.r_Gameboard = new Dictionary<Index, Cell>();
            this.r_TotalWinsNeeded = i_RowCount * i_ColumnCount / 2;
            this.r_PreviouslyPlayedCells = new List<Cell>();
            this.r_PlayerList = new List<Player>();
            this.m_CurrentRoundPreviousSelection = null;
            this.m_IsThisTheFirstTurnInARound = true;
            this.m_CurrentActivePlayerIndex = 0;
            this.m_CurrentTotalWins = 0;
            this.r_IndexAi = null;
            this.r_ColumnCount = i_ColumnCount;
            this.r_RowCount = i_RowCount;

            if (i_IsSecondPlayerAi)
            {
                this.r_IndexAi = 1; // it is always the second player by design.
            }

            this.addPlayers(i_FirstPlayerName, i_IsSecondPlayerAi, i_SecondPlayerName, i_SecondPlayerColor);

            this.fillGameBoard();
        }

        public void RespondToSelection(int i_ChosenColumn, int i_ChosenRow)
        {
            Index selectedIndex = new Index(i_ChosenColumn, i_ChosenRow);
            Cell selectedCell = this.getCellAtIndex(selectedIndex);

            selectedCell.TemporarilyReveal(this.getCurrentPlayerColor());

            if (this.m_IsThisTheFirstTurnInARound)
            {
                if (!(this.getCurrentPlayer() is Ai))
                {
                    this.resetAfterRound();
                }

                this.m_CurrentRoundPreviousSelection = selectedIndex;
            }
            else
            {
                Index previousMove = this.m_CurrentRoundPreviousSelection.GetValueOrDefault();

                if (this.checkForAWin(selectedIndex, previousMove))
                {
                    this.handleWin(selectedIndex, previousMove);
                }

                this.advancePlayer();
            }

            this.r_PreviouslyPlayedCells.Add(selectedCell);

            this.m_IsThisTheFirstTurnInARound = !this.m_IsThisTheFirstTurnInARound;

            if (this.IsGameFinished())
            {
                this.handleGameOver();
            }
        }

        public bool IsGameFinished()
        {
            return this.m_CurrentTotalWins == this.r_TotalWinsNeeded;
        }

        public string WhoWon()
        {
            int scoreBest = 0;
            int scoreSecondBest = 0;
            string bestPlayerName = string.Empty;
            string secondBestPlayerName = string.Empty;

            foreach (Player player in this.r_PlayerList)
            {
                int currentPlayerScore = player.GetPairs();

                if (currentPlayerScore >= scoreSecondBest)
                {
                    if (currentPlayerScore >= scoreBest)
                    {
                        scoreBest = currentPlayerScore;
                        bestPlayerName = player.GetPlayerName();
                    }
                    else
                    {
                        scoreSecondBest = currentPlayerScore;
                        secondBestPlayerName = player.GetPlayerName();
                    }
                }
            }

            return $"1st place: {bestPlayerName} at {scoreBest}{Environment.NewLine}2nd place {secondBestPlayerName} at {scoreSecondBest}";
        }

        public void SubscribeToCellVisualUpdate(int i_Row, int i_Column, Action<string, bool, Color?> i_ActionToSubscribeTo)
        {
            Index indexToSubscribeTo = new Index(i_Column, i_Row);
            this.r_Gameboard[indexToSubscribeTo].SubscribeToVisualUpdateAction(i_ActionToSubscribeTo);
        }

        public void SubscribeToCurrentPlayer(Action<string, Color?> i_ActionToSubscribeTo)
        {
            this.m_CurrentPlayerVisualUpdateAction += i_ActionToSubscribeTo;
            this.uiHandlePlayerChange();
        }

        public void SubscribeToPlayerScoreboard(List<Action<string, Color?>> i_ActionList)
        {
            this.m_PlayerScoreBoardUpdateActionList = i_ActionList;
            this.uiHandlePlayerScoreBoardUpdate();
        }

        public void SubscribeToGameOver(Action i_ActionToSubscribeTo)
        {
            this.m_GameOverAction += i_ActionToSubscribeTo;
        }

        public int GetColumnCount()
        {
            return this.r_ColumnCount;
        }

        public int GetRowCount()
        {
            return this.r_RowCount;
        }

        public int GetPlayerCount()
        {
            return this.r_PlayerList.Count;
        }

        private void addPlayers(string i_PlayerOneName, bool i_IsSecondPlayerAi, string i_SecondPlayerName, Color? i_SecondPlayerColor)
        {
           this.r_PlayerList.Add(new Player(i_PlayerOneName, null));
           this.r_PlayerList.Add(i_IsSecondPlayerAi ? new Ai(null, this.r_ColumnCount, this.r_RowCount) : new Player(i_SecondPlayerName, i_SecondPlayerColor));

           if (this.r_PlayerList.Last() is Ai aiPlayer)
           {
               aiPlayer.SubscribeToMakeSelection(this.RespondToSelection);
           }
        }

        private void fillGameBoard()
        {
            List<Index> possibleIndexes = new List<Index>();
            Random randomGenerator = new Random();

            int asciiRangeStart = 65;
            int asciiRangeStop = 90;

            int columnCount = this.GetColumnCount();
            int rowCount = this.GetRowCount();

            for (int currentColumn = 0; currentColumn < columnCount; currentColumn++)
            {
                for (int currentRow = 0; currentRow < rowCount; currentRow++)
                {
                    possibleIndexes.Add(new Index(currentColumn, currentRow));
                }
            }

            while (possibleIndexes.Count > 1)
            {
                int asciiSymbolNumberToAssign = randomGenerator.Next(asciiRangeStart, asciiRangeStop + 1);
                char symbolToAssign = Convert.ToChar(asciiSymbolNumberToAssign);

                for (int i = 0; i < 2; i++)
                {
                    int positionInArray = randomGenerator.Next(0, possibleIndexes.Count);

                    Index randomIndex = possibleIndexes[positionInArray];
                    Cell cellAtIndex = new Cell(symbolToAssign.ToString());
                    possibleIndexes.RemoveAt(positionInArray);
                    this.r_Gameboard.Add(randomIndex, cellAtIndex);
                }
            }
        }

        private Cell getCellAtIndex(Index i_IndexToFind)
        {
            return this.r_Gameboard[i_IndexToFind];
        }

        private void resetAfterRound()
        {
            this.m_CurrentRoundPreviousSelection = null;

            foreach (Cell aCellFromAPreviousRound in this.r_PreviouslyPlayedCells)
            {
                aCellFromAPreviousRound.VisualUpdate();
            }

            this.r_PreviouslyPlayedCells.Clear();
        }

        private bool checkForAWin(Index i_IndexA, Index i_IndexB)
        {
            Cell cellAtIndexA = this.getCellAtIndex(i_IndexA);
            Cell cellAtIndexB = this.getCellAtIndex(i_IndexB);

            Console.WriteLine($"i_IndexA != i_IndexB {i_IndexA != i_IndexB} cellAtIndexA == cellAtIndexB {cellAtIndexA == cellAtIndexB} cellAtIndexA.IsHidden() {cellAtIndexA.IsHidden()}");

            return i_IndexA != i_IndexB && cellAtIndexA == cellAtIndexB && cellAtIndexA.IsHidden();
        }

        private Player getCurrentPlayer()
        {
            return this.r_PlayerList[this.m_CurrentActivePlayerIndex];
        }

        private Color getCurrentPlayerColor()
        {
            return this.getCurrentPlayer().GetColor();
        }

        private void handleWin(Index i_IndexA, Index i_IndexB)
        {
            Cell[] cellsToReveal = { this.getCellAtIndex(i_IndexA), this.getCellAtIndex(i_IndexB) };
            Color colorToSet = this.getCurrentPlayerColor();

            foreach (Cell cellToReveal in cellsToReveal)
            {
                cellToReveal.Reveal(colorToSet);
                cellToReveal.VisualUpdate();
            }

            if (this.r_IndexAi.HasValue)
            {
                if (this.r_PlayerList[this.r_IndexAi.GetValueOrDefault()] is Ai aiPlayer)
                {
                    aiPlayer.ObserveWin(i_IndexA);
                    aiPlayer.ObserveWin(i_IndexB);
                }
            }

            this.m_CurrentTotalWins++;

            this.addPairRevealToPlayer();

            this.uiHandlePlayerScoreBoardUpdate();
        }

        private void addPairRevealToPlayer()
        {
            this.r_PlayerList[this.m_CurrentActivePlayerIndex].AddPairReveal();
        }

        private void advancePlayer()
        {
            this.m_CurrentActivePlayerIndex++;
            if (this.m_CurrentActivePlayerIndex >= this.r_PlayerList.Count)
            {
                this.m_CurrentActivePlayerIndex = 0;
            }

            this.uiHandlePlayerChange();
            this.ifAiPlay();
        }

        private void ifAiPlay()
        {
            Player currentPlayer = this.getCurrentPlayer();
            if (currentPlayer is Ai currentAiPlayer)
            {
                for (int i = 0; i < 2; i++)
                {
                    currentAiPlayer.Play();
                }
            }
        }

        private void uiHandlePlayerChange()
        {
            this.m_CurrentPlayerVisualUpdateAction($"Current Player: {this.getCurrentPlayer().GetPlayerName()}", this.getCurrentPlayerColor());
        }

        private void uiHandlePlayerScoreBoardUpdate()
        {
            int playersIndex = 0;

            foreach (Action<string, Color?> visualUpdateAction in this.m_PlayerScoreBoardUpdateActionList)
            {
                Player player = this.r_PlayerList[playersIndex];
                visualUpdateAction.Invoke(player.ToString(), player.GetColor());
                playersIndex += 1;
            }
        }

        private void handleGameOver()
        {
            this.m_GameOverAction.Invoke();
        }
    }
}
