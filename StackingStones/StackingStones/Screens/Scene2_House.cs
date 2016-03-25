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
    public class Scene2_House : IScreen
    {
        private TextBox _textBox;
        private Sprite _houseExteriorPanorama;
        private Sprite _houseInterior;
        private Sprite _lady;
        private Sprite _puppers;

        public Scene2_House()
        {
            Song backgroundMusic = Game1.ContentManager.Load<Song>("Music\\449897_Prologos");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Volume = 0.5f;

            _houseExteriorPanorama = new Sprite("Backgrounds\\Placeholders\\houseByForest", new Vector2(0, 0), 0f, 1f, 0.5f);
            _houseInterior = new Sprite("Backgrounds\\Placeholders\\houseInterior", new Vector2(0, 0), 0f, 1f, 0.5f);

            var effects = new List<IEffect>();
            effects.Add(new Fade(0f, 1f, 0.6f));
            effects.Add(new Pan(new Vector2(0, 0), new Vector2(-1280, 0), 5.5f)); // TO DO - reset speed to 0.5
            var effect = new MultiStageEffect(effects);
            effect.Completed += ExteriorPanCompleted;
            
            _houseExteriorPanorama.Apply(effect);

            var script = new Script();
            script.Dialogue.Add(new Dialogue("", "My family settled this land years ago.", Color.Green));
            script.Dialogue.Add(new Dialogue("", "It's just me and [sound SoundEffects\\328729__ivolipa__dog-bark]Puppers now.", Color.Green));

            // fade out exterior and fade in interior
            // fade in old lady
            script.Dialogue.Add(new Dialogue("Old Lady", "[event enterHouse]Alright Puppers, I hear you. Time for your walk is it?", Color.White));
            // fade in puppers
            script.Dialogue.Add(new Dialogue("Puppers", "[event showPuppers]*[sound SoundEffects\\328729__ivolipa__dog-bark]Bark bark!*", Color.White));
            script.Dialogue.Add(new Dialogue("Old Lady", "*Chuckles*\nWell, I suppose it's a beautiful day for it.\nNow where did I put that darn leash?", Color.White));

            List<string> choices = new List<string>();
            choices.Add("Take Puppers for a walk.");

            script.Choice = new Choice("", choices, new Vector2(240, 500));
            _textBox = new TextBox(new Vector2(240, 500), script);
            _textBox.ScriptedEventReached += _textBox_ScriptedEventReached;
            _textBox.Completed += StartLookingForLeash;

            _lady = new Sprite("Sprites\\lady", new Vector2(0, 0), 0f, 1f, 0.5f);
            _puppers = new Sprite("Sprites\\puppersNoLeash", new Vector2(800, 0), 0f, 1f, 0.5f);
        }

        private void StartLookingForLeash(TextBox sender)
        {
            _lady.Apply(new Fade(1f, 0f, 0.75f));
            _puppers.Apply(new Fade(1f, 0f, 0.75f));
            _textBox.Hide(1f);
        }

        private void _textBox_ScriptedEventReached(TextBox sender, string eventId)
        {
            if(eventId == "enterHouse")
            {
                _houseExteriorPanorama.Apply(new Fade(1f, 0f, 0.5f));
                _houseInterior.Apply(new Fade(0f, 1f, 0.5f));
                _lady.Apply(new Fade(0f, 1f, 0.3f));
            }
            else if(eventId == "showPuppers")
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

            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _houseExteriorPanorama.Update(gameTime);
            _houseInterior.Update(gameTime);
            _textBox.Update(gameTime);
            _lady.Update(gameTime);
            _puppers.Update(gameTime);
        }
    }
}
