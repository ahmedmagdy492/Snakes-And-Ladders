using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.UI
{
    public class UILabel : UIElement
    {
        public string Text { get; set; }
        public Color TextColor { get; set; }
        public SpriteFont Font { get; set; }

        public UILabel(GraphicsContext graphicsMetaData, string text) : base(graphicsMetaData)
        {
            Text = text;
            Background = Color.White;
            Padding = new Padding(0);
            TextColor = Color.Black;
            Font = _graphicsMetaData.Font;
            Position = new Vector2(0, 0);
            Vector2 fontSize = graphicsMetaData.Font.MeasureString(text);
            Size = new Vector2(fontSize.X, fontSize.Y);
        }

        public override void Draw()
        {
            _graphicsMetaData.SpriteBatch.DrawString(Font, Text, Position, TextColor);
        }

        public override void HandleEvent(UIEvent e)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
