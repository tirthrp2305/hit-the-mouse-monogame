using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HitTheMouse.UI;
using HitTheMouse.Core; // For SceneManager
using System.Diagnostics;
using System;

namespace HitTheMouse.Scenes
{
    public class AboutScene : BaseScene
    {
        private Texture2D backgroundTexture;

        public AboutScene(Game game, GraphicsDevice graphicsDevice)
            : base(game, graphicsDevice)
        {
        }

        public override void Load()
        {
            backgroundTexture = Game.Content.Load<Texture2D>("Backgrounds/background_about");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Clear the screen with a vibrant solid color
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // Draw background
            spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), Color.White);


            spriteBatch.End();
        }

    }
}
