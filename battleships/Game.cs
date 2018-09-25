using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using battleships.Models;

namespace battleships
{
    class Game
    {
        private Grid gridCPU;
        private Grid gridPlayer;
        private const string Battleship = "BATTLESHIP";
        private const string Destroyer = "DESTROYER";
        private const int BattleshipSize = 5;
        private const int DestroyerSize = 4;
        private List<Ship> ships = new List<Ship>();
        private const char Hit = 'H';
        private const char Miss = 'M';

        public void Start()
        {
            gridCPU = new Grid();
            gridPlayer = new Grid(false);
            ships = CreateShips();

            bool random = true;
            while (random)
            {
                Console.Clear();
                RandomizeGrid();
                PrintGrid(gridCPU);
                Console.Write("Randomize CPU grid (Enter Y/N) :  ");
                random = !(Console.ReadLine().ToLower() == "n");
            }
            bool end = false;
            string message = string.Empty;
            while (!end)
            {
                Console.Clear();
                RandomizeGrid();
                PrintGrid(gridPlayer);
                Console.WriteLine(message);
                if (!HasGameEnded())
                {
                    message = string.Empty;
                    Console.Write("Enter your guess: ");
                    var userGuess = Console.ReadLine()?.ToUpper();
                    if (userGuess == "EXIT")
                        return;

                    if (userGuess.Length < 2 || userGuess.Length > 3)
                    {
                        message = "Guess must be in the form of ColumnRow, e.g: B5";
                    }
                    else if (!"ABCDEFGHIJ".Contains(userGuess[0]))
                    {
                        message = "Guess must begin with a valid column letter (A-J)";
                    }
                    else if (!int.TryParse(userGuess.Substring(1), out var row) || row < 1 || row > 10)
                    {
                        message = "Guess must end with a valid row number (1-10)";
                    }
                    else
                    {
                        var col = userGuess[0] - 65; //due to formatting
                        row--;
                        if (char.IsLetter(gridCPU.grid[row, col]) &&
                            gridPlayer.grid[row, col] != Hit &&
                            gridPlayer.grid[row, col] != Miss)
                        {
                            message = "HIT!";
                            gridPlayer.grid[row, col] = Hit;
                        }
                        else if (!char.IsLetter(gridCPU.grid[row, col]) &&
                                 gridPlayer.grid[row, col] != Hit &&
                                 gridPlayer.grid[row, col] != Miss)
                        {
                            message = "MISS!";
                            gridPlayer.grid[row, col] = Miss;
                        }
                        else if (gridPlayer.grid[row, col] == Hit)
                        {
                            message = "Grid position already hit";
                        }
                        else if (gridPlayer.grid[row, col] == Miss)
                        {
                            message = "Grid position already missed";
                        }
                    }
                }
                else
                {
                    end = true;
                    Console.WriteLine("All ships sunk - Game Over");
                }
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public List<Ship> CreateShips()
        {
            List<Ship> shipList = new List<Ship>();
            Ship ship = new Ship();
            ship.Name = Battleship;
            ship.Size = BattleshipSize;
            shipList.Add(ship);
            ship = new Ship();
            ship.Name = Destroyer;
            ship.Size = DestroyerSize;
            shipList.Add(ship);
            ship = new Ship();
            ship.Name = Destroyer;
            ship.Size = DestroyerSize;
            shipList.Add(ship);
            return shipList;
        }

        private void RandomizeGrid()
        {
            //initialise grid
            gridCPU.Initialise();
            
            Random rnd = new Random();
            
            foreach (var ship in ships)
            {
                var vertical = (rnd.Next(0, 100) > 50); //randomly choose orientation
                if (vertical)
                {
                    bool success = false;
                    while (!success)
                    {
                        int x = rnd.Next(0, 10);
                        int y = rnd.Next(0, 10 - ship.Size + 1);
                        bool createShip = true;
                        for (int i = 0; i < ship.Size; i++)
                        {
                            if (char.IsLetter(gridCPU.grid[x, y + i]))
                                createShip = false;
                        }
                        if (createShip)
                        {
                            for (int i = 0; i < ship.Size; i++)
                            {
                                gridCPU.grid[x, y + i] = ship.Name[0];
                            }
                            success = true;
                        }
                    }
                }
                else
                {
                    bool success = false;
                    while (!success)
                    {
                        int x = rnd.Next(0, 10 - ship.Size + 1);
                        int y = rnd.Next(0, 10);
                        bool createShip = true;
                        for (int i = 0; i < ship.Size; i++)
                        {
                            if (char.IsLetter(gridCPU.grid[x + i, y]))
                                createShip = false;
                        }
                        if (createShip)
                        {
                            for (int i = 0; i < ship.Size; i++)
                            {
                                gridCPU.grid[x + i, y] = ship.Name[0];
                            }
                            success = true;
                        }
                    }
                }
            }
        }

        public void PrintGrid(Grid grid)
        {
            Console.WriteLine("     |  A  |  B  |  C  |  D  |  E  |  F  |  G  |  H  |  I  |  J   |");
            Console.WriteLine("-----#-----#-----#-----#-----#-----#-----#-----#-----#-----#------#");

            for (var x = 0; x < grid.Size; x++)
            {
                Console.Write(x == 9 ? $" {x + 1} " : $" {x + 1}  ");
                for (var y = 0; y < 10; y++)
                {
                    Colorize(grid.grid[x, y]);
                }
                Console.Write("  |\r\n");
                Console.WriteLine("-----#-----#-----#-----#-----#-----#-----#-----#-----#-----#------#");
            }
        }

        public static void Colorize(char useThis)
        {
            /*
             * Legend:
             * B = Battleship
             * D = Destroyer
             * H = Hit
             * M = Miss
             */
            switch (useThis)
            {
                case '.':
                case ' ':
                    Console.Write(" | ");
                    Console.Write("   ");
                    break;
                case 'D':
                    Console.Write(" | ");
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write(" D ");
                    break;
                case 'B':
                    Console.Write(" | ");
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" B ");
                    break;
                case Hit:
                    Console.Write(" | ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" X ");
                    break;
                case Miss:
                    Console.Write(" | ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" X ");
                    break;
            }
            Console.ResetColor();
        }

        private bool HasGameEnded()
        {
            for (int y = 0; y < gridCPU.Size; y++)
            {
                for (int x = 0; x < gridCPU.Size; x++)
                {
                    if (char.IsLetter(gridCPU.grid[x, y]) && gridPlayer.grid[x, y] != Hit)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
