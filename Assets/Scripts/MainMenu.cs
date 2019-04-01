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
        /// <summary>
        /// New game button
        /// </summary>
        [SerializeField]
        private Button newGameButton;
        /// <summary>
        /// Continue button
        /// </summary>
        [SerializeField]
        private Button continueButton;
        /// <summary>
        /// History button
        /// </summary>
        [SerializeField]
        private Button historyButton;
        /// <summary>
        /// Options button
        /// </summary>
        [SerializeField]
        private Button optionsButton;

        private void Start()
        {
            //play main menu music
            MusicManager.Instance.PlayMainMenuMusic();
            
            newGameButton.OnClickAsObservable().Subscribe(_ => NewGame());
            continueButton.OnClickAsObservable().Subscribe(_ => ContinueGame());
            historyButton.OnClickAsObservable().Subscribe(_ => History());
            optionsButton.OnClickAsObservable().Subscribe(_ => Options());
            
            //disable continue button if no snapshot
            if (!DataHelper.FileExists("snapshot")) continueButton.interactable = false;
            
            //disable history button if no history
            if (!DataHelper.FileExists("history")) historyButton.interactable = false;
        }

        /// <summary>
        /// Go to new game
        /// </summary>
        private static void NewGame()
        {
            BetweenSceneData.Mode = BetweenSceneData.GameMode.NewGame;
            SceneManager.LoadSceneAsync("Scenes/TicTacToe");
        }

        /// <summary>
        /// Go to continue game
        /// </summary>
        private static void ContinueGame()
        {
            BetweenSceneData.Mode = BetweenSceneData.GameMode.ContinueGame;
            BetweenSceneData.SnapShot = DataHelper.GetJsonContent<GameSnapshot>("snapshot");
            SceneManager.LoadSceneAsync("Scenes/TicTacToe");
        }

        /// <summary>
        /// Go to history
        /// </summary>
        private static void History()
        {
            SceneManager.LoadSceneAsync("Scenes/History");
        }

        /// <summary>
        /// Go to options
        /// </summary>
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