using TicTacToe.IO;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TicTacToe
{
    /// <inheritdoc />
    /// <summary>
    /// Main menu of the game
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private Button newGameButton;
        [SerializeField]
        private Button continueButton;
        [SerializeField]
        private Button historyButton;
        [SerializeField]
        private Button optionsButton;

        private void Start()
        {
            newGameButton.OnClickAsObservable().Subscribe(_ => NewGame());
            continueButton.OnClickAsObservable().Subscribe(_ => ContinueGame());
            historyButton.OnClickAsObservable().Subscribe(_ => History());
            optionsButton.OnClickAsObservable().Subscribe(_ => Options());
            
            //disable continue button if no snapshot
            if (!StreamingAssetsHelper.FileExists("snapshot")) continueButton.interactable = false;
            
            //disable history button if no history
            if (!StreamingAssetsHelper.FileExists("history")) historyButton.interactable = false;
        }

        private static void NewGame()
        {
            BetweenSceneData.Mode = BetweenSceneData.GameMode.NewGame;
            SceneManager.LoadSceneAsync("Scenes/TicTacToe");
        }

        private static void ContinueGame()
        {
            BetweenSceneData.Mode = BetweenSceneData.GameMode.ContinueGame;
            BetweenSceneData.SnapShot = StreamingAssetsHelper.GetJsonContent<GameSnapshot>("snapshot");
            SceneManager.LoadSceneAsync("Scenes/TicTacToe");
        }

        private static void History()
        {
            SceneManager.LoadSceneAsync("Scenes/History");
        }

        private static void Options()
        {
            SceneManager.LoadSceneAsync("Scenes/Options");
        }
        
        /// <summary>
        /// Go back to main menu
        /// </summary>
        public static void GoBackToMainMenu()
        {
            SceneManager.LoadSceneAsync("Scenes/MainMenu");
        }
    }
}