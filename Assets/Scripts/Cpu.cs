using System;
using TicTacToe.IA;
using UniRx;
using UnityEngine;
using Random = System.Random;

namespace TicTacToe
{
    /// <summary>
    /// CPU class. Handles how IA is playing the game.
    /// </summary>
    public static class Cpu
    {
        /// <summary>
        /// Play the CPU turn
        /// </summary>
        /// <param name="grid">the grid to play on</param>
        public static void PlayTurn(TicTacToeGrid grid)
        {
            var smartness = PlayerPrefs.GetInt("CPU Smartness", 50);
            var random = new Random(DateTime.Now.Millisecond).Next(101);

            IPlayTurnStrategy strategy;
            
            if(smartness == 0) strategy = new RandomSlotPlayTurnStrategy();
            else if(random <= smartness) strategy = new SmartPlayTurnStrategy();
            else strategy = new RandomSlotPlayTurnStrategy();
            
            //Simulate 1s thinking
            Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(time =>
            {
                strategy.ChooseSlot(grid).PlaceSymbol.Execute();
            });
        }
    }
}