using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace battleships.Models
{
    public class Grid
    {
        public int Size = 10;

        public char[,] grid { get; set; }
        private char symbol { get; set; }

        public Grid(bool cpu = true)
        {
            grid = new char[Size, Size];
            symbol = cpu ? '.' : ' ';
            Initialise();
        }

        public void Initialise()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    grid[x, y] = symbol;
                }
            }
        }
        
    }
}
