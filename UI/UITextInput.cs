using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SnakeAndLadders.UI
{
    public enum InputType
    {
        IP,
        Text,
        NumbersOnly
    }

    public class UITextInput : UIElement
    {
        private bool isKeyPressed = false;
        private bool isInFocus = false;
        private Texture2D _texture;
        private double blinkerTimer = 0;
        private bool isCursorVisable = true;
        public string Value { get; set; }
        public Color TextColor { get; set; }
        public SpriteFont Font { get; set; }
        public InputType InputType { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public UITextInput(GraphicsContext graphicsMetaData) : base(graphicsMetaData)
        {
            Background = Color.White;
            Padding = new Padding(10);
            TextColor = Color.White;
            Font = graphicsMetaData.Font;
            Position = new Vector2(0, 0);
            Width = 250;
            Height = (int)Font.MeasureString("A").Y + (int)Padding.top + (int)Padding.bottom;
            Value = "";
            Size = new Vector2(Width, Height);
            _texture = _graphicsMetaData.ContentManager.Load<Texture2D>("input_1"); //new Texture2D(_graphicsMetaData.SpriteBatch.GraphicsDevice, 1, 1);
            //_texture.SetData([Color.White]);
        }

        private bool IsClicked(Vector2 mousePosition)
        {
            var surroundingRect = new Rectangle(new Point((int)Position.X, (int)Position.Y), new Point((int)Size.X, (int)Size.Y));
            return surroundingRect.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y));
        }

        public override void Draw()
        {
            _graphicsMetaData.SpriteBatch.Draw(_texture, new Rectangle(new Point((int)Position.X, (int)Position.Y), new Point((int)Size.X, (int)Size.Y)), Background);
            if(Value.Length > 0)
            {
                _graphicsMetaData.SpriteBatch.DrawString(Font, Value, new Vector2((float)Padding.left + Position.X, (float)Padding.top + Position.Y), TextColor);
            }

            if(isInFocus)
            {
                if(isCursorVisable)
                {
                    Vector2 cursorPosition = new Vector2(Position.X + (int)Padding.left + Font.MeasureString(Value).X, Position.Y + (int)Padding.top);
                    _graphicsMetaData.SpriteBatch.DrawString(_graphicsMetaData.Font, "|", cursorPosition, Color.White);
                }
            }
        }

        public override void HandleEvent(UIEvent e)
        {
            switch(e.Type)
            {
                case UIEventType.MouseClick:
                    if(!isInFocus && IsClicked(e.MousePosition))
                    {
                        isInFocus = true;
                    }
                    if(!IsClicked(e.MousePosition))
                    {
                        isInFocus = false;
                    }
                    break;
                case UIEventType.KeyboardPress:
                    switch(InputType)
                    {
                        case InputType.IP:
                            if(isInFocus && !isKeyPressed)
                            {
                                if (Font.MeasureString(Value).X < (Width - Padding.right - Padding.left))
                                {
                                    if (char.IsDigit((char)e.KeyPressed))
                                    {
                                        Value += (char)e.KeyPressed;
                                    }
                                    if (e.KeyPressed == Microsoft.Xna.Framework.Input.Keys.OemPeriod)
                                    {
                                        Value += '.';
                                    }
                                }
                                if (e.KeyPressed == Microsoft.Xna.Framework.Input.Keys.Back)
                                {
                                    if (Value.Length > 0)
                                    {
                                        Value = Value.Remove(Value.Length - 1);
                                    }
                                }
                                isKeyPressed = true;
                            }
                            break;
                    }
                    break;
                case UIEventType.KeyboardRelease:
                        isKeyPressed = false;
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            blinkerTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if(blinkerTimer >= 0.5)
            {
                isCursorVisable = !isCursorVisable;
                blinkerTimer = 0;
            }
        }
    }
}
