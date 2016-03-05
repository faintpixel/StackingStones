using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;

namespace StackingStones.Effects
{
    public class Wait : IEffect
    {
        public event EffectEvent Completed;

        public void Start(Sprite sprite)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
