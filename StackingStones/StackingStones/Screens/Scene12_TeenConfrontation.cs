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
    public class Scene12_TeenConfrontation : ScreenBase, IScreen
    {
        private Sprite _black;
        private Sprite _background;

        public event ScreenEvent Completed;

        public Scene12_TeenConfrontation()
        {
            _black = new Sprite("Backgrounds\\black", new Vector2(0, 0), 1f, 1f, 0.5f);
            _background = new Sprite("Backgrounds\\houseExteriorDark2", new Vector2(0, 0), 0f, 1f, 0.5f);

            List<IEffect> effects = new List<IEffect>();
            effects.Add(new Fade(0f, 1f, 0.2f));

            var effect = new SimultaneousEffects(effects);
            effect.Completed += ScreenTransitioned;
            _background.Apply(effect);

            base.StartShowingMessage += Scene_StartShowingMessage;
            base.DoneShowingMessage += Scene_DoneShowingMessage;

            Music.Play("Music\\587419_StainedGlassampSpookySkele", 0f, true);
            Music.FadeToVolume(0.2f, 0.2f);
        }

        private void Scene_DoneShowingMessage(object sender, EventArgs e)
        {
        }

        private void Scene_StartShowingMessage(object sender, EventArgs e)
        {
        }

        private void ScreenTransitioned(IEffect sender)
        {
            var script = new Script();
            script.Dialogue = new List<Dialogue>();
            script.Dialogue.Add(new Dialogue("Old Lady", "What's with all the rocks here? And why did you follow me home?", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Teenagers", "We stacked the stones and you followed them to us.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Teenagers", "We know who you are.", Constants.SPEAKER_TEXT_COLOR));

            var choices = new List<string>();
            choices.Add("Oh you do, do you?");
            choices.Add("Look, I don't know what kind of things you kids are into but its freaking me out...");
            choices.Add("Trespassing on my property, threatening me...");
            script.Choice = new Choice("", choices, Constants.CHOICE_POSITION);
            script.Choice.ChoiceSelected += FirstResponseToTeens;
            _textBox = new TextBox(Constants.TEXTBOX_POSITION, script);
            
            _textBox.Show(true);
        }

        private void FirstResponseToTeens(Choice sender)
        {
            var script = new Script();
            script.Dialogue = new List<Dialogue>();

            if (sender.SelectedChoiceIndex == 0)
            {
                script.Dialogue.Add(new Dialogue("Old Lady", "Well I may not know your names but I know exactly what you kids are on about.", Constants.SPEAKER_TEXT_COLOR));
                script.Dialogue.Add(new Dialogue("Teenagers", "Good, then you know why we are here and what we want.", Constants.SPEAKER_TEXT_COLOR));
                script.Dialogue.Add(new Dialogue("Teenager", "I have summoned you and I demand entrance to your court as is my right.", Constants.SPEAKER_TEXT_COLOR));
                script.Dialogue.Add(new Dialogue("Old Lady", "Oh jeeze, what is going on.", Constants.SPEAKER_TEXT_COLOR));
                script.Dialogue.Add(new Dialogue("Teenagers", "Humanity owes you a debt and it has been centuries since you collected.", Constants.SPEAKER_TEXT_COLOR));
                script.Dialogue.Add(new Dialogue("Old Lady", "No, you are making a mistake.", Constants.SPEAKER_TEXT_COLOR));
                script.Dialogue.Add(new Dialogue("Teenagers", "We know exactly what we're doing.", Constants.SPEAKER_TEXT_COLOR));
                script.Dialogue.Add(new Dialogue("Teenagers", "We're following the rules laid out by you.", Constants.SPEAKER_TEXT_COLOR));
            }
            else if(sender.SelectedChoiceIndex == 1)
            {
                script.Dialogue.Add(new Dialogue("Old Lady", "and I would appreciate it if you would kindly leave.", Constants.SPEAKER_TEXT_COLOR));
                script.Dialogue.Add(new Dialogue("Teenagers", "Leave? But we followed the instructions exactly.", Constants.SPEAKER_TEXT_COLOR));
                script.Dialogue.Add(new Dialogue("Teenagers", "Do you really not know what we are doing here?", Constants.SPEAKER_TEXT_COLOR));
                script.Dialogue.Add(new Dialogue("Old Lady", "I DO know what you're doing, you're breaking the law. You need to go. Now!", Constants.SPEAKER_TEXT_COLOR));
                script.Dialogue.Add(new Dialogue("Old Lady", "I've already called the sheriff.", Constants.SPEAKER_TEXT_COLOR));
                script.Dialogue.Add(new Dialogue("Teenagers", "I am sorry if this is a mistake but I cannot go back before I complete the ritual.", Constants.SPEAKER_TEXT_COLOR));
                script.Dialogue.Add(new Dialogue("Teenagers", "The people who told me about this place, who gave me the ritual, well I can't face\nthem again, not empty handed.", Constants.SPEAKER_TEXT_COLOR));
            }
            else
            {
                script.Dialogue.Add(new Dialogue("Old Lady", "The sherif is on his way and I promise you I will press charges.", Constants.SPEAKER_TEXT_COLOR));
                script.Dialogue.Add(new Dialogue("Teenagers", "You're testing us. You don't believe we'll go through with it.", Constants.SPEAKER_TEXT_COLOR));
                script.Dialogue.Add(new Dialogue("Teenagers", "Well we'll finish the ritual and you'll have no choice...", Constants.SPEAKER_TEXT_COLOR));
            }

            _textBox = new TextBox(Constants.TEXTBOX_POSITION, script);
            _textBox.Completed += StartTeenRitual;

            _textBox.Show(false);
        }

        private void StartTeenRitual(TextBox sender)
        {
            var script = new Script();
            script.Dialogue = new List<Dialogue>();
            script.Dialogue.Add(new Dialogue("Teenagers", "Verum in forma transmutare obsecro te, quis magicis creaturis saltus.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Teenagers", "Confracta, signa, et portae ejus consumptae sunt ultima reserata.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Teenagers", "Nunc ostende te. Ut mandetis ea!", Constants.SPEAKER_TEXT_COLOR));

            _textBox = new TextBox(Constants.TEXTBOX_POSITION, script);

            _textBox.Show(false);
        }

        public void Draw()
        {
            _black.Draw();
            _background.Draw();
            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _black.Update(gameTime);
            _background.Update(gameTime);
            _textBox.Update(gameTime);
            Music.Update(gameTime);
        }
    }
}
