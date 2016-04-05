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
        private IScreen _savedScreen;

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

            //Scene2_House_Completed(null);
            //Scene3_WalkingDog_Completed(null);
            //Scene_StartHiddenAnimalsMiniGame(null);
            //Scene_StartSquirrelMiniGame(null);

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
            var scene = new Scene4_WalkingInWoods();
            scene.Completed += Scene4_WalkingInWoods_Completed;
            scene.StartHiddenAnimalsMiniGame += Scene_StartHiddenAnimalsMiniGame;
            scene.StartSquirrelMiniGame += Scene_StartSquirrelMiniGame;
            _currentScreen = scene;
        }

        private void Scene_StartSquirrelMiniGame(IScreen sender)
        {
            var screen = new Scene5a_SquirrelMiniGame();
            screen.Completed += Scene5a_SquirrelGame_Completed;
            _savedScreen = _currentScreen;
            _currentScreen = screen;
        }

        private void Scene5a_SquirrelGame_Completed(IScreen sender)
        {
            // TO DO - mark squirrel game completed
            _currentScreen = _savedScreen;
        }

        private void Scene_StartHiddenAnimalsMiniGame(IScreen sender)
        {
            var scene = new Scene5b_HiddenAnimalsMiniGame();
            scene.Completed += Scene5b_HiddenAnimals_Completed;
            _savedScreen = _currentScreen;

            _currentScreen = scene;
        }

        private void Scene5b_HiddenAnimals_Completed(IScreen sender)
        {
            _currentScreen = _savedScreen;
        }

        private void Scene4_WalkingInWoods_Completed(IScreen sender)
        {
            Console.WriteLine("not implemented yet.");
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
