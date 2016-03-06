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
        private Sprite _character;
        private Song _backgroundMusic;

        public SampleScene()
        {
            _backgroundMusic = Game1.ContentManager.Load<Song>("Samples\\606658_Scarlett-Isle");
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_backgroundMusic); 
            
            string text = "\"Like omg did you see what davidwinters is wearing?\nIt's[speed 30] soooooooooo.........[speed 1500] [speed 100]weird?\"";


            _textBox = new TextBox(new Vector2(20, 400), "Girl", text, 50);
            _background = new Sprite("Samples\\testBackground", new Vector2(0, 0), 0f, 0.45f, 1f);


            var backgroundTransitionEffects = new List<IEffect>();
            backgroundTransitionEffects.Add(new Fade(0f, 1f, 0.5f));
            backgroundTransitionEffects.Add(new Pan(new Vector2(0, 0), new Vector2(-300, 0), 0.5f));
            backgroundTransitionEffects.Add(new Sound("Samples\\young_female_laugh"));
            var backgroundTransition = new MultiStageEffect(backgroundTransitionEffects);
            backgroundTransition.Completed += BackgroundTransition_Completed;
            _background.Apply(backgroundTransition);

            _character = new Sprite("Samples\\sampleCharacters", new Vector2(0, 100), 0f, 1.75f, 1f);            
        }

        private void BackgroundTransition_Completed(IEffect sender)
        {
          //  _background.Apply(new Fade(1f, 0.75f, 1f));
            Fade characterFade = new Fade(0f, 1f, 1f);
            characterFade.Completed += CharacterFade_Completed;
            _character.Apply(characterFade);
        }

        private void CharacterFade_Completed(IEffect sender)
        {

            _textBox.Show();
        }

        public void Draw()
        {
            _background.Draw();
            _character.Draw();
            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _textBox.Update(gameTime);
            _background.Update(gameTime);
            _character.Update(gameTime);
        }
    }
}
