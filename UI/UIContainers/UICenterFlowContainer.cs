using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeAndLadders.Helpers;
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
        public bool HasBackgroundImage { get; set; } = false;

        public UICenterFlowContainer(GraphicsContext graphicsMetaData, bool hasBackgroundImage) : base(graphicsMetaData)
        {
            HasBackgroundImage = hasBackgroundImage;
            Position = new Vector2(0, 0);
            Margin = new Padding(0);
            Border = new Border
            {
                width =  2,
                color = Color.Black
            };
            Background = new Color(0x00, 0x00, 0x00, 0x00);

            if (hasBackgroundImage)
            {
                _backgroundTexture = _graphicsMetaData.ContentManager.Load<Texture2D>("window_2");
            }
            else
            {
                _borderTexture = new Texture2D(graphicsMetaData.GraphicsDeviceManager.GraphicsDevice, 1, 1);
                _borderTexture.SetData([Border.color]);
                _backgroundTexture = new Texture2D(graphicsMetaData.GraphicsDeviceManager.GraphicsDevice, 1, 1);
                _backgroundTexture.SetData([Background]);
            }
        }

        public void ChangeBackground(Color newColor)
        {
            if (!HasBackgroundImage)
            {
                if (_backgroundTexture != null)
                {
                    _backgroundTexture.SetData([newColor]);
                }
            }
        }

        public override void Draw()
        {
            // drawing the background
            _graphicsMetaData.SpriteBatch.Draw(_backgroundTexture, new Rectangle(Position.ToPoint(), new Point(GetWidth(), GetHeight())), Color.White);

            Vector2 refPosition = new Vector2(Position.X, Position.Y);
            int xi = 0, yi = 0;
            int containerWidth = GetWidth(), containerHeight = GetHeight();
            foreach (var item in Children)
            {
                if(FlowDirection == UIFlowContainerDirection.TopToBottom)
                {
                    if(item is UIContainer)
                    {
                        item.Position = new Vector2(refPosition.X + ((containerWidth - ((UIContainer)item).GetWidth()) / 2) + xi, refPosition.Y + yi);
                        yi += ((UIContainer)item).GetHeight() + Margin.top.ToInt() + Margin.bottom.ToInt();
                    }
                    else
                    {
                        item.Position = new Vector2(refPosition.X + ((containerWidth - item.Size.X.ToInt()) / 2) + xi, refPosition.Y + yi);
                        yi += item.Size.Y.ToInt() + Margin.top.ToInt() + Margin.bottom.ToInt();
                    }
                }
                else if(FlowDirection == UIFlowContainerDirection.RightToLeft)
                {
                    item.Position = new Vector2(refPosition.X + xi, refPosition.Y + yi);
                    if(item is UIContainer)
                    {
                        xi += ((UIContainer)item).GetWidth() + Margin.left.ToInt() + Margin.right.ToInt();
                    }
                    else
                    {
                        xi += item.Size.X.ToInt() + Margin.left.ToInt() + Margin.right.ToInt();
                    }
                }

                item.Draw();
            }

            if(!HasBackgroundImage && Border.width > 0)
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
            foreach (var item in Children)
            {
                item.HandleEvent(e);
            }
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
                    if(item is UIContainer)
                    {
                        totalWidth += ((UIContainer)item).GetWidth() + Margin.left.ToInt() + Margin.right.ToInt();
                    }
                    else
                    {
                        totalWidth += (int)item.Size.X + Margin.left.ToInt() + Margin.right.ToInt();
                    }
                }
            }
            else if(FlowDirection == UIFlowContainerDirection.TopToBottom)
            {
                int maxWidth = 0;
                foreach (var item in Children)
                {
                    if(item is UIContainer)
                    {
                        if (((UIContainer)item).GetWidth() > maxWidth)
                        {
                            maxWidth = ((UIContainer)item).GetWidth();
                        }
                    }
                    else
                    {
                        if (item.Size.X.ToInt() > maxWidth)
                        {
                            maxWidth = item.Size.X.ToInt();
                        }
                    }
                }

                return maxWidth + Margin.left.ToInt() + Margin.right.ToInt() + Border.width;
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
                    if(item is UIContainer)
                    {
                        totalHeight += ((UIContainer)item).GetHeight() + Margin.top.ToInt() + Margin.bottom.ToInt();
                    }
                    else
                    {
                        totalHeight += item.Size.Y.ToInt() + Margin.top.ToInt() + Margin.bottom.ToInt();
                    }
                }
            }
            else if (FlowDirection == UIFlowContainerDirection.RightToLeft)
            {
                int maxHeight = 0;
                foreach (var item in Children)
                {
                    if(item is UIContainer)
                    {
                        if (((UIContainer)item).GetHeight() > maxHeight)
                        {
                            maxHeight = ((UIContainer)item).GetHeight();
                        }
                    }
                    else
                    {
                        if (item.Size.Y.ToInt() > maxHeight)
                        {
                            maxHeight = item.Size.Y.ToInt();
                        }
                    }
                }

                return maxHeight + Margin.top.ToInt() + Margin.bottom.ToInt() + Border.width;
            }

            return totalHeight + Border.width;
        }
    }
}
