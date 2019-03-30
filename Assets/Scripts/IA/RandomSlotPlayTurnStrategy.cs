using System;
using System.Linq;

namespace TicTacToe.IA
{
    /// <inheritdoc />
    /// <summary>
    /// Play turn by choosing random slot on the grid
    /// </summary>
    public class RandomSlotPlayTurnStrategy : IPlayTurnStrategy
    {
        /// <inheritdoc />
        public GridSlot ChooseSlot(TicTacToeGrid grid)
        {
            var freeSlots = grid.Slots.Where(s => s.IsFree).ToList();
            return freeSlots[new Random(DateTime.Now.Millisecond).Next(freeSlots.Count)];
        }
    }
}