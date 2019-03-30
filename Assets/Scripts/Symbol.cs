using UnityEngine;

namespace TicTacToe
{
    /// <inheritdoc />
    /// <summary>
    /// Symbol that you can put on a slot.
    /// </summary>
    public class Symbol : MonoBehaviour
    {
        /// <summary>
        /// Type of symbol (cross, circle)
        /// </summary>
        public enum SymbolType
        {
            Cross,
            Circle
        }

        /// <summary>
        /// Type of the symbol
        /// </summary>
        [SerializeField]
        private SymbolType type;
        
        /// <summary>
        /// Type of the symbol
        /// </summary>
        public SymbolType Type => type;

        /// <summary>
        /// Attach the symbol to a slot
        /// </summary>
        /// <param name="slot">Slot to be attached to</param>
        public void AttachToSlot(GridSlot slot)
        {
            var t = transform; //introduce variable cuz it's more efficient (even if here it's not a performance critical context)
            t.SetParent(slot.transform, false);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
        }
    }
}