using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackingStones.Effects;

namespace StackingStones.GameObjects
{
    public class Sprite : IGameObject
    {
        public Vector2 Position;
        public float Alpha;
        public float Scale;
        private SpriteAnimations _animations;
        private int _currentAnimationFrame;
        private string _currentAnimation;
        private TimeSpan _timeOfLastFrameChange;        
        private List<IEffect> _effects;
        private float _depth;

        public Sprite(SpriteAnimations animations, string currentAnimation, Vector2 position, float alpha, float scale, float depth)
        {
            Scale = scale;
            Position = position;
            _animations = animations;
            _currentAnimation = currentAnimation;
            _currentAnimationFrame = 0;
            _timeOfLastFrameChange = TimeSpan.MinValue;
            Alpha = alpha;
            _effects = new List<IEffect>();
            _depth = depth;
        }

        public Sprite(string imageName, Vector2 position, float alpha, float scale, float depth)
        {
            Dictionary<string, List<string>> animationFrames = new Dictionary<string, List<string>>();
            animationFrames.Add("idle", new List<string>());
            animationFrames["idle"].Add(imageName);
            SpriteAnimations animations = new SpriteAnimations(0, false, animationFrames);

            Scale = scale;
            Position = position;
            _animations = animations;
            _currentAnimation = "idle";
            _currentAnimationFrame = 0;
            _timeOfLastFrameChange = TimeSpan.MinValue;
            Alpha = alpha;
            _effects = new List<IEffect>();
            _depth = depth;
        }

        public void RemoveEffect(IEffect effect)
        {
            _effects.Remove(effect);
        }

        public void RemoveAllEffects()
        {
            _effects.Clear();
        }

        public void Draw()
        {
            Game1.SpriteBatch.Draw(_animations.Animations[_currentAnimation][_currentAnimationFrame], Position, null, new Color(Color.White, Alpha), 0f, Vector2.Zero, Scale, SpriteEffects.None, _depth);
            //Game1.SpriteBatch.Draw(_animations.Animations[_currentAnimation][_currentAnimationFrame], Position, new Color(Color.White, Alpha));
        }

        public void Update(GameTime gameTime)
        {
            for(int i = _effects.Count - 1; i >= 0; i--)
                _effects[i].Update(gameTime);

            // advance the animation
            if (_currentAnimationFrame + 1 >= _animations.Animations[_currentAnimation].Count)
            {
                if(_animations.Loop)
                    _currentAnimationFrame = 0;
            }                
            else
                _currentAnimationFrame++;
        }

        public void SetAnimation(string animationName, int frame = 0)
        {
            // might need to check if the animation exists before doing this... probably ok though since we're the only ones using it.
            _currentAnimationFrame = frame; // if we set it to a frame that isn't 0 we might have problems with the draw function... could potentially try drawing a frame that is out of bounds.
            _currentAnimation = animationName;
        }

        public void Apply(IEffect effect)
        {
            effect.Start(this);
            _effects.Add(effect);
        }
    }
}
