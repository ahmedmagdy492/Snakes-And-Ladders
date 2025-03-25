using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeAndLadders.Helpers;
using SnakeAndLadders.UI;
using SnakeAndLadders.UI.Screens;
using System;

namespace SnakeAndLadders;

public class MainGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private ScreenNaviagor _screenNavigator;
    private SpriteFont _font;
    private GraphicsContext _graphicsContext;

    public MainGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = Constants.SCREEN_WIDTH;
        _graphics.PreferredBackBufferHeight = Constants.SCREEN_HEIGHT;
        _graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.Title = "Snakes And Ladders";
        _screenNavigator = ScreenNaviagor.CreateInstance();
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("SecondFont");
        _graphicsContext = new GraphicsContext
        {
            Font = _font,
            GraphicsDeviceManager = _graphics,
            SpriteBatch = _spriteBatch,
            ContentManager = Content,
            ScreenWidth = _graphics.PreferredBackBufferWidth,
            ScreenHeight = _graphics.PreferredBackBufferHeight,
            ClearColor = Constants.CLEAR_COLOR
        };
        TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d);
        _screenNavigator.PushScreen(new MainMenuScreen(_graphicsContext));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _screenNavigator.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Constants.CLEAR_COLOR);

        _spriteBatch.Begin();

        _screenNavigator.Draw();
        //Texture2D _texture = new Texture2D(GraphicsDevice, 1, 1);
        //_texture.SetData([Color.White]);
        //int rowsCount = 10, colsCount = 20;
        //int width = 60, height = 60;
        //int totalWidth = colsCount * width;
        //int totalHeight = rowsCount * height;
        //int x = 0, y = 600;
        //int counter = 1;
        //for (int i = 0; i < rowsCount; i++)
        //{
        //    if(i % 2 == 0)
        //    {
        //        x = 0;
        //        for (int j = 0; j < colsCount; ++j)
        //        {
        //            if (j % 2 == 0)
        //            {
        //                _spriteBatch.Draw(_texture, new Rectangle(new Point(x, y), new Point(width, height)), Color.SandyBrown);
        //                _spriteBatch.DrawString(_font, counter.ToString(), new Vector2(x, y), Color.Black);
        //            }
        //            else
        //            {
        //                _spriteBatch.Draw(_texture, new Rectangle(new Point(x, y), new Point(width, height)), Color.SaddleBrown);
        //                _spriteBatch.DrawString(_font, counter.ToString(), new Vector2(x, y), Color.White);
        //            }
        //            counter++;
        //            x += width;
        //        }
        //        x = 0;
        //    }
        //    else
        //    {
        //        x = totalWidth - width;
        //        for (int j = 0; j < colsCount; ++j)
        //        {
        //            if (j % 2 == 0)
        //            {
        //                _spriteBatch.Draw(_texture, new Rectangle(new Point(x, y), new Point(width, height)), Color.SandyBrown);
        //                _spriteBatch.DrawString(_font, counter.ToString(), new Vector2(x, y), Color.Black);
        //            }
        //            else
        //            {
        //                _spriteBatch.Draw(_texture, new Rectangle(new Point(x, y), new Point(width, height)), Color.SaddleBrown);
        //                _spriteBatch.DrawString(_font, counter.ToString(), new Vector2(x, y), Color.White);
        //            }
        //            counter++;
        //            x -= width;
        //        }
        //        x = totalWidth - width;
        //    }
        //    y -= height;
        //}

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
