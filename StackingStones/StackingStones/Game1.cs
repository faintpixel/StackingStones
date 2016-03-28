using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StackingStones.Screens;
using System;

namespace StackingStones
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public static SpriteBatch SpriteBatch;
        public static ContentManager ContentManager;
        public static readonly int WIDTH = 1280;
        public static readonly int HEIGHT = 720;

        private IScreen _currentScreen;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = WIDTH;
            _graphics.PreferredBackBufferHeight = HEIGHT;
            _graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
            ContentManager = Content;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.Window.Title = "Stacking Stones";
            this.IsMouseVisible = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Game1.ACTUAL_WIDTH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);


            var scene = new Scene2_House();
            scene.Completed += Scene2_House_Completed;
            _currentScreen = scene;

            Scene2_House_Completed(null);
            //_currentScreen = new TestScreenZoomToLocation();
        }

        private void Scene2_House_Completed(IScreen sender)
        {
            var scene = new Scene3_WalkingDog();
            scene.Completed += Scene3_WalkingDog_Completed;
            _currentScreen = scene;
        }

        private void Scene3_WalkingDog_Completed(IScreen sender)
        {
            Console.WriteLine("Not implemented.");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _currentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White); // TO DO - move this to the individual screens

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            _currentScreen.Draw();

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
