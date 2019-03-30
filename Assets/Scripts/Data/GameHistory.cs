using System;
using System.Collections.Generic;

namespace TicTacToe
{
    /// <summary>
    /// History of game plays
    /// </summary>
    [Serializable]
    public class GameHistory
    {
        /// <summary>
        /// List of plays
        /// </summary>
        public List<GameData> plays;

        public GameHistory()
        {
            plays = new List<GameData>();
        }
    }
}