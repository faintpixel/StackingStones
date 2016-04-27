using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;
using StackingStones.Effects;
using StackingStones.Models;
using Microsoft.Xna.Framework.Media;

namespace StackingStones.Screens
{
    public class Scene2_House : ScreenBase, IScreen
    {
        private Sprite _houseExteriorPanorama;
        private Sprite _houseInterior;
        private Sprite _lady;
        private Sprite _puppers;
        private Sprite _leash;
        private Sprite _oldPhoto;
        private ScreenInteraction _findTheLeash;
        private bool _foundLeash;
        private HotSpot _door;

        public event ScreenEvent Completed;

        public Scene2_House()
        {
            Music.Play("Music\\449897_Prologos", 0.5f, false);

            _houseExteriorPanorama = new Sprite("Backgrounds\\Placeholders\\houseByForest", new Vector2(0, 0), 0f, 1f, 0.5f);
            _houseInterior = new Sprite("Backgrounds\\houseInterior", new Vector2(0, 0), 0f, 1f, 0.5f);

            _oldPhoto = new Sprite("Sprites\\oldPhoto", new Vector2(100, 10), 0f, 1f, 0.5f);
            _leash = new Sprite("Sprites\\leash", new Vector2(100, 10), 0f, 1f, 0.5f);

            var effects = new List<IEffect>();
            effects.Add(new Fade(0f, 1f, 0.45f));
            effects.Add(new Pan(new Vector2(0, 0), new Vector2(-1280, 0), 0.5f)); // TO DO - reset speed to 0.5
            var effect = new MultiStageEffect(effects);
            effect.Completed += ExteriorPanCompleted;
            
            _houseExteriorPanorama.Apply(effect);

            var script = new Script();
            script.Dialogue.Add(new Dialogue("", "My family settled this land years ago.", Constants.NARATOR_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("", "It's just me and [sound SoundEffects\\328729__ivolipa__dog-bark]Puppers now.", Constants.NARATOR_TEXT_COLOR));

            script.Dialogue.Add(new Dialogue("Old Lady", "[event enterHouse]Alright Puppers, I hear you. Time for your walk is it?", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Puppers", "[event showPuppers]*[sound SoundEffects\\328729__ivolipa__dog-bark]Bark bark!*", Constants.SPEAKER_TEXT_COLOR));
            script.Dialogue.Add(new Dialogue("Old Lady", "*Chuckles*\nWell, I suppose it's a beautiful day for it.\nNow where did I put that darn leash?", Constants.SPEAKER_TEXT_COLOR));

            List<string> choices = new List<string>();
            choices.Add("Take Puppers for a walk.");

            script.Choice = new Choice("", choices, new Vector2(240, 500));
            _textBox = new TextBox(new Vector2(240, 500), script);
            _textBox.ScriptedEventReached += Message_ScriptedEventReached;
            _textBox.Completed += StartLookingForLeash;

            _lady = new Sprite("Sprites\\lady", new Vector2(0, 0), 0f, 1f, 0.5f);
            _puppers = new Sprite("Sprites\\puppersNoLeash", new Vector2(800, 0), 0f, 1f, 0.5f);

            base.DoneShowingMessage += Scene2_House_DoneShowingMessage;
            base.StartShowingMessage += Scene2_House_StartShowingMessage;
            InitializeFindTheLeash();
        }

        private void Scene2_House_StartShowingMessage(object sender, EventArgs e)
        {
            _findTheLeash.Active = false;
        }

        private void Scene2_House_DoneShowingMessage(object sender, EventArgs e)
        {
            _findTheLeash.Active = true;

            if (_oldPhoto.Alpha != 0)
                _oldPhoto.Apply(new Fade(1f, 0f, 1f));

            if (_leash.Alpha != 0)
                _leash.Apply(new Fade(1f, 0f, 1f));
        }

        private void InitializeFindTheLeash()
        {
            _foundLeash = false;
            List<HotSpot> hotSpots = new List<HotSpot>();

            var oldPhoto = new HotSpot(new Rectangle(248, 216, 70, 60), "Old photo");
            oldPhoto.Clicked += OldPhoto_Clicked;
            hotSpots.Add(oldPhoto);

            _door = new HotSpot(new Rectangle(943, 0, 286, 516), "Door");
            _door.Clicked += Door_Clicked;
            hotSpots.Add(_door);

            var window = new HotSpot(new Rectangle(447, 87, 204, 159), "Window");
            window.Clicked += Window_Clicked;
            hotSpots.Add(window);

            var cupboard = new HotSpot(new Rectangle(212, 330, 246, 126), "Cupboard");
            cupboard.Clicked += Cupboard_Clicked;
            hotSpots.Add(cupboard);
            _findTheLeash = new ScreenInteraction(false, hotSpots);
        }

        private void Cupboard_Clicked(HotSpot sender)
        {
            _leash.Apply(new Fade(0f, 1f, 0.25f));
            List<string> messages = new List<string>();
            messages.Add("Hmm what's in here...\nAh, found the leash!");
            messages.Add("Now we can go for our walk, Puppers.");
            ShowMessage(messages);

            _door.HasBeenClicked = false;

            _foundLeash = true;
        }

        private void Window_Clicked(HotSpot sender)
        {
            ShowMessage("Warm and sunny. Just the way Puppers likes it!");
        }

        private void Door_Clicked(HotSpot sender)
        {
            if(_foundLeash)
            {
                FinishedLeashMinigame();
            }
            else
                ShowMessage("Wait, I still haven't found that darn leash."); // TO DO - add whining dog sound
        }

        private void OldPhoto_Clicked(HotSpot sender)
        {
            _oldPhoto.Apply(new Fade(0f, 1f, 0.75f));
            ShowMessage("Mom and dad. I miss them every day.");
        }

        private void FinishedLeashMinigame()
        {
            var script = new Script();
            script.Dialogue = new List<Dialogue>();
            script.Dialogue.Add(new Dialogue("Old Lady", "Here we go![sound SoundEffects\\328729__ivolipa__dog-bark] You better behave this time Puppers. No chasing squirrels today!", Constants.SPEAKER_TEXT_COLOR));

            _findTheLeash.Active = false;
            _textBox = new TextBox(new Vector2(240, 500), script);
            _textBox.Completed += findTheLeashGameCompleted;
            _textBox.Show(true);
        }

        private void findTheLeashGameCompleted(TextBox sender)
        {
            Music.FadeToVolume(0f, 0.25f);
            _textBox.Hide(1f);
            List<IEffect> effects = new List<IEffect>();
            effects.Add(new Fade(1f, 0f, 0.5f));
            effects.Add(new Zoom(1f, 2f, Vector2.Zero, 0.5f));
            effects.Add(new Pan(new Vector2(0, 0), new Vector2(-1300, 0), 4.5f));

            SimultaneousEffects effect = new SimultaneousEffects(effects);
            effect.Completed += SceneTransitionCompleted;
            _houseInterior.Apply(effect);
        }

        private void SceneTransitionCompleted(IEffect sender)
        {
            if (Completed != null)
                Completed(this);
        }

        private void StartLookingForLeash(TextBox sender)
        {
            _lady.Apply(new Fade(1f, 0f, 0.75f));
            _puppers.Apply(new Fade(1f, 0f, 0.75f));
            _textBox.Hide(1f);
            _findTheLeash.Active = true;
        }

        protected override void Message_ScriptedEventReached(TextBox sender, string eventId)
        {
            if (eventId == "enterHouse")
            {
                _houseExteriorPanorama.Apply(new Fade(1f, 0f, 0.5f));
                _houseInterior.Apply(new Fade(0f, 1f, 0.5f));
                _lady.Apply(new Fade(0f, 1f, 0.75f));
            }
            else if (eventId == "showPuppers")
            {
                _puppers.Apply(new Fade(0f, 1f, 1f));
            }
        }

        private void ExteriorPanCompleted(IEffect sender)
        {
            _textBox.Show(true);
        }

        public void Draw()
        {
            _houseExteriorPanorama.Draw();
            _houseInterior.Draw();
            
            _lady.Draw();
            _puppers.Draw();
            _oldPhoto.Draw();
            _leash.Draw();

            _findTheLeash.Draw();

            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _houseExteriorPanorama.Update(gameTime);
            _houseInterior.Update(gameTime);
            _textBox.Update(gameTime);
            _lady.Update(gameTime);
            _puppers.Update(gameTime);
            _oldPhoto.Update(gameTime);
            _leash.Update(gameTime);
            _findTheLeash.Update(gameTime);
            Music.Update(gameTime);
        }
    }
}
