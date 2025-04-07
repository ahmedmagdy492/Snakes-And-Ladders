﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using SnakeAndLadders.Helpers;
using SnakeAndLadders.Models;
using SnakeAndLadders.Services;
using SnakeAndLadders.UI.UIContainers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SnakeAndLadders.UI.Screens
{
    public class ClientNetworkGamePlayScreen : Screen
    {
        private UILabel _player1Name;
        private UILabel _player2Name;
        private UIImage _diceImageUI;
        private GameLogic _gameLogic;
        private readonly List<Player> _players;
        private bool _isPlayingAnimation = false;
        private readonly SoundEffect _diceSE;
        private readonly Song _winSong;
        private GameState _curGameState;
        private UIButton _rollDiceButton;
        private UILabel _gameStatusLabel;
        private readonly NetworkClient _networkClient;
        private readonly DiceRollerService _diceRollerService;

        public ClientNetworkGamePlayScreen(NetworkClient networkClient, GraphicsContext graphicsMetaData, List<Player> players) : base(graphicsMetaData)
        {
            _networkClient = networkClient;
            _diceRollerService = new DiceRollerService();
            _players = players;
            _diceSE = _graphicsMetaData.ContentManager.Load<SoundEffect>("dice-roll");
            _winSong = _graphicsMetaData.ContentManager.Load<Song>("win");
            Init();
            _networkClient.OnDataReceived += NetworkClient_OnDataReceived;
            _networkClient.OnOtherPeerDisconnected += NetworkClient_OnOtherPeerDisconnected;
            _rollDiceButton.IsEnabled = false;
        }

        private void NetworkClient_OnOtherPeerDisconnected()
        {
            ScreenNaviagor.CreateInstance().ClearScreens(new MainMenuScreen(_graphicsMetaData));
        }

        private void NetworkClient_OnDataReceived(byte[] data)
        {
            var msg = MessageParserService.Decode(data);
            if (msg.Type == MessageType.PlayerMove)
            {
                _rollDiceButton.IsEnabled = false;
                _diceSE.Play();
                int diceValue = _gameLogic.PlayDice();
                _diceImageUI.ReloadImage(diceValue.ToString());

                _gameLogic.MoveCurrentPlayingPlayer(diceValue);
                _isPlayingAnimation = true;
                ChangePlayersColors();
                _rollDiceButton.IsEnabled = true;
            }
        }

        private void Init()
        {
            _player1Name = new UILabel(_graphicsMetaData, _players[0].PlayerName);
            UIImage player1Img = new UIImage(_graphicsMetaData, "p1");
            _player2Name = new UILabel(_graphicsMetaData, _players[1].PlayerName);
            UIImage player2Img = new UIImage(_graphicsMetaData, "p2");
            _rollDiceButton = new UIButton(_graphicsMetaData, "Roll Dice");
            _rollDiceButton.OnClick += RollDiceButton_OnClick;
            UICenterFlowContainer mainContainer = new UICenterFlowContainer(_graphicsMetaData);
            mainContainer.FlowDirection = UIFlowContainerDirection.RightToLeft;
            mainContainer.Position = new Vector2(20, 20);
            mainContainer.Margin = new Padding(10);
            mainContainer.Border = new Border
            {
                width = 0,
                color = Color.White
            };
            _gameStatusLabel = new UILabel(_graphicsMetaData, "");
            _gameStatusLabel.TextColor = Color.White;

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
            UICenterFlowContainer rightPanel = new UICenterFlowContainer(_graphicsMetaData);
            rightPanel.Border = new Border
            {
                width = 0,
                color = Color.White
            };

            buttonsPanel.Margin = new Padding(10);

            buttonsPanel.Children.Add(_rollDiceButton);
            buttonsPanel.Children.Add(_diceImageUI);
            buttonsPanel.Children.Add(pauseButton);
            rightPanel.Children.Add(buttonsPanel);
            rightPanel.Children.Add(_gameStatusLabel);

            int yOffset = mainContainer.GetHeight() + mainContainer.Margin.top.ToInt() + mainContainer.Margin.bottom.ToInt() + boardContainer.Margin.top.ToInt() + boardContainer.Margin.bottom.ToInt();

            int xOffset = mainContainer.Margin.left.ToInt() + mainContainer.Margin.right.ToInt() + boardContainer.Margin.left.ToInt() + boardContainer.Margin.right.ToInt() + 20;
            boardContainer.Size = boardUIImage.Size;

            mainContainer.Children.Add(boardContainer);
            mainContainer.Children.Add(rightPanel);
            mainContainer.Children.Add(player1Img);
            mainContainer.Children.Add(_player1Name);
            mainContainer.Children.Add(player2Img);
            mainContainer.Children.Add(_player2Name);

            _uiContainers.Push(mainContainer);

            _gameLogic = new GameLogic(_players, GamePlayMode.AganistPlayer);
            _curGameState = GameState.Playing;
            _gameLogic.OnWining += GameLogic_OnWining;
            ChangePlayersColors();
            var currentPlayer = _gameLogic.GetCurrentPlayingPlayer();
            _gameStatusLabel.Text = currentPlayer.PlayerName + " is Playing ...";
        }

        private void GameLogic_OnWining(Player wonPlayer)
        {
            var prevSong = MediaPlayer.Queue.ActiveSong;
            MediaPlayer.Play(_winSong);
            _curGameState = GameState.Ended;
            ScreenNaviagor.CreateInstance().PushScreen(new TwoButtonsDialog(_graphicsMetaData, $"{wonPlayer.PlayerName} Won the Game", okBtnText: "Main Menu",
            onOkBtnClick: (UIElement arg1, UIEvent arg2) => {
                ScreenNaviagor.CreateInstance().ClearScreens(new MainMenuScreen(_graphicsMetaData));
            }, hideCloseButton: true));
            MediaPlayer.Play(prevSong);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_curGameState == GameState.Ended || _curGameState == GameState.Paused)
                return;

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
                    currentPlayer.MovingCellNo -= 1;
                    currentPlayer.Position = movingVec;
                    if (_players[0].CurrentCellNo == _players[1].CurrentCellNo)
                    {
                        currentPlayer.Position = new Vector2(movingVec.X + 10, movingVec.Y);
                    }
                    _isPlayingAnimation = false;
                    _gameLogic.ChangePlayerTurn();
                    ChangePlayersColors();
                    _gameStatusLabel.Text = _gameLogic.GetCurrentPlayingPlayer().PlayerName + " is Playing ...";

                    if (_gameLogic.GetCurrentPlayingPlayer().PlayerName != "You")
                    {
                        _rollDiceButton.IsEnabled = false;
                    }
                    else
                    {
                        _rollDiceButton.IsEnabled = true;
                    }
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

        private void ChangePlayersColors()
        {
            var currentPlayer = _gameLogic.GetCurrentPlayingPlayer();
            _player1Name.TextColor = currentPlayer.PlayerName == _player1Name.Text ? Color.YellowGreen : Color.Red;
            _player2Name.TextColor = currentPlayer.PlayerName == _player2Name.Text ? Color.YellowGreen : Color.Red;
        }

        private async void RollDiceButton_OnClick(UIElement clickedBtn, UIEvent e)
        {
            if(_curGameState == GameState.Playing)
            {
                clickedBtn.IsEnabled = false;
                var currentPlayer = _gameLogic.GetCurrentPlayingPlayer();
                await _networkClient.Send(MessageParserService.Encode(new GameProtocol
                {
                    Type = MessageType.ClientPlay,
                    DataLen = 0,
                }));
                clickedBtn.IsEnabled = true;
            }
        }

        private void PauseButton_OnClick(UIElement clickedBtn, UIEvent e)
        {
            _curGameState = GameState.Paused;
            var pauseDialog = new TwoButtonsDialog(_graphicsMetaData, "Pause Menu", "Exit", "Back",
            (UIElement arg1, UIEvent arg2) =>
            {
                ScreenNaviagor.CreateInstance().PopScreen();
                _curGameState = GameState.Playing;
            },
            (UIElement arg1, UIEvent arg2) =>
            {
                ScreenNaviagor.CreateInstance().PopScreen();
                ScreenNaviagor.CreateInstance().PopScreen();
            });
            ScreenNaviagor.CreateInstance().PushScreen(pauseDialog);
        }

        public override void Dispose()
        {
            _networkClient.Dispose();
        }
    }
}
