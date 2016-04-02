using Microsoft.Xna.Framework;
using StackingStones.Effects;
using StackingStones.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackingStones.MiniGames.Squirrel
{
    public class Critter : IGameObject
    {
        private Sprite _sprite;
        private List<Path> _paths;
        private int _currentPathIndex;
        public string Name;

        public event EventHandler CompletedMovement;

        public Critter(string name, string imageResource, List<Path> paths)
        {
            Name = name;
            _currentPathIndex = 0;
            _sprite = new Sprite(imageResource, new Vector2(0, 0), 0f, 1f, 0.5f);
            _paths = paths;
        }

        public void Draw()
        {
            _sprite.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _sprite.Update(gameTime);
        }

        public void Start()
        {
            Random rnd = new Random();
            _currentPathIndex = rnd.Next(0, _paths.Count);
            float speed = rnd.Next(100, 200) / 100;

            var path = _paths[_currentPathIndex];
            var pan = new Pan(path.Start, new Vector2(path.EndX, path.Start.Y), speed);
            pan.Completed += CritterDoneMovingOut;
            _sprite.Apply(pan);
            _sprite.Alpha = 1f;
        }

        private void CritterDoneMovingOut(IEffect sender)
        {
            Random rnd = new Random();
            float speed = rnd.Next(200, 500) / 100;

            var path = _paths[_currentPathIndex];
            var pan = new Pan(_sprite.Position, new Vector2(path.StartX, _sprite.Position.Y), speed);
            pan.Completed += PathCompleted;
            _sprite.Apply(pan);
        }

        private void PathCompleted(IEffect sender)
        {
            if (CompletedMovement != null)
                CompletedMovement(this, null);
        }

        public bool Intersects(Rectangle rectangle)
        {
            Rectangle spriteRectangle = _sprite.GetCollisionRectangle();
            return rectangle.Intersects(spriteRectangle);
        }
    }
}
