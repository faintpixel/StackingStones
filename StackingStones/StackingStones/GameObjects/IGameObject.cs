using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackingStones.GameObjects
{
    public interface IGameObject
    {
        void Update(GameTime gameTime);
        void Draw();
    }
}
