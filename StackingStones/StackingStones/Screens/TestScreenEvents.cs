using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;
using StackingStones.Effects;

namespace StackingStones.Screens
{
    public class TestScreenEvents : IScreen
    {
        private List<Sprite> _sprites;

        public event ScreenEvent Completed;

        public TestScreenEvents()
        {
            _sprites = new List<Sprite>();

            _sprites.Add(new Sprite("Samples\\testBackground", new Vector2(0, 0), 1f, 1f, 1f));
            List<IEffect> effects = new List<IEffect>();

            List<IEffect> simultaneousEffects = new List<IEffect>();
            simultaneousEffects.Add(new Fade(0f, 0.5f, 0.5f));
            simultaneousEffects.Add(new Pan(new Vector2(0, 0), new Vector2(-200, 0), 1));

            effects.Add(new SimultaneousEffects(simultaneousEffects));
            effects.Add(new Zoom(1f, 0.75f, Vector2.Zero, 0.1f));

            var effect = new MultiStageEffect(effects);
            effect.Completed += screenFadeInCompleted;

            _sprites[0].Apply(effect);

            _sprites.Add(new Sprite("Samples\\sampleCharacters", new Vector2(0, 300), 0f, 1f, 1f));
        }

        void screenFadeInCompleted(IEffect sender)
        {
            _sprites[1].Apply(new Fade(0f, 1f, 1.5f));
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
