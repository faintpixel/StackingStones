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
    class Scene4_WalkingInWoods : ScreenBase, IScreen
    {
        private Sprite _background;
        private Sprite _smallSquirrel;
        private Sprite _smallBird;
        private Sprite _squirrel;
        private ScreenInteraction _explore;

        public event ScreenEvent Completed;
        public event ScreenEvent StartSquirrelMiniGame;
        public event ScreenEvent StartHiddenAnimalsMiniGame;

        public Scene4_WalkingInWoods()
        {
            _background = new Sprite("Backgrounds\\Placeholders\\walkingInWoods", new Vector2(0, 0), 0f, 1f, 0.5f);
            _background.Apply(new Fade(0f, 1f, 0.5f));

            _smallSquirrel = new Sprite("Sprites\\squirrel2", new Vector2(977, 473), 0f, 0.12f, 0.5f);
            _smallBird = new Sprite("Sprites\\bird", new Vector2(506, 80), 0f, 0.12f, 0.5f);

            _squirrel = new Sprite("Sprites\\squirrel", new Vector2(-300, 550), 1f, 0.30f, 1f);

            List<IEffect> effects = new List<IEffect>();
            effects.Add(new Pan(_squirrel.Position, new Vector2(-75, _squirrel.Position.Y), 0.5f));
            effects.Add(new Wait(1500));
            effects.Add(new Sound("SoundEffects\\122261__echobones__angry-squirrel1-edited"));
            effects.Add(new Pan(new Vector2(-75, _squirrel.Position.Y), new Vector2(1280, _squirrel.Position.Y), 4f));

            var effect = new MultiStageEffect(effects);
            effect.Completed += SquirrelDoneRunningAround;
            _squirrel.Apply(effect);

            InitializeExplore();

            this.DoneShowingMessage += Scene4_WalkingInWoods_DoneShowingMessage;
            this.StartShowingMessage += Scene4_WalkingInWoods_StartShowingMessage;
        }

        private void Scene4_WalkingInWoods_StartShowingMessage(object sender, EventArgs e)
        {
            _explore.Active = false;
        }

        private void Scene4_WalkingInWoods_DoneShowingMessage(object sender, EventArgs e)
        {
            _explore.Active = true;
        }

        private void InitializeExplore()
        {
            List<HotSpot> hotSpots = new List<HotSpot>();

            var bird = new HotSpot(new Rectangle(493, 70, 119, 92), "Bird");
            bird.Clicked += Bird_Clicked;
            hotSpots.Add(bird);

            var squirrel = new HotSpot(new Rectangle(962, 474, 138, 90), "Squirrel");
            squirrel.Clicked += Squirrel_Clicked;
            hotSpots.Add(squirrel);

            var arrow = new HotSpot(new Rectangle(1148, 580, 115, 106), "Continue on path");
            arrow.Clicked += Arrow_Clicked;
            hotSpots.Add(arrow);

            _explore = new ScreenInteraction(false, hotSpots);
        }

        private void Arrow_Clicked(HotSpot sender)
        {
            Console.WriteLine("Not implemented yet.");
        }

        private void Squirrel_Clicked(HotSpot sender)
        {
            var dialogue = new List<Dialogue>();
            dialogue.Add(new Dialogue("Old Lady", "Oh dear, not again!", Color.SaddleBrown));
            dialogue.Add(new Dialogue("Old Lady", "Get back here Puppers!", Color.SaddleBrown));
            ShowMessage(dialogue);
            this.DoneShowingMessage += DoneShowingSquirrelMessage;
        }

        private void DoneShowingSquirrelMessage(object sender, EventArgs e)
        {
            if (StartSquirrelMiniGame != null)
                StartSquirrelMiniGame(this);
        }

        private void Bird_Clicked(HotSpot sender)
        {
            var dialogue = new List<Dialogue>();
            dialogue.Add(new Dialogue("Old Lady", "Oh look Puppers, it's a song bird!", Color.SaddleBrown));
            dialogue.Add(new Dialogue("Puppers", "*[sound SoundEffects\\328729__ivolipa__dog-bark]Woof woof!*", Color.SaddleBrown));
            dialogue.Add(new Dialogue("Old Lady", "Hm? Do you see something else boy?", Color.SaddleBrown));
            ShowMessage(dialogue);
            this.DoneShowingMessage += DoneShowingBirdMessage;
        }

        private void DoneShowingBirdMessage(object sender, EventArgs e)
        {
            if (StartHiddenAnimalsMiniGame != null)
                StartHiddenAnimalsMiniGame(this);
        }

        private void SquirrelDoneRunningAround(IEffect sender)
        {
            _smallSquirrel.Apply(new Fade(0f, 1f, 0.5f));
            var birdFade = new Fade(0f, 1f, 0.25f);
            birdFade.Completed += BirdFade_Completed;
            _smallBird.Apply(birdFade);
            
        }

        private void BirdFade_Completed(IEffect sender)
        {
            // TO DO - play a bird sound
            _explore.Active = true;
        }

        public void Draw()
        {
            _background.Draw();
            _smallSquirrel.Draw();
            _smallBird.Draw();
            _squirrel.Draw();
            _explore.Draw();
            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _background.Update(gameTime);
            _smallSquirrel.Update(gameTime);
            _smallBird.Update(gameTime);
            _squirrel.Update(gameTime);
            _explore.Update(gameTime);
            _textBox.Update(gameTime);
        }
    }
}
