using Microsoft.Xna.Framework;
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
        private readonly NetworkServer _networkServer;
        private bool _isSomeoneConnected = false;
        private UIButton _waitingBtn;

        public CreateServerScreen(GraphicsContext graphicsMetaData) : base(graphicsMetaData)
        {
            _networkServer = new NetworkServer();
            Init();
            #pragma warning disable CS4014 
            _networkServer.SetupServer(
                (ConnectedClientInfo clientInfo) => 
                {
                    _isSomeoneConnected = true;
                    _waitingBtn.Text = clientInfo.IPAddress;
                }
            );
            _networkServer.OnDataReceived += NetworkServer_OnDataReceived;
            _networkServer.OnOtherPeerDisconnected += NetworkServer_OnOtherPeerDisconnected;
            #pragma warning restore CS4014
        }

        private void NetworkServer_OnDataReceived(byte[] data)
        {
            _waitingBtn.Text = MessageParserService.DecodeString(data);
        }

        private async void NetworkServer_OnOtherPeerDisconnected()
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

            UIFlexDimContainer connectedPlayersContainer = new UIFlexDimContainer(_graphicsMetaData);
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

        private async void StartGameButton_OnClick(UIElement btn, UIEvent e)
        {
            var startMsg = new GameProtocol
            {
                Type = MessageType.GameStart,
                Data = [1],
                DataLen = 1
            };
            byte[] data = MessageParserService.Encode(startMsg);
            try
            {
                await _networkServer.Send(data);
            }
            catch (Exception ex)
            {
                new TwoButtonsDialog(_graphicsMetaData, ex.Message, hideCloseButton: true, onOkBtnClick: (btn, e) => {
                    ScreenNaviagor.CreateInstance().PopScreen();
                });
            }
        }

        private void ExitButton_OnClick(UIElement btn, UIEvent e)
        {
            ScreenNaviagor.CreateInstance().PopScreen();
        }

        public override void Update(GameTime gameTime)
        {
            if(_isSomeoneConnected)
            {
                base.Update(gameTime);
            }
        }

        public override void Dispose()
        {
            _networkServer.Dispose();
        }
    }
}
