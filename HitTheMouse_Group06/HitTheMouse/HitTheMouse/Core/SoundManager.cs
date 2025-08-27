using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace HitTheMouse.Core
{
    public enum SoundType
    {
        Click,
        Hit,
        BackgroundMusic
    }

    public static class SoundManager
    {
        private static Dictionary<SoundType, SoundEffect> _soundEffects = new Dictionary<SoundType, SoundEffect>();
        private static Song _backgroundMusic;

        public static void LoadContent(ContentManager content)
        {
            _soundEffects[SoundType.Click] = content.Load<SoundEffect>("Music/click");
            _soundEffects[SoundType.Hit] = content.Load<SoundEffect>("Music/hit");
            _backgroundMusic = content.Load<Song>("Music/background_music");
        }

        public static void PlaySound(SoundType soundType)
        {
            if (_soundEffects.ContainsKey(soundType))
            {
                _soundEffects[soundType].Play();
            }
        }

        public static void PlayMusic()
        {
            if (_backgroundMusic != null)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(_backgroundMusic);
            }
        }

        public static void StopMusic()
        {
            MediaPlayer.Stop();
        }
    }
}
