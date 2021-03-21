using SeaBattleLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattleGame
{
    /// <summary>
    /// Poseidon class is a player of Sea Battle game that uses game algorithm to target Cells. Implementes the IPlayer interface.
    /// </summary>
    public class Poseidon : IPlayer
    {
        /// <summary>
        /// Name of the player.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// User's Board Manager.
        /// </summary>
        private readonly BoardManager boardManager = new BoardManager();

        /// <summary>
        ///  List of Cells that user has targeted on the opponents Board.
        /// </summary>
        private readonly List<Cell> targetedCells = new List<Cell>();

        private List<Cell> smartList;

        /// <summary>
        /// Property of Board Manager from IPlayer interface implementation.
        /// </summary>
        public BoardManager BoardManager { get => this.boardManager; }

        /// <summary>
        /// Property of targeted Cells' List from IPlayer interface implementation.
        /// </summary>
        public List<Cell> TargetedCells { get => targetedCells; }

        /// <summary>
        /// Property of Name from IPlayer interface implementation.
        /// </summary>
        public string Name { get => name; }

        /// <summary>
        /// Parameterized construcor.
        /// </summary>
        /// <param name="name">String representation of Player's name.</param>
        public Poseidon(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Checks if the given Cell has the result of "blow past", "hit" or "killed". Should compair the given Cell to the players Board to get the result.
        /// </summary>
        /// <param name="target">The targeted Cell coming from the opponent.</param>
        /// <returns>0 if "blow past", 1 if "hit", 2 if "killed".</returns>
        public byte CheckTargetedCell(Cell target)
        {
            return this.BoardManager.IsHit(target);
        }

        /// <summary>
        /// Creates a Boat of <param name="boatLenght"></param> lenght.
        /// </summary>
        /// <param name="boatNumber">The ordinal of the given-sized Boats of the player's Board.</param>
        /// <param name="boatLenght">The size/lenght of Boat to be created.</param>
        public void CreateBoat(int boatNumber, int boatLenght)
        {
            Random rand = new Random();
            List<Cell> cells;
            bool IsCreated;
            do
            {
                IsCreated = true;
                cells = new List<Cell>();
                char letter = (char)rand.Next(65, 74);
                int number = rand.Next(1, 10);

                int direction = rand.Next(1, 10);

                if (direction % 2 == 1)
                {
                    int end = letter + boatLenght;

                    for (; letter < end; letter++)
                    {
                        string cellString = letter.ToString() + number.ToString();

                        if (BoardManager.IsValidBoardCell(cellString) && BoardManager.IsFree(cellString) && BoardManager.IsAcceptable(cellString))
                        {
                            cells.Add(new Cell(letter, number));
                        }
                        else
                        {
                            IsCreated = false;
                            break;
                        }
                    }
                }
                else
                {
                    int end = number + boatLenght;
                    for (; number < end; number++)
                    {
                        string cellString = letter.ToString() + number.ToString();

                        if (BoardManager.IsValidBoardCell(cellString) && BoardManager.IsFree(cellString) && BoardManager.IsAcceptable(cellString))
                        { 
                            cells.Add(new Cell(letter, number));
                        }
                        else
                        {
                            IsCreated = false;
                            break;
                        }
                    }
                }
            }
            while (!IsCreated);

            BoardManager.AddBoat(new Boat(cells));
        }

        /// <summary>
        /// Gets the Cell to be targeted on the opponents Board.
        /// </summary>
        /// <returns>Cell to be targeted on the opponents Board.</returns>
        public Cell GetTargetedCell()
        {
            if (TargetedCells.Count != 0) 
            {
                Cell recentCell = TargetedCells[^1];

                if (recentCell.Status == 1)
                {
                    Cell newTargetCell;
                    string newTargetCellString = string.Empty;
                    for (int i = recentCell.Number - 1; i <= recentCell.Number + 1; i++)
                    {
                        for (int j = recentCell.Letter - 1; j <= recentCell.Letter + 1; j++)
                        {
                            if (i != recentCell.Number && j != recentCell.Letter)
                            {
                                continue;
                            }

                            newTargetCellString = ((char)j).ToString() + i.ToString();

                            if (BoardManager.IsValidBoardCell(newTargetCellString) &&
                                IsNearestPossibleCell(recentCell,newTargetCellString) &&
                                !TargetedCells.ContainsCell(newTargetCellString))
                            {
                                newTargetCell = new Cell(newTargetCellString);
                                TargetedCells.Add(newTargetCell);
                                return newTargetCell;
                            }
                        }
                    }
                }
            }

            return GetRandgomCell();
        }

        /// <summary>
        /// Gets a randomly generated Cell to target if the numbe of targeted Cells is less than 50. 
        /// Uses the smart list of reminder Cells to get a new target cell if the number of previously targeted Cells is greater than 50.
        /// </summary>
        /// <returns>New Cell to be targeted.</returns>
        private Cell GetRandgomCell()
        {
            Cell newTargetCell;
            Random rand = new Random();

            if (TargetedCells.Count <= 50)
            {
                string newTargetCellString;
                do
                {
                    newTargetCellString = ((char)rand.Next(65, 74)).ToString() + rand.Next(1, 10).ToString();
                }
                while (TargetedCells.ContainsCell(newTargetCellString) || TargetedCells.IsPerifericToHitCells(newTargetCellString));
                newTargetCell = new Cell(newTargetCellString);
            }
            else 
            {
                if (this.smartList == null) 
                {
                    this.smartList = GetSmartCellList();
                }
                
                newTargetCell = smartList[rand.Next(0, smartList.Count - 1)];
                smartList.Remove(newTargetCell);
            }

            TargetedCells.Add(newTargetCell);
            return newTargetCell;
        }

        /// <summary>
        /// Gets a list of remaining Cells to be hit on opponents board.
        /// </summary>
        /// <returns>The list of remaining Cells on opponents board.</returns>
        private List<Cell> GetSmartCellList() 
        {
            var remainingCells = new List<Cell>();
        
            for (int number = 1; number <= 10; number++)
            {
                for (int letter = 65; letter <= 74; letter++)
                {
                    Cell remainingCell = new Cell((char)letter, number);
                    if (!TargetedCells.ContainsCell(remainingCell)) 
                    {
                        remainingCells.Add(remainingCell);
                    }
                }
            }
            return remainingCells;
        }

        /// <summary>
        /// Checks if the current nearest Cell of the last target can be another possible target according to game laws.
        /// </summary>
        /// <param name="recentCell">The recent last target.</param>
        /// <param name="nearestCellString">Periferic nearest Cell to the last targeted Cell.</param>
        /// <returns>True if the given cell is possible to hit the next according the game laws, False if not.</returns>
        private bool IsNearestPossibleCell(Cell recentCell, string nearestCellString)
        {
            Cell nearestCell = new Cell(nearestCellString);
            for (int i = nearestCell.Number - 1; i <= nearestCell.Number + 1; i++)
            {
                for (int j = nearestCell.Letter - 1; j <= nearestCell.Letter + 1; j++)
                {
                    Cell perifericCell = new Cell(((char)j).ToString() + i.ToString());

                    if (perifericCell != nearestCell && perifericCell != recentCell && TargetedCells.ContainsAsHitCell(perifericCell))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Checks it all the Boats of the player are "killed" to end the game.
        /// </summary>
        /// <returns>True if all the Boats of the player are "killed", False if the Board of the player still has active Boats.</returns>
        public bool IsFloatDestroyed()
        {
            return this.BoardManager.CountOfBoats == 0;
        }
    }
}
