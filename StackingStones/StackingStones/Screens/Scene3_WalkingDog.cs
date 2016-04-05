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
    public class Scene3_WalkingDog : ScreenBase, IScreen
    {
        private Sprite _background;
        public ScreenInteraction _explore;
        private Sprite _puppers;

        public event ScreenEvent Completed;
        
        public Scene3_WalkingDog()
        {
            Music.Play("Music\\506950_wandschrank---Waldigkeit", 0f, true);
            Music.FadeToVolume(0.5f, 0.25f);

            _background = new Sprite("Backgrounds\\Placeholders\\houseExterior", new Vector2(0, 0), 0f, 2f, 0.5f);
            _puppers = new Sprite("Sprites\\puppersNoLeash", new Vector2(800, 0), 0f, 1f, 0.5f);

            List<IEffect> effects = new List<IEffect>();
            effects.Add(new Fade(0f, 1f, 0.5f));
            effects.Add(new Zoom(2f, 1f, new Vector2(0, 0), 0.5f));

            var effect = new SimultaneousEffects(effects);
            effect.Completed += ScreenTransitioned;
            _background.Apply(effect);

            base.StartShowingMessage += Scene3_WalkingDog_StartShowingMessage;
            base.DoneShowingMessage += Scene3_WalkingDog_DoneShowingMessage;

            InitializeExploration();
        }

        private void Scene3_WalkingDog_DoneShowingMessage(object sender, EventArgs e)
        {
            _explore.Active = true;
        }

        private void Scene3_WalkingDog_StartShowingMessage(object sender, EventArgs e)
        {
            _explore.Active = false;
        }

        private void ScreenTransitioned(IEffect sender)
        {
            _explore.Active = true;
        }

        private void InitializeExploration()
        {
            var hotSpots = new List<HotSpot>();

            var puppers = new HotSpot(new Rectangle(706, 466, 159, 134), "Puppers");
            puppers.Clicked += Puppers_Clicked;
            hotSpots.Add(puppers);

            var trees = new HotSpot(new Rectangle(474, 8, 806, 430), "The woods");
            trees.Clicked += Trees_Clicked;
            hotSpots.Add(trees);

            var cabin = new HotSpot(new Rectangle(0, 0, 297, 531), "Cabin");
            cabin.Clicked += Cabin_Clicked;
            hotSpots.Add(cabin);

            var path = new HotSpot(new Rectangle(904, 438, 114, 116), "Path");
            path.Clicked += Path_Clicked;
            hotSpots.Add(path);

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

        private void Cabin_Clicked(HotSpot sender)
        {
            ShowMessage("It may be drafty, but it sure has stood the test of time.");
        }

        private void Trees_Clicked(HotSpot sender)
        {
            ShowMessage("You'd think I'd be sick of these trees after having lived here so long, but I get excited\nfor each walk. You never know what you'll see!");
        }

        private void Puppers_Clicked(HotSpot sender)
        {
            _puppers.Apply(new Fade(0f, 1f, 1f));
            var dialogue = new List<Dialogue>();
            dialogue.Add(new Dialogue("Puppers", "*[sound SoundEffects\\328729__ivolipa__dog-bark]Woof woof!*", Constants.SPEAKER_TEXT_COLOR));
            dialogue.Add(new Dialogue("", "[event hidePuppers]For all the years he's been chasing squirrels, I don't think he has ever caught one.", Constants.NARATOR_TEXT_COLOR));
            ShowMessage(dialogue);
        }

        protected override void Message_ScriptedEventReached(TextBox sender, string eventId)
        {
            if (eventId == "hidePuppers")
                _puppers.Apply(new Fade(1f, 0f, 1f));
        }

        public void Draw()
        {
            _background.Draw();
            _puppers.Draw();
            _explore.Draw();
            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _background.Update(gameTime);
            _puppers.Update(gameTime);
            _explore.Update(gameTime);
            _textBox.Update(gameTime);
            Music.Update(gameTime);
        }
    }
}
