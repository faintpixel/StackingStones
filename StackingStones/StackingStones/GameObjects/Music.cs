using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackingStones.GameObjects
{
    public static class Music
    {
        private static float _targetVolume;
        private static float _fadeSpeed;
        private static MusicState _state;

        public static void Play(string contentName, float volume, bool repeating)
        {
            Song music = Game1.ContentManager.Load<Song>(contentName);
            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = repeating;
            MediaPlayer.Volume = volume;
            _state = MusicState.Playing;
            _targetVolume = volume;
        }

        public static void FadeToVolume(float volume, float speed)
        {
            if (volume > MediaPlayer.Volume)
                _state = MusicState.FadeIn;
            else
                _state = MusicState.FadeOut;

            _targetVolume = volume;
            _fadeSpeed = speed;
        }

        public static void Update(GameTime gameTime)
        {
            if(_state == MusicState.FadeIn)
            {
                float amountToChange = _fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                MediaPlayer.Volume += amountToChange;
                if(MediaPlayer.Volume >= _targetVolume)
                {
                    MediaPlayer.Volume = _targetVolume;
                    _state = MusicState.Playing;
                }
            }
            else if(_state == MusicState.FadeOut)
            {
                float amountToChange = _fadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                MediaPlayer.Volume -= amountToChange;
                if (MediaPlayer.Volume <= _targetVolume)
                {
                    MediaPlayer.Volume = _targetVolume;
                    _state = MusicState.Playing;
                }
            }
        }

        private enum MusicState
        {
            FadeIn,
            FadeOut,
            Playing
        }
    }
}
