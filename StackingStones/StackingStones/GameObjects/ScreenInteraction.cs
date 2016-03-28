using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StackingStones.Input;
using Microsoft.Xna.Framework.Input;
using StackingStones.Models;

namespace StackingStones.GameObjects
{
    public class ScreenInteraction : IGameObject
    {
        public bool Active;

        private Sprite _regularCursor;
        private Sprite _searchCursor;
        private MouseHelper _mouseHelper;
        private bool _overHotSpot;
        private HotSpot _selectedHotSpot;
        private SpriteFont _font;

        private List<HotSpot> _hotSpots;

        public ScreenInteraction(bool active, List<HotSpot> hotSpots)
        {
            Active = active;
            _font = Game1.ContentManager.Load<SpriteFont>("DefaultFont");

            _mouseHelper = new MouseHelper();
            _mouseHelper.MouseButtonPressed += _mouseHelper_MouseButtonPressed;

            _regularCursor = new Sprite("Sprites\\regularCursor", new Vector2(0, 0), 1f, 1f, 1f);
            _searchCursor = new Sprite("Sprites\\searchCursor", new Vector2(0, 0), 1f, 1f, 1f);
            _overHotSpot = false;

            _hotSpots = hotSpots;
        }

        private void _mouseHelper_MouseButtonPressed(MouseHelper sender, MouseButtons button, MouseState state)
        {
            string message = String.Format("clicked at {0}, {1}", state.X, state.Y);
            if (_overHotSpot)
            {
                _selectedHotSpot.Click();
                message += " - " + _selectedHotSpot.Name;
            }
            Console.WriteLine(message);
        }

        public void Draw()
        {
            if(Active)
            {
                if (_overHotSpot)
                {
                    _searchCursor.Draw();
                    DrawSelectedHotSpotName();
                }
                else
                    _regularCursor.Draw();
            }
        }

        private void DrawSelectedHotSpotName()
        {
            Vector2 position = new Vector2(_selectedHotSpot.Location.Center.X, _selectedHotSpot.Location.Center.Y);
            Vector2 textSize = _font.MeasureString(_selectedHotSpot.Name);
            position.X -= textSize.X / 2;
            position.Y -= textSize.Y / 2;

            Color color = Color.Yellow;
            if (_selectedHotSpot.HasBeenClicked)
                color = Color.Orange;
            DrawText(_selectedHotSpot.Name, position, color, Color.Black);
        }

        private void DrawText(string text, Vector2 position, Color color, Color outlineColor)
        {
            Game1.SpriteBatch.DrawString(_font, text, new Vector2(position.X + 1, position.Y + 1), outlineColor);
            Game1.SpriteBatch.DrawString(_font, text, new Vector2(position.X + 1, position.Y - 1), outlineColor);
            Game1.SpriteBatch.DrawString(_font, text, new Vector2(position.X - 1, position.Y + 1), outlineColor);
            Game1.SpriteBatch.DrawString(_font, text, new Vector2(position.X - 1, position.Y - 1), outlineColor);
            Game1.SpriteBatch.DrawString(_font, text, position, color);
        }

        public void Update(GameTime gameTime)
        {
            if (Active)
            {
                _mouseHelper.Update();
                MouseState state = Mouse.GetState();
                _regularCursor.Position = new Vector2(state.X, state.Y);
                _searchCursor.Position = new Vector2(state.X, state.Y);

                CheckIfOverHotSpot(state.X, state.Y);
            }
        }

        private void CheckIfOverHotSpot(float x, float y)
        {
            bool overHotSpot = false;

            foreach (HotSpot spot in _hotSpots)
            {
                if (spot.Location.Contains(x, y))
                {
                    overHotSpot = true;
                    _selectedHotSpot = spot;
                    break;
                }
            }

            _overHotSpot = overHotSpot;
        }
    }
}
