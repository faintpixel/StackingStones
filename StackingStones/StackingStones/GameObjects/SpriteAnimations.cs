using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackingStones.GameObjects
{
    public class SpriteAnimations
    {
        public int Speed;
        public Dictionary<string, List<Texture2D>> Animations;
        public bool Loop; // TO DO - make it so we can set this for individual animations

        public SpriteAnimations(int speed, bool loop, Dictionary<string, List<string>> animationFrames)
        {
            Animations = new Dictionary<string, List<Texture2D>>();
            Speed = speed;
            Loop = loop;
            foreach(var frame in animationFrames)
            {
                List<Texture2D> frameTextures = new List<Texture2D>();
                foreach (var frameName in frame.Value)
                    frameTextures.Add(Game1.ContentManager.Load<Texture2D>(frameName));
                Animations.Add(frame.Key, frameTextures);
            }
        }
    }
}
