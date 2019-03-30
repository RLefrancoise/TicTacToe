using TMPro;
using UnityEngine;
using UniRx;

namespace TicTacToe
{
    /// <inheritdoc />
    /// <summary>
    /// Human user display
    /// </summary>
    public class Hud : MonoBehaviour
    {
        /// <summary>
        /// text displaying active player
        /// </summary>
        [SerializeField]
        private TMP_Text activePlayerText;

        [SerializeField]
        private GameObject gameOverPrefab;

        [SerializeField]
        private GameObject youWinPrefab;

        [SerializeField]
        private GameObject drawPrefab;
        
        private void Start()
        {
            //listen current player to display current player name
            GameManager.Instance.CurrentPlayer.Subscribe(UpdateCurrentPlayerName);
            //listen if game is won to display "You Win!" or "Game Over" message
            GameManager.Instance.WinTheGame.Subscribe(DisplayYouWinOrGameOver);
        }

        /// <summary>
        /// Update current player name
        /// </summary>
        /// <param name="playerType"></param>
        private void UpdateCurrentPlayerName(PlayerType playerType)
        {
            var playerName = playerType == PlayerType.Cpu 
                ? PlayerType.Cpu.ToString() 
                : GameManager.Instance.PlayerPseudo;
            
            activePlayerText.text = $"Active player: {playerName}";
        }

        private void DisplayYouWinOrGameOver(WinnerType playerType)
        {
            //game is over, hide current player name
            activePlayerText.gameObject.SetActive(false);
            
            switch (playerType)
            {
                case WinnerType.Human:
                    Instantiate(youWinPrefab);
                    break;
                case WinnerType.Cpu:
                    Instantiate(gameOverPrefab);
                    break;
                case WinnerType.Draw:
                    Instantiate(drawPrefab);
                    break;
            }
        }
    }
}