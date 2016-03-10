using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;
using System.Timers;

namespace StackingStones.Effects
{
    public class Wait : IEffect
    {
        private int _milliseconds;
        private bool _active;
        private Sprite _sprite;

        public event EffectEvent Completed;

        public Wait(int milliseconds)
        {
            _milliseconds = milliseconds;
            _active = false;
        }

        public void Start(Sprite sprite)
        {
            _active = true;
            _sprite = sprite;
            Timer timer = new Timer(_milliseconds);
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_active)
            {
                _active = false;
                if (Completed != null)
                    Completed(this);
                _sprite.RemoveEffect(this); // TO DO - the sprite should do this itself
            }
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
