using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StackingStones.GameObjects;
using StackingStones.Effects;
using StackingStones.Models;

namespace StackingStones.Screens
{
    public class Scene13_WrathOfTheSpirit : ScreenBase, IScreen
    {
        private Sprite _background;
        private ScreenInteraction _explore;

        public event ScreenEvent Completed;

        public Scene13_WrathOfTheSpirit()
        {
            _background = new Sprite("Backgrounds\\Placeholders\\stackedStones", new Vector2(0, 0), 0f, 1f, 0.5f);

            var fade = new Fade(0f, 1f, 0.5f);
            fade.Completed += Fade_Completed;
            _background.Apply(fade);

            List<HotSpot> hotSpots = new List<HotSpot>();
            var stones = new HotSpot(new Rectangle(1025, 394, 154, 154), "Stacked Stones");
            stones.Clicked += Stones_Clicked;
            hotSpots.Add(stones);

            var next = new HotSpot(new Rectangle(1149, 606, 92, 86), "Continue on path");
            next.Clicked += Next_Clicked;
            hotSpots.Add(next);

            _explore = new ScreenInteraction(false, hotSpots);

            this.StartShowingMessage += Scene6_StackedStones_StartShowingMessage;
            this.DoneShowingMessage += Scene6_StackedStones_DoneShowingMessage;
            Music.Play("Music\\213893_misterious_classical_filmm", 0f, true);
            Music.FadeToVolume(0.5f, 0.5f);
        }

        private void Scene6_StackedStones_DoneShowingMessage(object sender, EventArgs e)
        {
            _explore.Active = true;
        }

        private void Scene6_StackedStones_StartShowingMessage(object sender, EventArgs e)
        {
            _explore.Active = false;
        }

        private void Next_Clicked(HotSpot sender)
        {
            _explore.Active = false;

            var fade = new Fade(1f, 0f, 1f);
            fade.Completed += FadeOut_Completed;
            _background.Apply(fade);
        }

        private void FadeOut_Completed(IEffect sender)
        {
            if (Completed != null)
                Completed(this);
        }

        private void Stones_Clicked(HotSpot sender)
        {
            ShowMessage("Well, I'll be! I wonder where this came from?");
        }

        private void Fade_Completed(IEffect sender)
        {
            _explore.Active = true;
        }

        public void Draw()
        {
            _background.Draw();
            _explore.Draw();

            _textBox.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _background.Update(gameTime);
            _explore.Update(gameTime);

            _textBox.Update(gameTime);
            Music.Update(gameTime);
        }
    }
}
