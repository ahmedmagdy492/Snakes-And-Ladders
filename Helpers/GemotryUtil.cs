using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.Helpers
{
    public static class GemotryUtil
    {
        public static bool IsPointWithinRect(Vector2 mousePosition, Rectangle rect)
        {
            return rect.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y));
        }
    }
}
