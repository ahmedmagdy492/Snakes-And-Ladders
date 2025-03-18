using Microsoft.Xna.Framework;
using SnakeAndLadders.Helpers;
using SnakeAndLadders.UI.UIContainers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.UI.Screens
{
    public class GamePlayScreen : Screen
    {
        private UILabel _playerTurn;
        public GamePlayScreen(GraphicsContext graphicsMetaData) : base(graphicsMetaData)
        {
            Init();
        }

        private void Init()
        {
            _playerTurn = new UILabel(_graphicsMetaData, "Turn -> Player 1");
            UIButton rollDiceButton = new UIButton(_graphicsMetaData, "Roll Dice");
            UIAlignContainer mainContainer = new UIAlignContainer(_graphicsMetaData);
            mainContainer.Position = new Vector2(20, 20);
            mainContainer.Margin = new Padding(10);
            mainContainer.Border = new Border
            {
                width = 0,
                color = Color.White
            };

            UIFlexDimContainer boardContainer = new UIFlexDimContainer(_graphicsMetaData);
            boardContainer.Margin = new Padding(10);
            boardContainer.Border = new Border { width = 0 };
            boardContainer.FlowDirection = UIFlowContainerDirection.RightToLeft;
            UIImage uIImage = new UIImage(_graphicsMetaData, "board");
            boardContainer.Children.Add(uIImage);
            UIButton pauseButton = new UIButton(_graphicsMetaData, "Pause");
            pauseButton.OnClick += PauseButton_OnClick;

            UICenterFlowContainer bottomContainer = new UICenterFlowContainer(_graphicsMetaData);
            bottomContainer.Border = new Border
            {
                width = 0,
                color = Color.White
            };

            bottomContainer.Margin = new Padding(10);
            bottomContainer.FlowDirection = UIFlowContainerDirection.RightToLeft;

            bottomContainer.Children.Add(_playerTurn);
            bottomContainer.Children.Add(rollDiceButton);
            bottomContainer.Children.Add(pauseButton);

            int yOffset = mainContainer.GetHeight() + mainContainer.Margin.top.ToInt() + mainContainer.Margin.bottom.ToInt() + boardContainer.Margin.top.ToInt() + boardContainer.Margin.bottom.ToInt();

            int xOffset = mainContainer.Margin.left.ToInt() + mainContainer.Margin.right.ToInt() + boardContainer.Margin.left.ToInt() + boardContainer.Margin.right.ToInt() + 20;
            boardContainer.Size = uIImage.Size;

            mainContainer.Children.Add(bottomContainer);
            mainContainer.Children.Add(boardContainer);

            _uiContainers.Push(mainContainer);
        }

        private void PauseButton_OnClick(UIElement arg1, UIEvent arg2)
        {
            ScreenNaviagor.CreateInstance().PopScreen();
            Debug.WriteLine("hereeed");
        }
    }
}
