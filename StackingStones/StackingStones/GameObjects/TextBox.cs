using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StackingStones.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;

namespace StackingStones.GameObjects
{
    public class TextBox : IGameObject
    {
        private Sprite _background;
        private Sprite _next;
        private SpriteFont _font;
        private readonly string _originalText;
        private string _visibleText;
        private Vector2 _textPosition;
        private bool _drawText;
        private int _textSpeed;
        private Dictionary<int, string[]> _commands;
        private Timer _textTimer;
        private bool _active;
        private bool _allTextDisplayed;
        private string _speakerName;
        private Vector2 _speakerNamePosition;
        SoundEffect _blip;

        public TextBox(Vector2 position, string speakerName, string text, int textSpeed)
        {
            _active = false;
            _allTextDisplayed = false;
            _font = Game1.ContentManager.Load<SpriteFont>("DefaultFont");
            _blip = Game1.ContentManager.Load<SoundEffect>("blip");
            _background = new Sprite("defaultTextBox", position, 0f, 1f, 0.5f);
            _next = new Sprite("next", new Vector2(position.X + 720, position.Y + 150), 0f, 1f, 1f);

            InitializeCommands(text);

            string regex = "(\\[.*?\\])";
            _originalText = Regex.Replace(text, regex, "");

            _speakerName = speakerName;

            _visibleText = "";
            _speakerNamePosition = new Vector2(position.X + 20, position.Y + 20);
            if (string.IsNullOrEmpty(speakerName))
                _textPosition = _speakerNamePosition;
            else
                _textPosition = new Vector2(position.X + 30, position.Y + 60);
            _textSpeed = textSpeed;
            _drawText = false;            
        }

        public void Show()
        {
            _active = true;
            var fade = new Fade(0f, 1f, 1.5f);
            fade.Completed += TextBoxVisible;
            _background.Apply(fade);
        }

        private void InitializeCommands(string text)
        {
            _commands = new Dictionary<int, string[]>();
            int indexModifier = 0;

            for(int i = 0; i < text.Length; i++)
            {
                if(text[i] == '[' && text[i-1] != '\\')
                {
                    string fullCommand = text.Substring(i + 1).Split(']')[0];
                    string[] splitCommand = fullCommand.Split(' ');

                    _commands.Add(i - indexModifier, splitCommand);
                    indexModifier += fullCommand.Length + 2;
                }
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_allTextDisplayed == false)
            {
                if (_visibleText != _originalText)
                {
                    int index = _visibleText.Length;
                    char nextCharacter = _originalText[index];

                    if (_commands.ContainsKey(index))
                    {
                        string[] splitCommand = _commands[index];
                        if (splitCommand[0] == "speed")
                        {
                            _textSpeed = int.Parse(splitCommand[1]);
                            SetTimer();
                        }
                    }

                    _visibleText += nextCharacter;
                    //_blip.Play(0.4f, 0, 0);
                }
                else
                    Done();
            }
        }

        private void Done()
        {
            _allTextDisplayed = true;
            _next.Apply(new Flash(0f, 1f, 1f));
        }

        private void SetTimer()
        {
            if (_textTimer != null)
                _textTimer.Enabled = false;

            _textTimer = new Timer(_textSpeed);
            _textTimer.Elapsed += Timer_Elapsed;
            _textTimer.AutoReset = true;
            _textTimer.Enabled = true;
        }

        private void TextBoxVisible(IEffect sender)
        {
            _drawText = true;
            SetTimer();
        }

        public void Update(GameTime gameTime)
        {
            if (_active)
            {
                _background.Update(gameTime);
                _next.Update(gameTime);
            }
        }

        public void Draw()
        {
            _background.Draw();
            Game1.SpriteBatch.DrawString(_font, _speakerName, _speakerNamePosition, new Color(Color.Aqua, _background.Alpha));
            if (_drawText)
            {
                Game1.SpriteBatch.DrawString(_font, _visibleText, _textPosition, Color.White);
            }
            _next.Draw();
        }
    }
}
