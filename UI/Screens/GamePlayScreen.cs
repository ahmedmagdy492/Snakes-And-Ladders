using Microsoft.Xna.Framework;
using SnakeAndLadders.Helpers;
using SnakeAndLadders.Models;
using SnakeAndLadders.Services;
using SnakeAndLadders.UI.UIContainers;
using System.Collections.Generic;
using System.Text;

namespace SnakeAndLadders.UI.Screens
{
    public class GamePlayScreen : Screen
    {
        private UILabel _playerTurn;
        private UIImage _diceImageUI;
        private DiceRollerService _diceRollerService;
        private GameLogic _gameLogic;
        private readonly List<Player> _players;

        public GamePlayScreen(GraphicsContext graphicsMetaData, List<Player> players) : base(graphicsMetaData)
        {
            _players = players;
            _diceRollerService = new DiceRollerService();
            Init();
        }

        private void Init()
        {
            _playerTurn = new UILabel(_graphicsMetaData, $"Turn -> -------------");
            UIButton rollDiceButton = new UIButton(_graphicsMetaData, "Roll Dice");
            rollDiceButton.OnClick += RollDiceButton_OnClick;
            UICenterFlowContainer mainContainer = new UICenterFlowContainer(_graphicsMetaData);
            mainContainer.FlowDirection = UIFlowContainerDirection.RightToLeft;
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
            UIImage boardUIImage = new UIImage(_graphicsMetaData, "board");
            boardContainer.Children.Add(boardUIImage);
            UIButton pauseButton = new UIButton(_graphicsMetaData, "Pause");
            pauseButton.OnClick += PauseButton_OnClick;
            _diceImageUI = new UIImage(_graphicsMetaData, "1");

            UIAlignContainer topContainer = new UIAlignContainer(_graphicsMetaData);
            topContainer.Border = new Border
            {
                width = 0,
                color = Color.White
            };

            topContainer.Margin = new Padding(10);

            topContainer.Children.Add(_playerTurn);
            topContainer.Children.Add(rollDiceButton);
            topContainer.Children.Add(_diceImageUI);
            topContainer.Children.Add(pauseButton);

            int yOffset = mainContainer.GetHeight() + mainContainer.Margin.top.ToInt() + mainContainer.Margin.bottom.ToInt() + boardContainer.Margin.top.ToInt() + boardContainer.Margin.bottom.ToInt();

            int xOffset = mainContainer.Margin.left.ToInt() + mainContainer.Margin.right.ToInt() + boardContainer.Margin.left.ToInt() + boardContainer.Margin.right.ToInt() + 20;
            boardContainer.Size = boardUIImage.Size;

            mainContainer.Children.Add(topContainer);
            mainContainer.Children.Add(boardContainer);

            _uiContainers.Push(mainContainer);

            _gameLogic = new GameLogic(_players);
        }

        public override void Draw()
        {
            base.Draw();
            // TODO: draw the players
            var players = _gameLogic.GetPlayers();

            foreach(var player in players)
            {
                _graphicsMetaData.SpriteBatch.Draw(player.Texture, new Rectangle(player.Position.ToPoint(), new Point(player.Texture.Width, player.Texture.Height)), Color.White);
            }
        }

        private void RollDiceButton_OnClick(UIElement arg1, UIEvent arg2)
        {
            int diceValue = _diceRollerService.RollTheDice();
            _diceImageUI.ReloadImage(diceValue.ToString());

            _gameLogic.MoveCurrentPlayingPlayer(diceValue);
            _playerTurn.Text = $"Turn -> {_gameLogic.GetCurrentPlayingPlayer()}";
        }

        private void PauseButton_OnClick(UIElement arg1, UIEvent arg2)
        {
            ScreenNaviagor.CreateInstance().PopScreen();
        }
    }
}
