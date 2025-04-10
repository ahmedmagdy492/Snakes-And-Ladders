using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnakeAndLadders.Helpers;
using SnakeAndLadders.Models;
using SnakeAndLadders.Services;
using SnakeAndLadders.UI.UIContainers;
using System;
using System.Collections.Generic;

namespace SnakeAndLadders.UI.Screens
{
    public class ConnectToServerDialogBox : Screen
    {
        private readonly INetworkManager _networkManager;
        private UITextInput _ipAddressTInput;

        private void Init()
        {
            UICenterFlowContainer connectDialogBox = new UICenterFlowContainer(_graphicsMetaData);
            connectDialogBox.ChangeBackground(new Color(0x00, 0x00, 0x00, 0xdd));

            connectDialogBox.Margin = new Padding(20);
            UILabel uILabel = new UILabel(_graphicsMetaData, "Enter Friend IP");
            uILabel.TextColor = Color.White;
            _ipAddressTInput = new UITextInput(_graphicsMetaData);

            UIButton connectButton = new UIButton(_graphicsMetaData, "Connect");
            connectButton.OnClick += ConnectButton_OnClick;
            UIButton closeButton = new UIButton(_graphicsMetaData, "Close");
            closeButton.OnClick += CloseButton_OnClick;

            connectDialogBox.Children.Add(uILabel);
            connectDialogBox.Children.Add(_ipAddressTInput);
            connectDialogBox.Children.Add(connectButton);
            connectDialogBox.Children.Add(closeButton);
            connectDialogBox.Position = new Vector2((_graphicsMetaData.ScreenWidth - connectDialogBox.GetWidth()) / 2, 200);
            _uiContainers.Push(connectDialogBox);
            IsDialog = true;
        }

        private async void ConnectButton_OnClick(UIElement btn, UIEvent e)
        {
            if (!string.IsNullOrWhiteSpace(_ipAddressTInput.Value))
            {
                try
                {
                    await _networkManager.Connect(_ipAddressTInput.Value, Constants.SERVER_PORT);

                    var dialogBox = new TwoButtonsDialog(_graphicsMetaData, "Connected. Waiting for other player to start", onOkBtnClick: (UIElement arg1, UIEvent arg2) =>
                    {

                    }, hideCloseButton: true);
                    dialogBox.IsDialog = false;
                    dialogBox.Background = new Color(0x00, 0x00, 0x00);
                    ScreenNaviagor.CreateInstance().PushScreen(dialogBox);
                    await _networkManager.StartReceiving();
                }
                catch (Exception ex)
                {
                    var dialogBox = new TwoButtonsDialog(_graphicsMetaData, ex.Message, onOkBtnClick: (UIElement arg1, UIEvent arg2) =>
                    {
                        ScreenNaviagor.CreateInstance().ClearScreens(new MainMenuScreen(_graphicsMetaData));
                    }, hideCloseButton: true);
                    dialogBox.IsDialog = false;
                    dialogBox.Background = new Color(0x00, 0x00, 0x00);

                    ScreenNaviagor.CreateInstance().PushScreen(dialogBox);
                }
            }
        }

        public ConnectToServerDialogBox(GraphicsContext graphicsMetaData) : base(graphicsMetaData)
        {
            _networkManager = new NetworkManager(NetworkMode.Client);
            _networkManager.OnDataReceived += NetworkClient_OnDataReceived;
            _networkManager.OnOtherPeerDisconnected += NetworkClient_OnOtherPeerDisconnected;
            Init();
        }

        private void NetworkClient_OnOtherPeerDisconnected()
        {
            ScreenNaviagor.CreateInstance().PopScreen();
        }

        private void NetworkClient_OnDataReceived(byte[] rawMsg)
        {
            var gameStartData = MessageParserService.Decode(rawMsg);
            if(gameStartData.Type == MessageType.GameStart)
            {
                ScreenNaviagor.CreateInstance().PopScreen();
                ScreenNaviagor.CreateInstance().PushScreen(new NetworkedGamePlayScreen(_graphicsMetaData, new List<Player>{
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
                }, _networkManager, PlayerType.Client));
            }
        }

        private void CloseButton_OnClick(UIElement arg1, UIEvent arg2)
        {
            ScreenNaviagor.CreateInstance().PopScreen();
        }

        public override void Dispose()
        {
            _networkManager.Dispose();
        }
    }
}
