using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackingStones.GameObjects;

namespace StackingStones.Effects
{
    public class Fade : IEffect
    {
        private float _speed;
        private float _endAlpha;
        private float _startAlpha;
        private Sprite _sprite;
        private bool _active;
        private FadeType _type;

        public event EffectEvent Completed;

        public Fade(float startAlpha, float endAlpha, float speed)
        {
            _active = false;
            _endAlpha = endAlpha;
            _startAlpha = startAlpha;
            _speed = speed;
            if (startAlpha > _endAlpha)
                _type = FadeType.FadingOut;
            else
                _type = FadeType.FadingIn;
        }

        public void Start(Sprite sprite)
        {
            _sprite = sprite;
            _sprite.Alpha = _startAlpha;
            _active = true;            
        }

        private void Finish()
        {
            _sprite.Alpha = _endAlpha;
            _active = false;
            if (Completed != null)
                Completed(this);
            _sprite.RemoveEffect(this);
        }

        public void Update(GameTime gameTime)
        {
            if(_active)
            {
                float amountToChange = _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_type == FadeType.FadingIn)
                {
                    if (_sprite.Alpha < _endAlpha)
                        _sprite.Alpha += amountToChange;
                    else
                        Finish();
                }
                else
                {
                    if (_sprite.Alpha > _endAlpha)
                        _sprite.Alpha -= amountToChange;
                    else
                        Finish();
                }
            }
            // TO DO - use the game time to set the speed            
        }

        private enum FadeType
        {
            FadingIn,
            FadingOut
        }
    }
}
