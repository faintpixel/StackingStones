using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;
using StackingStones.Effects;

namespace StackingStones.Screens
{
    public class TestScreenText : IScreen
    {
        private TextBox _textBox;
        private Sprite _background;
        private Sprite _character;

        public TestScreenText()
        {
            //string text = "This is how I normally talk, but sometimes I like to talk[speed 1000] slower.[speed 50] tee hee!";
            string text = "\"Like omg did you see what davidwinters is wearing?\nIt's[speed 50] soooooooooo.........[speed 1500] [speed 100]weird?\"";

            _textBox = new TextBox(new Vector2(20, 400), "Girl", text, 100);
            _background = new Sprite("Samples\\testBackground", new Vector2(0, 0), 0.75f, 0.45f, 1f);
            _character = new Sprite("Samples\\sampleCharacters", new Vector2(0, 100), 0f, 1.75f, 1f);

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
