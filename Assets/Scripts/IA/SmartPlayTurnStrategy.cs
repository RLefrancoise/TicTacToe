using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe.IA
{
    /// <inheritdoc />
    /// <summary>
    /// Smart play turn strategy
    /// </summary>
    public class SmartPlayTurnStrategy : IPlayTurnStrategy
    {
        /// <inheritdoc />
        public GridSlot ChooseSlot(TicTacToeGrid grid)
        {
            //look for a row, column or diagonal that would be almost full
            var lines = grid.Rows.ToList();
            lines.AddRange(grid.Cols);
            lines.AddRange(grid.Diagonals);
            
            //for us
            foreach (var line in lines)
            {
                GridSlot slot;
                if (ChooseRemainingFreeSlot(line, Symbol.SymbolType.Cross, out slot))
                    return slot;
            }
            
            //for player
            foreach (var line in lines)
            {
                GridSlot slot;
                if (ChooseRemainingFreeSlot(line, Symbol.SymbolType.Circle, out slot))
                    return slot;
            }
            
            //If a corner is available, choose a corner
            var cornersIndexes = new []{0, 2, 6, 8};
            var cornersFreeSlots = grid.Slots.ByIndexes(cornersIndexes).Where(s => s.IsFree).ToList();
            if (cornersFreeSlots.Any())
            {
                return cornersFreeSlots[new Random(DateTime.Now.Millisecond).Next(cornersFreeSlots.Count())];
            }
            
            //in this end, choose a random slot if all other cases are not encountered
            var freeSlots = grid.Slots.Where(s => s.IsFree).ToList();
            return freeSlots[new Random(DateTime.Now.Millisecond).Next(freeSlots.Count)];
        }

        private static bool ChooseRemainingFreeSlot(IEnumerable<GridSlot> slots, Symbol.SymbolType symbolType, out GridSlot chosenSlot)
        {
            var list = slots.ToList();
            if (list.Count(s => !s.IsFree && s.Symbol.Value.Type == symbolType) == 2 && list.Any(s => s.IsFree))
            {
                //get index of slot in row that is not yet full, we are gonna choose this one
                chosenSlot = list.First(s => s.IsFree);
                return true;
            }

            chosenSlot = null;
            return false;
        }
    }
}