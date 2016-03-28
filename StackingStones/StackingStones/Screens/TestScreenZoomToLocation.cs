using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;
using StackingStones.Effects;

namespace StackingStones.Screens
{
    public class TestScreenZoomToLocation : IScreen
    {
        private Sprite _ball1;
        private Sprite _ball2;

        public event ScreenEvent Completed;

        public TestScreenZoomToLocation()
        {
            _ball1 = new Sprite("Samples\\circle1", new Vector2(50, 50), 1f, 1f, 0.5f);
            _ball2 = new Sprite("Samples\\circle1", new Vector2(50, 200), 1f, 1f, 0.5f);

            _ball1.Apply(new Zoom(1f, 2f, Vector2.Zero, 0.25f));

            var effects = new List<IEffect>();
            effects.Add(new Zoom(1f, 2f, Vector2.Zero, 0.25f));
            effects.Add(new Pan(new Vector2(50, 200), new Vector2(200, 200), 0.25f));

            _ball2.Apply(new SimultaneousEffects(effects));
        }

        public void Draw()
        {
            _ball1.Draw();
            _ball2.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _ball1.Update(gameTime);
            _ball2.Update(gameTime);
        }
    }
}
