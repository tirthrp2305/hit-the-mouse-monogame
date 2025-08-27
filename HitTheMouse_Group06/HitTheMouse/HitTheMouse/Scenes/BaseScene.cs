using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HitTheMouse.Scenes
{
    public abstract class BaseScene
    {
        protected GraphicsDevice GraphicsDevice;
        protected Game Game; // reference to main game if needed

        public BaseScene(Game game, GraphicsDevice graphicsDevice)
        {
            Game = game;
            GraphicsDevice = graphicsDevice;
        }

        public virtual void Load() { }
        public virtual void Unload() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
