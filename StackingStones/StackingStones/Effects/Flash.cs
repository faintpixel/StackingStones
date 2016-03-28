using Microsoft.Xna.Framework;
using StackingStones.GameObjects;

namespace StackingStones.Effects
{
    public class Flash : IEffect
    {
        private float _minAlpha;
        private float _maxAlpha;
        private Sprite _sprite;
        private float _speed;
        private IEffect _currentEffect;
        private bool _active;

        public event EffectEvent Completed;

        public Flash(float minAlpha, float maxAlpha, float speed)
        {
            _active = false;
            _maxAlpha = maxAlpha;
            _minAlpha = minAlpha;
            _speed = speed;
        }

        public void Start(Sprite sprite)
        {
            _active = true;
            _sprite = sprite;
            FadeOutComplete(this);
        }

        public void Update(GameTime gameTime)
        {
            if(_active)
                _currentEffect.Update(gameTime);
        }

        private void FadeOutComplete(IEffect sender)
        {
            _currentEffect = new Fade(_sprite.Alpha, _maxAlpha, _speed);
            _currentEffect.Completed += FadeInComplete;
            _currentEffect.Start(_sprite);
        }

        private void FadeInComplete(IEffect sender)
        {
            _currentEffect = new Fade(_sprite.Alpha, _minAlpha, _speed);
            _currentEffect.Completed += FadeOutComplete;
            _currentEffect.Start(_sprite);
        }
    }
}
