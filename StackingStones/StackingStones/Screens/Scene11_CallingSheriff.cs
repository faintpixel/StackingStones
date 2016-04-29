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
    public class Scene11_CallingSheriff : ScreenBase, IScreen
    {
        private Sprite _black;
        private Sprite _background;
        private Sprite _backgroundOpenDoor;
        private Sprite _sheriffBackground;
        private ScreenInteraction _explore;
        private HotSpot _outside;
        private HotSpot _door;

        public event ScreenEvent Completed;

        public Scene11_CallingSheriff()
        {
            _black = new Sprite("Backgrounds\\black", new Vector2(0, 0), 1f, 1f, 0.5f);
            _background = new Sprite("Backgrounds\\houseInteriorDark", new Vector2(0, 0), 0f, 1f, 0.5f);
            _backgroundOpenDoor = new Sprite("Backgrounds\\houseInteriorDarkOpenDoor", new Vector2(0, 0), 0f, 1f, 0.5f);
            _sheriffBackground = new Sprite("Backgrounds\\Placeholders\\sheriffBackground", new Vector2(0, 0), 0f, 1f, 0.5f);

            List<IEffect> effects = new List<IEffect>();
            effects.Add(new Fade(0f, 1f, 0.2f));

            var effect = new SimultaneousEffects(effects);
            effect.Completed += ScreenTransitioned;
            _background.Apply(effect);

            Music.FadeToVolume(0f, 0.2f);

            this.DoneShowingMessage += Scene11_CallingSheriff_DoneShowingMessage;
            this.StartShowingMessage += Scene11_CallingSheriff_StartShowingMessage;

            InitializeExploration();
        }

        private void Scene11_CallingSheriff_StartShowingMessage(object sender, EventArgs e)
        {
            _explore.Active = false;
        }

        private void Scene11_CallingSheriff_DoneShowingMessage(object sender, EventArgs e)
        {
            _explore.Active = true;
        }

        private void InitializeExploration()
        {
            var hotSpots = new List<HotSpot>();

            var window = new HotSpot(new Rectangle(459, 93, 181, 150), "Window");
            window.Clicked += Window_Clicked;
            hotSpots.Add(window);

            _door = new HotSpot(new Rectangle(950, 8, 282, 509), "Door");
            _door.Clicked += Door_Clicked;
            hotSpots.Add(_door);

            _outside = new HotSpot(new Rectangle(950, 8, 282, 509), "Outside");
            _outside.Clicked += Outside_Clicked;
            _outside.Active = false;
            hotSpots.Add(_outside);

            _explore = new ScreenInteraction(false, hotSpots);
            _explore.Active = false;
        }

        private void Outside_Clicked(HotSpot sender)
        {
            ShowMessage("That's it. I've had enough of these shenanigans.");
        }

        private void Door_Clicked(HotSpot sender)
        {
            _backgroundOpenDoor.Apply(new Fade(0f, 1f, 0.5f));
            ShowMessage("There's nobody there?");
            _door.Active = false;
            _outside.Active = true;
        }

        private void Window_Clicked(HotSpot sender)
        {
            ShowMessage("It's so dark... I can't see a thing.");
        }

        private void StartConversationWithSheriff()
        {
            var script = new Script();
            script.Dialogue = new List<Dialogue>();
            script.Dialogue.Add(new Dialogue("Old Lady", "Sheriff I need you to get down here to the cabin right away.", Constants.SPEAKER_TEXT_COLOR));
            //script.Dialogue.Add(new Dialogue("Old Lady", "There are some kids trespassing here and I think they are trying to scare me or\nsomething.", Constants.SPEAKER_TEXT_COLOR));
            //script.Dialogue.Add(new Dialogue("Sheriff", "Slow down a second. What do you mean trying to scare you, what are they doing?", Constants.SPEAKER_TEXT_COLOR));
            //script.Dialogue.Add(new Dialogue("Old Lady", "Well, it sounds silly but they put these rocks all over my property.", Constants.SPEAKER_TEXT_COLOR));
            //script.Dialogue.Add(new Dialogue("Old Lady", "Like stacks of rocks.", Constants.SPEAKER_TEXT_COLOR));
            //script.Dialogue.Add(new Dialogue("Sheriff", "Well... ok.", Constants.SPEAKER_TEXT_COLOR));
            //script.Dialogue.Add(new Dialogue("Sheriff", "We'll send someone out there to get them off your property but I'm sure there\nisn't anything to be worried about.", Constants.SPEAKER_TEXT_COLOR));
            //script.Dialogue.Add(new Dialogue("Sheriff", "You remember what it was like to be a kid.", Constants.SPEAKER_TEXT_COLOR));
            //script.Dialogue.Add(new Dialogue("Old Lady", "I'm not an idiot, Sheriff.", Constants.SPEAKER_TEXT_COLOR));
            //script.Dialogue.Add(new Dialogue("Old Lady", "I know it sounds dumb but you'll have to see for yourself its really creepy.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Sheriff", "Ok, we'll see you soon. Just stay in your home. ", Constants.SPEAKER_TEXT_COLOR));

            _textBox = new TextBox(Constants.TEXTBOX_POSITION, script);
            _textBox.Completed += DoneTalkingWithSheriff;
            _textBox.Show(true);

            _sheriffBackground.Apply(new Fade(0f, 1f, 0.5f));
        }

        private void DoneTalkingWithSheriff(TextBox sender)
        {
            _textBox.Hide(0.5f);
            var fade = new Fade(1f, 0f, 0.5f);
            fade.Completed += SheriffTransitionCompleted;
            _sheriffBackground.Apply(fade);
        }

        private void SheriffTransitionCompleted(IEffect sender)
        {
            var script = new Script();
            script.Dialogue = new List<Dialogue>();
            script.Dialogue.Add(new Dialogue("Old Lady", "*Sigh*", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Old Lady", "Well, nothing to do but wait for the sheriff I suppose.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Old Lady", "Lets find you a snack P[sound SoundEffects\\knock]u-", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Old Lady", "[event spookyMusic]Hm? That can't be the sheriff already...", Constants.SPEAKER_TEXT_COLOR));
            
            _textBox = new TextBox(Constants.TEXTBOX_POSITION, script);
            _textBox.Completed += DoorKnocked;
            _textBox.ScriptedEventReached += _textBox_ScriptedEventReached;
            _textBox.Show(true);
        }

        private void _textBox_ScriptedEventReached(TextBox sender, string eventId)
        {
            if (eventId == "spookyMusic")
            {
                Music.Play("Music\\563169_Ghostly-Ambience", 0f, true);
                Music.FadeToVolume(1f, 0.5f);
            }              

        }

        private void DoorKnocked(TextBox sender)
        {
            _textBox.Hide(0.5f);
            _explore.Active = true;
        }

        private void ScreenTransitioned(IEffect sender)
        {
            var script = new Script();
            script.Dialogue = new List<Dialogue>();
            script.Dialogue.Add(new Dialogue("Old Lady", "I've had just about enough of this.", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Old Lady", "Come on Puppers, lets call the sheriff.", Constants.SPEAKER_TEXT_COLOR));

            _textBox = new TextBox(Constants.TEXTBOX_POSITION, script);
            _textBox.Completed += IntroCompleted;
            _textBox.Show(true);
        }

        private void IntroCompleted(TextBox sender)
        {
            StartConversationWithSheriff();
        }

        public void Draw()
        {
            _black.Draw();
            _background.Draw();
            _backgroundOpenDoor.Draw();
            _sheriffBackground.Draw();
            _textBox.Draw();
            _explore.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _black.Update(gameTime);
            _background.Update(gameTime);
            _backgroundOpenDoor.Update(gameTime);
            _sheriffBackground.Update(gameTime);
            _textBox.Update(gameTime);
            Music.Update(gameTime);
            _explore.Update(gameTime);
        }
    }
}
