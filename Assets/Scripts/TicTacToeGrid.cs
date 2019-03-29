using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

namespace TicTacToe
{
    public class TicTacToeGrid : MonoBehaviour
    {
        [SerializeField]
        private List<GridSlot> slots;

        public List<GridSlot> Slots => slots;

        public List<List<GridSlot>> rows;
        public List<List<GridSlot>> cols;
        public List<List<GridSlot>> diagonals;
        
        /// <summary>
        /// Check if all slots have same symbol and are not free
        /// </summary>
        /// <param name="slots">slots to check</param>
        /// <returns>true if all symbols are the same, false otherwise</returns>
        public static bool AreSameSymbol(IEnumerable<GridSlot> slots)
        {
            var gridSlots = slots.ToList();
            
            if (gridSlots.Any(slot => slot.Symbol.Value == null)) return false;
            
            var firstSymbol = gridSlots.First().Symbol.Value.Type;
            return gridSlots.All(slot => !slot.IsFree && slot.Symbol.Value.Type == firstSymbol);
        }
    }
}