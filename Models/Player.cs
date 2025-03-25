using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.Models
{
    public class Player
    {
        public string PlayerName { get; set; }
        public Texture2D Texture { get; set; }
        public int CurrentCellNo { get; set; } = 1;
        public int MovingCellNo { get; set; } = 1;
        public Vector2 Position { get; set; }
    }
}
