using TMPro;
using UnityEngine;

namespace TicTacToe
{
    /// <inheritdoc />
    /// <summary>
    /// History entry
    /// </summary>
    public class HistoryEntry : MonoBehaviour
    {
        /// <summary>
        /// Color to use when player won the game
        /// </summary>
        [SerializeField]
        private Color humanWinColor;
        /// <summary>
        /// Color to use when CPU won the game
        /// </summary>
        [SerializeField]
        private Color cpuWinColor;
        /// <summary>
        /// Color yo use when it was a draw
        /// </summary>
        [SerializeField]
        private Color drawWinColor;
        /// <summary>
        /// Win status (Human, cpu, draw)
        /// </summary>
        [SerializeField]
        private TMP_Text winStatus;
        /// <summary>
        /// Player name chosen when play has been done
        /// </summary>
        [SerializeField]
        private TMP_Text playerName;
        /// <summary>
        /// Date of the entry
        /// </summary>
        [SerializeField]
        private TMP_Text date;

        /// <summary>
        /// Init from game data
        /// </summary>
        /// <param name="gameData">game data to init from</param>
        public void InitFromData(GameData gameData)
        {
            playerName.text = gameData.playerName;
            date.text = gameData.date;

            switch (gameData.winner)
            {
                case "Cpu":
                    winStatus.text = "Game Over";
                    winStatus.color = cpuWinColor;
                    break;
                case "Draw":
                    winStatus.text = "Draw";
                    winStatus.color = drawWinColor;
                    break;
                default:
                    winStatus.text = "You Win";
                    winStatus.color = humanWinColor;
                    break;
            }
        }
    }
}