using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;
using Microsoft.Xna.Framework.Media;
using StackingStones.Effects;
using StackingStones.Models;
using Microsoft.Xna.Framework.Audio;

namespace StackingStones.Screens
{
    public class Scene12_TeenConfrontation : ScreenBase, IScreen
    {
        private Sprite _black;
        private Sprite _background;

        private Sprite _ladyAngry;
        private Sprite _teens;
        private Sprite _puppers;
        private Sprite _puppersTransformed;

        public event ScreenEvent Completed;

        public Scene12_TeenConfrontation()
        {
            _black = new Sprite("Backgrounds\\black", new Vector2(0, 0), 1f, 1f, 0.5f);
            _background = new Sprite("Backgrounds\\houseExteriorDark2", new Vector2(0, 0), 0f, 1f, 0.5f);

            _ladyAngry = new Sprite("Sprites\\lady-angry", new Vector2(10, 100), 0f, 1f, 0.5f);
            _teens = new Sprite("Sprites\\hoodedTeens", new Vector2(775, 100), 0f, 1f, 0.5f);
            _puppers = new Sprite("Sprites\\puppersPreTransformed", new Vector2(200, 100), 0f, 1f, 0.5f);
            _puppersTransformed = new Sprite("Sprites\\puppersTransformed", new Vector2(100, 0), 0f, 1f, 0.5f);

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

            _teens.Apply(new Fade(0f, 1f, 0.5f));
            _ladyAngry.Apply(new Fade(0f, 1f, 0.5f));
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
            //Music.Play("Music\\563169_Ghostly-Ambience", 1f, true);
            RitualFadeInComplete(null);

            var script = new Script();
            script.Dialogue = new List<Dialogue>();
            script.Dialogue.Add(new Dialogue("Teenagers", "Verum in forma transmutare obsecro te, quis magicis creaturis saltus.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Teenagers", "Confracta, signa, et portae ejus consumptae sunt ultima reserata.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Teenagers", "[sound SoundEffects\\34878__sruddi1__whine1]Nunc ostende te. Ut mandetis ea!", Constants.SPEAKER_TEXT_COLOR));

            _textBox = new TextBox(Constants.TEXTBOX_POSITION, script);
            _textBox.Completed += RitualComplete;
            _textBox.Show(false);
        }

        private void RitualComplete(TextBox sender)
        {
            SoundEffect bark = Game1.ContentManager.Load<SoundEffect>("SoundEffects\\transformation");
            bark.Play();


            _textBox.Hide(1f);
            _background.RemoveAllEffects();

            var fade = new Fade(_background.Alpha, 0f, 0.5f);
            fade.Completed += FinalRitualFadeOutCompleted;
            _background.Apply(fade);
        }

        private void FinalRitualFadeOutCompleted(IEffect sender)
        {
            // fade back in, with puppers
            _ladyAngry.Apply(new Fade(1f, 0f, 0.5f));
            _teens.Apply(new Fade(1f, 0f, 0.5f));

            var fade = new Fade(0f, 1f, 0.15f);
            fade.Completed += StartTransformation;
            _puppers.Apply(fade);
            _background.Apply(new Fade(0f, 1f, 0.15f));
        }

        private void StartTransformation(IEffect sender)
        {
            _puppers.Apply(new Fade(1f, 0f, 0.5f));
            var fade = new Fade(0f, 1f, 0.15f);
            fade.Completed += TransformationComplete;
            _puppersTransformed.Apply(fade);
        }

        private void TransformationComplete(IEffect sender)
        {
            _puppersTransformed.Apply(new Pan(new Vector2(_puppersTransformed.Position.X, _puppersTransformed.Position.Y), new Vector2(-100, _puppersTransformed.Position.Y), 0.5f));
            _teens.Apply(new Fade(0f, 1f, 0.5f));

            var script = new Script();
            script.Dialogue = new List<Dialogue>();
            script.Dialogue.Add(new Dialogue("Teenager", "My apologies lord, I did not recognize you. ", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "Do not call me lord, foolish child.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "You have doomed yourselves and you have slaughtered innocents.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Teenagers", "Y...you can't hurt us. We know the rules you are bound to. ", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "You know nothing.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "You have been manipulated.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "You have shared secrets that were not meant to be shared.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "You have brought an audience to a private ceremony and by the same rights you\ninvoke I am forced to contain these secrets.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Teenagers", "No, that isn't right.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Teenagers", "I followed the instructions, you came to us and I offered myself.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Teenagers", "Now you are bound by ancient laws to take me into your court to live forever.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "My court?", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "Child, I hold court wherever the green things grow and the wild things crawl.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "My court is everywhere the starlight shines at night and where the lichens glow in the\ndarkest reaches.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "But you are right, I will keep you in my court as the ceremony expects.", Constants.SPEAKER_TEXT_COLOR));

            _textBox = new TextBox(Constants.TEXTBOX_POSITION, script);
            _textBox.Completed += TransitionToNextScreen;
            _textBox.Show(false);
        }

        private void TransitionToNextScreen(TextBox sender)
        {
            _textBox.Hide(1f);
            var fade = new Fade(1f, 0f, 0.5f);
            fade.Completed += DoneFadeOut;
            _background.Apply(fade);
            _teens.Apply(new Fade(1f, 0f, 0.5f));
            _puppersTransformed.Apply(new Fade(1f, 0f, 0.5f));
        }

        private void DoneFadeOut(IEffect sender)
        {
            if (Completed != null)
                Completed(this);
        }

        private void RitualFadeOutComplete(IEffect sender)
        {
            var fade = new Fade(0f, 1f, 0.3f);
            fade.Completed += RitualFadeInComplete;
            _background.Apply(fade);
        }

        private void RitualFadeInComplete(IEffect sender)
        {
            var fade = new Fade(_background.Alpha, 0f, 0.3f);
            fade.Completed += RitualFadeOutComplete;
            _background.Apply(fade);
        }

        public void Draw()
        {
            _black.Draw();
            _background.Draw();
            _puppers.Draw();
            _puppersTransformed.Draw();
            _teens.Draw();
            _ladyAngry.Draw();
            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _black.Update(gameTime);
            _background.Update(gameTime);
            _textBox.Update(gameTime);
            _ladyAngry.Update(gameTime);
            _teens.Update(gameTime);
            _puppersTransformed.Update(gameTime);
            _puppers.Update(gameTime);
            Music.Update(gameTime);
        }
    }
}
