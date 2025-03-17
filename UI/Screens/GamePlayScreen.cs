using Microsoft.Xna.Framework;
using SnakeAndLadders.UI.UIContainers;
using System;
using System.Collections.Generic;
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

            int yOffset = mainContainer.GetHeight() + (int)mainContainer.Margin.top + (int)mainContainer.Margin.bottom + (int)boardContainer.Margin.top + (int)boardContainer.Margin.bottom;
            int xOffset = (int)mainContainer.Margin.left + (int)mainContainer.Margin.right + (int)boardContainer.Margin.left + (int)boardContainer.Margin.right + 20;
            boardContainer.Size = uIImage.Size;

            mainContainer.Children.Add(bottomContainer);
            mainContainer.Children.Add(boardContainer);

            _uiContainers.Push(mainContainer);
        }
    }
}
