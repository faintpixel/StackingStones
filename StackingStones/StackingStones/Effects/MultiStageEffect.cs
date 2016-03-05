using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;

namespace StackingStones.Effects
{
    public class MultiStageEffect : IEffect
    {

        private List<IEffect> _effects;
        private bool _active;
        private Sprite _sprite;

        public event EffectEvent Completed;

        public MultiStageEffect(List<IEffect> effects)
        {
            _active = false;
            _effects = new List<IEffect>();

            foreach (IEffect effect in effects)
            {
                effect.Completed += StageCompleted;
                _effects.Add(effect);
            }
        }

        private void StageCompleted(IEffect sender)
        {
            _effects.Remove(sender);
            Start(_sprite);
        }

        public void Start(Sprite sprite)
        {
            _active = true;
            _sprite = sprite;
            if (_effects.Count > 0)
                _effects[0].Start(sprite);
            else
                Finish();
        }

        private void Finish()
        {
            _active = false;
            if (Completed != null)
                Completed(this);
            _sprite.RemoveEffect(this);
        }

        public void Update(GameTime gameTime)
        {
            if (_active)
                for (int i = _effects.Count - 1; i >= 0; i--)
                    _effects[i].Update(gameTime);
        }
    }
}
