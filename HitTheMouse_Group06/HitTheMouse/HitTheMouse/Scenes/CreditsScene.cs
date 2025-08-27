using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HitTheMouse.UI;
using HitTheMouse.Core;

namespace HitTheMouse.Scenes
{
    public class CreditScene : BaseScene
    {

        public CreditScene(Game game, GraphicsDevice graphicsDevice) : base(game, graphicsDevice)
        {
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);
            spriteBatch.Begin();

            // Draw background
            spriteBatch.Draw(Game.Content.Load<Texture2D>("Backgrounds/background_credits"), new Vector2(0, 0), Color.White);

            spriteBatch.End();
        }
    }
}
