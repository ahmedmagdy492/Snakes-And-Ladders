using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SnakeAndLadders.Helpers;
using SnakeAndLadders.UI.UIContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.UI.Screens
{
    public class CreateServerScreen : Screen
    {
        public CreateServerScreen(GraphicsContext graphicsMetaData) : base(graphicsMetaData)
        {
            Init();
        }

        private void Init()
        {
            UIButton gameOptionsBtn = new UIButton(_graphicsMetaData, "Game Options");
            UIAlignContainer mainContainer = new UIAlignContainer(_graphicsMetaData);
            mainContainer.Position = new Vector2(20, 20);
            mainContainer.Margin = new Padding(10);
            mainContainer.Border = new Border
            {
                width = 0,
                color = Color.White
            };

            UIFlexDimContainer connectedPlayersContainer = new UIFlexDimContainer(_graphicsMetaData);
            connectedPlayersContainer.Margin = new Padding(10);
            connectedPlayersContainer.FlowDirection = UIFlowContainerDirection.RightToLeft;
            UIButton youBtn = new UIButton(_graphicsMetaData, "Your computer");
            youBtn.Background = Color.GreenYellow;
            UIButton waitingBtn = new UIButton(_graphicsMetaData, "Wating for another player to connect ...");
            connectedPlayersContainer.Children.Add(youBtn);
            connectedPlayersContainer.Children.Add(waitingBtn);
            UIButton startGameButton = new UIButton(_graphicsMetaData, "Start Game");
            UIButton exitButton = new UIButton(_graphicsMetaData, "Exit");
            exitButton.OnClick += ExitButton_OnClick;

            UICenterFlowContainer bottomContainer = new UICenterFlowContainer(_graphicsMetaData);
            bottomContainer.Border = new Border
            {
                width = 0,
                color = Color.White
            };

            bottomContainer.Margin = new Padding(10);
            bottomContainer.FlowDirection = UIFlowContainerDirection.RightToLeft;

            bottomContainer.Children.Add(startGameButton);
            bottomContainer.Children.Add(exitButton);

            mainContainer.Children.Add(gameOptionsBtn);

            int yOffset = bottomContainer.GetHeight() + mainContainer.GetHeight() + bottomContainer.Margin.top.ToInt() + bottomContainer.Margin.bottom.ToInt() + mainContainer.Margin.top.ToInt() + mainContainer.Margin.bottom.ToInt() + connectedPlayersContainer.Margin.top.ToInt() + connectedPlayersContainer.Margin.bottom.ToInt();

            int xOffset = mainContainer.Margin.left.ToInt() + mainContainer.Margin.right.ToInt() + connectedPlayersContainer.Margin.left.ToInt() + connectedPlayersContainer.Margin.right.ToInt() + 20;
            connectedPlayersContainer.Size = new Vector2(_graphicsMetaData.ScreenWidth - xOffset, _graphicsMetaData.ScreenHeight - yOffset);
            
            mainContainer.Children.Add(connectedPlayersContainer);
            mainContainer.Children.Add(bottomContainer);

            _uiContainers.Push(mainContainer);
        }

        private void ExitButton_OnClick(UIElement arg1, UIEvent arg2)
        {
            ScreenNaviagor.CreateInstance().PopScreen();
        }
    }
}
