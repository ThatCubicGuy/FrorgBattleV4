using FrogBattleV4.Input;
using FrogBattleV4.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FrogBattleV4;

public class FrogBattle : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch = null!;
    private RectangularMenu _menu = null!;
    private Sprite _cellSprite;
    private Sprite _cursorSprite;

    public FrogBattle()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        var cellTexture = Content.Load<Texture2D>("textures/gui/Cell");
        var cursorTexture = Content.Load<Texture2D>("textures/gui/Cursor");
        _cellSprite = new Sprite(new Point(100, 100), cellTexture);
        _cursorSprite = new Sprite(new Point(100, 150), cursorTexture);

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        GameInput.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        if (GameInput.GetState().IsPressed(InputAction.Up))
        {
            GraphicsDevice.Clear(Color.Red);
        }

        if (GameInput.GetState().IsPressed(InputAction.Down))
        {
            GraphicsDevice.Clear(Color.Blue);
        }

        if (GameInput.GetState().IsPressed(InputAction.Left))
        {
            GraphicsDevice.Clear(Color.Green);
        }

        if (GameInput.GetState().IsPressed(InputAction.Right))
        {
            GraphicsDevice.Clear(Color.Yellow);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}