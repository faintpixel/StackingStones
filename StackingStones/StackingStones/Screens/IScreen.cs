using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackingStones.Screens
{
    public interface IScreen
    {
        void Update(GameTime gameTime);
        void Draw();

        event ScreenEvent Completed;
    }

    public delegate void ScreenEvent(IScreen sender);
}
