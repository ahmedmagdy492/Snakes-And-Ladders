using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeAndLadders.Helpers;
using SnakeAndLadders.UI;

namespace SnakeAndLadders;

public class MainGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Screen _mainScreen;
    private SpriteFont _font;
    private GraphicsMetaData _graphcisMetaData;

    public MainGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = Constants.SCREEN_WIDTH;
        _graphics.PreferredBackBufferHeight = Constants.SCREEN_HEIGHT;
        _graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("MainFont");
        _graphcisMetaData = new GraphicsMetaData
        {
            Font = _font,
            GraphicsDeviceManager = _graphics,
            SpriteBatch = _spriteBatch,
            ScreenWidth = _graphics.PreferredBackBufferWidth,
            ScreenHeight = _graphics.PreferredBackBufferHeight,
            ClearColor = Constants.CLEAR_COLOR
        };
        _mainScreen = new Screen(_graphcisMetaData);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _mainScreen.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Constants.CLEAR_COLOR);

        _spriteBatch.Begin();

        _mainScreen.Draw();

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
