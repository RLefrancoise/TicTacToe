namespace TicTacToe
{
    /// <summary>
    /// Data between scenes
    /// </summary>
    public static class BetweenSceneData
    {
        /// <summary>
        /// Game mode type
        /// </summary>
        public enum GameMode
        {
            /// <summary>
            /// New game mode
            /// </summary>
            NewGame,
            /// <summary>
            /// Continue game mode
            /// </summary>
            ContinueGame
        }

        /// <summary>
        /// Game mode
        /// </summary>
        public static GameMode Mode { get; set; }
        
        /// <summary>
        /// Snapshot
        /// </summary>
        public static GameSnapshot SnapShot { get; set; }
    }
}