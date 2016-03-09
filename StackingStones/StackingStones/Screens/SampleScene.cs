using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;
using StackingStones.Effects;
using StackingStones.Models;

namespace StackingStones.Screens
{
    public class SampleScene : IScreen
    {
        private TextBox _textBox;
        private Sprite _background;
        private Sprite _character;

        public SampleScene()
        {
            Script script = new Script();
            script.Dialogue.Add(new Dialogue("", "Walking through the field you encouter two young women.", Color.Green));
            script.Dialogue.Add(new Dialogue("Girl", "\"Like omg did you see what davidwinters is wearing?[sound Samples\\young_female_laugh]\nIt's[speed 30] soooooooooo.........[speed 1500] [speed 50]weird?\"", Color.White));
            script.Dialogue.Add(new Dialogue("Girl #2", "\"Do you think he picked it out himself, or did he lose a bet?\"", Color.Pink));

            List<string> choices = new List<string>();
            choices.Add("\"haha yeah.....\"");
            choices.Add("\"I like the hat.\"");
            choices.Add("Awkward silence");
            choices.Add("\"M'lady you have insulted my good friend. Prepare to die.\"");

            script.Choice = new Choice("How will you respond to the girls take on David's fashion?", choices, new Vector2(20, 400));
            script.Choice.ChoiceSelected += PlayerRespondsToDavidsFashion;
            _textBox = new TextBox(new Vector2(20, 400), script);
            _background = new Sprite("Samples\\testBackground", new Vector2(0, 0), 0.75f, 0.45f, 1f);
            _character = new Sprite("Samples\\sampleCharacters", new Vector2(0, 100), 0f, 1.75f, 1f);

            Fade characterFade = new Fade(0f, 1f, 1f);
            characterFade.Completed += CharacterFade_Completed;
            _character.Apply(characterFade);
        }

        private void PlayerRespondsToDavidsFashion(Choice sender)
        {
            Script script = new Script();
            if (sender.SelectedChoiceIndex == 1)
            {
                script.Dialogue.Add(new Dialogue("Girl", "\"The hat is the worst part!\"", Color.White));
                script.Dialogue.Add(new Dialogue("Girl #2", "\"I think it might even be racist!\"", Color.White));
            }
            else if (sender.SelectedChoiceIndex == 2)
            {
                script.Dialogue.Add(new Dialogue("Girl", "...", Color.White));
            }
            else if (sender.SelectedChoiceIndex == 3)
            {
                script.Dialogue.Add(new Dialogue("Girl", "Relax, we're just joking.", Color.White));
                script.Dialogue.Add(new Dialogue("Girl #2", "Yeah, calm down.", Color.White));
            }
            script.Dialogue.Add(new Dialogue("Girl", "Oh well. I guess we have more important things to discuss anyways.", Color.White));
            script.Dialogue.Add(new Dialogue("Girl #2", "Agreed.", Color.White));
            script.Dialogue.Add(new Dialogue("Girl", "Have you seen how dreamy that Arto guy is? Wowza.", Color.White));

            script.Choice = new Choice("", new List<string>(), new Vector2(20, 400));

            _textBox = new TextBox(new Vector2(20, 400), script);
            _textBox.Show();
        }

        private void CharacterFade_Completed(IEffect sender)
        {
            _textBox.Show(true);
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
