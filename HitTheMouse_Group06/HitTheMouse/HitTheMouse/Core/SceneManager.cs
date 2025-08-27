using HitTheMouse.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HitTheMouse.Core
{
    public static class SceneManager
    {
        private static BaseScene _currentScene;

        public static BaseScene CurrentScene => _currentScene;

        public static void ChangeScene(BaseScene newScene)
        {
            if (_currentScene is GameplayScene && newScene is MenuScene)
            {
                ((GameplayScene)_currentScene).SaveScore();
                ((GameplayScene)_currentScene).Load();
            }

            _currentScene?.Unload();
            _currentScene = newScene;
            _currentScene.Load();

            if (_currentScene is LeaderBoardScene)
            {
                ((LeaderBoardScene)_currentScene).Load();
            }
        }

        public static void Update(GameTime gameTime)
        {
            _currentScene?.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            _currentScene?.Draw(spriteBatch);
        }


    }
}
