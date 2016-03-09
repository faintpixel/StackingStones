using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StackingStones.Models
{
    public class Dialogue
    {
        public string Speaker;
        public string Text;
        public string RawText;
        public Color Color;
        public int TextSpeed;

        public Dictionary<int, string[]> Commands;

        public Dialogue(string speaker, string rawText, Color color, int textSpeed = 50)
        {
            Speaker = speaker;
            RawText = rawText;
            Color = color;
            TextSpeed = textSpeed;

            InitializeCommands(rawText);

            string regex = "(\\[.*?\\])";
            Text = Regex.Replace(rawText, regex, "");
        }

        private void InitializeCommands(string text)
        {
            Commands = new Dictionary<int, string[]>();
            int indexModifier = 0;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '[' && (i - 1 < 0 || text[i - 1] != '\\'))
                {
                    string fullCommand = text.Substring(i + 1).Split(']')[0];
                    string[] splitCommand = fullCommand.Split(' ');

                    Commands.Add(i - indexModifier, splitCommand);
                    indexModifier += fullCommand.Length + 2;
                }
            }
        }
    }
}
