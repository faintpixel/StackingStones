using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;

namespace StackingStones.Effects
{
    public class Pan : IEffect // Maybe should rename this to move?
    {
        private Sprite _sprite;
        private Vector2 _startPosition;
        private Vector2 _endPosition;
        private float _speed;
        private bool _active;
        private HorizontalDirection _horizontalDirection;
        private VerticalDirection _verticalDirection;

        public event EffectEvent Completed;

        public Pan(Vector2 startPosition, Vector2 endPosition, float speed)
        {
            _startPosition = startPosition;
            _endPosition = endPosition;
            _speed = speed * 100;

            if (_startPosition.X == _endPosition.X)
                _horizontalDirection = HorizontalDirection.None;
            else if (_startPosition.X > _endPosition.X)
                _horizontalDirection = HorizontalDirection.Left;
            else
                _horizontalDirection = HorizontalDirection.Right;

            if (_startPosition.Y == _endPosition.Y)
                _verticalDirection = VerticalDirection.None;
            else if (_startPosition.Y > _endPosition.Y)
                _verticalDirection = VerticalDirection.Up;
            else
                _verticalDirection = VerticalDirection.Down;

            _active = false;
        }

        public void Start(Sprite sprite)
        {
            _sprite = sprite;
            _sprite.Position = new Vector2(_startPosition.X, _startPosition.Y);
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
                MoveHorizontally(gameTime);
                MoveVertically(gameTime);
            }
        }

        private void MoveVertically(GameTime gameTime)
        {
            float amountToChange = _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_verticalDirection == VerticalDirection.Down)
            {
                if (_sprite.Position.Y < _endPosition.Y)
                    _sprite.Position.Y += amountToChange;
                else
                    Finish();
            }
            else if(_verticalDirection == VerticalDirection.Up)
            {
                if (_sprite.Position.Y > _endPosition.Y)
                    _sprite.Position.Y -= amountToChange;
                else
                    Finish();
            }
        }

        private void MoveHorizontally(GameTime gameTime)
        {
            float amountToChange = _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_horizontalDirection == HorizontalDirection.Left)
            {
                if (_sprite.Position.X > _endPosition.X)
                    _sprite.Position.X -= amountToChange;
                else
                    Finish();
            }
            else if (_horizontalDirection == HorizontalDirection.Right)
            {
                if (_sprite.Position.X < _endPosition.X)
                    _sprite.Position.X += amountToChange;
                else
                    Finish();
            }
        }

        private enum HorizontalDirection
        {
            Left, 
            Right,
            None
        }

        private enum VerticalDirection
        {
            Up,
            Down,
            None
        }
    }
}
