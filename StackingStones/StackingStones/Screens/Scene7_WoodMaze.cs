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
    public class Scene7_WoodMaze : ScreenBase, IScreen
    {
        private Sprite _background;

        private Sprite _wolf;
        private HotSpot _wolfHotSpot;

        private Sprite _trash;
        private HotSpot _trashHotSpot;

        private Sprite _bear;
        private HotSpot _bearHotSpot;

        private List<Sprite> _stones;
        private int _stoneIndex;

        private ScreenInteraction _explore;
        private int _timesFollowedStones;
        private int _timesNotFollowedStones;

        public event ScreenEvent Completed;

        public Scene7_WoodMaze()
        {
            _stones = new List<Sprite>();
            _timesFollowedStones = 0;
            _timesNotFollowedStones = 0;

            _background = new Sprite("Backgrounds\\Placeholders\\woodMaze", new Vector2(0, 0), 0f, 1f, 0.5f);
            _stones.Add(new Sprite("MiniGames\\WoodMaze\\leftStones", new Vector2(87, 360), 0f, 1f, 0.5f));
            _stones.Add(new Sprite("MiniGames\\WoodMaze\\rightStones", new Vector2(1080, 414), 0f, 1f, 0.5f));
            _stones.Add(new Sprite("MiniGames\\WoodMaze\\topStones", new Vector2(721, 214), 0f, 1f, 0.5f));

            _stones[0].Apply(new Fade(0f, 1f, 0.5f));

            _bear = new Sprite("MiniGames\\WoodMaze\\bear", new Vector2(987, 263), 0f, 1f, 0.5f);
            _wolf = new Sprite("MiniGames\\WoodMaze\\wolf", new Vector2(183, 233), 0f, 1f, 0.5f);
            _trash = new Sprite("MiniGames\\WoodMaze\\trash", new Vector2(685, 360), 0f, 1f, 0.5f);

            var fade = new Fade(0f, 1f, 0.5f);
            fade.Completed += Fade_Completed;
            _background.Apply(fade);

            List<HotSpot> hotSpots = new List<HotSpot>();
            var left = new HotSpot(new Rectangle(43, 583, 88, 84), "Left");
            left.Clicked += Left_Clicked;
            hotSpots.Add(left);

            var right = new HotSpot(new Rectangle(1134, 590, 82, 77), "Right");
            right.Clicked += Right_Clicked;
            hotSpots.Add(right);

            var top = new HotSpot(new Rectangle(605, 273, 71, 71), "Forward");
            top.Clicked += Top_Clicked;
            hotSpots.Add(top);

            _trashHotSpot = new HotSpot(new Rectangle(685, 360, 130, 117), "Litter");
            _trashHotSpot.Clicked += _trashHotSpot_Clicked;
            _trashHotSpot.Active = false;
            hotSpots.Add(_trashHotSpot);

            _bearHotSpot = new HotSpot(new Rectangle(987, 263, 188, 129), "Bear");
            _bearHotSpot.Clicked += _bearHotSpot_Clicked;
            _bearHotSpot.Active = false;
            hotSpots.Add(_bearHotSpot);

            _wolfHotSpot = new HotSpot(new Rectangle(183, 233, 151, 105), "Wolf");
            _wolfHotSpot.Clicked += _wolfHotSpot_Clicked;
            _wolfHotSpot.Active = false;
            hotSpots.Add(_wolfHotSpot);

            _explore = new ScreenInteraction(false, hotSpots);

            this.StartShowingMessage += Scene7_WoodMaze_StartShowingMessage;
            this.DoneShowingMessage += Scene7_WoodMaze_DoneShowingMessage;
        }

        private void _wolfHotSpot_Clicked(HotSpot sender)
        {
            ShowMessage("Mr. Wolf again! If I didn't know better I'd think you were following me.");
        }

        private void _bearHotSpot_Clicked(HotSpot sender)
        {
            ShowMessage("Oh, she's taking a nap.\nI certainly wouldn't want to wake a sleeping bear. Better go on my way.");
        }

        private void _trashHotSpot_Clicked(HotSpot sender)
        {
            ShowMessage("That answers that!\nSomeone is here that shouldn't be and throwing their trash about the place as well.\nWait til I get my hands on them!"); // TO DO - grumpy lady
        }

        private void Left_Clicked(HotSpot sender)
        {
            PathClicked(Direction.Left);
        }

        private void Right_Clicked(HotSpot sender)
        {
            PathClicked(Direction.Right);
        }

        private void Top_Clicked(HotSpot sender)
        {
            PathClicked(Direction.Top);
        }

        private void PathClicked(Direction direction)
        {
            Direction stoneDirection;
            if (_stoneIndex == 0)
                stoneDirection = Direction.Left;
            else if (_stoneIndex == 1)
                stoneDirection = Direction.Right;
            else
                stoneDirection = Direction.Top;

            bool followingStones = direction == stoneDirection;
            ChangeScene(followingStones);
        }

        private void Scene7_WoodMaze_DoneShowingMessage(object sender, EventArgs e)
        {
            _explore.Active = true;
        }

        private void Scene7_WoodMaze_StartShowingMessage(object sender, EventArgs e)
        {
            _explore.Active = false;
        }

        private void Fade_Completed(IEffect sender)
        {
            _explore.Active = true;
        }

        private void ChangeDynamicHotSpot(bool show, Sprite sprite, HotSpot hotSpot)
        {
            if (show)
                sprite.Apply(new Fade(0f, 1f, 1f));
            else
                sprite.Apply(new Fade(1f, 0f, 1f));

            hotSpot.Active = show;
        }

        private void ChangeScene(bool followedStones)
        {
            Console.WriteLine("Followed stones: " + followedStones);
            if (followedStones)
                _timesFollowedStones++;

            if (_timesFollowedStones >= 3)
                LeaveMaze();
            else
            {
                _timesNotFollowedStones++;
              
                ShowNextStone();

                if (_timesNotFollowedStones == 1)
                    ChangeDynamicHotSpot(true, _trash, _trashHotSpot);
                else if(_timesNotFollowedStones == 2)
                {
                    ChangeDynamicHotSpot(false, _trash, _trashHotSpot);
                    ChangeDynamicHotSpot(true, _bear, _bearHotSpot);
                }
                else if (_timesNotFollowedStones == 3)
                {
                    ChangeDynamicHotSpot(false, _bear, _bearHotSpot);
                    ChangeDynamicHotSpot(true, _wolf, _wolfHotSpot);
                }
                else if (_timesNotFollowedStones == 4)
                {
                    ChangeDynamicHotSpot(false, _wolf, _wolfHotSpot);
                }
                else
                {
                    ShowMessage("I wonder if these stones lead somewhere...");
                    _timesNotFollowedStones = 0;
                }

                // show one of the bear, wolf, or trash
            }
        }

        private void LeaveMaze()
        {
            _explore.Active = false;
            var fade = new Fade(1f, 0f, 1f);
            fade.Completed += FadeOutCompleted;
            _background.Apply(fade);

            _stones[_stoneIndex].Apply(new Fade(1f, 0f, 1f));
            _bear.Apply(new Fade(_bear.Alpha, 0f, 1f));
            _trash.Apply(new Fade(_trash.Alpha, 0f, 1f));
            _wolf.Apply(new Fade(_wolf.Alpha, 0f, 1f));
        }

        private void FadeOutCompleted(IEffect sender)
        {
            if (Completed != null)
                Completed(this);
        }

        public void Draw()
        {
            _background.Draw();
            foreach (var stone in _stones)
                stone.Draw();
            _wolf.Draw();
            _bear.Draw();
            _trash.Draw();

            _explore.Draw();

            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _background.Update(gameTime);
            _explore.Update(gameTime);
            foreach (var stone in _stones)
                stone.Update(gameTime);

            _bear.Update(gameTime);
            _trash.Update(gameTime);
            _wolf.Update(gameTime);

            _textBox.Update(gameTime);
        }

        private void ShowNextStone()
        {
            Sprite previousStone = _stones[_stoneIndex];

            previousStone.Apply(new Fade(previousStone.Alpha, 0f, 1f));

            if (_stoneIndex + 1 >= 3)
                _stoneIndex = 0;
            else
                _stoneIndex++;

            Sprite newStone = _stones[_stoneIndex];
            newStone.Apply(new Fade(0f, 1f, 1f));
        }

        private enum Direction
        {
            Left = 0,
            Right = 1,
            Top = 2
        }
    }
}
