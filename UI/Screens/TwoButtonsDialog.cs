using Microsoft.Xna.Framework;
using SnakeAndLadders.UI.UIContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.UI.Screens
{
    public class TwoButtonsDialog : Screen
    {
        private string _textMsg = "";
        private string _closeBtnText;
        private string _okBtnText;
        private Action<UIElement, UIEvent> onCloseBtnClicked;
        private Action<UIElement, UIEvent> onOkBtnClicked;
        private void Init()
        {
            UICenterFlowContainer connectDialogBox = new UICenterFlowContainer(_graphicsMetaData);

            connectDialogBox.ChangeBackground(new Color(0x00, 0x00, 0x00, 0xaa));
            connectDialogBox.Margin = new Padding(20);
            UILabel uILabel = new UILabel(_graphicsMetaData, _textMsg);
            uILabel.TextColor = Color.White;

            UIButton connectButton = new UIButton(_graphicsMetaData, _okBtnText);
            connectButton.OnClick += onOkBtnClicked;
            UIButton closeButton = new UIButton(_graphicsMetaData, _closeBtnText);
            closeButton.OnClick += onCloseBtnClicked;

            connectDialogBox.Children.Add(uILabel);
            connectDialogBox.Children.Add(connectButton);
            connectDialogBox.Children.Add(closeButton);
            connectDialogBox.Position = new Vector2((_graphicsMetaData.ScreenWidth - connectDialogBox.GetWidth()) / 2, 200);
            _uiContainers.Push(connectDialogBox);
            IsDialog = true;
        }

        public TwoButtonsDialog(GraphicsContext graphicsMetaData, string textMsg, string closeBtnText = "Cancel", string okBtnText = "OK", Action<UIElement, UIEvent> onOkBtnClick = null, Action<UIElement, UIEvent> onCloseBtnClick = null) : base(graphicsMetaData)
        {
            _textMsg = textMsg;
            _closeBtnText = closeBtnText;
            _okBtnText = okBtnText;
            this.onCloseBtnClicked = onCloseBtnClick;
            this.onOkBtnClicked = onOkBtnClick;
            Init();
        }
    }
}
