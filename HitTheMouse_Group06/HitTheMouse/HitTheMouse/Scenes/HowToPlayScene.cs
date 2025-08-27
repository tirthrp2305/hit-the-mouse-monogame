using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HitTheMouse.UI;
using HitTheMouse.Core; // For SceneManager

namespace HitTheMouse.Scenes
{
    public class HowToPlayScene : BaseScene
    {


        public HowToPlayScene(Game game, GraphicsDevice graphicsDevice) : base(game, graphicsDevice)
        {
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.ForestGreen);
            spriteBatch.Begin();

            // Draw background
            spriteBatch.Draw(Game.Content.Load<Texture2D>("Backgrounds/background_howtoplay"), new Vector2(0, 0), Color.White);


            spriteBatch.End();
        }
    }
}
