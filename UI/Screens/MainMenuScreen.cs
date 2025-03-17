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

namespace SnakeAndLadders.UI.Screens
{
    public class MainMenuScreen : Screen
    {
        public MainMenuScreen(GraphicsContext graphicsMetaData) : base(graphicsMetaData)
        {
            Init();
        }

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
    }
}
