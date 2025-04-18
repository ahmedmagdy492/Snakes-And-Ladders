﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeAndLadders.Helpers;
using SnakeAndLadders.Models;
using SnakeAndLadders.Services;
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
        private readonly INetworkManager _networkManager;
        private bool _isSomeoneConnected = false;
        private UIButton _waitingBtn;

        public CreateServerScreen(GraphicsContext graphicsMetaData) : base(graphicsMetaData)
        {
            _networkManager = new NetworkManager(NetworkMode.Server);
            Init();
            #pragma warning disable CS4014 
            _networkManager.SetupServer(
                (ConnectedClientInfo clientInfo) => 
                {
                    _isSomeoneConnected = true;
                    _waitingBtn.Text = clientInfo.IPAddress;
                }
            );
            _networkManager.OnOtherPeerDisconnected += NetworkManager_OnOtherPeerDisconnected;
            #pragma warning restore CS4014
        }

        private async void NetworkManager_OnOtherPeerDisconnected()
        {
            _isSomeoneConnected = false;
            _waitingBtn.Text = "Client Disonnected";
            await Task.Run(async () =>
            {
                await Task.Delay(1000);
                ScreenNaviagor.CreateInstance().PopScreen();
            });
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

            UIFlexDimContainer connectedPlayersContainer = new UIFlexDimContainer(_graphicsMetaData, true);
            connectedPlayersContainer.Margin = new Padding(10);
            connectedPlayersContainer.FlowDirection = UIFlowContainerDirection.RightToLeft;
            UIButton youBtn = new UIButton(_graphicsMetaData, "Your computer");
            youBtn.Background = Color.GreenYellow;
            _waitingBtn = new UIButton(_graphicsMetaData, "Wating for another player to connect ...");
            connectedPlayersContainer.Children.Add(youBtn);
            connectedPlayersContainer.Children.Add(_waitingBtn);
            
            UIButton startGameButton = new UIButton(_graphicsMetaData, "Start Game");
            startGameButton.OnClick += StartGameButton_OnClick;
            UIButton exitButton = new UIButton(_graphicsMetaData, "Exit");
            exitButton.Id = "Exit".GetHashCode();
            exitButton.OnClick += ExitButton_OnClick;

            UICenterFlowContainer bottomContainer = new UICenterFlowContainer(_graphicsMetaData, false);
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

        private async void StartGameButton_OnClick(UIElement btn, UIEvent e)
        {
            if (!_isSomeoneConnected)
            {
                return;
            }

            await _networkManager.Send(MessageParserService.Encode(new GameProtocol
            {
                Type = MessageType.GameStart,
            }));
            ScreenNaviagor.CreateInstance().PushScreen(new NetworkedGamePlayScreen(_graphicsMetaData, new List<Player>
            {
                new Player
                {
                    CurrentCellNo = 1,
                    MovingCellNo = 1,
                    PlayerName = "Player 1",
                    Position = Vector2.Zero,
                    Texture = _graphicsMetaData.ContentManager.Load<Texture2D>("p1")
                },
                new Player
                {
                    CurrentCellNo = 1,
                    MovingCellNo = 1,
                    PlayerName = "Player 2",
                    Position = Vector2.Zero,
                    Texture = _graphicsMetaData.ContentManager.Load<Texture2D>("p2")
                }
            }, _networkManager, PlayerType.Server));
        }

        private void ExitButton_OnClick(UIElement btn, UIEvent e)
        {
            ScreenNaviagor.CreateInstance().PopScreen();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Dispose()
        {
            _networkManager.Dispose();
        }
    }
}
