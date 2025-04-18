using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SnakeAndLadders.UI.UIContainers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.UI.Screens
{
    public class MainMenuScreen : Screen
    {
        
        private readonly Song _bgSE;
        public MainMenuScreen(GraphicsContext graphicsMetaData) : base(graphicsMetaData)
        {
            Init();
            _bgSE = _graphicsMetaData.ContentManager.Load<Song>("bg_sound");
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_bgSE);
        }

        private void Init()
        {
            UILabel gamename = new UILabel(_graphicsMetaData, "");
            UIButton vsComputerBtn = new UIButton(_graphicsMetaData, "VS Computer");
            vsComputerBtn.OnClick += PlayVersusComp_OnClick;

            UIButton playWithFriendBtn = new UIButton(_graphicsMetaData, "Play With Friend");
            playWithFriendBtn.OnClick += PlayWithFriendBtn_OnClick;

            UIButton createServerBtn = new UIButton(_graphicsMetaData, "Create a Server");
            createServerBtn.OnClick += CreateServerBtn_OnClick;

            UIImage titleImg = new UIImage(_graphicsMetaData, "title");

            UICenterFlowContainer mainContainer = new UICenterFlowContainer(_graphicsMetaData, false);
            mainContainer.Border = new Border { width = 0, color = Color.White };
            mainContainer.Margin = new Padding(top: 20, right: 0, left: 0, bottom: 0);
            mainContainer.Children.Add(titleImg);
            mainContainer.Children.Add(gamename);
            mainContainer.Children.Add(gamename);
            mainContainer.Children.Add(gamename);
            mainContainer.Children.Add(vsComputerBtn);
            mainContainer.Children.Add(playWithFriendBtn);
            mainContainer.Children.Add(createServerBtn);
            mainContainer.Position = new Vector2((_graphicsMetaData.ScreenWidth - mainContainer.GetWidth()) / 2, 200);
            _uiContainers.Push(mainContainer);
        }

        private void CreateServerBtn_OnClick(UIElement arg1, UIEvent arg2)
        {
            ScreenNaviagor.CreateInstance().PushScreen(new CreateServerScreen(_graphicsMetaData));
        }

        private void PlayWithFriendBtn_OnClick(UIElement arg1, UIEvent arg2)
        {
            ScreenNaviagor.CreateInstance().PushScreen(new ConnectToServerDialogBox(_graphicsMetaData));
        }

        private void PlayVersusComp_OnClick(UIElement arg1, UIEvent arg2)
        {
            ScreenNaviagor.CreateInstance().PushScreen(new GamePlayScreen(_graphicsMetaData, new List<Models.Player>
            {
                new Models.Player
                {
                    CurrentCellNo = 1,
                    PlayerName = "Player 1",
                    Texture = _graphicsMetaData.ContentManager.Load<Texture2D>("p1"),
                    Position = Vector2.Zero
                },
                new Models.Player
                {
                    CurrentCellNo = 1,
                    PlayerName = "Computer",
                    Texture = _graphicsMetaData.ContentManager.Load<Texture2D>("p2"),
                    Position = Vector2.Zero
                }
            }));
        }

        public override void Dispose()
        {
        }
    }
}
