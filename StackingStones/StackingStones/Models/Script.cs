using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackingStones.Models
{
    public class Script
    {
        public List<Dialogue> Dialogue;
        public Choice Choice;

        public Script()
        {
            Dialogue = new List<Dialogue>();
        }
    }
}
