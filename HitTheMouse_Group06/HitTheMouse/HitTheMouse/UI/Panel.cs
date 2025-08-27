using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HitTheMouse.UI
{
    public class Panel
    {
        private Texture2D _backgroundTexture;
        private Rectangle _drawRect;
        private Color _color;

        public Panel(Texture2D backgroundTexture, Rectangle drawRect, Color color)
        {
            _backgroundTexture = backgroundTexture;
            _drawRect = drawRect;
            _color = color;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_backgroundTexture, _drawRect, _color);
        }
    }
}
