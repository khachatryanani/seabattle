using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattleLib
{
    /// <summary>
    /// Manages the entities of Sea batlle library during the game.
    /// </summary>
    public class BoardManager
    {
        /// <summary>
        /// Board object that holds the players fleet.
        /// </summary>
        public Board Board { get; private set; }

        /// <summary>
        /// Keeps the count of active (not sunk) boats in the players fleet.
        /// </summary>
        public int CountOfBoats { get; private set; }

        /// <summary>
        /// The list of cells that are out of players fleet and were targeted by the opponent.
        /// </summary>
        public List<Cell> BlowPastCells { get; set; }


        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public BoardManager()
        {
            Board = new Board();
            BlowPastCells = new List<Cell>();
        }

        /// <summary>
        /// Adds a Boat object to the player's fleet (Board object).
        /// </summary>
        /// <param name="boat">Boat object to add to the player's fleet.</param>
        /// <returns>True of Boat is succefully added to the Board, False if one of the cells was invalid.</returns>
        public bool AddBoat(Boat boat)
        {
            var boatcells = boat.BoatCells;

            foreach (var cell in boatcells)
            {
                if (Board[cell.ToString()] != null)
                {
                    return false;
                }
            }

            foreach (var cell in boatcells)
            {
                Board[cell.ToString()] = boat;

                // Changes the Cell status from 0 (default empty) to 1 (active).
                cell.Status = 1;
            }

            ++this.CountOfBoats;

            return true;
        }

        /// <summary>
        /// Check if the opponent's targeted cell hits any of the Boats on players Board.
        /// </summary>
        /// <param name="targetedCell">Target Cell of the opponent.</param>
        /// <returns>0 if the cell was "blow past", 1 if a Boat for "damaged", 2 if a Boat was "killed".</returns>
        public byte IsHit(Cell targetedCell)
        {
            string target = targetedCell.ToString();
            Boat targetdBoat = Board[target];

            if (targetdBoat != null)
            {
                // Find the Cell of the Boat that was targetd and change its status from 1 (active) to 2 (hit).
                foreach (var cell in targetdBoat.BoatCells)
                {
                    if (cell == targetedCell)
                    {
                        cell.Status = 2;

                        ++targetdBoat.CountOfDamagedCells;
                        break;
                    }
                }

                if (targetdBoat.CountOfDamagedCells == targetdBoat.Len)
                {
                    // Change the status of targeted Boat from 1(damaged) to 2 (sunk)
                    targetdBoat.Status = 2;

                    this.CountOfBoats--;

                    targetedCell.Status = 2;
                    return 2;
                }
                else
                {
                    targetdBoat.Status = 1;

                    // Change the status of targeted Cell to 1 (which is this case will mean the last target had damaged a Boat).
                    targetedCell.Status = 1;

                    return 1;
                }
            }
            else
            {
                // Change the status of targeted Cell to 0 (which is this case will mean the last target had blown past).
                targetedCell.Status = 0;

                BlowPastCells.Add(targetedCell);

                return 0;
            }
        }

        #region Cell Validation Methods

        /// <summary>
        /// Check if the cell if empty to use.
        /// </summary>
        /// <param name="cell">String representation of the Cell.</param>
        /// <returns>True if the Cell is empty to use, False if there is already a Boat on the Cell.</returns>
        public bool IsFree(string cell)
        {
            return this.Board[cell] == null;
        }

        /// <summary>
        /// Check if the given Cell is in the limits of A-J X 1-10 board.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns>True if the Cell is within the limits of the board, False if the cell is out of limits of the board.</returns>
        public bool IsValidBoardCell(string cell)
        {
            if (string.IsNullOrEmpty(cell) || cell == null)
            {
                return false;
            }

            if (cell.Length > 3 && cell.Length < 2)
            {
                return false;
            }

            if (cell.Length == 2)
            {
                if (int.TryParse(cell[1].ToString(), out int number) && number != 0 && (int)cell[0] >= 65 && (int)cell[0] <= 74)
                {
                    return true;
                }
                return false;
            }

            if (cell.Length == 3)
            {
                if (int.TryParse(cell.Substring(1, 2), out int number) && number == 10 && (int)cell[0] >= 65 && (int)cell[0] <= 74)
                {
                    return true;
                }
                return false;
            }

            return false;
        }

        /// <summary>
        /// Checks if the given cell will be acceptable place for a boat. Boats should have at least 1-cell distance on every direction from all the other boats on the board.
        /// </summary>
        /// <param name="cell">String representation of Cell to check the acceptability for.</param>
        /// <returns>True if the Cell is acceptable by the board game laws, False if Cell is not acceptable.</returns>
        public bool IsAcceptable(string cell)
        {
            Cell cellToCheck = new Cell(cell);
            for (int i = cellToCheck.Number - 1; i <= cellToCheck.Number + 1; i++)
            {
                for (int j = cellToCheck.Letter - 1; j <= cellToCheck.Letter + 1; j++)
                {
                    Cell perifericCell = new Cell(((char)j).ToString() + i.ToString());
                    if (perifericCell != cellToCheck && Board.ContainsCell(perifericCell))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion
    }
}
