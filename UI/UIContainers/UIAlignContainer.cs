using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeAndLadders.Helpers;

namespace SnakeAndLadders.UI.UIContainers
{
    public enum ContainerAlignment
    {
        AlignLeft,
        AlignRight
    }

    public class UIAlignContainer : UIContainer
    {
        private Texture2D _borderTexture;
        private Texture2D _backgroundTexture;
        public ContainerAlignment Alignment { get; set; } = ContainerAlignment.AlignLeft;

        public UIAlignContainer(GraphicsContext graphicsMetaData) : base(graphicsMetaData)
        {
            Position = new Vector2(0, 0);
            Margin = new Padding(0);
            Border = new Border
            {
                width = 2,
                color = Color.Black
            };
            //Background = _graphicsMetaData.ClearColor;

            _borderTexture = new Texture2D(graphicsMetaData.GraphicsDeviceManager.GraphicsDevice, 1, 1);
            _borderTexture.SetData([Border.color]);
            _backgroundTexture = new Texture2D(graphicsMetaData.GraphicsDeviceManager.GraphicsDevice, 1, 1);
            _backgroundTexture.SetData([Background]);
        }

        public override void Draw()
        {
            // drawing the background
            _graphicsMetaData.SpriteBatch.Draw(_backgroundTexture, new Rectangle(Position.ToPoint(), new Point(GetWidth(), GetHeight())), Color.White);

            Vector2 refPosition = new Vector2(Position.X, Position.Y);
            int yi = 0;
            int containerWidth = GetWidth(), containerHeight = GetHeight();

            foreach (var item in Children)
            {
                if (Alignment == ContainerAlignment.AlignLeft)
                {
                    if (item is UIContainer)
                    {
                        item.Position = new Vector2(refPosition.X, refPosition.Y + yi);
                        yi += ((UIContainer)item).GetHeight() + Margin.top.ToInt() + Margin.bottom.ToInt();
                    }
                    else
                    {
                        item.Position = new Vector2(refPosition.X, refPosition.Y + yi);
                        yi += item.Size.Y.ToInt() + Margin.top.ToInt() + Margin.bottom.ToInt();
                    }
                }
                else if (Alignment == ContainerAlignment.AlignRight)
                {
                    if (item is UIContainer)
                    {
                        item.Position = new Vector2(refPosition.X + containerWidth - ((UIContainer)item).GetWidth(), refPosition.Y + yi);
                        yi += ((UIContainer)item).GetHeight() + Margin.top.ToInt() + Margin.bottom.ToInt();
                    }
                    else
                    {
                        item.Position = new Vector2(refPosition.X + containerWidth - item.Size.X, refPosition.Y + yi);
                        yi += item.Size.Y.ToInt() + Margin.top.ToInt() + Margin.bottom.ToInt();
                    }
                }

                item.Draw();
            }

            if (Border.width > 0)
            {
                // top
                _graphicsMetaData.SpriteBatch.Draw(_borderTexture, new Rectangle(Position.ToPoint(), new Point(GetWidth(), Border.width)), Border.color);

                //// Bottom
                _graphicsMetaData.SpriteBatch.Draw(_borderTexture, new Rectangle(Position.X.ToInt(), Position.Y.ToInt() + GetHeight() - Border.width, GetWidth(), Border.width), Border.color);

                //// Left
                _graphicsMetaData.SpriteBatch.Draw(_borderTexture, new Rectangle(Position.ToPoint(), new Point(Border.width, GetHeight())), Border.color);

                //// Right
                _graphicsMetaData.SpriteBatch.Draw(_borderTexture, new Rectangle(Position.X.ToInt() + GetWidth() - Border.width, Position.Y.ToInt(), Border.width, GetHeight()), Border.color);
            }
        }

        public override void HandleEvent(UIEvent e)
        {
            foreach (var item in Children)
            {
                item.HandleEvent(e);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var item in Children)
            {
                item.Update(gameTime);
            }
        }

        public override int GetWidth()
        {
            int maxWidth = 0;
            foreach (var item in Children)
            {
                if (item is UIContainer)
                {
                    if (((UIContainer)item).GetWidth() > maxWidth)
                    {
                        maxWidth = ((UIContainer)item).GetWidth();
                    }
                }
                else
                {
                    if ((int)item.Size.X > maxWidth)
                    {
                        maxWidth = (int)item.Size.X;
                    }
                }
            }

            return maxWidth + (int)Margin.left + (int)Margin.right + Border.width;
        }

        public override int GetHeight()
        {
            int totalHeight = 0;
            foreach (var item in Children)
            {
                if (item is UIContainer)
                {
                    totalHeight += ((UIContainer)item).GetHeight() + (int)Margin.top + (int)Margin.bottom;
                }
                else
                {
                    totalHeight += (int)item.Size.Y + (int)Margin.top + (int)Margin.bottom;
                }
            }

            return totalHeight + Border.width;
        }
    }
}
