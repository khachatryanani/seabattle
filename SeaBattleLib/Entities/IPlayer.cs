using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattleLib
{
    /// <summary>
    /// Interface for any SeaBattle player class. Defines the main methodes and properties that make the game possible to manage.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Holds the players name.
        /// </summary>
        string Name { get;}

        /// <summary>
        /// Each player has its own BoardManager for its own float of boats.
        /// </summary>
        BoardManager BoardManager { get; }

        // List of all targeted cells on the opponents board (all targtes on opponents float).
        List<Cell> TargetedCells { get; }

        /// <summary>
        /// Creates a Boat of <param name="boatLenght"></param> lenght.
        /// </summary>
        /// <param name="boatNumber">The ordinal of the given-sized Boats of the player's Board.</param>
        /// <param name="boatLenght">The size/lenght of Boat to be created.</param>
        void CreateBoat(int boatNumber, int boatLenght);

        /// <summary>
        /// Gets the Cell to be targeted on the opponents Board.
        /// </summary>
        /// <returns>Cell to be targeted on the opponents Board.</returns>
        Cell GetTargetedCell();

        /// <summary>
        /// Checks if the given Cell has the result of "blow past", "hit" or "killed". Should compair the given Cell to the players Board to get the result.
        /// </summary>
        /// <param name="target">The targeted Cell coming from the opponent.</param>
        /// <returns>0 if "blow past", 1 if "hit", 2 if "killed".</returns>
        byte CheckTargetedCell(Cell target);

        /// <summary>
        /// Checks it all the Boats of the player are "killed" to end the game.
        /// </summary>
        /// <returns>True if all the Boats of the player are "killed", False if the Board of the player still has active Boats.</returns>
        bool IsFloatDestroyed();
    }
}
