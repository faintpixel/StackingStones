using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;

namespace StackingStones.Effects
{
    public class Shake : IEffect
    {
        private float _amount;
        private TimeSpan _duration;
        private int _speed;
        private bool _active;
        private Sprite _sprite;
        private Vector2 _originalPosition;

        public event EffectEvent Completed;

        public Shake(float amount, TimeSpan duration, int speed)
        {
            _amount = amount;
            _duration = duration;
            _speed = speed;
            _active = false;
        }

        public void Start(Sprite sprite)
        {
            _sprite = sprite;
            _originalPosition = new Vector2(_sprite.Position.X, _sprite.Position.Y);
            _active = true;
        }

        public void Update(GameTime gameTime)
        {
            // use the pan effect to shift it around
        }
    }
}
