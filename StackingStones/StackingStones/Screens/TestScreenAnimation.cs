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
    public class TestScreenAnimation : IScreen
    {
        private Sprite _girls;

        public TestScreenAnimation()
        {
            var animations = new Dictionary<string, List<string>>();
            animations.Add("leftTalking", new List<string> { "Samples\\sampleCharacters" });
            animations.Add("rightTalking", new List<string> { "Samples\\sampleCharacters-rightTalking" });
            animations.Add("idle", new List<string> { "Samples\\sampleCharacters-neitherTalking" });
            animations.Add("blushing", new List<string> { "Samples\\sampleCharacters", "Samples\\sampleCharacters-blush1", "Samples\\sampleCharacters-blush2", "Samples\\sampleCharacters-blush3", "Samples\\sampleCharacters-blush4", "Samples\\sampleCharacters-blush5" });

            var animationDictionary = new Dictionary<string, List<string>>();
            animationDictionary.Add("pulse", new List<string>());
            animationDictionary["pulse"].Add("Samples\\circle1");
            animationDictionary["pulse"].Add("Samples\\circle2");
            animationDictionary["pulse"].Add("Samples\\circle3");
            animationDictionary["pulse"].Add("Samples\\circle2");


            var girlAnimations = new SpriteAnimations(150, false, animations);
            _girls = new Sprite(girlAnimations, "blushing", new Vector2(0, 100), 1f, 1.75f, 1f);
            //_girls = new Sprite("Samples\\circle1", new Vector2(0, 100), 1f, 1.75f, 1f);
        }

        public void Draw()
        {
            _girls.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _girls.Update(gameTime);
        }
    }
}
