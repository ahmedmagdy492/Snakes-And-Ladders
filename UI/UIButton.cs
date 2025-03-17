using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeAndLadders.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SnakeAndLadders.UI
{
    public class UIButton : UIElement
    {
        private Texture2D _texture;
        private bool _isClickEventOn = false;
        private Rectangle boundingRect;

        public event Action<UIElement, UIEvent> OnClick;

        public string Text { get; set; }
        public Color TextColor { get; set; }
        public SpriteFont Font { get; set; }

        public UIButton(GraphicsContext graphicsMetaData, string text) : base(graphicsMetaData)
        {
            Text = text;
            Background = Color.White;
            Padding = new Padding(10);
            TextColor = Color.Black;
            Font = graphicsMetaData.Font;
            Position = new Vector2(0, 0);
            double width = Padding.left + Padding.right + Font.MeasureString(text).X;
            double height = Padding.top + Padding.bottom + Font.MeasureString(text).Y;
            Size = new Vector2((float)width, (float)height);
            _texture = new Texture2D(_graphicsMetaData.SpriteBatch.GraphicsDevice, 1, 1);
            _texture.SetData([Color.White]);
            boundingRect = new Rectangle(new Point((int)Position.X, (int)Position.Y), new Point((int)Size.X, (int)Size.Y));
        }

        public override void Draw()
        {
            _graphicsMetaData.SpriteBatch.Draw(_texture, new Rectangle(new Point((int)Position.X, (int)Position.Y), new Point((int)Size.X, (int)Size.Y)), Background);
            _graphicsMetaData.SpriteBatch.DrawString(Font, Text, new Vector2((float)Padding.left + Position.X, (float)Padding.top + Position.Y), TextColor);
        }

        public override void HandleEvent(UIEvent e)
        {
            if(!IsEnabled)
            {
                return;
            }

            switch(e.Type)
            {
                case UIEventType.MouseClick:
                    if(GemotryUtil.IsPointWithinRect(e.MousePosition, boundingRect) && !_isClickEventOn)
                    {
                        if(OnClick != null)
                        {
                            OnClick(this, e);
                        }
                        _isClickEventOn = true;
                    }
                    break;
                case UIEventType.MouseRelease:
                    if(_isClickEventOn)
                    {
                        _isClickEventOn = false;
                    }
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
