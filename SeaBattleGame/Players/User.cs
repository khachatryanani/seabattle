using System;
using System.Collections.Generic;
using System.Text;
using SeaBattleLib;

namespace SeaBattleGame
{
    /// <summary>
    /// User class is a player of Sea Battle game that uses the Console for sending the targeted cells and reviewint the results. Implementes the IPlayer interface.
    /// </summary>
    public class User : IPlayer
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

        /// <summary>
        /// Property of Board Manager from IPlayer interface implementation.
        /// </summary>
        public BoardManager BoardManager { get => boardManager; }

        /// <summary>
        /// Property of targeted Cells' List from IPlayer interface implementation.
        /// </summary>
        public List<Cell> TargetedCells { get => targetedCells; }

        /// <summary>
        /// Property of Name from IPlayer interface implementation.
        /// </summary>
        public string Name { get => name; }

        /// <summary>
        /// Parameterized construcor
        /// </summary>
        /// <param name="name">String representation of Player's name</param>
        public User(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Gets the start Cell coordinates from Console for the Boat to created for User.
        /// </summary>
        /// <param name="boatNumber">Ordinal number of Boat from <param name="boatLenght"> - lenght boats to get the start cell for.</param>
        /// <param name="boatLenght">Lenght/size of the Boat to get the start cell for.</param>
        /// <returns>Cell object as the start for the Boat.</returns>
        private Cell GetBoatStartCell(int boatNumber, int boatLenght)
        {
            bool IsValid;
            string cellString;
            do
            {
                IsValid = true;
                Console.Write($"Enter the start cell for the N:{boatNumber} of {boatLenght}-cell lenght boats: ");
                cellString = Console.ReadLine().ToUpper();
                if (!(BoardManager.IsValidBoardCell(cellString) && BoardManager.IsAcceptable(cellString)))
                {
                    IsValid = false;
                    Console.WriteLine("Not a valid cell, try another one!");
                }
            }
            while (!IsValid);

            return new Cell(cellString);
        }

        /// <summary>
        /// Gets the start Cell coordinates from Console for the Boat to created for User.
        /// </summary>
        /// <param name="boatNumber">Ordinal number of Boat from <param name="boatLenght"> - lenght boats to get the end cell for.</param>
        /// <param name="boatLenght">Lenght/size of the Boat to get the end cell for.</param>
        /// <returns>Cell object as the end for the Boat.</returns>
        private Cell GetBoatEndCell(int boatNumber, int boatLenght)
        {
            bool IsValid;
            string cellString;
            do
            {
                IsValid = true;
                Console.Write($"Enter the end cell for the N:{boatNumber} of {boatLenght}-cell lenght boats: ");
                cellString = Console.ReadLine().ToUpper();
                if (!(BoardManager.IsValidBoardCell(cellString) && BoardManager.IsAcceptable(cellString)))
                {
                    IsValid = false;
                    Console.WriteLine("Not a valid cell, try another one!");
                }
            }
            while (!IsValid);

            return new Cell(cellString);
        }

        //IPlayer interface implementation

        /// <summary>
        /// Creates a Boat of <param name="boatLenght"></param> lenght.
        /// </summary>
        /// <param name="boatNumber">The ordinal of the given-sized Boats of the player's Board.</param>
        /// <param name="boatLenght">The size/lenght of Boat to be created.</param>
        public void CreateBoat(int boatNumber, int boatLenght)
        {
            List<Cell> cells = new List<Cell>();
            Cell startCell = GetBoatStartCell(boatNumber, boatLenght);
            Cell endCell;
            if (boatLenght == 1)
            {
                endCell = startCell;
            }
            else 
            {
                bool isValidLenght;
                do
                {
                    isValidLenght = true;
                    endCell = GetBoatEndCell(boatNumber, boatLenght);
                    if (!((startCell.Number == endCell.Number || startCell.Letter == endCell.Letter) && startCell.CellDif(endCell) == boatLenght - 1)) 
                    {
                        isValidLenght = false;
                        Console.WriteLine($"Not a valid lenght. You should create a boat of {boatLenght}-cell lenght.");
                    }
                }
                while (!isValidLenght);
            }
            
            if (startCell.Letter == endCell.Letter)
            {
                if (startCell.Number <= endCell.Number)
                {
                    for (int i = startCell.Number; i <= endCell.Number; i++)
                    {
                        cells.Add(new Cell(startCell.Letter, i));
                    }
                }
                else 
                {
                    for (int i = endCell.Number; i <= startCell.Number; i++)
                    {
                        cells.Add(new Cell(startCell.Letter, i));
                    }
                }
            }
            else if (startCell.Number == endCell.Number) 
            {
                if (startCell.Letter <= endCell.Letter)
                {
                    for (int i = startCell.Letter; i <= endCell.Letter; i++)
                    {
                        cells.Add(new Cell((char)i, startCell.Number));
                    }
                }
                else 
                {
                    for (int i = endCell.Letter; i <= startCell.Letter; i++)
                    {
                        cells.Add(new Cell((char)i, startCell.Number));
                    }
                }
            }

            BoardManager.AddBoat(new Boat(cells));
        }

        /// <summary>
        /// Gets the Cell to be targeted on the opponents Board.
        /// </summary>
        /// <returns>Cell to be targeted on the opponents Board.</returns>
        public Cell GetTargetedCell()
        {
            bool isValid;
            string cellString = string.Empty;
            Console.SetCursorPosition(0, Console.CursorTop);
            if (TargetedCells.Count != 0) 
            {
                AnalyzeResult(TargetedCells[^1]);
            }

            Console.Write("Enter your target cell: ");
            do
            {
                isValid = true;
                               
                cellString = Console.ReadLine().ToUpper();
                if (!BoardManager.IsValidBoardCell(cellString))
                {
                    isValid = false;

                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.WriteLine(new string(' ', 70));
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write("Your cell is out of board, try another one: ");

                }
                else 
                {
                    if (TargetedCells.ContainsCell(cellString))
                    {
                        isValid = false;

                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Console.WriteLine(new string(' ', 70));
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Console.Write("You have already targeted this cell, try another one: ");
                    }
                }
             
            }
            while (!isValid);

            Cell target = new Cell(cellString);
            TargetedCells.Add(target);

            return target;
        }

        /// <summary>
        /// Checks if the given Cell has the result of "blow past", "hit" or "killed". Should compair the given Cell to the players Board to get the result.
        /// </summary>
        /// <param name="target">The targeted Cell coming from the opponent.</param>
        /// <returns>0 if "blow past", 1 if "hit", 2 if "killed".</returns>
        public byte CheckTargetedCell(Cell target)
        {
            return BoardManager.IsHit(target);
        }

        /// <summary>
        /// Checks it all the Boats of the player are "killed" to end the game.
        /// </summary>
        /// <returns>True if all the Boats of the player are "killed", False if the Board of the player still has active Boats.</returns>
        public bool IsFloatDestroyed()
        {
            return BoardManager.CountOfBoats == 0;
        }

        /// <summary>
        /// Receives the last targeted Cell and displays result in Console.
        /// </summary>
        /// <param name="recentCell">The last targeted Cell of player.</param>
        private void AnalyzeResult(Cell recentCell) 
        {
            switch (recentCell.Status) 
            {
                case 0:
                    Console.WriteLine("Ups, blow past!");
                    break;
                case 1:
                    Console.WriteLine("Boat damaged, keep on!");
                    break;
                case 2:
                    Console.WriteLine("Great! Boat sunk!");
                    break;
            }
        }
    }
}
