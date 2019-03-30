using System;
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
                : GameManager.Instance.PseudoPanel.Pseudo.Value;
            
            activePlayerText.text = $"Active player: {playerName}";
        }

        private void DisplayYouWinOrGameOver(PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.Human:
                    Instantiate(youWinPrefab);
                    break;
                case PlayerType.Cpu:
                    Instantiate(gameOverPrefab);
                    break;
            }
        }
    }
}