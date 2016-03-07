using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;
using StackingStones.Effects;
using Microsoft.Xna.Framework.Media;

namespace StackingStones.Screens
{
    public class SampleScene : IScreen
    {
        private TextBox _textBox;
        private Sprite _background;
        private Sprite _girls;
        private Sprite _david;
        private Song _backgroundMusic;

        public SampleScene()
        {
            _backgroundMusic = Game1.ContentManager.Load<Song>("Samples\\606658_Scarlett-Isle");
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_backgroundMusic); 
            
            _background = new Sprite("Samples\\testBackground", new Vector2(0, 0), 0f, 0.45f, 1f);

            var backgroundTransitionEffects = new List<IEffect>();
            backgroundTransitionEffects.Add(new Fade(0f, 1f, 0.5f));
            backgroundTransitionEffects.Add(new Pan(new Vector2(0, 0), new Vector2(-300, 0), 0.5f));
            backgroundTransitionEffects.Add(new Sound("Samples\\young_female_laugh"));
            var backgroundTransition = new MultiStageEffect(backgroundTransitionEffects);
            backgroundTransition.Completed += BackgroundTransition_Completed;
            _background.Apply(backgroundTransition);

            _girls = new Sprite("Samples\\sampleCharacters", new Vector2(0, 100), 0f, 1.75f, 1f);
            _david = new Sprite("Samples\\dw", new Vector2(500, 100), 0f, 1.75f, 1f);
            InitializeGirlsTalkingAboutDavid();
        }

        private void InitializeGirlsTalkingAboutDavid()
        {
            List<string> girlsText = new List<string>();
            girlsText.Add("\"Like omg did you see what davidwinters is wearing?\nIt's[speed 30] soooooooooo.........[speed 1500] [speed 50]weird?\"");
            girlsText.Add("\"Do you think he picked it out himself, or did he lose a bet?\"");

            _textBox = new TextBox(new Vector2(20, 400), "Girl", girlsText, 50);
            _textBox.Completed += GirlsDoneTalkingAboutDavid;
        }

        private void GirlsDoneTalkingAboutDavid(TextBox sender)
        {
            _david.Apply(new Fade(0f, 1f, 1f));
            _girls = new Sprite("Samples\\sampleCharacters2", new Vector2(0, 100), 1f, 1.75f, 1f);

            List<string> davidsText = new List<string>();
            davidsText.Add("\"um[speed 500].......\"");
            davidsText.Add("[speed 50]\"*gulp*[speed 10]hello ladies how are you my name is david it is nice to meet you\nI think you are very pretty and I like the way you smell\nmaybe we can hang out sometime ok sorry goodbye\"");
            davidsText.Add("[speed 50]...");
            davidsText.Add("[speaker Girls]...");
            davidsText.Add("[speaker David]...");
            davidsText.Add("\"um bye\"");
            _textBox = new TextBox(new Vector2(20, 400), "David", davidsText, 50);
            _textBox.Completed += DavidDoneTalkingToGirls;
            _textBox.Show();
            // fade in david
            // set him to be the speaker
            // say some stuff
            // change girl sprite
        }

        private void DavidDoneTalkingToGirls(TextBox sender)
        {
            Fade fade = new Fade(1f, 0f, 0.45f);
            fade.Completed += DavidGone;
            _david.Apply(fade);
        }

        private void DavidGone(IEffect sender)
        {
            _girls = new Sprite("Samples\\sampleCharacters", new Vector2(0, 100), 1f, 1.75f, 1f);
            List<string> girlsText = new List<string>();
            girlsText.Add("\"Is it just me or are things getting weird?\"");
            girlsText.Add("\"...... I do like that hat though.\"");

            _textBox = new TextBox(new Vector2(20, 400), "Girl", girlsText, 50);
            _textBox.Show();
        }

        private void BackgroundTransition_Completed(IEffect sender)
        {
            
            Fade characterFade = new Fade(0f, 1f, 1f);
            characterFade.Completed += CharacterFade_Completed;
            _girls.Apply(characterFade);
        }

        private void CharacterFade_Completed(IEffect sender)
        {

            _textBox.Show();
        }

        public void Draw()
        {
            _background.Draw();
            _girls.Draw();
            _david.Draw();
            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _textBox.Update(gameTime);
            _background.Update(gameTime);
            _girls.Update(gameTime);
            _david.Update(gameTime);
        }
    }
}
