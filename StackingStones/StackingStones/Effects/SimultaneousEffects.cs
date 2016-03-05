using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;

namespace StackingStones.Effects
{
    public class SimultaneousEffects : IEffect
    {
        private List<IEffect> _effects;
        private bool _active;
        private Sprite _sprite;

        public event EffectEvent Completed;

        public SimultaneousEffects(List<IEffect> effects)
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
        }

        public void Start(Sprite sprite)
        {
            _active = true;
            _sprite = sprite;
            foreach (IEffect effect in _effects)
                effect.Start(_sprite);
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
            {
                for (int i = _effects.Count - 1; i >= 0; i--)
                    _effects[i].Update(gameTime);

                if (_effects.Count == 0)
                    Finish();
            }
                
        }
    }
}
