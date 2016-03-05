using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackingStones.GameObjects;

namespace StackingStones.Effects
{
    public interface IEffect
    {
        void Update(GameTime gameTime);
        void Start(Sprite sprite);

        event EffectEvent Completed;
    }

    public delegate void EffectEvent(IEffect sender);
}
