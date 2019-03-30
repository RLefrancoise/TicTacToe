using System;
using System.Linq;

namespace TicTacToe
{
    public class Cpu
    {
        public void PlayTurn(TicTacToeGrid grid)
        {
            var chosenSlot = ChooseRandomSlot(grid);
            chosenSlot.PlaceSymbol.Execute();
        }

        private GridSlot ChooseRandomSlot(TicTacToeGrid grid)
        {
            var freeSlots = grid.Slots.Where(s => s.IsFree).ToList();
            return freeSlots[new Random().Next(0, freeSlots.Count - 1)];
        }
    }
}