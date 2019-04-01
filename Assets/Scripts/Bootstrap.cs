using UnityEngine;
using UnityEngine.SceneManagement;

namespace TicTacToe
{
    public class Bootstrap : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadSceneAsync("Scenes/MainMenu");
        }
    }
}