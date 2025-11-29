using System;
using System.Collections.Generic;
using System.Globalization;
using FrogBattleV4.Core.Effects;
using FrogBattleV4.Core.Localization;
using FrogBattleV4.Core.Settings;
using FrogBattleV4.ScreenManagers;
using FrogBattleV4.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrogBattleV4.Core
{
    /// <summary>
    /// The main class for the game, responsible for managing game components, settings, 
    /// and platform-specific configurations.
    /// </summary>
    /// <remarks>
    /// This class is the entry point for the game and handles initialization, content loading,
    /// and screen management.
    /// </remarks>}
    public class FrogBattleV4Game : Game
    {
        // Resources for drawing.
        private GraphicsDeviceManager graphicsDeviceManager;

        // Manages the game's screen transitions and screens.
        private ScreenManager screenManager;

        // Manages game settings, such as preferences and configurations.
        private SettingsManager<FrogBattleV4Settings> settingsManager;

        // Manages leaderboard data for tracking high scores and achievements.
        private SettingsManager<FrogBattleV4Leaderboard> leaderboardManager;

        // Texture for rendering particles.
        private Texture2D particleTexture;

        // Manages particle effects in the game.
        private ParticleManager particleManager;

        /// <summary>
        /// Indicates if the game is running on a mobile platform.
        /// </summary>
        public readonly static bool IsMobile = OperatingSystem.IsAndroid() || OperatingSystem.IsIOS();

        /// <summary>
        /// Indicates if the game is running on a desktop platform.
        /// </summary>
        public readonly static bool IsDesktop =
            OperatingSystem.IsMacOS() || OperatingSystem.IsLinux() || OperatingSystem.IsWindows();

        /// <summary>
        /// Initializes a new instance of the game. Configures platform-specific settings, 
        /// initializes services like settings and leaderboard managers, and sets up the 
        /// screen manager for screen transitions.
        /// </summary>
        public FrogBattleV4Game()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Share GraphicsDeviceManager as a service.
            Services.AddService(typeof(GraphicsDeviceManager), graphicsDeviceManager);

            // Determine the appropriate settings storage based on the platform.
            ISettingsStorage storage;
            if (IsMobile)
            {
                storage = new MobileSettingsStorage();
                graphicsDeviceManager.IsFullScreen = true;
                IsMouseVisible = false;
            }
            else if (IsDesktop)
            {
                storage = new DesktopSettingsStorage();
                graphicsDeviceManager.IsFullScreen = false;
                IsMouseVisible = true;
            }
            else
            {
                throw new PlatformNotSupportedException();
            }

            // Initialize settings and leaderboard managers.
            settingsManager = new SettingsManager<FrogBattleV4Settings>(storage);
            Services.AddService(typeof(SettingsManager<FrogBattleV4Settings>), settingsManager);

            leaderboardManager = new SettingsManager<FrogBattleV4Leaderboard>(storage);
            Services.AddService(typeof(SettingsManager<FrogBattleV4Leaderboard>), leaderboardManager);

            Content.RootDirectory = "Content";

            // Configure screen orientations.
            graphicsDeviceManager.SupportedOrientations =
                DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

            // Initialize the screen manager.
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
        }

        /// <summary>
        /// Initializes the game, including setting up localization and adding the 
        /// initial screens to the ScreenManager.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Load supported languages and set the default language.
            List<CultureInfo> cultures = LocalizationManager.GetSupportedCultures();
            var languages = new List<CultureInfo>();
            for (int i = 0; i < cultures.Count; i++)
            {
                languages.Add(cultures[i]);
            }

            var selectedLanguage = languages[settingsManager.Settings.Language].Name;
            LocalizationManager.SetCulture(selectedLanguage);

            // Add background and main menu screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
        }

        /// <summary>
        /// Loads game content, such as textures and particle systems.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // Load a texture for particles and initialize the particle manager.
            particleTexture = Content.Load<Texture2D>("Sprites/blank");
            particleManager = new ParticleManager(particleTexture, new Vector2(400, 200));

            // Share the particle manager as a service.
            Services.AddService(typeof(ParticleManager), particleManager);
        }
    }
}