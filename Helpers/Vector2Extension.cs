using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.Helpers
{
    public static class Vector2Extension
    {
        public static Point ToPoint(this Vector2 value)
        {
            return new Point(value.X.ToInt(), value.Y.ToInt());
        }
    }
}
