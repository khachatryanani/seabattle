using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattleLib
{
    /// <summary>
    /// Represents the Boat objects that are held in Board.
    /// </summary>
    public class Boat
    {
        /// <summary>
        /// Keeps the Cell objects of the Boat.
        /// </summary>
        public List<Cell> BoatCells { get; private set; }

        /// <summary>
        /// Specifies the lenght of the Boat or the number of Cells it has. Options for this game are: 1,2,3,4.
        /// </summary>
        public int Len { get; set; }

        /// <summary>
        /// Keeps the count of damaged Cells of the Boat.
        /// </summary>
        public int CountOfDamagedCells { get; set; }

        /// <summary>
        /// Keeps the status of the Boat: if it is OK, damaged or sunk.
        /// 0 if OK (default), 1 if Damaged, 2 if Sunk.
        /// </summary>
        public int Status { get; set; }


        /// <summary>
        /// Parapeterized Constructor that received a list of Cells to create a Boat Object.
        /// </summary>
        /// <param name="boatCells">List of Cells the Boat will be made of.</param>
        public Boat(List<Cell> boatCells)
        {
            this.Len = boatCells.Count;
            this.BoatCells = boatCells;
        }
    }
}
