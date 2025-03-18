using Microsoft.Xna.Framework;
using SnakeAndLadders.Helpers;
using SnakeAndLadders.Services;
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
        private UIImage _diceImageUI;
        private DiceRollerService _diceRollerService;

        public GamePlayScreen(GraphicsContext graphicsMetaData) : base(graphicsMetaData)
        {
            _diceRollerService = new DiceRollerService();
            Init();
        }

        private void Init()
        {
            _playerTurn = new UILabel(_graphicsMetaData, "Turn -> Player 1");
            UIButton rollDiceButton = new UIButton(_graphicsMetaData, "Roll Dice");
            rollDiceButton.OnClick += RollDiceButton_OnClick;
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
            _diceImageUI = new UIImage(_graphicsMetaData, "1");

            UICenterFlowContainer topContainer = new UICenterFlowContainer(_graphicsMetaData);
            topContainer.Border = new Border
            {
                width = 0,
                color = Color.White
            };

            topContainer.Margin = new Padding(10);
            topContainer.FlowDirection = UIFlowContainerDirection.RightToLeft;

            topContainer.Children.Add(_playerTurn);
            topContainer.Children.Add(rollDiceButton);
            topContainer.Children.Add(_diceImageUI);
            topContainer.Children.Add(pauseButton);

            int yOffset = mainContainer.GetHeight() + mainContainer.Margin.top.ToInt() + mainContainer.Margin.bottom.ToInt() + boardContainer.Margin.top.ToInt() + boardContainer.Margin.bottom.ToInt();

            int xOffset = mainContainer.Margin.left.ToInt() + mainContainer.Margin.right.ToInt() + boardContainer.Margin.left.ToInt() + boardContainer.Margin.right.ToInt() + 20;
            boardContainer.Size = uIImage.Size;

            mainContainer.Children.Add(topContainer);
            mainContainer.Children.Add(boardContainer);

            _uiContainers.Push(mainContainer);
        }

        public override void Draw()
        {
            base.Draw();
            // TODO: draw the players
        }

        private void RollDiceButton_OnClick(UIElement arg1, UIEvent arg2)
        {
            int diceValue = _diceRollerService.RollTheDice();
            _diceImageUI.ReloadImage(diceValue.ToString());
        }

        private void PauseButton_OnClick(UIElement arg1, UIEvent arg2)
        {
            ScreenNaviagor.CreateInstance().PopScreen();
        }
    }
}
