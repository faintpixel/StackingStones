using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;
using StackingStones.Effects;

namespace StackingStones.Screens
{
    public class TestScreenSpeeds : IScreen
    {
        private List<Sprite> _sprites;

        public TestScreenSpeeds()
        {
            _sprites = new List<Sprite>();

            var animationDictionary = new Dictionary<string, List<string>>();
            animationDictionary.Add("pulse", new List<string>());
            animationDictionary["pulse"].Add("Samples\\circle1");
            animationDictionary["pulse"].Add("Samples\\circle2");
            animationDictionary["pulse"].Add("Samples\\circle3");
            animationDictionary["pulse"].Add("Samples\\circle2");

            animationDictionary.Add("shrink", new List<string>());
            animationDictionary["pulse"].Add("Samples\\circle1");
            animationDictionary["pulse"].Add("Samples\\circle2");
            animationDictionary["pulse"].Add("Samples\\circle3");

            animationDictionary.Add("idle", new List<string>());
            animationDictionary["idle"].Add("Samples\\circle1");
            SpriteAnimations animations = new SpriteAnimations(1, true, animationDictionary);

            _sprites.Add(new Sprite(animations, "idle", new Vector2(50, 50), 1f, 1f, 0.5f));
            _sprites.Add(new Sprite(animations, "idle", new Vector2(150, 50), 1f, 1f, 0.5f));
            _sprites.Add(new Sprite(animations, "idle", new Vector2(250, 50), 1f, 1f, 0.5f));

            _sprites[0].Apply(new Fade(0f, 1f, 1));
            _sprites[1].Apply(new Fade(0f, 1f, 0.1f));
            _sprites[2].Apply(new Fade(0f, 1f, 0.05f));

            _sprites.Add(new Sprite(animations, "idle", new Vector2(50, 250), 1f, 1f, 0.5f));
            _sprites.Add(new Sprite(animations, "idle", new Vector2(150, 250), 1f, 1f, 0.5f));
            _sprites.Add(new Sprite(animations, "idle", new Vector2(250, 250), 1f, 1f, 0.5f));

            _sprites[3].Apply(new Zoom(0f, 1f, 1));
            _sprites[4].Apply(new Zoom(0f, 1f, 0.1f));
            _sprites[5].Apply(new Zoom(0f, 1f, 0.05f));

            _sprites.Add(new Sprite(animations, "idle", new Vector2(50, 350), 1f, 1f, 0.5f));
            _sprites.Add(new Sprite(animations, "idle", new Vector2(150, 350), 1f, 1f, 0.5f));
            _sprites.Add(new Sprite(animations, "idle", new Vector2(250, 350), 1f, 1f, 0.5f));

            _sprites[6].Apply(new Pan(new Vector2(50, 350), new Vector2(250, 350), 0.05f));
            _sprites[7].Apply(new Pan(new Vector2(150, 350), new Vector2(350, 350), 0.1f));
            _sprites[8].Apply(new Pan(new Vector2(250, 350), new Vector2(450, 350), 1f));

            //_sprites.Add(new Sprite("testBackground", new Vector2(0, 0), 0f, 3f, 0.8f));
            //_sprites[4].Apply(new Fade(0f, 1f, 10));
            ////_sprites[4].Apply(new Zoom(3f, 0.5f, 10));
            //_sprites[4].Apply(new Zoom(0f, 0.5f, 10));
        }

        public void Draw()
        {
            foreach (Sprite sprite in _sprites)
                sprite.Draw();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Sprite sprite in _sprites)
                sprite.Update(gameTime);
        }
    }
}
