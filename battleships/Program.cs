using System;
using System.Collections.Generic;
using System.Linq;

namespace battleships
{
    class Program
    {
        private static int GridSize = 10;
        private static string Battleship = "BATTLESHIP";
        private static string Destroyer = "DESTROYER";

        static void Main(string[] args)
        {
            /*
            bool random = true;
            while (random)
            {
                Console.Clear();
                RandomizeGrid();
                PrintGrid(gridCPU);
                Console.Write("Randomize grid (Enter Y/N) :  ");
                if (Console.ReadLine().ToLower() == "n")
                {
                    random = false;
                }
            }
            */
            bool end = false;
            string message = string.Empty;
            while (!end)
            {
                Console.Clear();
                RandomizeGrid();
                PrintGrid(gridPlayer);
                Console.WriteLine(message);
                if (!IsGameEnded())
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
                    else if (!int.TryParse(userGuess.Substring(1), out var rowNumber)
                             || rowNumber < 1 || rowNumber > 10)
                    {
                        message = "Guess must end with a valid row number (1-10)";
                    }
                    else
                    {
                        var colNumber = userGuess[0] - 65;
                        rowNumber--;
                        if (char.IsLetter(gridCPU[rowNumber, colNumber]) &&
                            gridPlayer[rowNumber, colNumber] != 'H' &&
                            gridPlayer[rowNumber, colNumber] != 'M')
                        {
                            message = "HIT!";
                            gridPlayer[rowNumber, colNumber] = 'H';
                        }
                        else if (!char.IsLetter(gridCPU[rowNumber, colNumber]) &&
                                 gridPlayer[rowNumber, colNumber] != 'H' &&
                                 gridPlayer[rowNumber, colNumber] != 'M')
                        {
                            message = "MISS!";
                            gridPlayer[rowNumber, colNumber] = 'M';
                        }
                        else if (gridPlayer[rowNumber, colNumber] == 'H')
                        {
                            message = "Grid position already hit";
                        }
                        else if (gridPlayer[rowNumber, colNumber] == 'M')
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

        private static bool IsGameEnded()
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int a = 0; a < GridSize; a++)
                {
                    if (char.IsLetter(gridCPU[i, a]) && gridPlayer[i, a] != 'H')
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static void RandomizeGrid()
        {
            /*
            //initialise grid
            for (int i = 0; i < GridSize; i++)
            {
                for (int a = 0; a < GridSize; a++)
                {
                    gridCPU[i, a] = '.';
                }
            }
            */
            Random rnd = new Random();
            List<string> ships = new List<string> { Battleship, Destroyer, Destroyer };
            Dictionary<string, int> shipSize = new Dictionary<string, int> {{ Battleship, 5}, { Destroyer, 4}};
            foreach (var ship in ships)
            {
                var vertical = (rnd.Next(0, 100) > 50); //randomly choose orientation
                if (vertical)
                {
                    bool success = false;
                    while (!success)
                    {
                        int x = rnd.Next(0, 10);
                        int y = rnd.Next(0, 10 - shipSize[ship] + 1);
                        bool createShip = true;
                        for (int i = 0; i < shipSize[ship]; i++)
                        {
                            if (char.IsLetter(gridCPU[x, y + i]))
                                createShip = false;
                        }
                        if (createShip)
                        {
                            for (int i = 0; i < shipSize[ship]; i++)
                            {
                                gridCPU[x, y + i] = ship[0];
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
                        int x = rnd.Next(0, 10 - shipSize[ship] + 1);
                        int y = rnd.Next(0, 10);
                        bool createShip = true;
                        for (int i = 0; i < shipSize[ship]; i++)
                        {
                            if (char.IsLetter(gridCPU[x + i, y]))
                                createShip = false;
                        }
                        if (createShip)
                        {
                            for (int i = 0; i < shipSize[ship]; i++)
                            {
                                gridCPU[x + i, y] = ship[0];
                            }
                            success = true;
                        }
                    }
                }
            }
        }

        private static char[,] gridCPU = {
        {'.', '.', '.', '.', '.', '.', '.', '.', '.', '.'},
        {'.', '.', '.', '.', '.', '.', '.', '.', '.', '.'},
        {'.', '.', '.', '.', '.', '.', '.', '.', '.', '.'},
        {'.', '.', '.', '.', '.', '.', '.', '.', '.', '.'},
        {'.', '.', '.', '.', '.', '.', '.', '.', '.', '.'},
        {'.', '.', '.', '.', '.', '.', '.', '.', '.', '.'},
        {'.', '.', '.', '.', '.', '.', '.', '.', '.', '.'},
        {'.', '.', '.', '.', '.', '.', '.', '.', '.', '.'},
        {'.', '.', '.', '.', '.', '.', '.', '.', '.', '.'},
        {'.', '.', '.', '.', '.', '.', '.', '.', '.', '.'}};

        private static char[,] gridPlayer = {
        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}};

        public static void PrintGrid(char[,] grid)
        {
            Console.WriteLine("     |  A  |  B  |  C  |  D  |  E  |  F  |  G  |  H  |  I  |  J   |");
            Console.WriteLine("-----#-----#-----#-----#-----#-----#-----#-----#-----#-----#------#");

            for (var i = 0; i < 10; i++)
            {
                Console.Write(i == 9 ? $" {i + 1} " : $" {i + 1}  ");
                for (var j = 0; j < 10; j++)
                {
                    ShipColors(grid[i, j]);
                }
                
                Console.Write("  |\r\n");
                Console.WriteLine("-----#-----#-----#-----#-----#-----#-----#-----#-----#-----#------#");
            }
        }
        
        public static void ShipColors(char useThis)
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
                //case '.':
                case ' ':
                    Console.Write(" | ");
                    Console.Write("   ");
                    break;
                //case 'D':
                //    Console.Write(" | ");
                //    Console.BackgroundColor = ConsoleColor.Blue;
                //    Console.Write(" D ");
                //    break;
                //case 'B':
                //    Console.Write(" | ");
                //    Console.BackgroundColor = ConsoleColor.Green;
                //    Console.ForegroundColor = ConsoleColor.Black;
                //    Console.Write(" B ");
                //    break;
                case 'H':
                    Console.Write(" | ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" X ");
                    break;
                case 'M':
                    Console.Write(" | ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" X ");
                    break;
            }
            Console.ResetColor();
        }
    }
}
