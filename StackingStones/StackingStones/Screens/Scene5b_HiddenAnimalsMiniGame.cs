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
    public class Scene5b_HiddenAnimalsMiniGame : ScreenBase, IScreen
    {
        private Sprite _background;
        private ScreenInteraction _explore;
        private HotSpot _toad;
        private HotSpot _bear;
        private HotSpot _porcupine;
        private HotSpot _wolf;
        private HotSpot _skunk;
        private HotSpot _bird;
        private HotSpot _deer;
        private bool _done;

        public event ScreenEvent Completed;

        public Scene5b_HiddenAnimalsMiniGame()
        {
            _done = false;

            _background = new Sprite("Backgrounds\\hiddenAnimals", new Vector2(0, 0), 0f, 1f, 0.5f);
            var fade = new Fade(0f, 1f, 0.5f);
            fade.Completed += Fade_Completed;
            _background.Apply(fade);
            
            List<HotSpot> hotSpots = new List<HotSpot>();

            _toad = new HotSpot(new Rectangle(0, 247, 101, 140), "Toad");
            _toad.Clicked += Toad_Clicked;
            hotSpots.Add(_toad);

            _bear = new HotSpot(new Rectangle(143, 457, 225, 146), "Bear");
            _bear.Clicked += Bear_Clicked;
            hotSpots.Add(_bear);

            _porcupine = new HotSpot(new Rectangle(438, 546, 168, 75), "Porcupine");
            _porcupine.Clicked += Porcupine_Clicked;
            hotSpots.Add(_porcupine);

            _wolf = new HotSpot(new Rectangle(522, 267, 84, 83), "Wolf");
            _wolf.Clicked += Wolf_Clicked;
            hotSpots.Add(_wolf);

            _bird = new HotSpot(new Rectangle(627, 267, 57, 46), "Bird");
            _bird.Clicked += Bird_Clicked;
            hotSpots.Add(_bird);

            _skunk = new HotSpot(new Rectangle(1133, 517, 100, 67), "Skunk");
            _skunk.Clicked += Skunk_Clicked;
            hotSpots.Add(_skunk);

            _deer = new HotSpot(new Rectangle(1110, 203, 81, 136), "Deer");
            _deer.Clicked += Deer_Clicked;
            hotSpots.Add(_deer);

            _explore = new ScreenInteraction(false, hotSpots);
            this.StartShowingMessage += Scene5b_HiddenAnimalsMiniGame_StartShowingMessage;
            this.DoneShowingMessage += Scene5b_HiddenAnimalsMiniGame_DoneShowingMessage;
        }

        private void Fade_Completed(IEffect sender)
        {
            _explore.Active = true;
        }

        private void Scene5b_HiddenAnimalsMiniGame_DoneShowingMessage(object sender, EventArgs e)
        {
            _explore.Active = true;

            bool clickedAll = _deer.HasBeenClicked && _skunk.HasBeenClicked && _bird.HasBeenClicked && _wolf.HasBeenClicked && _porcupine.HasBeenClicked && _bear.HasBeenClicked && _toad.HasBeenClicked;

            if (clickedAll && _done == false)
            {
                _done = true;
                ShowMessage("We saw all our friends today, Puppers, and I think we may have made a new one.\nStrange how they all get along isn't it.");
            }
            else if (clickedAll && _done)
            {
                if (Completed != null)
                    Completed(this);
            }
        }

        private void Scene5b_HiddenAnimalsMiniGame_StartShowingMessage(object sender, EventArgs e)
        {
            _explore.Active = false;
        }

        private void Deer_Clicked(HotSpot sender)
        {
            ShowMessage("Gasp! Look there Puppers, it's a deer. That's a new one isn't it! We'll have to think\nof a name for him.");
        }

        private void Skunk_Clicked(HotSpot sender)
        {
            ShowMessage("Hello Pierre, don't worry, I won't bother you today.");
        }

        private void Bird_Clicked(HotSpot sender)
        {
            ShowMessage("Listen to her sing."); // TO DO - play a sound
        }

        private void Wolf_Clicked(HotSpot sender)
        {
            ShowMessage("I see you Mr. Wolf, always skulking about. Don't you even think twice about laying\na paw on Puppers!");
        }

        private void Porcupine_Clicked(HotSpot sender)
        {
            ShowMessage("Careful with that one Puppers. He'll prick your nose if you're not careful.");
        }

        private void Bear_Clicked(HotSpot sender)
        {
            ShowMessage("Oh look, it's mother bear. We rescued her and her cubs a decade ago from a trap\ndown by the old river.");
        }

        private void Toad_Clicked(HotSpot sender)
        {
            ShowMessage("I like to call him Todd the toad. He's not very friendly, but I suppose that's true\nfor most toads.");
        }

        public void Draw()
        {
            _background.Draw();
            _explore.Draw();
            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _background.Update(gameTime);
            _explore.Update(gameTime);
            _textBox.Update(gameTime);
        }
    }
}
