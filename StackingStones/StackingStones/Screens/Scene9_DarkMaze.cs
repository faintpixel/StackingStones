using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;
using StackingStones.Effects;
using StackingStones.Models;

namespace StackingStones.Screens
{
    public class Scene9_DarkMaze : ScreenBase, IScreen
    {
        private Sprite _black;
        private Sprite _dayBackground;
        private Sprite _background;

        private Sprite _wolf;
        private HotSpot _wolfHotSpot;

        private Sprite _middleStone;
        private HotSpot _middleStoneHotSpot;

        private Sprite _bear;
        private HotSpot _bearHotSpot;

        private List<Sprite> _leftStones;
        private List<Sprite> _rightStones;
        private List<Sprite> _topStones;

        private int _sceneIndex;

        private ScreenInteraction _explore;

        public event ScreenEvent Completed;

        public Scene9_DarkMaze()
        {
            _sceneIndex = 0;

            _dayBackground = new Sprite("Backgrounds\\woodMaze", new Vector2(0, 0), 0f, 1f, 0.5f);
            _background = new Sprite("MiniGames\\DarkMaze\\darkMaze", new Vector2(0, 0), 0f, 1f, 0.5f);
            _black = new Sprite("Backgrounds\\black", new Vector2(0, 0), 0f, 1f, 0.5f);


            _bear = new Sprite("MiniGames\\DarkMaze\\bear", new Vector2(998, 210), 0f, 1f, 0.5f);
            _wolf = new Sprite("MiniGames\\DarkMaze\\wolf", new Vector2(445, 244), 0f, 1f, 0.5f);
            _middleStone = new Sprite("MiniGames\\DarkMaze\\stone1", new Vector2(639, 466), 0f, 1f, 0.5f);

            var fade = new Fade(0f, 1f, 0.5f);
            fade.Completed += Fade_Completed;
            _dayBackground.Apply(fade);

            List<HotSpot> hotSpots = new List<HotSpot>();
            var left = new HotSpot(new Rectangle(158, 602, 94, 90), "Left");
            left.Clicked += PathClicked;
            hotSpots.Add(left);

            var right = new HotSpot(new Rectangle(1063, 611, 109, 81), "Right");
            right.Clicked += PathClicked;
            hotSpots.Add(right);

            var top = new HotSpot(new Rectangle(652, 301, 86, 92), "Forward");
            top.Clicked += PathClicked;
            hotSpots.Add(top);

            _middleStoneHotSpot = new HotSpot(new Rectangle(639, 466, 120, 102), "Stones");
            _middleStoneHotSpot.Clicked += _stone_Clicked;
            _middleStoneHotSpot.Active = false;
            hotSpots.Add(_middleStoneHotSpot);

            _bearHotSpot = new HotSpot(new Rectangle(998, 210, 119, 142), "Bear");
            _bearHotSpot.Clicked += _bearHotSpot_Clicked;
            _bearHotSpot.Active = false;
            hotSpots.Add(_bearHotSpot);

            _wolfHotSpot = new HotSpot(new Rectangle(445, 244, 164, 116), "Wolf");
            _wolfHotSpot.Clicked += _wolfHotSpot_Clicked;
            _wolfHotSpot.Active = false;
            hotSpots.Add(_wolfHotSpot);

            var leftStone = new HotSpot(new Rectangle(263, 491, 94, 94), "Stones");
            leftStone.Clicked += _stone_Clicked;
            leftStone.Active = false;
            hotSpots.Add(leftStone);

            var rightStone = new HotSpot(new Rectangle(897, 479, 95, 106), "Stones");
            rightStone.Clicked += _stone_Clicked;
            rightStone.Active = false;
            hotSpots.Add(rightStone);

            var topStone = new HotSpot(new Rectangle(767, 301, 90, 92), "Stones");
            topStone.Clicked += _stone_Clicked;
            topStone.Active = false;
            hotSpots.Add(topStone);

            _explore = new ScreenInteraction(false, hotSpots);

            InitializeStones();

            this.StartShowingMessage += Scene_StartShowingMessage;
            this.DoneShowingMessage += Scene_DoneShowingMessage;

            Music.Play("Music\\563169_Ghostly-Ambience", 0f, true);
            Music.FadeToVolume(1f, 0.3f);
        }

        private void InitializeStones()
        {
            Vector2 leftStone = new Vector2(263, 491);
            Vector2 rightStone = new Vector2(897, 479);
            Vector2 topStone = new Vector2(767, 301);

            _leftStones = new List<Sprite>();
            _rightStones = new List<Sprite>();
            _topStones = new List<Sprite>();

            _leftStones.Add(new Sprite("MiniGames\\DarkMaze\\stone1", leftStone, 0f, 1f, 0.5f));
            _leftStones.Add(new Sprite("MiniGames\\DarkMaze\\stone2", leftStone, 0f, 1f, 0.5f));
            _leftStones.Add(new Sprite("MiniGames\\DarkMaze\\stone3", leftStone, 0f, 1f, 0.5f));
            _leftStones.Add(new Sprite("MiniGames\\DarkMaze\\stone1", leftStone, 0f, 1f, 0.5f));

            _rightStones.Add(new Sprite("MiniGames\\DarkMaze\\stone2", rightStone, 0f, 1f, 0.5f));
            _rightStones.Add(new Sprite("MiniGames\\DarkMaze\\stone3", rightStone, 0f, 1f, 0.5f));
            _rightStones.Add(new Sprite("MiniGames\\DarkMaze\\stone1", rightStone, 0f, 1f, 0.5f));
            _rightStones.Add(new Sprite("MiniGames\\DarkMaze\\stone2", rightStone, 0f, 1f, 0.5f));

            _topStones.Add(new Sprite("MiniGames\\DarkMaze\\stone3", topStone, 0f, 1f, 0.5f));
            _topStones.Add(new Sprite("MiniGames\\DarkMaze\\stone1", topStone, 0f, 1f, 0.5f));
            _topStones.Add(new Sprite("MiniGames\\DarkMaze\\stone2", topStone, 0f, 1f, 0.5f));
            _topStones.Add(new Sprite("MiniGames\\DarkMaze\\stone3", topStone, 0f, 1f, 0.5f));
        }

        private void _wolfHotSpot_Clicked(HotSpot sender)
        {
            ShowMessage("I know Mr. Wolf is still around here somewhere. He's never far off.");
        }

        private void _bearHotSpot_Clicked(HotSpot sender)
        {
            ShowMessage("Oh hello mother bear! You don't look happy at all. I'd better be on my way then.");
        }

        private void _stone_Clicked(HotSpot sender)
        {
            ShowMessage("How can there be so many? Who could have set them up so quickly?"); 
        }

        private void PathClicked(HotSpot sender)
        {
            _explore.Active = false;
            // check if it's the last one
            if (_sceneIndex == 3)
            {
                var fade = new Fade(1f, 0f, 0.5f);
                fade.Completed += ExitMaze;
                _leftStones[_sceneIndex].Apply(fade);
                _rightStones[_sceneIndex].Apply(new Fade(1f, 0f, 0.5f));
                _topStones[_sceneIndex].Apply(new Fade(1f, 0f, 0.5f));
                _dayBackground.Alpha = 0f;
                _black.Alpha = 1f;
                _background.Apply(new Fade(1f, 0f, 0.5f));
                _bear.Apply(new Fade(1f, 0f, 0.5f));
            }
            else
            {
                var fade = new Fade(1f, 0f, 1f);
                fade.Completed += PreviousViewFadedOut;
                _leftStones[_sceneIndex].Apply(fade);
                _rightStones[_sceneIndex].Apply(new Fade(1f, 0f, 1f));
                _topStones[_sceneIndex].Apply(new Fade(1f, 0f, 1f));

                _middleStoneHotSpot.Active = false;
                _bearHotSpot.Active = false;
                _wolfHotSpot.Active = false;
                _middleStone.Apply(new Fade(_middleStone.Alpha, 0f, 1f));
                _bear.Apply(new Fade(_bear.Alpha, 0f, 1f));
                _wolf.Apply(new Fade(_wolf.Alpha, 0f, 1f));
            }
        }

        private void ExitMaze(IEffect sender)
        {
            if (Completed != null)
                Completed(this);
        }

        private void PreviousViewFadedOut(IEffect sender)
        {
            _sceneIndex++;

            var fade = new Fade(0f, 1f, 1f);
            fade.Completed += NewViewFadedIn;
            _leftStones[_sceneIndex].Apply(fade);
            _rightStones[_sceneIndex].Apply(new Fade(0f, 1f, 1f));
            _topStones[_sceneIndex].Apply(new Fade(0f, 1f, 1f));

            if(_sceneIndex == 1)
            {
                _middleStone.Apply(new Fade(0f, 1f, 1f));
                _middleStoneHotSpot.Active = true;
            }
            else if(_sceneIndex == 2)
            {
                _wolf.Apply(new Fade(0f, 1f, 1f));
                _wolfHotSpot.Active = true;
            }
            else if (_sceneIndex == 3)
            {
                _bear.Apply(new Fade(0f, 1f, 1f));
                _bearHotSpot.Active = true;
            }
        }

        private void NewViewFadedIn(IEffect sender)
        {
            _explore.Active = true;
        }

        private void Scene_DoneShowingMessage(object sender, EventArgs e)
        {
            _explore.Active = true;
        }

        private void Scene_StartShowingMessage(object sender, EventArgs e)
        {
            _explore.Active = false;
        }

        private void Fade_Completed(IEffect sender)
        {
            var fade = new Fade(0f, 1f, 0.2f);
            fade.Completed += StartNighttime;
            _background.Apply(fade);

            _leftStones[_sceneIndex].Apply(new Fade(0f, 1f, 0.2f));
            _rightStones[_sceneIndex].Apply(new Fade(0f, 1f, 0.2f));
            _topStones[_sceneIndex].Apply(new Fade(0f, 1f, 0.2f));
        }

        private void StartNighttime(IEffect sender)
        {
            _explore.Active = true;
        }

        private void FadeOutCompleted(IEffect sender)
        {
            if (Completed != null)
                Completed(this);
        }

        public void Draw()
        {
            _black.Draw();
            _dayBackground.Draw();
            _background.Draw();
            foreach (var stone in _leftStones)
                stone.Draw();
            foreach (var stone in _rightStones)
                stone.Draw();
            foreach (var stone in _topStones)
                stone.Draw();

            _wolf.Draw();
            _bear.Draw();
            _middleStone.Draw();

            _explore.Draw();

            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _background.Update(gameTime);
            _dayBackground.Update(gameTime);
            _black.Update(gameTime);
            _explore.Update(gameTime);
            foreach (var stone in _leftStones)
                stone.Update(gameTime);
            foreach (var stone in _rightStones)
                stone.Update(gameTime);
            foreach (var stone in _topStones)
                stone.Update(gameTime);

            _bear.Update(gameTime);
            _middleStone.Update(gameTime);
            _wolf.Update(gameTime);

            _textBox.Update(gameTime);
            Music.Update(gameTime);
        }

        private enum Direction
        {
            Left = 0,
            Right = 1,
            Top = 2
        }
    }
}
