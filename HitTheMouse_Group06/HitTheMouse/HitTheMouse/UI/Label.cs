using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HitTheMouse.UI
{
    public class Label
    {
        private string _text;
        private SpriteFont _font;
        private Vector2 _position;
        private Color _color;
        private float _scale;

        public Label(SpriteFont font, string text, Vector2 position, Color color, float scale = 1f)
        {
            _font = font;
            _text = text;
            _position = position;
            _color = color;
            _scale = scale;
        }

        public string Text
        {
            get => _text;
            set => _text = value;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, _text, _position, _color, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }
    }
}
