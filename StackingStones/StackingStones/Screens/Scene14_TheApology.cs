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
    public class Scene14_TheApology : ScreenBase, IScreen
    {
        private Sprite _black;
        private Sprite _background;

        private Sprite _lady;
        private Sprite _ladySurprised;
        private Sprite _puppers;
        private Sprite _fairy;

        public event ScreenEvent Completed;

        public Scene14_TheApology()
        {
            _black = new Sprite("Backgrounds\\black", new Vector2(0, 0), 1f, 1f, 0.5f);
            _background = new Sprite("Backgrounds\\houseExteriorDark2", new Vector2(0, 0), 0f, 1f, 0.5f);

            _lady = new Sprite("Sprites\\lady-scared", new Vector2(875, 50), 0f, 1f, 0.5f);
            _ladySurprised = new Sprite("Sprites\\lady-surprised", new Vector2(720, 50), 0f, 1f, 0.5f);
            _fairy = new Sprite("Sprites\\fairy", new Vector2(875, 50), 0f, 1f, 0.5f);
            _puppers = new Sprite("Sprites\\puppersTransformed", new Vector2(-100, 0), 0f, 1f, 0.5f);

            List<IEffect> effects = new List<IEffect>();
            effects.Add(new Fade(0f, 1f, 0.5f));

            var effect = new SimultaneousEffects(effects);
            effect.Completed += ScreenTransitioned;
            _background.Apply(effect);

            base.StartShowingMessage += Scene_StartShowingMessage;
            base.DoneShowingMessage += Scene_DoneShowingMessage;
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
            script.Dialogue.Add(new Dialogue("Old Lady", "Puppers?! What are you?", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Old Lady", "Please don't hurt me.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "Mother, do not fear. I could never hurt you.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "You and your family have been my companions and helped keep my woods for\ncenturies.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "I have nothing but gratitude for you and yours.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Old Lady", "I'll keep your secret puppers, I promise. I won't tell a soul.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "Mother, I would trust you with my kingdom, but there is evil lurking that you do not\nunderstand.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "You and I cannot stay her any longer.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "These woods we have stood vigil over must be left in the hands of man and we\nmust flee.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Old Lady", "Puppers, I can't go anywhere. I've lived here my entire life.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "Yes, I know you have, but there is no other way.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "I will not harm you but you cannot stay here.", Constants.SPEAKER_TEXT_COLOR));

            _textBox = new TextBox(Constants.TEXTBOX_POSITION, script);
            _textBox.Completed += TransitionToTransformation;
            _textBox.Show(true);

            _lady.Apply(new Fade(0f, 1f, 0.5f));
            _puppers.Apply(new Fade(0f, 1f, 0.5f));
        }

        private void TransitionToTransformation(TextBox sender)
        {
            _textBox.Hide(1f);
            _lady.Apply(new Fade(1f, 0f, 0.5f));

            var fade = new Fade(0f, 1f, 0.5f);
            fade.Completed += BeginTransformation;
            _ladySurprised.Apply(fade);
        }

        private void BeginTransformation(IEffect sender)
        {
            SoundEffect sound = Game1.ContentManager.Load<SoundEffect>("SoundEffects\\216089__richerlandtv__magic");
            sound.Play();
            _ladySurprised.Apply(new Fade(1f, 0f, 0.2f));
            var fade = new Fade(0f, 1f, 0.2f);
            fade.Completed += TransformationComplete;
            _fairy.Apply(fade);
        }

        private void TransformationComplete(IEffect sender)
        {
            _fairy.Apply(new Fade(1f, 0f, 0.5f));

            var script = new Script();
            script.Dialogue = new List<Dialogue>();
            script.Dialogue.Add(new Dialogue("Puppers?", "As for you meddling children...", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "You will stay here and witness your folly.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers?", "I cannot guard this forest so you will be at the mercy of the men who live here.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Teenagers", "*moan*", Constants.SPEAKER_TEXT_COLOR));

            _textBox = new TextBox(Constants.TEXTBOX_POSITION, script);
            _textBox.Completed += PuppersLeaving;
            _textBox.Show(true);
        }

        private void PuppersLeaving(TextBox sender)
        {
            _textBox.Hide(1f);
            var fade = new Fade(1f, 0f, 0.5f);
            fade.Completed += FadeOutScene;
            _puppers.Apply(fade);
        }

        private void FadeOutScene(IEffect sender)
        {
            Music.FadeToVolume(0f, 0.5f);
            var fade = new Fade(1f, 0f, 0.5f);
            fade.Completed += Fade_Completed;
            _background.Apply(fade);
        }

        private void Fade_Completed(IEffect sender)
        {
            if (Completed != null)
                Completed(this);
        }

        public void Draw()
        {
            _black.Draw();
            _background.Draw();
            _puppers.Draw();
            _lady.Draw();
            _ladySurprised.Draw();
            _fairy.Draw();
            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _black.Update(gameTime);
            _background.Update(gameTime);
            _textBox.Update(gameTime);
            _lady.Update(gameTime);
            _ladySurprised.Update(gameTime);
            _puppers.Update(gameTime);
            Music.Update(gameTime);
            _fairy.Update(gameTime);
        }
    }
}
