using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeAndLadders.UI.UIContainers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.UI
{
    public class Screen
    {
        private readonly UIEventManager _uIEventManager;
        private readonly Stack<UIContainer> _uiContainers;
        private readonly GraphicsContext _graphicsMetaData;

        private void Init()
        {
            UILabel label = new UILabel(_graphicsMetaData, "Snakes And Ladders");
            UIButton vsComputerBtn = new UIButton(_graphicsMetaData, "VS Computer");
            vsComputerBtn.OnClick += PlayVersusComp_OnClick;
            
            UIButton playWithFriendBtn = new UIButton(_graphicsMetaData, "Play With Friend");
            playWithFriendBtn.OnClick += PlayWithFriendBtn_OnClick;

            UIButton createServerBtn = new UIButton(_graphicsMetaData, "Create a Server");

            UICenterFlowContainer mainContainer = new UICenterFlowContainer(_graphicsMetaData);
            mainContainer.Border = new Border { width = 0, color = Color.White };
            mainContainer.Margin = new Padding(top: 20, right: 0, left: 0, bottom: 0);
            mainContainer.Children.Add(label);
            mainContainer.Children.Add(vsComputerBtn);
            mainContainer.Children.Add(playWithFriendBtn);
            mainContainer.Children.Add(createServerBtn);
            mainContainer.Position = new Vector2((_graphicsMetaData.ScreenWidth - mainContainer.GetWidth()) / 2, 200);
            _uiContainers.Push(mainContainer);
        }

        private void PlayWithFriendBtn_OnClick(UIElement arg1, UIEvent arg2)
        {
            UICenterFlowContainer connectDialogBox = new UICenterFlowContainer(_graphicsMetaData);

            connectDialogBox.Background = Color.Wheat;
            connectDialogBox.Margin = new Padding(20);
            UILabel uILabel = new UILabel(_graphicsMetaData, "Enter Friend IP");
            UITextInput ipAddress = new UITextInput(_graphicsMetaData);
            
            UIButton connectButton = new UIButton(_graphicsMetaData, "Connect");
            UIButton closeButton = new UIButton(_graphicsMetaData, "Close");
            closeButton.OnClick += CloseButton_OnClick;

            connectDialogBox.Children.Add(uILabel);
            connectDialogBox.Children.Add(ipAddress);
            connectDialogBox.Children.Add(connectButton);
            connectDialogBox.Children.Add(closeButton);
            connectDialogBox.Position = new Vector2((_graphicsMetaData.ScreenWidth - connectDialogBox.GetWidth()) / 2, 200);
            _uiContainers.Push(connectDialogBox);
        }

        private void CloseButton_OnClick(UIElement arg1, UIEvent arg2)
        {
            _uiContainers.Pop();
        }

        private void PlayVersusComp_OnClick(UIElement arg1, UIEvent arg2)
        {
            Debug.WriteLine("hereeee");
        }

        public Screen(GraphicsContext graphicsMetaData)
        {
            _uIEventManager = new UIEventManager();
            _uiContainers = new Stack<UIContainer>();
            _graphicsMetaData = graphicsMetaData;

            Init();
        }

        /// <summary>
        /// Gets UI Elements only for the top level container
        /// </summary>
        /// <returns></returns>
        private List<UIElement> GetUIElementsFromContainers()
        {
            List<UIElement> uIElements = new List<UIElement>();
            UIContainer topContainer = _uiContainers.Peek();

            foreach (var item in topContainer.Children)
            {
                uIElements.Add(item);
            }

            return uIElements;
        }

        public void Draw()
        {
            Stack<UIContainer> tempContainers = new Stack<UIContainer>();
            foreach(var container in _uiContainers)
            {
                tempContainers.Push(container);
            }

            while(tempContainers.Count > 0)
            {
                tempContainers.Pop().Draw();
            }
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                UIEvent mouseEvent = new UIEvent
                {
                    KeyPressed = Keys.None,
                    MousePosition = new Vector2(mouseState.Position.X, mouseState.Position.Y),
                    Type = UIEventType.MouseClick
                };
                _uIEventManager.PushEvent(mouseEvent);
            }
            
            if(mouseState.LeftButton == ButtonState.Released)
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

            if(keys.Length > 0)
            {
                foreach(var key in keys)
                {
                    if(keyboardState.IsKeyDown(key))
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

            if(keyboardState.GetPressedKeyCount() == 0)
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

            foreach(var item in _uiContainers)
            {
                item.Update(gameTime);
            }
        }
    }
}
