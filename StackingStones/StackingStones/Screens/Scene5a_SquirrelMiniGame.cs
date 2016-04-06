using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;
using StackingStones.Effects;
using StackingStones.MiniGames.Squirrel;
using StackingStones.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace StackingStones.Screens
{
    public class Scene5a_SquirrelMiniGame : ScreenBase, IScreen
    {
        private Sprite _background;
        private Sprite _tree;
        private List<Sprite> _counters;

        private int _hits;
        private Critter _activeCritter;

        private List<Critter> _leftCritters;
        private List<Critter> _rightCritters;

        private MouseHelper _mouse;
        private Sprite _paw;

        private SoundEffect _hitSound;
        private SoundEffect _badSound;
        private bool _gameActive;
        private bool _gameWon;

        public event ScreenEvent Completed;
        
        public Scene5a_SquirrelMiniGame()
        {
            _gameActive = false;
            _hits = 0;
            _background = new Sprite("MiniGames\\Squirrel\\background", new Vector2(0, 0), 0f, 1f, 0.5f);

            _counters = new List<Sprite>();

            _counters.Add(new Sprite("MiniGames\\Squirrel\\counter", new Vector2(10, 10), 0f, 1f, 0.5f));
            _counters.Add(new Sprite("MiniGames\\Squirrel\\counter", new Vector2(60, 10), 0f, 1f, 0.5f));
            _counters.Add(new Sprite("MiniGames\\Squirrel\\counter", new Vector2(110, 10), 0f, 1f, 0.5f));

            _hitSound = Game1.ContentManager.Load<SoundEffect>("SoundEffects\\328729__ivolipa__dog-bark");
            _badSound = Game1.ContentManager.Load<SoundEffect>("SoundEffects\\34878__sruddi1__whine1");

            _tree = new Sprite("MiniGames\\Squirrel\\tree", new Vector2(0, 0), 0f, 1f, 0.5f);

            Fade fade = new Fade(0f, 1f, 0.5f);
            fade.Completed += Fade_Completed;
            _background.Apply(fade);

            this.DoneShowingMessage += Scene5a_SquirrelMiniGame_DoneShowingMessage;
            InitializeCritters();
            _tree.Alpha = 1f; // to do - remove

            _mouse = new MouseHelper();
            _mouse.MouseButtonPressed += _mouse_MouseButtonPressed;

            _paw = new Sprite("MiniGames\\Squirrel\\paw", new Vector2(0, 0), 1f, 1f, 1f);
        }

        private void _mouse_MouseButtonPressed(MouseHelper sender, MouseButtons button, Microsoft.Xna.Framework.Input.MouseState state)
        {
            if (_gameActive)
            {
                Rectangle collisionRectangle = new Rectangle(state.X + 9, state.Y + 18, 90, 94);
                if (_activeCritter.Intersects(collisionRectangle))
                {
                    Console.WriteLine("Clicked on " + _activeCritter.Name);
                    if (_activeCritter.Name == "Squirrel")
                    {
                        _hitSound.Play();
                        _hits++;
                        _counters[_hits - 1].Alpha = 1f;
                        if (_hits >= 3)
                            EndGame();
                    }
                    else if (_hits != 0)
                    {
                        _badSound.Play();
                        _counters[_hits - 1].Alpha = 0f;
                        _hits--;
                    }
                    else
                        _badSound.Play();
                }
            }
        }

        private void EndGame()
        {
            _gameWon = true;
            _gameActive = false;
            ShowMessage("Looks like Puppers is finally tired of playing. We should be moving on.");                    
        }

        private void InitializeCritters()
        {
            _leftCritters = new List<Critter>();
            _rightCritters = new List<Critter>();

            InitializeLeftPorcupine();
            InitializeLeftToad();
            InitializeLeftSquirrel();
            InitializeLeftSkunk();

            InitializeRightPorcupine();
            InitializeRightToad();
            InitializeRightSquirrel();
            InitializeRightSkunk();
        }

        private void InitializeLeftSkunk()
        {
            var paths = new List<Path>();
            paths.Add(new Path(new Vector2(400, 203), 260));
            var skunk = new Critter("Skunk", "MiniGames\\Squirrel\\skunk", paths);
            skunk.CompletedMovement += CritterDoneMoving;

            _leftCritters.Add(skunk);
        }

        private void InitializeLeftSquirrel()
        {
            var paths = new List<Path>();
            paths.Add(new Path(new Vector2(550, 203), 360));
            paths.Add(new Path(new Vector2(550, 143), 410));
            paths.Add(new Path(new Vector2(550, 323), 300));
            var squirrel = new Critter("Squirrel", "MiniGames\\Squirrel\\squirrel", paths);
            squirrel.CompletedMovement += CritterDoneMoving;

            _leftCritters.Add(squirrel);
        }

        private void InitializeLeftToad()
        {
            var paths = new List<Path>();
            paths.Add(new Path(new Vector2(500, 203), 400));
            paths.Add(new Path(new Vector2(500, 143), 410));
            paths.Add(new Path(new Vector2(500, 323), 290));
            var toad = new Critter("Toad", "MiniGames\\Squirrel\\toad", paths);
            toad.CompletedMovement += CritterDoneMoving;

            _leftCritters.Add(toad);
        }

        private void InitializeLeftPorcupine()
        {
            var paths = new List<Path>();
            paths.Add(new Path(new Vector2(400, 203), 200));
            var porcupine = new Critter("Porcupine", "MiniGames\\Squirrel\\porcupine", paths);
            porcupine.CompletedMovement += CritterDoneMoving;

            _leftCritters.Add(porcupine);
        }

        private void InitializeRightSkunk()
        {
            var paths = new List<Path>();
            paths.Add(new Path(new Vector2(650, 153), 880));
            paths.Add(new Path(new Vector2(650, 203), 950));
            var skunk = new Critter("Skunk", "MiniGames\\Squirrel\\skunkRight", paths);
            skunk.CompletedMovement += CritterDoneMoving;

            _rightCritters.Add(skunk);
        }

        private void InitializeRightSquirrel()
        {
            var paths = new List<Path>();
            paths.Add(new Path(new Vector2(650, 153), 880));
            paths.Add(new Path(new Vector2(650, 253), 920));
            var squirrel = new Critter("Squirrel", "MiniGames\\Squirrel\\squirrelRight", paths);
            squirrel.CompletedMovement += CritterDoneMoving;

            _rightCritters.Add(squirrel);
        }

        private void InitializeRightToad()
        {
            var paths = new List<Path>();
            paths.Add(new Path(new Vector2(650, 153), 920));
            paths.Add(new Path(new Vector2(650, 253), 960));
            var toad = new Critter("Toad", "MiniGames\\Squirrel\\toadRight", paths);
            toad.CompletedMovement += CritterDoneMoving;

            _rightCritters.Add(toad);
        }

        private void InitializeRightPorcupine()
        {
            var paths = new List<Path>();
            paths.Add(new Path(new Vector2(650, 153), 800));
            paths.Add(new Path(new Vector2(650, 123), 860));
            var porcupine = new Critter("Porcupine", "MiniGames\\Squirrel\\porcupineRight", paths);
            porcupine.CompletedMovement += CritterDoneMoving;

            _rightCritters.Add(porcupine);
        }

        private void CritterDoneMoving(object sender, EventArgs e)
        {
            if (_gameActive)
            {
                Random rnd = new Random();
                var side = rnd.Next(0, 2);

                List<Critter> critters;

                if (side == 0)
                    critters = _rightCritters;
                else if (side == 1)
                    critters = _leftCritters;
                else
                    throw new Exception("This shouldn't happen");

                var index = rnd.Next(0, critters.Count);

                _activeCritter = critters[index];
                _activeCritter.Start();
            }
        }

        private void Scene5a_SquirrelMiniGame_DoneShowingMessage(object sender, EventArgs e)
        {
            if (_gameWon)
            {
                Music.FadeToVolume(0f, 1f);
                Fade fade = new Fade(1f, 0f, 1f);
                fade.Completed += ScreenFadeOutDone;
                _background.Apply(fade);
                _tree.Alpha = 0f;
                
            }
            else
            {
                _tree.Alpha = 1f;
                _gameActive = true;
                CritterDoneMoving(null, null);
            }
        }

        private void ScreenFadeOutDone(IEffect sender)
        {
            if (Completed != null)
                Completed(this);
        }

        private void Fade_Completed(IEffect sender)
        {
            ShowMessage("Bop the squirrel 3 times.\nWatch out for other critters.");
        }

        public void Draw()
        {
            _background.Draw();

            if (_gameActive)
            {
                foreach (var sprite in _leftCritters)
                    sprite.Draw();

                foreach (var sprite in _rightCritters)
                    sprite.Draw();
            }

            _tree.Draw();

            if(_gameActive)
            {
                foreach (var counter in _counters)
                    counter.Draw();
                _paw.Draw();
            }
            

            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _background.Update(gameTime);
            _tree.Update(gameTime);

            foreach (var sprite in _leftCritters)
                sprite.Update(gameTime);

            foreach (var sprite in _rightCritters)
                sprite.Update(gameTime);

            _textBox.Update(gameTime);
            _mouse.Update();
            foreach (var counter in _counters)
                counter.Update(gameTime);

            MouseState state = Mouse.GetState();
            _paw.Position = new Vector2(state.X, state.Y);

            Music.Update(gameTime);
        }
    }
}
