using TMPro;
using UnityEngine;
using UniRx;

namespace TicTacToe
{
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

        private void Start()
        {
            GameManager.Instance.CurrentPlayer.Subscribe(UpdateCurrentPlayerName);
        }

        /// <summary>
        /// Update current player name
        /// </summary>
        /// <param name="playerType"></param>
        private void UpdateCurrentPlayerName(PlayerType playerType)
        {
            var playerName = playerType == PlayerType.Cpu ? PlayerType.Cpu.ToString() : GameManager.Instance.PseudoPanel.Pseudo.Value;
            activePlayerText.text = $"Active player: {playerName}";
        }
    }
}