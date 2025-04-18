﻿using Microsoft.Xna.Framework;
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

        public event Action<UIElement, UIEvent> OnClick;

        public string Text { get; set; }
        public Color TextColor { get; set; }
        public SpriteFont Font { get; set; }

        public UIButton(GraphicsContext graphicsMetaData, string text) : base(graphicsMetaData)
        {
            Text = text;
            Background = Color.White;
            Padding = new Padding(20);
            TextColor = new Color(0x99, 0x66, 0x33);
            Font = graphicsMetaData.Font;
            Position = new Vector2(0, 0);
            double width = Padding.left + Padding.right + Font.MeasureString(text).X;
            double height = Padding.top + Padding.bottom + Font.MeasureString(text).Y;
            Size = new Vector2((float)width, (float)height);
            _texture = _graphicsMetaData.ContentManager.Load<Texture2D>("btn_1");
        }

        public override void Draw()
        {
            if(!_isClickEventOn)
            {
                _graphicsMetaData.SpriteBatch.Draw(_texture, new Rectangle(Position.ToPoint(), Size.ToPoint()), Background);
                _graphicsMetaData.SpriteBatch.DrawString(Font, Text, new Vector2(Padding.left + Position.X, Padding.top + Position.Y), TextColor);
            }
            else
            {
                _graphicsMetaData.SpriteBatch.Draw(_texture, new Rectangle(Position.ToPoint(), Size.ToPoint()), TextColor);
                _graphicsMetaData.SpriteBatch.DrawString(Font, Text, new Vector2(Padding.left + Position.X, Padding.top + Position.Y), Background);
            }
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
                    if(GemotryUtil.IsPointWithinRect(e.MousePosition, new Rectangle(Position.ToPoint(), Size.ToPoint())) && !_isClickEventOn)
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
