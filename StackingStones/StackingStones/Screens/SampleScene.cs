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
    public class SampleScene : IScreen
    {
        private TextBox _textBox;
        private Sprite _background;
        private Sprite _squirrel;
        private Sprite _girls;
        private Sprite _hearts;

        public event ScreenEvent Completed;

        public SampleScene()
        {
            Song backgroundMusic = Game1.ContentManager.Load<Song>("Music\\wow");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.Volume = 0.5f;
            
            _background = new Sprite("Backgrounds\\hill", new Vector2(0, 0), 0.75f, 0.35f, 1f);
            _squirrel = new Sprite("Sprites\\squirrel", new Vector2(-300, 400), 1f, 0.30f, 1f);
            var backgroundTransitionEffects = new List<IEffect>();
            backgroundTransitionEffects.Add(new Fade(0f, 1f, 0.5f));
            backgroundTransitionEffects.Add(new Pan(new Vector2(0, 0), new Vector2(0, -150), 0.5f));
            //backgroundTransitionEffects.Add(new Pan(new Vector2(0, 0), new Vector2(-300, 0), 0.5f));
            var backgroundTransition = new MultiStageEffect(backgroundTransitionEffects);
            backgroundTransition.Completed += BackgroundTransition_Completed;    
            _background.Apply(backgroundTransition);


            _hearts = new Sprite("Samples\\hearts", new Vector2(150, 150), 0f, 1f, 1f);
            InitializeGirls();

            InitializeGirlsTalkingAboutDavidsFashion();
            
            
        }

        private void BackgroundTransition_Completed(IEffect sender)
        {
            List<IEffect> effects = new List<IEffect>();
            effects.Add(new Pan(_squirrel.Position, new Vector2(-75, _squirrel.Position.Y), 0.5f));
            effects.Add(new Wait(1500));
            effects.Add(new Sound("SoundEffects\\122261__echobones__angry-squirrel1-edited"));
            effects.Add(new Pan(new Vector2(-75, _squirrel.Position.Y), new Vector2(800, _squirrel.Position.Y), 4f));

            var effect = new MultiStageEffect(effects);
            effect.Completed += SquirrelDoneRunningAround;
            _squirrel.Apply(effect);
            
        }

        private void SquirrelDoneRunningAround(IEffect sender)
        {
            _textBox.Show(true);
        }

        private void InitializeGirls()
        {
            var animations = new Dictionary<string, List<string>>();
            animations.Add("leftTalking", new List<string> { "Samples\\sampleCharacters" });
            animations.Add("rightTalking", new List<string> { "Samples\\sampleCharacters-rightTalking" });
            animations.Add("idle", new List<string> { "Samples\\sampleCharacters-neitherTalking" });
            animations.Add("blushing", new List<string> { "Samples\\sampleCharacters", "Samples\\sampleCharacters-blush1", "Samples\\sampleCharacters-blush2", "Samples\\sampleCharacters-blush3", "Samples\\sampleCharacters-blush4", "Samples\\sampleCharacters-blush5" });
            var girlAnimations = new SpriteAnimations(250, false, animations);
            _girls = new Sprite(girlAnimations, "leftTalking", new Vector2(0, 100), 0f, 1.75f, 1f);
        }
      
        private void InitializeGirlsTalkingAboutDavidsFashion()
        {
            Script script = new Script();
            script.Dialogue.Add(new Dialogue("", "Walking towards the hill you encouter two young women.", Color.Green));
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
            _textBox.ScriptedEventReached += _textBox_ScriptedEventReached;
            _textBox.DialogueLineComplete += DialogueLineComplete_GirlsTalkingAboutDavidsFashion;
        }

        private void DialogueLineComplete_GirlsTalkingAboutDavidsFashion(TextBox sender, Script script, int scriptIndex)
        {
            if (scriptIndex == 0)
            {
                Fade characterFade = new Fade(0f, 1f, 1f);
                _girls.Apply(characterFade);
            }
            else if (scriptIndex == 1)
                _girls.SetAnimation("rightTalking");
            else if (scriptIndex == 2)
                _girls.SetAnimation("idle");
        }

        private void PlayerRespondsToDavidsFashion(Choice sender)
        {
            Script script = new Script();
            if (sender.SelectedChoiceIndex == 1)
            {
                script.Dialogue.Add(new Dialogue("Girl", "[event leftGirlTalking]\"The hat is the worst part!\"", Color.White));
                script.Dialogue.Add(new Dialogue("Girl #2", "[event rightGirlTalking]\"I think it might even be racist!\"", Color.White));
            }
            else if (sender.SelectedChoiceIndex == 2)
            {
                script.Dialogue.Add(new Dialogue("Girl", "...", Color.White));
            }
            else if (sender.SelectedChoiceIndex == 3)
            {
                script.Dialogue.Add(new Dialogue("Girl", "[event leftGirlTalking]Relax, we're just joking.", Color.White));
                script.Dialogue.Add(new Dialogue("Girl #2", "[event rightGirlTalking]Yeah, calm down.", Color.White));
            }
            script.Dialogue.Add(new Dialogue("Girl", "[event leftGirlTalking]Oh well. I guess we have more important things to discuss anyways.", Color.White));
            script.Dialogue.Add(new Dialogue("Girl #2", "[event rightGirlTalking]Agreed.", Color.White));
            script.Dialogue.Add(new Dialogue("Girl", "[event wowza]Have you seen how dreamy that Arto guy is? Wowza.", Color.White));

            script.Choice = new Choice("", new List<string>(), new Vector2(20, 400));

            _textBox = new TextBox(new Vector2(20, 400), script);
            _textBox.ScriptedEventReached += _textBox_ScriptedEventReached;
            _textBox.Show();
        }

        private void _textBox_ScriptedEventReached(TextBox sender, string eventId)
        {
            if (eventId == "leftGirlTalking")
                _girls.SetAnimation("leftTalking");
            else if (eventId == "rightGirlTalking")
                _girls.SetAnimation("rightTalking");
            else if (eventId == "wowza")
            {
                _girls.SetAnimation("blushing");
                List<IEffect> effects = new List<IEffect>();
                effects.Add(new Zoom(0f, 1f, Vector2.Zero, 0.5f));
                effects.Add(new Fade(0f, 1f, 0.2f));
                effects.Add(new Pan(_hearts.Position, new Vector2(150, 50), 0.5f));
                
                var heartEffects = new SimultaneousEffects(effects);
                _hearts.Apply(heartEffects);
            }
        }

        public void Draw()
        {
            _background.Draw();
            _squirrel.Draw();
            _girls.Draw();
            _hearts.Draw();
            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _textBox.Update(gameTime);
            _background.Update(gameTime);
            _girls.Update(gameTime);
            _hearts.Update(gameTime);
            _squirrel.Update(gameTime);
        }
    }
}
