using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.UI.UIContainers
{
    public enum UIFlowContainerDirection
    {
        RightToLeft,
        TopToBottom
    }

    public struct Border
    {
        public int width;
        public Color color;
    }

    public class UICenterFlowContainer : UIContainer
    {
        private Texture2D _borderTexture;
        private Texture2D _backgroundTexture;
        public UIFlowContainerDirection FlowDirection { get; set; } = UIFlowContainerDirection.TopToBottom;
        public bool Scrollable { get; set; } = false;
        public Padding Margin { get; set; }
        public Border Border { get; set; }

        public UICenterFlowContainer(GraphicsContext graphicsMetaData) : base(graphicsMetaData)
        {
            Position = new Vector2(0, 0);
            Margin = new Padding(0);
            Border = new Border
            {
                width =  2,
                color = Color.Black
            };
            Background = _graphicsMetaData.ClearColor;

            _borderTexture = new Texture2D(graphicsMetaData.GraphicsDeviceManager.GraphicsDevice, 1, 1);
            _borderTexture.SetData([Border.color]);
            _backgroundTexture = new Texture2D(graphicsMetaData.GraphicsDeviceManager.GraphicsDevice, 1, 1);
            _backgroundTexture.SetData([Background]);
        }

        public override void Draw()
        {
            // drawing the background
            _graphicsMetaData.SpriteBatch.Draw(_backgroundTexture, new Rectangle((int)Position.X, (int)Position.Y, GetWidth(), GetHeight()), Color.White);

            Vector2 refPosition = new Vector2(Position.X, Position.Y);
            int xi = 0, yi = 0;
            int containerWidth = GetWidth(), containerHeight = GetHeight();
            foreach (var item in Children)
            {
                if(FlowDirection == UIFlowContainerDirection.TopToBottom)
                {
                    item.Position = new Vector2(refPosition.X + ((containerWidth - (int)item.Size.X) / 2) + xi, refPosition.Y + yi);
                    yi += (int)item.Size.Y + (int)Margin.top + (int)Margin.bottom;
                }
                else if(FlowDirection == UIFlowContainerDirection.RightToLeft)
                {
                    item.Position = new Vector2(refPosition.X + xi, refPosition.Y + yi);
                    xi += (int)item.Size.X + (int)Margin.left + (int)Margin.right;
                }

                item.Draw();
            }

            if(Border.width > 0)
            {
                // top
                _graphicsMetaData.SpriteBatch.Draw(_borderTexture, new Rectangle((int)Position.X, (int)Position.Y, GetWidth(), Border.width), Border.color);

                //// Bottom
                _graphicsMetaData.SpriteBatch.Draw(_borderTexture, new Rectangle((int)Position.X, (int)Position.Y + GetHeight() - Border.width, GetWidth(), Border.width), Border.color);

                //// Left
                _graphicsMetaData.SpriteBatch.Draw(_borderTexture, new Rectangle((int)Position.X, (int)Position.Y, Border.width, GetHeight()), Border.color);

                //// Right
                _graphicsMetaData.SpriteBatch.Draw(_borderTexture, new Rectangle((int)Position.X + GetWidth() - Border.width, (int)Position.Y, Border.width, GetHeight()), Border.color);
            }
        }
        public override void HandleEvent(UIEvent e)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach(var item in Children)
            {
                item.Update(gameTime);
            }
        }

        public override int GetWidth()
        {
            int totalWidth = 0;
            if(FlowDirection == UIFlowContainerDirection.RightToLeft)
            {
                foreach (var item in Children)
                {
                    totalWidth += (int)item.Size.X + (int)Margin.left + (int)Margin.right;
                }
            }
            else if(FlowDirection == UIFlowContainerDirection.TopToBottom)
            {
                int maxWidth = 0;
                foreach (var item in Children)
                {
                    if((int)item.Size.X > maxWidth)
                    {
                        maxWidth = (int)item.Size.X;
                    }
                }

                return maxWidth + (int)Margin.left + (int)Margin.right + Border.width;
            }

            return totalWidth + Border.width;
        }

        public override int GetHeight()
        {
            int totalHeight = 0;
            if (FlowDirection == UIFlowContainerDirection.TopToBottom)
            {
                foreach (var item in Children)
                {
                    totalHeight += (int)item.Size.Y + (int)Margin.top + (int)Margin.bottom;
                }
            }
            else if (FlowDirection == UIFlowContainerDirection.RightToLeft)
            {
                int maxHeight = 0;
                foreach (var item in Children)
                {
                    if ((int)item.Size.Y > maxHeight)
                    {
                        maxHeight = (int)item.Size.Y;
                    }
                }

                return maxHeight + (int)Margin.top + (int)Margin.bottom + Border.width;
            }

            return totalHeight + Border.width;
        }
    }
}
