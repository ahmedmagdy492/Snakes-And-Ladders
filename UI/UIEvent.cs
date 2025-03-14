using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.UI
{
    public enum UIEventType
    {
        None = 0,
        MouseClick,
        MouseRelease,
        KeyboardPress,
        KeyboardRelease,
        TextInput
    }

    public class UIEvent
    {
        public UIEventType Type { get; set; }
        public Vector2 MousePosition { get; set; }
        public Keys KeyPressed { get; set; }
    }
}
