using Microsoft.Xna.Framework;
using SnakeAndLadders.Helpers;
using SnakeAndLadders.Models;
using SnakeAndLadders.Services;
using SnakeAndLadders.UI.UIContainers;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SnakeAndLadders.UI.Screens
{
    public class GamePlayScreen : Screen
    {
        private UILabel _player1Name;
        private UILabel _player2Name;
        private UIImage _diceImageUI;
        private DiceRollerService _diceRollerService;
        private GameLogic _gameLogic;
        private readonly List<Player> _players;
        private bool _isPlayingAnimation = false;

        public GamePlayScreen(GraphicsContext graphicsMetaData, List<Player> players) : base(graphicsMetaData)
        {
            _players = players;
            _diceRollerService = new DiceRollerService();
            Init();
        }

        private void Init()
        {
            _player1Name = new UILabel(_graphicsMetaData, _players[0].PlayerName);
            _player2Name = new UILabel(_graphicsMetaData, _players[1].PlayerName);
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

            UICenterFlowContainer buttonsPanel = new UICenterFlowContainer(_graphicsMetaData);
            buttonsPanel.FlowDirection = UIFlowContainerDirection.RightToLeft;
            buttonsPanel.Border = new Border
            {
                width = 0,
                color = Color.White
            };
            UICenterFlowContainer leftPanel = new UICenterFlowContainer(_graphicsMetaData);
            leftPanel.Border = new Border
            {
                width = 0,
                color = Color.White
            };

            buttonsPanel.Margin = new Padding(10);

            buttonsPanel.Children.Add(_player1Name);
            buttonsPanel.Children.Add(_player2Name);
            buttonsPanel.Children.Add(rollDiceButton);
            buttonsPanel.Children.Add(_diceImageUI);
            buttonsPanel.Children.Add(pauseButton);
            leftPanel.Children.Add(buttonsPanel);

            int yOffset = mainContainer.GetHeight() + mainContainer.Margin.top.ToInt() + mainContainer.Margin.bottom.ToInt() + boardContainer.Margin.top.ToInt() + boardContainer.Margin.bottom.ToInt();

            int xOffset = mainContainer.Margin.left.ToInt() + mainContainer.Margin.right.ToInt() + boardContainer.Margin.left.ToInt() + boardContainer.Margin.right.ToInt() + 20;
            boardContainer.Size = boardUIImage.Size;

            mainContainer.Children.Add(boardContainer);
            mainContainer.Children.Add(leftPanel);

            _uiContainers.Push(mainContainer);

            _gameLogic = new GameLogic(_players);
            var currentPlayer = _gameLogic.GetCurrentPlayingPlayer();
            _player1Name.TextColor = currentPlayer.PlayerName == _player1Name.Text ? Color.YellowGreen : Color.Red;
            _player2Name.TextColor = currentPlayer.PlayerName == _player2Name.Text ? Color.YellowGreen : Color.Red;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(_isPlayingAnimation)
            {
                var currentPlayer = _gameLogic.GetCurrentPlayingPlayer();
                Vector2 movingVec = _gameLogic._currentMap._cellPositions[currentPlayer.MovingCellNo];

                if (currentPlayer.MovingCellNo < currentPlayer.CurrentCellNo)
                {
                    currentPlayer.MovingCellNo += 1;
                    currentPlayer.Position = movingVec;
                }
                else if(currentPlayer.MovingCellNo > currentPlayer.CurrentCellNo)
                {
                    currentPlayer.MovingCellNo -= 1;
                    currentPlayer.Position = movingVec;
                }
                else
                {
                    _isPlayingAnimation = false;
                    _gameLogic.ChangePlayerTurn();
                }
            }
        }

        public override void Draw()
        {
            base.Draw();

            var players = _gameLogic.GetPlayers();
            foreach(var player in players)
            {
                if(_isPlayingAnimation)
                {
                    if (player.PlayerName.Equals(_gameLogic.GetCurrentPlayingPlayer()))
                    {
                        _graphicsMetaData.SpriteBatch.Draw(player.Texture, new Rectangle(_gameLogic._currentMap._cellPositions[player.MovingCellNo].ToPoint(), new Point(player.Texture.Width, player.Texture.Height)), Color.White);
                    }
                    else
                    {
                        _graphicsMetaData.SpriteBatch.Draw(player.Texture, new Rectangle(player.Position.ToPoint(), new Point(player.Texture.Width, player.Texture.Height)), Color.White);
                    }
                }
                else
                {
                    _graphicsMetaData.SpriteBatch.Draw(player.Texture, new Rectangle(player.Position.ToPoint(), new Point(player.Texture.Width, player.Texture.Height)), Color.White);
                }
            }
        }

        private void RollDiceButton_OnClick(UIElement arg1, UIEvent arg2)
        {
            int diceValue = _diceRollerService.RollTheDice();
            _diceImageUI.ReloadImage(diceValue.ToString());

            _gameLogic.MoveCurrentPlayingPlayer(diceValue);
            var currentPlayer = _gameLogic.GetCurrentPlayingPlayer();
            _player1Name.TextColor = currentPlayer.PlayerName == _player1Name.Text ? Color.YellowGreen : Color.Red;
            _player2Name.TextColor = currentPlayer.PlayerName == _player2Name.Text ? Color.YellowGreen : Color.Red;
            _isPlayingAnimation = true;
        }

        private void PauseButton_OnClick(UIElement arg1, UIEvent arg2)
        {
            ScreenNaviagor.CreateInstance().PopScreen();
        }
    }
}
