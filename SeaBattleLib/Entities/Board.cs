using System;
using System.Collections;
using System.Collections.Generic;

namespace SeaBattleLib
{
    /// <summary>
    /// Represents an object of Board imitating the real SeaBattle boardgame's board. Holds the Boats of the player keeping their coordinates on the gameboard.
    /// </summary>
    public class Board : IEnumerable<Boat>
    {
        /// <summary>
        /// Imitation of players boat fleet. For each coordinate on the Board holds the reference to Boat object that stands there.
        /// </summary>
        private readonly Dictionary<string, Boat> fleet;

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public Board()
        {
            fleet = new Dictionary<string, Boat>();
        }

        /// <summary>
        /// Indexer to get the Boat object standing on the given cell.
        /// </summary>
        /// <param name="cell">String representation of Cell.</param>
        /// <returns>Boat object from Board standing on the given coordinate.</returns>
        public Boat this[string cell]
        {
            get
            {
                if (fleet.ContainsKey(cell))
                {
                    return fleet[cell];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                fleet[cell] = value;
            }
        }

        /// <summary>
        /// Check if there is a Boat object on the given cell.
        /// </summary>
        /// <param name="cell">Cell object to check.</param>
        /// <returns>True of Baord contains the cell in its Dictoray object, False if not.</returns>
        public bool ContainsCell(Cell cell) 
        {
            return this.fleet.ContainsKey(cell.ToString());
        }

        //IEnumarator interface implementation
        public IEnumerator<Boat> GetEnumerator()
        {
            return fleet.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)fleet.Values).GetEnumerator();
        }
    }
}
