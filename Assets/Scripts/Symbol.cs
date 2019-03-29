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

        public void AttachToSlot(GridSlot slot)
        {
            transform.SetParent(slot.transform, false);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}