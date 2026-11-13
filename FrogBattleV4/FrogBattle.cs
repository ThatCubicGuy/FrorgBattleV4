using FrogBattleV4.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FrogBattleV4;

public class FrogBattle : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch = null!;

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

        // TODO: use this.Content to load your game content here

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

        if (GameInput.GetState()[InputAction.Up].IsPressed())
        {
            GraphicsDevice.Clear(Color.Red);
        }

        if (GameInput.GetState()[InputAction.Down].IsPressed())
        {
            GraphicsDevice.Clear(Color.Blue);
        }

        if (GameInput.GetState()[InputAction.Left].IsPressed())
        {
            GraphicsDevice.Clear(Color.Green);
        }

        if (GameInput.GetState()[InputAction.Right].IsPressed())
        {
            GraphicsDevice.Clear(Color.Yellow);
        }

        base.Draw(gameTime);
    }
}