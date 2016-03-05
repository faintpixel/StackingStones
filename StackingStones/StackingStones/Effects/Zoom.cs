using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;

namespace StackingStones.Effects
{
    public class Zoom : IEffect
    {
        private float _startScale;
        private float _endScale;
        private Sprite _sprite;
        private float _speed;
        private bool _active;
        private ZoomType _type;

        public event EffectEvent Completed;

        public Zoom(float startScale, float endScale, float speed)
        {

            // TO DO - option to zoom from the center of the image or the corner? or maybe just do center
            _startScale = startScale;
            _endScale = endScale;
            _active = false;
            _speed = speed;
            if (startScale > endScale)
                _type = ZoomType.ZoomOut;
            else
                _type = ZoomType.ZoomIn;
        }

        public void Start(Sprite sprite)
        {
            _sprite = sprite;
            _sprite.Scale = _startScale;
            _active = true;
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
                float amountToChange = _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_type == ZoomType.ZoomIn)
                {
                    if (_sprite.Scale < _endScale)
                        _sprite.Scale += amountToChange;
                    else
                        Finish();
                }
                else
                {
                    if (_sprite.Scale > _endScale)
                        _sprite.Scale -= amountToChange;
                    else
                        Finish();
                }
            }
        }

        private enum ZoomType
        {
            ZoomIn,
            ZoomOut
        }
    }
}
