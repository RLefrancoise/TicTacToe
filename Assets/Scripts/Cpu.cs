using System;
using System.Linq;
using UniRx;

namespace TicTacToe
{
    public class Cpu
    {
        public void PlayTurn(TicTacToeGrid grid)
        {
            //Simulate 1s thinking
            Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(time =>
            {
                var chosenSlot = ChooseRandomSlot(grid);
                chosenSlot.PlaceSymbol.Execute();
            });
        }

        private GridSlot ChooseRandomSlot(TicTacToeGrid grid)
        {
            var freeSlots = grid.Slots.Where(s => s.IsFree).ToList();
            return freeSlots[new Random(DateTime.Now.Millisecond).Next(freeSlots.Count)];
        }
    }
}