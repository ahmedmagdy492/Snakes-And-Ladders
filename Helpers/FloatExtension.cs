using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.Helpers
{
    public static class FloatExtension
    {
        public static int ToInt(this float value)
        {
            return (int)Math.Round(value);
        }
    }
}
