using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.Models
{
    public class CellDetails
    {
        public int CellNo { get; set; }
        public bool HasSnake { get; set; }
        public bool HasLadder { get; set; }
        public int LadderEndCellNo { get; set; }
        public int SnakeEndCellNo { get; set; }
        public Vector2 Position { get; set; }
    }
}
