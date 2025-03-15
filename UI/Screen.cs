using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeAndLadders.UI.UIContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.UI
{
    public abstract class Screen
    {
        protected readonly GraphicsContext _graphicsMetaData;
        protected readonly UIEventManager _uIEventManager;
        protected readonly Stack<UIContainer> _uiContainers;
        public Screen(GraphicsContext graphicsMetaData)
        {
            _graphicsMetaData = graphicsMetaData;
            _uIEventManager = new UIEventManager();
            _uiContainers = new Stack<UIContainer>();
        }

        /// <summary>
        /// Gets UI Elements only for the top level container
        /// </summary>
        /// <returns></returns>
        protected List<UIElement> GetUIElementsFromContainers()
        {
            List<UIElement> uIElements = new List<UIElement>();
            UIContainer topContainer = _uiContainers.Peek();

            foreach (var item in topContainer.Children)
            {
                uIElements.Add(item);
            }

            return uIElements;
        }

        public virtual void Draw()
        {
            Stack<UIContainer> tempContainers = new Stack<UIContainer>();
            foreach (var container in _uiContainers)
            {
                tempContainers.Push(container);
            }

            while (tempContainers.Count > 0)
            {
                tempContainers.Pop().Draw();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                UIEvent mouseEvent = new UIEvent
                {
                    KeyPressed = Keys.None,
                    MousePosition = new Vector2(mouseState.Position.X, mouseState.Position.Y),
                    Type = UIEventType.MouseClick
                };
                _uIEventManager.PushEvent(mouseEvent);
            }

            if (mouseState.LeftButton == ButtonState.Released)
            {
                UIEvent mouseEvent = new UIEvent
                {
                    KeyPressed = Keys.None,
                    MousePosition = new Vector2(mouseState.Position.X, mouseState.Position.Y),
                    Type = UIEventType.MouseRelease
                };
                _uIEventManager.PushEvent(mouseEvent);
            }

            Keys[] keys = keyboardState.GetPressedKeys();

            if (keys.Length > 0)
            {
                foreach (var key in keys)
                {
                    if (keyboardState.IsKeyDown(key))
                    {
                        UIEvent keyboardEvent = new UIEvent
                        {
                            KeyPressed = key,
                            MousePosition = new Vector2(mouseState.Position.X, mouseState.Position.Y),
                            Type = UIEventType.KeyboardPress
                        };
                        _uIEventManager.PushEvent(keyboardEvent);
                    }
                }
            }

            if (keyboardState.GetPressedKeyCount() == 0)
            {
                UIEvent keyboardEvent = new UIEvent
                {
                    KeyPressed = Keys.None,
                    MousePosition = new Vector2(mouseState.Position.X, mouseState.Position.Y),
                    Type = UIEventType.KeyboardRelease
                };
                _uIEventManager.PushEvent(keyboardEvent);
            }

            _uIEventManager.ProcessEvents(GetUIElementsFromContainers());

            foreach (var item in _uiContainers)
            {
                item.Update(gameTime);
            }
        }
    }
}
