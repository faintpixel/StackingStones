using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;
using Microsoft.Xna.Framework.Audio;

namespace StackingStones.Effects
{
    public class Sound : IEffect
    {
        private SoundEffect _sound;

        public Sound(string soundEffectName)
        {
            _sound = Game1.ContentManager.Load<SoundEffect>(soundEffectName);
        }

        public event EffectEvent Completed;

        public void Start(Sprite sprite)
        {
            _sound.Play();
            if (Completed != null)
                Completed(this);
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
