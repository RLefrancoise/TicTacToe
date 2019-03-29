using UnityEngine;

namespace TicTacToe
{
    public class Symbol : MonoBehaviour
    {
        public enum SymbolType
        {
            Cross,
            Circle
        }

        [SerializeField]
        private SymbolType type;
        
        public SymbolType Type => type;
    }
}