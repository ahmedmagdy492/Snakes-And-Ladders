using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeAndLadders.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.UI
{
    public struct Padding
    {
        public readonly float top;
        public readonly float bottom;
        public readonly float left;
        public readonly float right;

        public Padding(float top, float bottom, float left, float right)
        {
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;
        }

        public Padding(float vertical, float horizental) : this(vertical, vertical, horizental, horizental)
        {
        }

        public Padding(float all) : this(all, all)
        {
        }
    }

    public abstract class UIElement
    {
        public int Id { get; set; } = 0;
        public Vector2 Size { get; set; }
        public Color Background { get; set; }
        public Padding Padding { get; set; }
        public Vector2 Position { get; set; }
        public bool IsEnabled { get; set; } = true;

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
