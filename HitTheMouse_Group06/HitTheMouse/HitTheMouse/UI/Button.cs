using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HitTheMouse.Core;
using System; // for Action
using System.Diagnostics; // for Debug.WriteLine

namespace HitTheMouse.UI
{
    public class Button
    {
        private Texture2D _texture;
        private Rectangle _drawRect;
        private Rectangle? _sourceRect;
        private string _text;
        private SpriteFont _font;
        private Color _textColor = Color.Black;
        private bool _isHovered;
        private bool _isPressed;

        public Action OnClick { get; set; }

        public Button(Texture2D texture, Rectangle drawRect, SpriteFont font, string text = "", Rectangle? sourceRect = null)
        {
            _texture = texture;
            _drawRect = drawRect;
            _sourceRect = sourceRect;
            _font = font;
            _text = text;
        }

        public void Update()
        {
            Vector2? clickPos = InputManager.GetClickPosition();
            Vector2? pointerPos = InputManager.GetCurrentPointerPosition();

            _isHovered = false;
            _isPressed = false;

            if (pointerPos.HasValue && _drawRect.Contains(pointerPos.Value))
            {
                _isHovered = true;
            }

            if (clickPos.HasValue && _drawRect.Contains(clickPos.Value))
            {
                _isPressed = true;
                OnClick?.Invoke();

                // Debugging: Log the click
                Debug.WriteLine($"Button '{_text}' clicked at position {clickPos.Value}");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color tint = Color.White;
            if (_isHovered) tint = Color.LightGray;

            spriteBatch.Draw(_texture, _drawRect, _sourceRect, tint);

            if (!string.IsNullOrEmpty(_text))
            {
                Vector2 textSize = _font.MeasureString(_text);
                Vector2 textPos = new Vector2(
                    _drawRect.X + (_drawRect.Width - textSize.X) / 2,
                    _drawRect.Y + (_drawRect.Height - textSize.Y) / 2
                );
                spriteBatch.DrawString(_font, _text, textPos, _textColor);
            }
        }
    }
}
