using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HitTheMouse.Entities
{
    public class Cloud
    {
        private Texture2D _texture;
        public Vector2 Position { get; private set; }
        private float _speed; // Pixels per second
        private float _width;
        private float _height;

        /// <summary>
        /// Initializes a new instance of the Cloud class.
        /// </summary>
        /// <param name="texture">Texture of the cloud.</param>
        /// <param name="position">Initial position of the cloud.</param>
        /// <param name="speed">Horizontal speed of the cloud.</param>
        /// <param name="width">Width of the cloud.</param>
        /// <param name="height">Height of the cloud.</param>
        public Cloud(Texture2D texture, Vector2 position, float speed, float width, float height)
        {
            _texture = texture;
            Position = position;
            _speed = speed;
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Updates the cloud's position based on its speed and the elapsed time.
        /// </summary>
        /// <param name="deltaTime">Elapsed time since the last update (in seconds).</param>
        public void Update(float deltaTime)
        {
            Position = new Vector2(Position.X + _speed * deltaTime, Position.Y);
        }

        /// <summary>
        /// Draws the cloud on the screen.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch used for drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, (int)_width, (int)_height), Color.White);
        }

        /// <summary>
        /// Determines whether the cloud has moved beyond the screen width.
        /// </summary>
        /// <param name="screenWidth">Width of the game screen.</param>
        /// <returns>True if the cloud is off-screen; otherwise, false.</returns>
        public bool IsOffScreen(int screenWidth)
        {
            return Position.X > screenWidth;
        }
    }
}
