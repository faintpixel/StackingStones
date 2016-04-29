using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;
using Microsoft.Xna.Framework.Media;
using StackingStones.Effects;
using StackingStones.Models;

namespace StackingStones.Screens
{
    public class Scene10_DarkHouseExterior : ScreenBase, IScreen
    {
        private Sprite _black;
        private Sprite _background;
        private ScreenInteraction _explore;

        public event ScreenEvent Completed;

        public Scene10_DarkHouseExterior()
        {
            _black = new Sprite("Backgrounds\\black", new Vector2(0, 0), 1f, 1f, 0.5f);
            _background = new Sprite("Backgrounds\\houseExteriorDark", new Vector2(0, 0), 0f, 1f, 0.5f);

            List<IEffect> effects = new List<IEffect>();
            effects.Add(new Fade(0f, 1f, 0.2f));

            var effect = new SimultaneousEffects(effects);
            effect.Completed += ScreenTransitioned;
            _background.Apply(effect);

            base.StartShowingMessage += Scene_StartShowingMessage;
            base.DoneShowingMessage += Scene_DoneShowingMessage;

            InitializeExploration();
        }

        private void Scene_DoneShowingMessage(object sender, EventArgs e)
        {
            _explore.Active = true;
        }

        private void Scene_StartShowingMessage(object sender, EventArgs e)
        {
            _explore.Active = false;
        }

        private void ScreenTransitioned(IEffect sender)
        {
            List<Dialogue> dialogue = new List<Dialogue>();
            dialogue.Add(new Dialogue("Old Lady", "What the hell is happening?", Constants.SPEAKER_TEXT_COLOR));
            dialogue.Add(new Dialogue("Old Lady", "Is somebody out there?", Constants.SPEAKER_TEXT_COLOR));
            dialogue.Add(new Dialogue("Old Lady", "Who put these stones here?", Constants.SPEAKER_TEXT_COLOR));
            dialogue.Add(new Dialogue("Puppers", "[sound SoundEffects\\34878__sruddi1__whine1]*Whimper*", Constants.SPEAKER_TEXT_COLOR));

            ShowMessage(dialogue);
        }

        private void InitializeExploration()
        {
            var hotSpots = new List<HotSpot>();

            var trees = new HotSpot(new Rectangle(674, 0, 606, 318), "The woods");
            trees.Clicked += Trees_Clicked;
            hotSpots.Add(trees);

            var door = new HotSpot(new Rectangle(37, 46, 207, 350), "Door");
            door.Clicked += Door_Clicked;
            hotSpots.Add(door);

            _explore = new ScreenInteraction(false, hotSpots);
        }

        private void Path_Clicked(HotSpot sender)
        {
            var script = new Script();
            script.Dialogue = new List<Dialogue>();

            script.Dialogue.Add(new Dialogue("", "Puppers looks ready to head into the woods.", Constants.NARATOR_TEXT_COLOR));

            var choices = new List<string>();
            choices.Add("Yes");
            choices.Add("Not yet");
            script.Choice = new Choice("Walk down the path?", choices, new Vector2(240, 500));
            script.Choice.ChoiceSelected += Choice_ChoiceSelected;

            _explore.Active = false;
            _textBox = new TextBox(new Vector2(240, 500), script);
            _textBox.ScriptedEventReached += Message_ScriptedEventReached;
            _textBox.Show(true);
        }

        private void Choice_ChoiceSelected(Choice sender)
        {
            _textBox.Hide(1f);

            if (sender.SelectedChoiceIndex == 0)
            {
                List<IEffect> effects = new List<IEffect>();
                effects.Add(new Fade(1f, 0f, 0.5f));
                effects.Add(new Zoom(1f, 2f, Vector2.Zero, 0.5f));
                effects.Add(new Pan(new Vector2(0, 0), new Vector2(-950, 0), 4.5f));

                SimultaneousEffects effect = new SimultaneousEffects(effects);
                effect.Completed += FadeOutCompleted;

                _background.Apply(effect);
            }
            else
                _explore.Active = true;
        }

        private void FadeOutCompleted(IEffect sender)
        {
            if (Completed != null)
                Completed(this);
        }

        private void Door_Clicked(HotSpot sender)
        {
            _explore.Active = false;
            var fade = new Fade(1f, 0f, 0.5f);
            fade.Completed += SceneDone;
            _background.Apply(fade);
        }

        private void SceneDone(IEffect sender)
        {
            if (Completed != null)
                Completed(this);
        }

        private void Trees_Clicked(HotSpot sender)
        {
            ShowMessage("I think that's enough walking for one night...");
        }

        public void Draw()
        {
            _black.Draw();
            _background.Draw();
            _explore.Draw();
            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _black.Update(gameTime);
            _background.Update(gameTime);
            _explore.Update(gameTime);
            _textBox.Update(gameTime);
            Music.Update(gameTime);
        }
    }
}
