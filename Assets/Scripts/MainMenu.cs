using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TicTacToe
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private Button newGameButton;
        [SerializeField]
        private Button continueButton;
        [SerializeField]
        private Button historyButton;

        private void Start()
        {
            newGameButton.OnClickAsObservable().Subscribe(_ => { SceneManager.LoadSceneAsync("Scenes/TicTacToe"); });
        }
    }
}