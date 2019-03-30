using TicTacToe.IO;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    /// <inheritdoc />
    /// <summary>
    /// History screen
    /// </summary>
    public class HistoryScreen : MonoBehaviour
    {
        /// <summary>
        /// Prefab of history entry
        /// </summary>
        [SerializeField]
        private GameObject historyEntryPrefab;
        
        /// <summary>
        /// Root of history list
        /// </summary>
        [SerializeField]
        private RectTransform historyList;
        
        /// <summary>
        /// Back button
        /// </summary>
        [SerializeField]
        private Button backButton;

        private void Start()
        {
            //go back to main menu when back button clicked
            backButton.OnClickAsObservable().Subscribe(_ => MainMenu.GoBackToMainMenu());

            //Read history
            var history = StreamingAssetsHelper.GetJsonContent<GameHistory>("history");

            foreach (var entry in history.plays)
            {
                var historyEntry = Instantiate(historyEntryPrefab, historyList).GetComponent<HistoryEntry>();
                historyEntry.InitFromData(entry);
            }

            //Reset scroll
            historyList.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
        }
    }
}