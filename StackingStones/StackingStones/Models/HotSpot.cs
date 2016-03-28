using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackingStones.Models
{
    public class HotSpot
    {
        public Rectangle Location;
        public string Name;
        public bool HasBeenClicked;

        public event HotSpotEvent Clicked;

        public HotSpot(Rectangle location, string name)
        {
            Location = location;
            Name = name;
            HasBeenClicked = false;
        }

        public void Click()
        {
            HasBeenClicked = true;
            if (Clicked != null)
                Clicked(this);
        }
    }

    public delegate void HotSpotEvent(HotSpot sender);
}
