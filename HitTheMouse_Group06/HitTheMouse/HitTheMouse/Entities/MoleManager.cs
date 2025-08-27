using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HitTheMouse.Entities
{
    public class MoleManager
    {
        private List<MoleHole> _holes;
        private Texture2D _spriteSheet;
        private Rectangle[] _frames;
        private float _frameDuration;
        private Random _rand = new Random();

        public MoleManager(Texture2D spriteSheet, Rectangle[] frames, float frameDuration)
        {
            _spriteSheet = spriteSheet;
            _frames = frames;
            _frameDuration = frameDuration;
            _holes = new List<MoleHole>();
        }

        /// <summary>
        /// Initializes the mole holes based on the current level's configuration.
        /// </summary>
        public void InitializeHoles(int holeCount, int rows, int columns, Vector2 startPosition, int gap, float spawnIntervalMin, float spawnIntervalMax)
        {
            _holes.Clear();

            // Layout holes in a grid
            int frameWidth = _frames[0].Width;
            int frameHeight = _frames[0].Height;

            int placed = 0;
            for (int r = 0; r < rows && placed < holeCount; r++)
            {
                for (int c = 0; c < columns && placed < holeCount; c++)
                {
                    Vector2 pos = new Vector2(startPosition.X + c * (frameWidth + gap), startPosition.Y + r * (frameHeight + gap));
                    _holes.Add(new MoleHole(_spriteSheet, pos, _frames, _frameDuration, spawnIntervalMin, spawnIntervalMax));
                    placed++;
                }
            }
        }


        /// <summary>
        /// Updates all mole holes.
        /// </summary>
        public void Update(GameTime gameTime, float stayTime)
        {
            foreach (var hole in _holes)
            {
                hole.Update(gameTime, stayTime);
            }
        }

        /// <summary>
        /// Draws all mole holes.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var hole in _holes)
            {
                hole.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Checks if any mole was hit based on the click position.
        /// </summary>
        public bool CheckHit(Vector2 clickPosition)
        {
            bool hit = false;
            foreach (var hole in _holes)
            {
                if (hole.CheckHit(clickPosition))
                {
                    hit = true;
                    break;
                }
            }
            return hit;
        }
    }
}
