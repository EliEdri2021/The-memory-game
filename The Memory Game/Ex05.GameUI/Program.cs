namespace Ex05.GameUI
{
    using System.Drawing;
    using System.Windows.Forms;

    public class Program
    {
        private const string k_AnotherRoundAsk = "Another Round?";

        public static void Main()
        {
            run();
        }

        private static void run()
        {
            StartupForm startupFrom = new StartupForm();
            startupFrom.ShowDialog();

            // if (startupFrom.GetIsGameStart()) - line commented out to disable X button ignore
            int columnCount = startupFrom.GetColumnCount();
            int rowCount = startupFrom.GetRowCount();
            string firstPlayerName = startupFrom.GetPlayerOneName();
            bool isSecondPlayerAi = startupFrom.GetIsSecondPlayerAi();
            string secondPlayerName = startupFrom.GetPlayerTwoName();
            Color? secondPlayerColor = null;

            if (!isSecondPlayerAi)
            {
                secondPlayerColor = Color.Chartreuse;
            }

            bool stopGame = false;

            while (!stopGame)
            {
                GameLogic.GameLogic gameLogic = new GameLogic.GameLogic(
                    columnCount,
                    rowCount,
                    firstPlayerName,
                    isSecondPlayerAi,
                    secondPlayerName,
                    secondPlayerColor);

                GameForm gameUi = new GameForm(ref gameLogic);
                gameUi.ShowDialog();

                string message = gameLogic.WhoWon();
                stopGame = !(gameLogic.IsGameFinished() && isAnotherGameDialog(message));
            }
        }

        private static bool isAnotherGameDialog(string i_Message)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(i_Message, k_AnotherRoundAsk, buttons);

            return result == DialogResult.Yes;
        }
    }
}
