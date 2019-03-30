using System;

namespace TicTacToe
{
    /// <summary>
    /// Snapshot of a play
    /// </summary>
    [Serializable]
    public class GameSnapshot
    {
        /// <summary>
        /// Name of the player
        /// </summary>
        public string playerName;

        /// <summary>
        /// Current player of the game
        /// </summary>
        public string currentPlayer;

        /// <summary>
        /// Grid state
        /// </summary>
        public string grid;
    }
}