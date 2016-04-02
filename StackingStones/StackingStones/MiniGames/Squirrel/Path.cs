using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackingStones.MiniGames.Squirrel
{
    public class Path
    {
        public Vector2 Start;
        public float EndX;
        public readonly float StartX;

        public Path(Vector2 start, float endX)
        {
            Start = start;
            EndX = endX;
            StartX = start.X;
        }
    }
}
