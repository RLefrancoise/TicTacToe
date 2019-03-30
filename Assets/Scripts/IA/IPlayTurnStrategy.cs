namespace TicTacToe.IA
{
    /// <summary>
    /// Interface for play turn strategy
    /// </summary>
    public interface IPlayTurnStrategy
    {
        /// <summary>
        /// Choose a slot to play
        /// </summary>
        /// <param name="grid">the grid to choose a slot on</param>
        /// <returns>the chosen slot</returns>
        GridSlot ChooseSlot(TicTacToeGrid grid);
    }
}