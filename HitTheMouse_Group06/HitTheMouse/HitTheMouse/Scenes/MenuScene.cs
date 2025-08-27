using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HitTheMouse.UI;
using HitTheMouse.Core;
using HitTheMouse.Entities;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace HitTheMouse.Scenes
{
    public class MenuScene : BaseScene
    {
        private SpriteFont _font;
        private Texture2D _buttonTexture;
        private Button _startButton;
        private Button _howToPlayButton;
        private Button _aboutButton;
        private Button _creditButton;
        private Button _leaderboardButton;
        private Button _quitButton;

        private Texture2D _backgroundTexture;
        private Texture2D _skyTexture;
        private Texture2D _cloudTexture; // Texture for clouds

        private string _title = "Hit The Mouse";

        // Cloud management
        private List<Cloud> _activeClouds;
        private float _cloudSpawnTimer;
        private float _cloudSpawnIntervalMin = 1.0f;
        private float _cloudSpawnIntervalMax = 3.0f;

        private Random _rand;

        public MenuScene(Game game, GraphicsDevice graphicsDevice) : base(game, graphicsDevice)
        {
            _activeClouds = new List<Cloud>();
            _cloudSpawnTimer = 0f;
            _rand = new Random();
        }

        public override void Load()
        {
            // Load fonts and textures
            _font = Game.Content.Load<SpriteFont>("Fonts/main_font");
            _buttonTexture = Game.Content.Load<Texture2D>("Textures/buttons");
            _backgroundTexture = Game.Content.Load<Texture2D>("Backgrounds/background_menu");
            _skyTexture = Game.Content.Load<Texture2D>("Textures/sky");
            _cloudTexture = Game.Content.Load<Texture2D>("Textures/sky");

            // Positioning buttons:
            int centerX = GraphicsDevice.Viewport.Width / 2;
            int startY = 150;
            int buttonHeight = 70;
            var buttonWidthRatio = buttonHeight * 2.5;
            int buttonWidth = (int)buttonWidthRatio;
            int spacing = 80;

            _startButton = new Button(_buttonTexture, new Rectangle(centerX - buttonWidth / 2, startY, buttonWidth, buttonHeight), _font, "Start Game");
            _startButton.OnClick = () => SceneManager.ChangeScene(new GameplayScene(Game, GraphicsDevice)); // Ensure GameplayScene exists

            _howToPlayButton = new Button(_buttonTexture, new Rectangle(centerX - buttonWidth / 2, startY + spacing, buttonWidth, buttonHeight), _font, "How to Play");
            _howToPlayButton.OnClick = () => SceneManager.ChangeScene(new HowToPlayScene(Game, GraphicsDevice));

            _aboutButton = new Button(_buttonTexture, new Rectangle(centerX - buttonWidth / 2, startY + spacing * 2, buttonWidth, buttonHeight), _font, "About");
            _aboutButton.OnClick = () => SceneManager.ChangeScene(new AboutScene(Game, GraphicsDevice));

            _creditButton = new Button(_buttonTexture, new Rectangle(centerX - buttonWidth / 2, startY + spacing * 3, buttonWidth, buttonHeight), _font, "Credits");
            _creditButton.OnClick = () => SceneManager.ChangeScene(new CreditScene(Game, GraphicsDevice));

            _leaderboardButton = new Button(_buttonTexture, new Rectangle(centerX - buttonWidth / 2, startY + spacing * 4, buttonWidth, buttonHeight), _font, "Leaderboard");
            _leaderboardButton.OnClick = () => SceneManager.ChangeScene(new LeaderBoardScene(Game, GraphicsDevice));

            _quitButton = new Button(_buttonTexture, new Rectangle(centerX - buttonWidth / 2, startY + spacing * 5, buttonWidth, buttonHeight), _font, "Quit");
            _quitButton.OnClick = () => Game.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            // Update buttons
            _startButton.Update();
            _howToPlayButton.Update();
            _aboutButton.Update();
            _creditButton.Update();
            _leaderboardButton.Update();
            _quitButton.Update();

            // Update cloud spawning
            _cloudSpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_cloudSpawnTimer >= GetRandomSpawnInterval())
            {
                SpawnCloud();
                _cloudSpawnTimer = 0f;
            }

            // Update active clouds
            for (int i = _activeClouds.Count - 1; i >= 0; i--)
            {
                _activeClouds[i].Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                if (_activeClouds[i].IsOffScreen(GraphicsDevice.Viewport.Width))
                {
                    _activeClouds.RemoveAt(i);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.LightSkyBlue);
            spriteBatch.Begin();

            // Draw skyTexture first (behind clouds)
            DrawBackgroundCentered(spriteBatch, _skyTexture);

            // Draw clouds on top of skyTexture
            foreach (var cloud in _activeClouds)
            {
                cloud.Draw(spriteBatch);
            }

            // Draw backgroundTexture on top of clouds, aligned to the bottom
            DrawBackgroundAlignedToBottom(spriteBatch, _backgroundTexture);


            // Draw buttons
            _startButton.Draw(spriteBatch);
            _howToPlayButton.Draw(spriteBatch);
            _aboutButton.Draw(spriteBatch);
            _creditButton.Draw(spriteBatch);
            _leaderboardButton.Draw(spriteBatch);
            _quitButton.Draw(spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// Draws a background texture aligned to the bottom of the screen,
        /// fitting the screen width while maintaining the aspect ratio.
        /// Portions of the background may extend beyond the screen vertically.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch used for drawing.</param>
        /// <param name="texture">Texture to draw.</param>
        private void DrawBackgroundAlignedToBottom(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (texture == null)
                return;

            float screenWidth = GraphicsDevice.Viewport.Width;
            float screenHeight = GraphicsDevice.Viewport.Height;

            float textureWidth = texture.Width;
            float textureHeight = texture.Height;

            // Calculate scale to fit the screen width
            float scale = screenWidth / textureWidth;

            // Calculate final size
            float finalWidth = textureWidth * scale;
            float finalHeight = textureHeight * scale + 20;

            // Align to bottom and center horizontally
            float startX = (screenWidth - finalWidth) / 2;
            float startY = screenHeight - finalHeight + 10;

            spriteBatch.Draw(texture, new Rectangle((int)startX, (int)startY, (int)finalWidth, (int)finalHeight), Color.White);
        }


        /// <summary>
        /// Draws a background texture centered horizontally and aligned to the bottom,
        /// fitting the width while maintaining the aspect ratio.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch used for drawing.</param>
        /// <param name="texture">Texture to draw.</param>
        private void DrawBackgroundCentered(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (texture == null)
                return;

            float screenWidth = GraphicsDevice.Viewport.Width;
            float screenHeight = GraphicsDevice.Viewport.Height;

            float textureWidth = texture.Width;
            float textureHeight = texture.Height;

            // Calculate scale to fit the screen width
            float scale = screenWidth / textureWidth;
            float scaledHeight = textureHeight * scale;

            // If scaledHeight exceeds screenHeight, adjust scale to fit height instead
            if (scaledHeight > screenHeight)
            {
                scale = screenHeight / textureHeight;
                scaledHeight = textureHeight * scale;
            }

            float finalWidth = textureWidth * scale;
            float finalHeight = textureHeight * scale;

            // Center horizontally and align to the bottom
            float startX = (screenWidth - finalWidth) / 2;
            float startY = screenHeight - finalHeight; // Align to bottom

            spriteBatch.Draw(texture, new Rectangle((int)startX, (int)startY, (int)finalWidth, (int)finalHeight), Color.White);
        }


        /// <summary>
        /// Spawns a new cloud with random properties.
        /// Ensures that no more than 3 clouds are active at any time.
        /// </summary>
        private void SpawnCloud()
        {
            if (_activeClouds.Count >= 3)
                return; // Maximum of 3 clouds

            // Determine how many new clouds to spawn based on existing clouds
            int existingClouds = _activeClouds.Count;
            int maxNewClouds = 3 - existingClouds;
            if (maxNewClouds <= 0)
                return;

            int newCloudsToSpawn = 0;
            double randValue = _rand.NextDouble();

            if (existingClouds == 0)
            {
                // 60% chance to spawn 1 cloud, 25% to spawn 2, 15% to spawn 3
                if (randValue < 0.6)
                    newCloudsToSpawn = 1;
                else if (randValue < 0.85)
                    newCloudsToSpawn = 2;
                else
                    newCloudsToSpawn = 3;
            }
            else if (existingClouds == 1)
            {
                // 10% chance to spawn 1 additional cloud
                if (randValue < 0.1)
                    newCloudsToSpawn = 1;
            }
            else if (existingClouds == 2)
            {
                // 5% chance to spawn 1 additional cloud
                if (randValue < 0.05)
                    newCloudsToSpawn = 1;
            }

            for (int i = 0; i < newCloudsToSpawn; i++)
            {
                // Random speed between 20 to 60 pixels per second
                float speed = (float)(_rand.NextDouble() * (60 - 20) + 20);

                // Random width between 100-150 pixels
                float width = (float)(_rand.NextDouble() * (150 - 100) + 100);
                // Maintain aspect ratio based on cloud texture
                float aspectRatio = (float)_cloudTexture.Height / _cloudTexture.Width;
                float height = width * aspectRatio;

                // Random Y position between 0 and screenHeight / 2
                float y = (float)(_rand.NextDouble() * (GraphicsDevice.Viewport.Height / 2));

                // Start at x = -width (off-screen left)
                Vector2 position = new Vector2(-width, y);

                // Create and add new cloud
                Cloud newCloud = new Cloud(_cloudTexture, position, speed, width, height);
                _activeClouds.Add(newCloud);
                Debug.WriteLine($"Spawned cloud at Y: {y}, Speed: {speed}, Size: ({width}, {height})");
            }
        }

        /// <summary>
        /// Generates a random spawn interval between cloud spawns.
        /// </summary>
        /// <returns>Random spawn interval in seconds.</returns>
        private float GetRandomSpawnInterval()
        {
            return (float)(_rand.NextDouble() * (_cloudSpawnIntervalMax - _cloudSpawnIntervalMin) + _cloudSpawnIntervalMin);
        }
    }
}
