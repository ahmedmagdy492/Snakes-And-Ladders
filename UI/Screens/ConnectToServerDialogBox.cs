using Microsoft.Xna.Framework;
using SnakeAndLadders.Helpers;
using SnakeAndLadders.UI.UIContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.UI.Screens
{
    public class ConnectToServerDialogBox : Screen
    {
        private void Init()
        {
            UICenterFlowContainer connectDialogBox = new UICenterFlowContainer(_graphicsMetaData);
            connectDialogBox.ChangeBackground(new Color(0x00, 0x00, 0x00, 0xdd));

            connectDialogBox.Margin = new Padding(20);
            UILabel uILabel = new UILabel(_graphicsMetaData, "Enter Friend IP");
            uILabel.TextColor = Color.White;
            UITextInput ipAddress = new UITextInput(_graphicsMetaData);

            UIButton connectButton = new UIButton(_graphicsMetaData, "Connect");
            UIButton closeButton = new UIButton(_graphicsMetaData, "Close");
            closeButton.OnClick += CloseButton_OnClick;

            connectDialogBox.Children.Add(uILabel);
            connectDialogBox.Children.Add(ipAddress);
            connectDialogBox.Children.Add(connectButton);
            connectDialogBox.Children.Add(closeButton);
            connectDialogBox.Position = new Vector2((_graphicsMetaData.ScreenWidth - connectDialogBox.GetWidth()) / 2, 200);
            _uiContainers.Push(connectDialogBox);
            IsDialog = true;
        }

        public ConnectToServerDialogBox(GraphicsContext graphicsMetaData) : base(graphicsMetaData)
        {
            Init();
        }

        private void CloseButton_OnClick(UIElement arg1, UIEvent arg2)
        {
            ScreenNaviagor.CreateInstance().PopScreen();
        }
    }
}
