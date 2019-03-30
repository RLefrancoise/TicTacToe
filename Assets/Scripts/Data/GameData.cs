using System;

namespace TicTacToe
{
    /// <summary>
    /// Game data of history or current game save state
    /// </summary>
    [Serializable]
    public class GameData
    {
        /// <summary>
        /// Name of the player
        /// </summary>
        public string playerName;

        /// <summary>
        /// Winner of the game
        /// </summary>
        public string winner;

        /// <summary>
        /// When the play was played
        /// </summary>
        public string date;
    }
}