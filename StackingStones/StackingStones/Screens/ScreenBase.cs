using Microsoft.Xna.Framework;
using StackingStones.GameObjects;
using StackingStones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackingStones.Screens
{
    public class ScreenBase
    {
        protected TextBox _textBox;

        protected event EventHandler DoneShowingMessage;
        protected event EventHandler StartShowingMessage;

        public ScreenBase()
        {
            var script = new Script();
            script.Dialogue.Add(new Dialogue("", "", Color.Black));
            _textBox = new TextBox(new Vector2(240, 500), script);
        }

        protected void ShowMessage(string message)
        {
            List<string> messages = new List<string>();
            messages.Add(message);
            ShowMessage(messages);
        }

        protected void ShowMessage(List<string> messages)
        {
            if (StartShowingMessage != null)
                StartShowingMessage(this, null);

            var script = new Script();
            script.Dialogue = new List<Dialogue>();
            foreach (var message in messages)
                script.Dialogue.Add(new Dialogue("", message, Color.Black));

            _textBox = new TextBox(new Vector2(240, 500), script);
            _textBox.ScriptedEventReached += Message_ScriptedEventReached;
            _textBox.Completed += TextBoxCompleted;
            _textBox.Show(true);
        }

        protected void ShowMessage(List<Dialogue> dialogue)
        {
            if (StartShowingMessage != null)
                StartShowingMessage(this, null);

            var script = new Script();
            script.Dialogue = dialogue;

            _textBox = new TextBox(new Vector2(240, 500), script);
            _textBox.Completed += TextBoxCompleted;
            _textBox.ScriptedEventReached += Message_ScriptedEventReached;
            _textBox.Show(true);
        }

        protected virtual void Message_ScriptedEventReached(TextBox sender, string eventId)
        {
            Console.WriteLine("No event handler for script.");
        }

        private void TextBoxCompleted(TextBox sender)
        {
            _textBox.Hide(1f);
            if (DoneShowingMessage != null)
                DoneShowingMessage(this, null);
        }

        
    }
}
