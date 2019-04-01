using UnityEngine;

namespace TicTacToe
{
    public class KeepAlive : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}