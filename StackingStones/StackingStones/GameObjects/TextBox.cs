using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StackingStones.Effects;
using StackingStones.Input;
using StackingStones.Models;
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
        private bool _active;
        private State _state;

        private Sprite _background;
        private Sprite _next;
        private SpriteFont _font;
        private SoundEffect _blip;

        private KeyboardHelper _keyboardHelper;
        private Timer _textTimer;

        private Vector2 _speakerNamePosition;
        private Vector2 _noSpeakerTextPosition;

        private Script _script;
        private int _scriptIndex;
        private string _writtenText;
        private bool _allTextDisplayed;

        private Vector2 _textPosition;
        private bool _drawText;
        

        public event TextBoxEvent Completed;
        public event DialogueEvent DialogueLineComplete;
        public event ScriptEvent ScriptedEventReached;

        public delegate void TextBoxEvent(TextBox sender);
        public delegate void DialogueEvent(TextBox sender, Script script, int scriptIndex);
        public delegate void ScriptEvent(TextBox sender, string eventId);

        public TextBox(Vector2 position, Script script)
        {
            _font = Game1.ContentManager.Load<SpriteFont>("DefaultFont");
            _blip = Game1.ContentManager.Load<SoundEffect>("blip");
            _background = new Sprite("defaultTextBox", position, 0f, 1f, 0.5f);
            _next = new Sprite("next", new Vector2(position.X + 720, position.Y + 150), 0f, 1f, 1f);

            _state = State.ShowingText;
            _active = false;

            _speakerNamePosition = new Vector2(position.X + 20, position.Y + 20);
            _noSpeakerTextPosition = new Vector2(position.X + 20, position.Y + 20);
            _textPosition = new Vector2(position.X + 30, position.Y + 60);

            _script = script;

            SetScriptIndex(0);
            
            _drawText = false;

            InitializeKeyboardHelper();
        }

        private void InitializeKeyboardHelper()
        {
            var keys = new List<Keys>();
            keys.Add(Keys.Enter);
            keys.Add(Keys.Space);
            _keyboardHelper = new KeyboardHelper(keys);
            _keyboardHelper.KeyPressed += _keyboardHelper_KeyReleased;
        }

        private void _keyboardHelper_KeyReleased(KeyboardHelper sender, Keys key)
        {
            if (_state == State.ShowingText)
                HandleKeyRelease_TextMode();
            else
                HandleKeyRelease_ChoiceMode();
        }

        private void HandleKeyRelease_TextMode()
        {
            if (_allTextDisplayed) // TO DO - need to actually draw the characters one at a time still to get the events firing and all that stuff.
            {
                if (DialogueLineComplete != null)
                    DialogueLineComplete(this, _script, _scriptIndex);

                if (_scriptIndex == _script.Dialogue.Count - 1)
                {
                    // All dialogue is done.
                    SwitchToChoices();
                }
                else
                {                    
                    SetScriptIndex(_scriptIndex + 1);
                }
            }
            else
            {
                _writtenText = _script.Dialogue[_scriptIndex].Text;
            }
        }

        private void HandleKeyRelease_ChoiceMode()
        {
            if (Completed != null)
                Completed(this);
        }

        private void SetScriptIndex(int index)
        {
            _scriptIndex = index;
            _writtenText = "";
            _allTextDisplayed = false;
            _next.RemoveAllEffects();
            _next.Alpha = 0f;

            if(index != 0) // kind of a workaround. we don't want it to start the timer until it's actually showing.
                SetTimer();
        }

        public void Show(bool fadeIn = false)
        {
            _active = true;
            if(fadeIn)
            {
                var fade = new Fade(0f, 1f, 1.5f);
                fade.Completed += TextBoxVisible;
                _background.Apply(fade);
            }
            else
            {
                _background.Alpha = 1f;
                TextBoxVisible(null);
            }            
        }

        public void Hide(float speed)
        {
            _active = false;
            _background.Apply(new Fade(1f, 0f, speed));
        }
        
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_allTextDisplayed == false)
            {
                if (_writtenText != _script.Dialogue[_scriptIndex].Text)
                {
                    int index = _writtenText.Length;
                    char nextCharacter = _script.Dialogue[_scriptIndex].Text[index];

                    if (_script.Dialogue[_scriptIndex].Commands.ContainsKey(index))
                    {
                        string[] splitCommand = _script.Dialogue[_scriptIndex].Commands[index];
                        if (splitCommand[0] == "speed")
                        {
                            _script.Dialogue[_scriptIndex].TextSpeed = int.Parse(splitCommand[1]);
                            SetTimer();
                        }
                        else if(splitCommand[0] == "sound")
                        {
                            SoundEffect sound = Game1.ContentManager.Load<SoundEffect>(splitCommand[1]);
                            sound.Play();
                        }
                        else if(splitCommand[0] == "event")
                        {
                            if (ScriptedEventReached != null)
                                ScriptedEventReached(this, splitCommand[1]);
                        }
                    }

                    _writtenText += nextCharacter;
                    //_blip.Play(0.4f, 0, 0);
                }
                else
                    DoneDialogue();
            }
        }

        private void DoneDialogue()
        {
            _allTextDisplayed = true;
            _next.Apply(new Flash(0f, 1f, 1f));
        }

        private void SwitchToChoices()
        {
            _state = State.ShowingChoices;
            _script.Choice.Start();
        }

        private void SetTimer()
        {
            if (_textTimer != null)
                _textTimer.Enabled = false;

            _textTimer = new Timer(_script.Dialogue[_scriptIndex].TextSpeed);
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
            _background.Update(gameTime);
            if (_active)
            {
                _next.Update(gameTime);
                _keyboardHelper.Update(gameTime);
                if(_state == State.ShowingChoices)
                    _script.Choice.Update(gameTime);
            }
        }

        public void Draw()
        {
            _background.Draw();
            if(_state == State.ShowingText)
                Game1.SpriteBatch.DrawString(_font, _script.Dialogue[_scriptIndex].Speaker, _speakerNamePosition, new Color(Color.Aqua, _background.Alpha));
            if (_drawText)
            {
                if (_state == State.ShowingText)
                {
                    Vector2 position = _textPosition;
                    if (string.IsNullOrEmpty(_script.Dialogue[_scriptIndex].Speaker))
                        position = _noSpeakerTextPosition;

                    Game1.SpriteBatch.DrawString(_font, _writtenText, position, _script.Dialogue[_scriptIndex].Color);
                }
                else
                {
                    _script.Choice.Draw(_background.Alpha);
                }
            }
            if(_active)
                _next.Draw();
        }

        private enum State
        {
            ShowingText,
            ShowingChoices
        }
    }
}
