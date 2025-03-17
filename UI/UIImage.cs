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
    public class UIImage : UIElement
    {
        private readonly Texture2D _texture;
        private bool _isClickEventOn = false;
        private Rectangle boundingRect;

        public string ImageUrl { get; set; }

        public event Action<UIElement, UIEvent> OnClick;

        public UIImage(GraphicsContext graphicsMetaData, string imgUrl) : base(graphicsMetaData)
        {
            _graphicsMetaData = graphicsMetaData;
            ImageUrl = imgUrl;
            _texture = graphicsMetaData.ContentManager.Load<Texture2D>(imgUrl);
            Size = new Vector2(_texture.Width, _texture.Height);
            Position = Vector2.Zero;
            boundingRect = new Rectangle(new Point((int)Position.X, (int)Position.Y), new Point((int)Size.X, (int)Size.Y));
            Background = Color.White;
        }

        public override void Draw()
        {
            _graphicsMetaData.SpriteBatch.Draw(_texture, Position, Background);
        }

        public override void HandleEvent(UIEvent e)
        {
            if (!IsEnabled)
            {
                return;
            }

            switch (e.Type)
            {
                case UIEventType.MouseClick:
                    if (GemotryUtil.IsPointWithinRect(e.MousePosition, boundingRect) && !_isClickEventOn)
                    {
                        if (OnClick != null)
                        {
                            OnClick(this, e);
                        }
                        _isClickEventOn = true;
                    }
                    break;
                case UIEventType.MouseRelease:
                    if (_isClickEventOn)
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
