#nullable disable
using FrorgWork.UI.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrorgWork;

/// <summary>
/// Base class for most games in the Frorgchise.
/// </summary>
public class FrorgGame : Game
{
    protected GraphicsDeviceManager Graphics { get; }
    protected SpriteBatch SpriteBatch { get; private set; }

    /// <summary>
    /// Creates the game instance.
    /// </summary>
    /// <param name="title">The title to display in the title bar of the game window.</param>
    /// <param name="width">The initial width, in pixels, of the game window.</param>
    /// <param name="height">The initial height, in pixels, of the game window.</param>
    /// <param name="fullScreen">Indicates if the game should start in fullscreen mode.</param>
    public FrorgGame(string title, int width, int height, bool fullScreen)
    {
        Graphics = new GraphicsDeviceManager(this);
        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferHeight = height;
        Graphics.IsFullScreen = fullScreen;
        Window.Title = title;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    public Textures Textures { get; protected set; }
    public Sounds Sounds { get; protected set; }

    protected override void Initialize()
    {
        base.Initialize();

        // Placed after base.Initialize() because we
        // need GraphicsDevice to be initialized
        SpriteBatch = new SpriteBatch(GraphicsDevice);
    }
}