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
    public class Scene8_Gully : ScreenBase, IScreen
    {
        private Sprite _background;
        private Sprite _teens;

        public event ScreenEvent Completed;

        public Scene8_Gully()
        {
            _background = new Sprite("Backgrounds\\Gully", new Vector2(0, 0), 0f, 1f, 0.5f);

            List<IEffect> effects = new List<IEffect>();
            effects.Add(new Pan(new Vector2(0, 0), new Vector2(0, -200), 0.5f));
            effects.Add(new Fade(0f, 1f, 0.25f));

            var transition = new SimultaneousEffects(effects);
            transition.Completed += Transition_Completed;
            _background.Apply(transition);

            _teens = new Sprite("Sprites\\teens", new Vector2(675, 100), 0f, 1f, 0.5f);
        }

        private void Transition_Completed(IEffect sender)
        {
            _teens.Apply(new Fade(0f, 1f, 1f));

            Script script = new Script();
            List<string> responseOptions = new List<string>();
            responseOptions.Add("I'm the one who should be asking questions. What are you doing in my woods?");
            responseOptions.Add("Hello there! I live just the other side of the woods. This is my property. Are you lost?");
            script.Choice = new Choice("", responseOptions, Constants.CHOICE_POSITION);
            script.Choice.ChoiceSelected += GreetingCompleted;
            script.Dialogue = new List<Dialogue>();
            script.Dialogue.Add(new Dialogue("Teenagers", "Hey, who's there?!", Constants.SPEAKER_TEXT_COLOR, 50));

            _textBox = new TextBox(Constants.TEXTBOX_POSITION, script);
            _textBox.Show(true);
        }

        private void GreetingCompleted(Choice sender)
        {
            if(sender.SelectedChoiceIndex == 0)
            {
                // grumpy response
                // fade in grumpy lady
                Script script = new Script();

                script.Dialogue = new List<Dialogue>();
                script.Dialogue.Add(new Dialogue("Teenagers", "What do you mean \"your woods\"? How can you own the woods?\nThey don't belong to anyone!", Constants.SPEAKER_TEXT_COLOR, 50));
                script.Dialogue.Add(new Dialogue("Old Lady", "Well it's actually very simple, but I'll let the sheriff explain.", Constants.SPEAKER_TEXT_COLOR, 50));
                script.Dialogue.Add(new Dialogue("Old Lady", "You've got about an hour to get the hell off my property before he comes and\nteaches you a bit about private property.", Constants.SPEAKER_TEXT_COLOR, 50));

                _textBox = new TextBox(Constants.TEXTBOX_POSITION, script);
                _textBox.Completed += DoneTalkingWithTeens;
                _textBox.Show(false);
            }
            else
            {
                // friendly response
                // fade in friendly lady
                Script script = new Script();
                List<string> responseOptions = new List<string>();
                responseOptions.Add("Yes actually and I would appreciate it if you didn't leave your trash in the forest and if you wouldn't leave your little piles of art laying all over either. ");
                responseOptions.Add("Doesn't matter how I found you. This is private property and I'm afraid I have to ask you to leave.");
                responseOptions.Add("Its not safe for kids to go trapsing about the woods. Do your parents know you're here?");

                script.Choice = new Choice("", responseOptions, Constants.CHOICE_POSITION);
                script.Choice.ChoiceSelected += SelectedHowTheyFoundTheTeens;

                script.Dialogue = new List<Dialogue>();
                script.Dialogue.Add(new Dialogue("Teenagers", "No, we're not lost, are you?", Constants.SPEAKER_TEXT_COLOR, 50));
                script.Dialogue.Add(new Dialogue("Teenagers", "Did you follow the stones here?", Constants.SPEAKER_TEXT_COLOR, 50));

                _textBox = new TextBox(Constants.TEXTBOX_POSITION, script);
                _textBox.Show(false);
            }
        }

        private void SelectedHowTheyFoundTheTeens(Choice sender)
        {
            if(sender.SelectedChoiceIndex == 0)
            {
                Script script = new Script();
                script.Dialogue = new List<Dialogue>();
                script.Dialogue.Add(new Dialogue("Teenagers", "You know its not art. We put them there to lead you to us. ", Constants.SPEAKER_TEXT_COLOR, 50));
                script.Dialogue.Add(new Dialogue("Old Lady", "No actually, I don't know what they are, but you're right they aren't art.\nYou need to leave now, go home.", Constants.SPEAKER_TEXT_COLOR, 50));

                _textBox = new TextBox(Constants.TEXTBOX_POSITION, script);
                _textBox.Completed += DoneTalkingWithTeens;
                _textBox.Show(false);
            }
            else if(sender.SelectedChoiceIndex == 1)
            {
                Script script = new Script();
                script.Dialogue = new List<Dialogue>();
                script.Dialogue.Add(new Dialogue("Teenagers", "How can you own the woods? They belong to everyone!", Constants.SPEAKER_TEXT_COLOR, 50));
                script.Dialogue.Add(new Dialogue("Old Lady", "Well its actually very simple but I'll let the sheriff explain.\nYou've got about an hour to get the hell off my property before he comes and\nteaches you a bit about private property.", Constants.SPEAKER_TEXT_COLOR, 50));

                _textBox = new TextBox(Constants.TEXTBOX_POSITION, script);
                _textBox.Completed += DoneTalkingWithTeens;
                _textBox.Show(false);
            }
            else
            {
                Script script = new Script();
                script.Dialogue = new List<Dialogue>();
                script.Dialogue.Add(new Dialogue("Teenagers", "Yeah sure they know.", Constants.SPEAKER_TEXT_COLOR, 50));
                script.Dialogue.Add(new Dialogue("Teenagers", "Hey listen, we come out here all the time. Don't worry about us.", Constants.SPEAKER_TEXT_COLOR, 50));
                script.Dialogue.Add(new Dialogue("Old Lady", "No, you listen.\nThis is my property and I don't want to be responsible for you kids getting hurt on it.", Constants.SPEAKER_TEXT_COLOR, 50));
                script.Dialogue.Add(new Dialogue("Old Lady", "I saw a bear on my way over here and I'm pretty sure you wouldn't be fine if you ran\ninto her.\nCome with me, we'll call your parents to come get you.", Constants.SPEAKER_TEXT_COLOR, 50));
                script.Dialogue.Add(new Dialogue("Teenagers", "No thanks, lady.", Constants.SPEAKER_TEXT_COLOR, 50));
                // TO DO - make the kids run off
                _textBox = new TextBox(Constants.TEXTBOX_POSITION, script);
                _textBox.Completed += DoneTalkingWithTeens;
                _textBox.Show(false);
            }
        }

        private void DoneTalkingWithTeens(TextBox sender)
        {
            _textBox.Hide(1f);
            _teens.Apply(new Fade(1f, 0f, 1f));
            var fade = new Fade(1f, 0f, 0.5f);
            fade.Completed += FadeOutCompleted;
            _background.Apply(fade);
        }

        private void FadeOutCompleted(IEffect sender)
        {
            if (Completed != null)
                Completed(this);
        }

        public void Draw()
        {
            _background.Draw();
            _teens.Draw();

            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _background.Update(gameTime);
            _teens.Update(gameTime);

            _textBox.Update(gameTime);
            Music.Update(gameTime);
        }
    }
}
