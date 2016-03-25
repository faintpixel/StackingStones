using StackingStones.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Timers;
using Microsoft.Xna.Framework.Graphics;
using StackingStones.Input;
using Microsoft.Xna.Framework.Input;

namespace StackingStones.Models
{
    public class Choice : IGameObject
    {
        public string Prompt;
        public readonly List<string> Choices;
        public int SelectedChoiceIndex;

        private Timer _textTimer;
        private int _writtenTextIndex;
        private string _writtenText;
        private bool _doneWritingChoices;

        private Vector2 _promptPosition;
        private List<Vector2> _positions;
        private SpriteFont _font;
        private KeyboardHelper _keyboardHelper;
        private bool _acceptingInput;
        private int _textSpeed = 20;

        public event ChoiceEvent ChoiceSelected;

        public delegate void ChoiceEvent(Choice sender);

        public Choice(string prompt, List<string> choices, Vector2 position)
        {
            _acceptingInput = false;
            _doneWritingChoices = false;
            Choices = choices;
            Prompt = prompt;
            SelectedChoiceIndex = 0;
            _writtenText = "";
            _writtenTextIndex = 0;
            _promptPosition = new Vector2(position.X + 20, position.Y + 20);
            _font = Game1.ContentManager.Load<SpriteFont>("DefaultFont");

            _positions = new List<Vector2>();
            for (int i = 0; i < Choices.Count; i++)
                _positions.Add(new Vector2(position.X + 30, position.Y + 60 + (25 * i)));

            List<Keys> keys = new List<Keys>();
            keys.Add(Keys.Up);
            keys.Add(Keys.Down);
            keys.Add(Keys.Space);
            keys.Add(Keys.Enter);
            _keyboardHelper = new KeyboardHelper(keys);
            _keyboardHelper.KeyPressed += _keyboardHelper_KeyPressed;
        }

        private void _keyboardHelper_KeyPressed(KeyboardHelper sender, Keys key)
        {
            if(key == Keys.Up)
            {
                if (SelectedChoiceIndex == 0)
                    SelectedChoiceIndex = Choices.Count - 1;
                else
                    SelectedChoiceIndex--;
            }
            else if(key == Keys.Down)
            {
                if (SelectedChoiceIndex == Choices.Count - 1)
                    SelectedChoiceIndex = 0;
                else
                    SelectedChoiceIndex++;
            }
            else
            {
                if (_doneWritingChoices)
                {
                    if (ChoiceSelected != null)
                        ChoiceSelected(this);
                }
                else
                {
                    _doneWritingChoices = true;
                    _writtenTextIndex = 100;
                }
            }
        }

        public void Draw(float alpha)
        {
            Game1.SpriteBatch.DrawString(_font, Prompt, _promptPosition, new Color(Color.Green, alpha));

            for(int i = 0; i < Choices.Count; i++)
            {
                Color color = new Color(Color.White, alpha);
                if (SelectedChoiceIndex == i)
                    color = new Color(Color.Yellow, alpha);

                if(_writtenTextIndex == i)
                    Game1.SpriteBatch.DrawString(_font, _writtenText, _positions[i], color);
                else if(_writtenTextIndex > i)
                    Game1.SpriteBatch.DrawString(_font, Choices[i], _positions[i], color);
            }
        }

        public void Draw()
        {
            Draw(1f);
        }

        public void Update(GameTime gameTime)
        {
            if(_acceptingInput)
                _keyboardHelper.Update(gameTime);
        }

        public void Start()
        {
            if (Choices.Count > 0)
            {
                _textTimer = new Timer(_textSpeed);
                _textTimer.Elapsed += _textTimer_Elapsed;
                _textTimer.Enabled = true;

                WaitABitForUserInput();
            }
        }

        private void _textTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_doneWritingChoices == false)
            {
                if (_writtenText != Choices[_writtenTextIndex])
                    _writtenText += Choices[_writtenTextIndex][_writtenText.Length];
                else if (_writtenTextIndex + 1 < Choices.Count)
                {
                    _writtenText = "";
                    _writtenTextIndex++;
                }
                else
                    _doneWritingChoices = true;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _acceptingInput = true;
        }

        private void WaitABitForUserInput()
        {
            // this is just to make sure the user doesn't accidentally select something.
            _acceptingInput = false;
            Timer timer = new Timer(100);
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
        }
    }
}
