using System;
using System.Linq;
using TicTacToe.IA;
using UniRx;

namespace TicTacToe
{
    /// <summary>
    /// CPU class. Handles how IA is playing the game.
    /// </summary>
    public class Cpu
    {
        /// <summary>
        /// Strategy to use by the CPU
        /// </summary>
        public IPlayTurnStrategy Strategy { get; set; }

        public Cpu(IPlayTurnStrategy strategy)
        {
            Strategy = strategy;
        }
        
        /// <summary>
        /// Play the CPU turn
        /// </summary>
        /// <param name="grid">the grid to play on</param>
        public void PlayTurn(TicTacToeGrid grid)
        {
            //Simulate 1s thinking
            Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(time =>
            {
                Strategy.ChooseSlot(grid).PlaceSymbol.Execute();
            });
        }
    }
}