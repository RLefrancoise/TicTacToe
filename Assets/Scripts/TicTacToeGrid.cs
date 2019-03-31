using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace TicTacToe
{
    /// <inheritdoc />
    /// <summary>
    /// Tic Tac Toe grid
    /// </summary>
    public class TicTacToeGrid : MonoBehaviour
    {
        /// <summary>
        /// Slots of the grid
        /// </summary>
        [SerializeField]
        private List<GridSlot> slots;
        /// <summary>
        /// Slots of the grid
        /// </summary>
        public List<GridSlot> Slots => slots;
        /// <summary>
        /// Grid rows
        /// </summary>
        public IEnumerable<IEnumerable<GridSlot>> Rows { get; private set; }
        /// <summary>
        /// grid columns
        /// </summary>
        public IEnumerable<IEnumerable<GridSlot>> Cols { get; private set; }
        /// <summary>
        /// Grid diagonals
        /// </summary>
        public IEnumerable<IEnumerable<GridSlot>> Diagonals { get; private set; }
        /// <summary>
        /// Is grid full ?
        /// </summary>
        public BoolReactiveProperty IsFull { get; private set; }
        
        private void Awake()
        {
            IsFull = new BoolReactiveProperty();
            
            Rows = new List<IEnumerable<GridSlot>>();
            Cols = new List<IEnumerable<GridSlot>>();
            Diagonals = new List<IEnumerable<GridSlot>>();
            
            //rows
            Rows = Rows.Append(Slots.Take(3));
            Rows = Rows.Append(Slots.Skip(3).Take(3));
            Rows = Rows.Append(Slots.Skip(6).Take(3));
            
            //cols
            Cols = Cols.Append(Slots.ByIndexes(0, 3, 6));
            Cols = Cols.Append(Slots.ByIndexes(1, 4, 7));
            Cols = Cols.Append(Slots.ByIndexes(2, 5, 8));
            
            //diagonals
            Diagonals = Diagonals.Append(Slots.ByIndexes(0, 4, 8));
            Diagonals = Diagonals.Append(Slots.ByIndexes(2, 4, 6));
            
            //display rows, cols, diagonals for debug purpose
            Debug.LogFormat("Rows: [{0}]", Rows.Aggregate("", (current, row) => current + "(" + string.Join(",", row.Select(s => s.name)) + ")"));
            Debug.LogFormat("Cols: [{0}]", Cols.Aggregate("", (current, col) => current + "(" + string.Join(",", col.Select(s => s.name)) + ")"));
            Debug.LogFormat("Diagonals: [{0}]", Diagonals.Aggregate("", (current, diagonal) => current + "(" + string.Join(",", diagonal.Select(s => s.name)) + ")"));
        }

        private void Start()
        {
            //listen slots to know if grid is full
            Slots.ForEach(s => s.Symbol.Subscribe(_ => CheckIsFull()));
        }
        
        /// <summary>
        /// Check if grid is full
        /// </summary>
        private void CheckIsFull()
        {
            IsFull.Value = Slots.All(s => !s.IsFree);
        }
        
        /// <summary>
        /// Check if all slots have same symbol and are not free
        /// </summary>
        /// <param name="slots">slots to check</param>
        /// <returns>true if all symbols are the same, false otherwise</returns>
        public static bool AreSameSymbol(IEnumerable<GridSlot> slots)
        {
            var gridSlots = slots.ToList();
            
            //if any slot is free, return false
            if (gridSlots.Any(slot => slot.IsFree)) return false;
            
            var firstSymbol = gridSlots.First().Symbol.Value.Type;
            return gridSlots.All(slot => slot.Symbol.Value.Type == firstSymbol);
        }
    }
}