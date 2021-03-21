using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable CS0660
#pragma warning disable CS0661

namespace SeaBattleLib
{
    /// <summary>
    /// The smallest unit of the Board the Sea battle game is played on. 
    /// </summary>
    public class Cell
    {
        // The CHAR coordinate of the Cell on the Game Board.
        public char Letter { get; set; }

        // The INTEGER coordinate of the Cell on the Game Board.
        public int Number { get; set; }

        /// <summary>
        /// Keeps the status of the Cell: if it is empty, active or hit.
        /// 0 if empty (default), 1 if active, 2 if hit.
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// Parameterized constructor of Cell Object that receives CHAR and INTEGER for its coordinates.
        /// </summary>
        /// <param name="letter">CHAR for the first part of coordinate.</param>
        /// <param name="number">INTEGER for the second part of coordinate.</param>
        public Cell(char letter, int number)
        {
            this.Letter = letter;
            this.Number = number;
        }

        /// <summary>
        /// Parameterized constructor of Cell Object that receives coordinates' string representation.
        /// </summary>
        /// <param name="cell">String representation of Cell's coordinates.</param>
        public Cell(string cell)
        {
            this.Letter = cell[0];
            this.Number = int.Parse(cell.Substring(1,cell.Length - 1).ToString());
        }

        /// <summary>
        /// Overriding the ToString() methode to return string representation of Cell's coordinates.
        /// </summary>
        /// <returns>String representation of Cell's coordinates.</returns>
        public override string ToString()
        {
            return Letter.ToString() + Number.ToString();
        }

        /// <summary>
        /// Overloading the equality operator to compare two Cell Objects.
        /// </summary>
        /// <param name="cell1">Cell object to compare.</param>
        /// <param name="cell2">Cell object to compare.</param>
        /// <returns>True of two Cells are identical with their coordinates, False it they have difference coordinates.</returns>
        public static bool operator ==(Cell cell1, Cell cell2)
        {
            return cell1.Letter == cell2.Letter && cell1.Number == cell2.Number;
        }

        /// <summary>
        /// Overloading the inequality operator to compare two Cell Objects.
        /// </summary>
        /// <param name="cell1">Cell object to compare.</param>
        /// <param name="cell2">Cell object to compare.</param>
        /// <returns>False of two Cells are identical with their coordinates, True it they have difference coordinates.</returns>
        public static bool operator !=(Cell cell1, Cell cell2)
        {
            return !(cell1.Letter == cell2.Letter && cell1.Number == cell2.Number);
        }

        /// <summary>
        /// Calculates the Cells' coordinates' difference vertically or horizontally: difference of Letter part or differenc of Number part of the coordinates.
        /// </summary>
        /// <param name="cell">Cell object to compare this object to.</param>
        /// <returns>Distance in integer representation.</returns>
        public int CellDif(Cell cell) 
        {
            if (this.Letter == cell.Letter)
            {
                return Math.Abs(this.Number - cell.Number);
            }
            else 
            {
                return Math.Abs(this.Letter - cell.Letter);
            }
        } 
    }
}
