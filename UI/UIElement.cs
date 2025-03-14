using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.UI
{
    public struct Padding
    {
        public readonly double top;
        public readonly double bottom;
        public readonly double left;
        public readonly double right;

        public Padding(double top, double bottom, double left, double right)
        {
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;
        }

        public Padding(double vertical, double horizental) : this(vertical, vertical, horizental, horizental)
        {
        }

        public Padding(int all) : this(all, all)
        {
        }
    }

    public abstract class UIElement
    {
        public Vector2 Size { get; set; }
        public Color Background { get; set; }
        public Padding Padding { get; set; }
        public Vector2 Position { get; set; }

        protected GraphicsContext _graphicsMetaData;

        public UIElement(GraphicsContext graphicsMetaData)
        {
            _graphicsMetaData = graphicsMetaData;
        }

        public abstract void Draw();
        public abstract void Update(GameTime gameTime);
        public abstract void HandleEvent(UIEvent e);
    }
}
