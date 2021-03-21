using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattleLib
{
    /// <summary>
    /// Extensions class
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Checks if the list of Cell objects contains the given cell.
        /// </summary>
        /// <param name="cells">List of Cell objects.</param>
        /// <param name="cellString">String representation of Cell object.</param>
        /// <returns>True is List of Cells contains the given Cell, False if the List of Cells does not contain the given Cell.</returns>
        public static bool ContainsCell(this List<Cell> cells, string cellString) 
        {
            if (cells == null) 
            {
                throw new ArgumentNullException(nameof(cells));
            }

            if (cellString == null) 
            {
                throw new ArgumentNullException(nameof(cellString));
            }

            if (string.IsNullOrEmpty(cellString)) 
            {
                throw new ArgumentException(nameof(cellString));
            }

            char letter = cellString[0];
            int.TryParse(cellString.Substring(1, cellString.Length - 1), out int number);
            foreach (var cell in cells)
            {
                if (cell.Letter == letter && cell.Number == number) 
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the list of Cell objects contains the given cell as a hit cell.
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="cellObj"></param>
        /// <returns></returns>
        public static bool ContainsAsHitCell(this List<Cell> cells, Cell cellObj)
        {
            if (cells == null) 
            {
                throw new ArgumentNullException(nameof(cells));
            }

            if (cellObj == null)
            {
                throw new ArgumentNullException(nameof(cellObj));
            }

            foreach (var cell in cells)
            {
                if (cell.Letter == cellObj.Letter && cell.Number == cellObj.Number && cell.Status != 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the list of Cell objects contains the given cell.
        /// </summary>
        /// <param name="cells">List of Cell objects.</param>
        /// <param name="cellObj">Cell object.</param>
        /// <returns>True is List of Cells contains the given Cell, False if the List of Cells does not contain the given Cell.</returns>
        public static bool ContainsCell(this List<Cell> cells, Cell cellObj)
        {
            if (cells == null)
            {
                throw new ArgumentNullException(nameof(cells));
            }

            if (cellObj == null)
            {
                throw new ArgumentNullException(nameof(cellObj));
            }

            foreach (var cell in cells)
            {
                if (cell.Letter == cellObj.Letter && cell.Number == cellObj.Number)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get the Cell object from the List of Cell objects.
        /// </summary>
        /// <param name="cells">List of Cell objects.</param>
        /// <param name="cellString">String representation of Cell object of which should be returned.</param>
        /// <returns>Cell object from the List of Cells.</returns>
        public static Cell GetCell (this List<Cell> cells, string cellString)
        {
            if (cells == null)
            {
                throw new ArgumentNullException(nameof(cells));
            }

            if (cellString == null)
            {
                throw new ArgumentNullException(nameof(cellString));
            }

            if (string.IsNullOrEmpty(cellString))
            {
                throw new ArgumentException(nameof(cellString));
            }

            char letter = cellString[0];
            int.TryParse(cellString.Substring(1, cellString.Length - 1), out int number);
            foreach (var cell in cells)
            {
                if (cell.Letter == letter && cell.Number == number)
                {
                    return cell;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if the <param name="cellToCheck"></param> is periferic cell to <param name="cell"></param> : 1-cell away on any direction.
        /// </summary>
        /// <param name="cellToCheck">Cell to check</param>
        /// <param name="cell">Cell to check for</param>
        /// <returns></returns>
        public static bool IsPerifericToCell(this Cell cellToCheck, Cell cell) 
        {
            if (cellToCheck == null)
            {
                throw new ArgumentNullException(nameof(cellToCheck));
            }

            if (cell == null)
            {
                throw new ArgumentNullException(nameof(cell));
            }

            Cell perifericCell;
            for (int i = cell.Number - 1; i <= cell.Number + 1; i++)
            {
                for (int j = cell.Letter - 1; j <= cell.Letter + 1; j++)
                {
                    string newCellString = ((char)j).ToString() + i.ToString();
                    perifericCell = new Cell(newCellString);
                    if (perifericCell == cellToCheck) 
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the given cell is periferic to any Cell in the list that is hit (is a boat cell).
        /// </summary>
        /// <param name="cells">List of Cells</param>
        /// <param name="cellString">String representation of Cell to check</param>
        /// <returns>True if the given cell is periferic to any hit cell in the List of Cells, False if not.</returns>
        public static bool IsPerifericToHitCells(this List<Cell> cells, string cellString) 
        {
            if (cells == null)
            {
                throw new ArgumentNullException(nameof(cells));
            }

            if (cellString == null)
            {
                throw new ArgumentNullException(nameof(cellString));
            }

            if (string.IsNullOrEmpty(cellString))
            {
                throw new ArgumentException(nameof(cellString));
            }

            Cell cellToCheck = new Cell(cellString);
            foreach (var cell in cells)
            {
                if (cell.Status != 0) 
                {
                    if (cellToCheck.IsPerifericToCell(cell))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
