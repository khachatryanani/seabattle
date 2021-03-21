using SeaBattleLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattleGame
{
    /// <summary>
    /// Organizes the game and takes turns between players.
    /// </summary>
    public class GameManager
    {
        /// <summary>
        /// Keeps Player's name whos turn is to play.
        /// </summary>
        private string turn;

        /// <summary>
        /// Array of players (2 players per game).
        /// </summary>
        private readonly IPlayer[] players;
        
        /// <summary>
        /// Parameterized constructor that receives 2 IPlayers to play the game.
        /// </summary>
        /// <param name="player1">Player who will start the game.</param>
        /// <param name="player2">Secon player.</param>
        public GameManager(IPlayer player1, IPlayer player2)
        {
            players = new IPlayer[2];
            players[0] = player1;
            players[1] = player2;

            turn = players[0].Name;
        }

        /// <summary>
        /// Starts the game with a little opening message in Console.
        /// </summary>
        public void PlayOpening() 
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("SEA BATTLE BOARD GAME");
            Console.Write("You will play with POSEIDON - the god of the sea, storms, earthquakes and horses.\nReady to start the game? Press any key to continue...");
            Console.ReadLine();
            Console.Clear();
        }

        /// <summary>
        /// Starts the Game and creates a fleet for players.
        /// </summary>
        public void StartGame()
        {
            PlayOpening();

            DrawInitialBoard(players[0]);
            for (int i = 4; i > 0; i--)
            {
                for (int j = 1; j <= 5 - i; j++)
                {
                    players[0].CreateBoat(j, i);
                    players[1].CreateBoat(j, i);

                    Console.Clear();
                    DrawInitialBoard(players[0]);
                }
            }
        }

        /// <summary>
        /// Runs the game manager.
        /// </summary>
        public void Run()
        {
            StartGame();

            Console.Clear();
            while (!IsGameOver())
            {
                Play();
            }

            Console.Clear();
            if (turn == players[0].Name)
            {
                DrawBoardOfTargetedCells(players[1]);
                Console.WriteLine($"{players[1].Name} wins!");
            }
            else 
            {
                DrawBoardOfTargetedCells(players[0]);
                Console.WriteLine($"{players[0].Name} wins!");
            }  
        }

        /// <summary>
        /// Plays the game with players and gives turns.
        /// </summary>
        public void Play()
        {
            if (turn == players[0].Name)
            {
                Console.Clear();
                DrawBoardOfTargetedCells(players[0]);
                DrawBoard(players[0]);

                Cell target = players[0].GetTargetedCell();
                players[1].CheckTargetedCell(target);

                turn = players[1].Name;
            }
            else
            {
                Cell target = players[1].GetTargetedCell();
                players[0].CheckTargetedCell(target);

                turn = players[0].Name;
            }
        }

        /// <summary>
        /// Draws in Console the complete board of player with the fleet placement and opponent's hit results.
        /// </summary>
        /// <param name="player">Player whos fleet shoud be displayed.</param>
        private void DrawBoard(IPlayer player)
        {
            Board board = player.BoardManager.Board;
            var blowPastCells = player.BoardManager.BlowPastCells;

            int top = -1;
            Console.SetCursorPosition(60, ++top);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{player.Name}'s fleet\n");
            Console.SetCursorPosition(60, ++top);
            Console.Write("     A   B   C   D   E   F   G   H   I   J\n");
            Console.SetCursorPosition(60, ++top);
            Console.Write("   +---+---+---+---+---+---+---+---+---+---+\n");
            Console.SetCursorPosition(60, ++top);

            for (int i = 1; i <= 10; i++)
            {
                if (i % 10 != 0)
                {
                    Console.Write($" {i} |");
                }
                else
                {
                    Console.Write($"{i} |");
                }

                for (int j = 1; j <= 10; j++)
                {
                    string cellString = ConvertIndexesToBoardCells(i, j);
                    if (board[cellString] != null)
                    {
                        List<Cell> boatCells = board[cellString].BoatCells;
                        Cell boatCell = boatCells.GetCell(cellString);
                        if (boatCell.Status == 2)
                        {
                            if (board[cellString].Status == 2)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                            }
                            else 
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" X ");
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.Write("   ");

                        }

                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("|");
                    }
                    else if (blowPastCells.ContainsCell(cellString))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" * ");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("|");
                    }
                    else
                    {
                        Console.Write("   |");
                    }
                }

                Console.Write("\n");
                Console.SetCursorPosition(60, ++top);
                Console.Write("   +---+---+---+---+---+---+---+---+---+---+\n");
                Console.SetCursorPosition(60, ++top);
            }
        }

        /// <summary>
        /// Draws in Console all the targeted Cell results by imitating opponents board.
        /// </summary>
        /// <param name="player"></param>
        private void DrawBoardOfTargetedCells(IPlayer player)
        {
            var targetedCells = player.TargetedCells;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{player.Name}'s hit results");
            Console.Write("     A   B   C   D   E   F   G   H   I   J\n");
            Console.Write("   +---+---+---+---+---+---+---+---+---+---+\n");

            for (int i = 1; i <= 10; i++)
            {
                if (i % 10 != 0)
                {
                    Console.Write($" {i} |");
                }
                else
                {
                    Console.Write($"{i} |");
                }

                for (int j = 1; j <= 10; j++)
                {
                    string cellString = ConvertIndexesToBoardCells(i, j);
                    if (targetedCells.ContainsCell(cellString))
                    {
                        Cell currentCell = targetedCells.GetCell(cellString);
                        if (currentCell.Status == 2 || currentCell.Status == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(" X ");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(" * ");
                        }

                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("|");
                    }
                    else
                    {
                        Console.Write("   |");
                    }
                }

                Console.Write("\n");
                Console.Write("   +---+---+---+---+---+---+---+---+---+---+\n");
            }
        }

        /// <summary>
        /// Draws in Console the initial fleet board for player to add boats.
        /// </summary>
        /// <param name="player">Player that adds boats to its fleet.</param>
        private void DrawInitialBoard(IPlayer player)
        {
            Board board = player.BoardManager.Board;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{player.Name}, create your fleet.\n");
            Console.Write("     A   B   C   D   E   F   G   H   I   J\n");
            Console.Write("   +---+---+---+---+---+---+---+---+---+---+\n");
            for (int i = 1; i <= 10; i++)
            {
                if (i % 10 != 0)
                {
                    Console.Write($" {i} |");
                }
                else
                {
                    Console.Write($"{i} |");
                }

                for (int j = 1; j <= 10; j++)
                {
                    string cellString = ConvertIndexesToBoardCells(i, j);
                    if (board[cellString] != null)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.Write("   ");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("|");
                    }
                    else
                    {
                        Console.Write("   |");
                    }
                }

                Console.Write("\n");
                Console.Write("   +---+---+---+---+---+---+---+---+---+---+\n");
            }
        }

        /// <summary>
        /// Helper method to convert i and j indexes to Sea Battle Board game cells.
        /// </summary>
        /// <param name="indexI">The first index that has values from 1 to 10.</param>
        /// <param name="indexJ">The second index that has values from 1 to 10.</param>
        /// <returns>String representation of Cell coordinates.</returns>
        private string ConvertIndexesToBoardCells(int indexI, int indexJ)
        {
            char letter = (char)(indexJ + 64);

            return letter.ToString() + indexI.ToString();
        }

        /// <summary>
        /// Checks if the game is over: if the fleet of one of the players is destroyed.
        /// </summary>
        /// <returns>True is the game is over, False if the game still continues.</returns>
        private bool IsGameOver()
        {
            if (turn == players[0].Name)
            {
                return players[0].IsFloatDestroyed();
            }
            else
            {
                return players[1].IsFloatDestroyed();
            }
        }
    }
}
