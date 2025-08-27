using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HitTheMouse.Core;
using HitTheMouse.Logic;
using HitTheMouse.Entities;
using HitTheMouse.UI;
using System.Diagnostics;

namespace HitTheMouse.Scenes
{
    public class GameplayScene : BaseScene
    {
        private SpriteFont _font;
        private Texture2D _backgroundTexture;
        private Texture2D _moleSpriteSheet;
        private Texture2D _buttonTexture;

        private MoleManager _moleManager;
        private GameState _gameState;

        private Label _scoreLabel;
        private Label _levelLabel;
        private Label _timeLabel;
        private Button _backButton;

        private Rectangle[] _moleFrames;
        private float _frameDuration = Config.FrameDuration; // Use consistent frame duration from Config

        private float _levelTime = 30f;  // 30 seconds per level
        private float _currentTimer;

        public GameplayScene(Game game, GraphicsDevice graphicsDevice) : base(game, graphicsDevice)
        {
            _gameState = new GameState();
            _gameState.StartGame(); // Start from level 1 (index 0)
        }

        public override void Load()
        {
            // Load content
            _backgroundTexture = Game.Content.Load<Texture2D>("Backgrounds/background_gameplay");
            _font = Game.Content.Load<SpriteFont>("Fonts/main_font");
            _moleSpriteSheet = Game.Content.Load<Texture2D>("Textures/mouse_spritesheet");
            _buttonTexture = Game.Content.Load<Texture2D>("Textures/buttons");

            // Setup mole frames
            // Assume the mouse_spritesheet is a single row of 6 frames
            int frameWidth = _moleSpriteSheet.Width / 6;
            int frameHeight = _moleSpriteSheet.Height;
            _moleFrames = new Rectangle[6];
            for (int i = 0; i < 6; i++)
            {
                _moleFrames[i] = new Rectangle(i * frameWidth, 0, frameWidth, frameHeight);
            }

            // Initialize MoleManager for current level
            InitializeLevel();

            // Initialize UI elements
            _scoreLabel = new Label(_font, "Score: 0", new Vector2(50, 50), Color.White);
            _levelLabel = new Label(_font, "Level: 1", new Vector2(50, 100), Color.White);
            _timeLabel = new Label(_font, "Time: 30", new Vector2(50, 150), Color.White);

            _backButton = new Button(_buttonTexture, new Rectangle(GraphicsDevice.Viewport.Width - 220, 50, 200, 50), _font, "Exit");
            _backButton.OnClick = () =>
            {
                SoundManager.StopMusic();

                SceneManager.ChangeScene(new MenuScene(Game, GraphicsDevice));
            };

            _currentTimer = _levelTime;

            SoundManager.PlayMusic();
        }

        /// <summary>
        /// Initializes the current level's configuration.
        /// </summary>
        private void InitializeLevel()
        {
            var levelData = _gameState.GetCurrentLevelData();

            _moleManager = new MoleManager(_moleSpriteSheet, _moleFrames, _frameDuration);

            int holeCount = levelData.HoleCount;
            int rows = levelData.Rows;
            int columns = levelData.Columns;
            int gap = 20; // You can adjust the gap as needed

            // Retrieve hole dimensions from the first frame
            int frameWidth = _moleFrames[0].Width;
            int frameHeight = _moleFrames[0].Height;

            // Calculate total grid dimensions
            float totalGridWidth = columns * frameWidth + (columns - 1) * gap;
            float totalGridHeight = rows * frameHeight + (rows - 1) * gap;

            // Calculate starting positions to center the grid
            float startX = (Config.ScreenWidth - totalGridWidth) / 2;
            float startY = (Config.ScreenHeight - totalGridHeight) / 2;

            Vector2 startPos = new Vector2(startX, startY);

            // Initialize mole holes with the calculated starting position
            _moleManager.InitializeHoles(
                holeCount: holeCount,
                rows: rows,
                columns: columns,
                startPosition: startPos,
                gap: gap,
                spawnIntervalMin: levelData.SpawnIntervalMin,
                spawnIntervalMax: levelData.SpawnIntervalMax
            );

        }



        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update UI elements that depend on state
            _scoreLabel.Text = "Score: " + ScoreManager.CurrentScore;
            _levelLabel.Text = "Level: " + (_gameState.CurrentLevelIndex + 1);
            _timeLabel.Text = "Time: " + Math.Ceiling(_currentTimer).ToString();

            _backButton.Update();

            // Update the MoleManager
            var currentLevelData = _gameState.GetCurrentLevelData();
            _moleManager.Update(gameTime, currentLevelData.MouseStayTime);

            // Check for hits
            Vector2? clickPos = InputManager.GetClickPosition();
            if (clickPos.HasValue)
            {
                bool hit = _moleManager.CheckHit(clickPos.Value);
                if (hit)
                {
                    // Add points for a hit
                    ScoreManager.AddPoints(10);
                    SoundManager.PlaySound(SoundType.Hit);

                    Debug.WriteLine($"Hit registered at position {clickPos.Value}");
                }
                else
                {
                    SoundManager.PlaySound(SoundType.Click);
                }
            }

            // Decrement timer
            _currentTimer -= dt;
            if (_currentTimer <= 0)
            {
                // Time up, go to next level or end game
                AdvanceLevelOrEndGame();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Advances to the next level or ends the game if no more levels are available.
        /// </summary>
        private void AdvanceLevelOrEndGame()
        {
            bool moreLevels = _gameState.NextLevel();
            if (moreLevels)
            {
                // Move to next level
                InitializeLevel();
                _currentTimer = _levelTime;
                Debug.WriteLine($"Advancing to Level {_gameState.CurrentLevelIndex + 1}");
            }
            else
            {
                // No more levels, game over
                SceneManager.ChangeScene(new LeaderBoardScene(Game, GraphicsDevice));
            }
        }

        public void SaveScore()
        {
            ScoreManager.SaveHighScore();
            
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.DarkGreen);

            spriteBatch.Begin();

            // Draw background
            spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            // Draw UI
            _scoreLabel.Draw(spriteBatch);
            _levelLabel.Draw(spriteBatch);
            _timeLabel.Draw(spriteBatch);
            _backButton.Draw(spriteBatch);

            // Draw moles
            _moleManager.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
