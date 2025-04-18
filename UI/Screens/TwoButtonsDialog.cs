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
        private UICenterFlowContainer _connectDialogBox;

        public override Color Background { 
            get
            {
                return _connectDialogBox.Background;
            }
            set {
                _connectDialogBox.ChangeBackground(value);
            } 
        }

        private void Init(bool hideCloseBtn)
        {
            _connectDialogBox = new UICenterFlowContainer(_graphicsMetaData, true, "backlayout");

            _connectDialogBox.Margin = new Padding(20);
            UILabel uILabel = new UILabel(_graphicsMetaData, _textMsg);
            uILabel.TextColor = Color.Black;

            UIButton connectButton = new UIButton(_graphicsMetaData, _okBtnText);
            connectButton.OnClick += onOkBtnClicked;

            _connectDialogBox.Children.Add(uILabel);
            _connectDialogBox.Children.Add(connectButton);
            if (!hideCloseBtn)
            {
                UIButton closeButton = new UIButton(_graphicsMetaData, _closeBtnText);
                closeButton.OnClick += onCloseBtnClicked;
                _connectDialogBox.Children.Add(closeButton);
            }
            
            _connectDialogBox.Position = new Vector2((_graphicsMetaData.ScreenWidth - _connectDialogBox.GetWidth()) / 2, 200);
            _uiContainers.Push(_connectDialogBox);
            IsDialog = true;
        }

        public override void Dispose()
        {
        }

        public TwoButtonsDialog(GraphicsContext graphicsMetaData, string textMsg, string closeBtnText = "Cancel", string okBtnText = "OK", Action<UIElement, UIEvent> onOkBtnClick = null, Action<UIElement, UIEvent> onCloseBtnClick = null, bool hideCloseButton = false) : base(graphicsMetaData)
        {
            _textMsg = textMsg;
            _closeBtnText = closeBtnText;
            _okBtnText = okBtnText;
            this.onCloseBtnClicked = onCloseBtnClick;
            this.onOkBtnClicked = onOkBtnClick;
            Init(hideCloseButton);
        }
    }
}
