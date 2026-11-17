#nullable disable
using System;
using FrogBattleV4.UI;
using FrorgWork;
using FrorgWork.Input;
using FrorgWork.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FrogBattleV4;

public class FrogBattle : FrorgGame
{
    private ISelectableMenu _menu;

    public FrogBattle() : base("FrorgBattleV4", 640, 480, false)
    {
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();

        // _menu = new ShardMenu(4, 3, new Point(5, 80), Textures.MenuButton, Textures.MenuButtonHover);
        // _menu = new JaggedShardMenu(new Point(5, 80), Textures.MenuButton, Textures.MenuButtonHover);
        _menu = new SillyMenu(new Point(100, 100), Textures.MenuButton);
    }

    protected override void LoadContent()
    {
        Textures = Textures with
        {
            MenuButton = Content.Load<Texture2D>("textures/gui/Cell"),
            MenuButtonHover = Content.Load<Texture2D>("textures/gui/Cursor"),
        };

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        GameInput.Update(gameTime);
        foreach (var action in Enum.GetValues<InputAction>())
        {
            if (GameInput.GetState()[action] == InputStatus.Pressed) _menu.HandleInput(action);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        SpriteBatch.Begin();

        _menu.Draw(SpriteBatch);

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}