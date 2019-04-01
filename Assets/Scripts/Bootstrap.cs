using UnityEngine;
using UnityEngine.SceneManagement;

namespace TicTacToe
{
    /// <inheritdoc />
    /// <summary>
    /// Bootstrap. Starts the application
    /// </summary>
    public class Bootstrap : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadSceneAsync("Scenes/MainMenu");
        }
    }
}