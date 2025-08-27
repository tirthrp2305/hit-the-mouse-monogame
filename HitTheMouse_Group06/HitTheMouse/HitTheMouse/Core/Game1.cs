using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using HitTheMouse.Scenes;

namespace HitTheMouse.Core
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set preferred backbuffer (adjust to your chosen resolution)
            _graphics.PreferredBackBufferWidth = Config.ScreenWidth;
            _graphics.PreferredBackBufferHeight = Config.ScreenHeight;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // Enable touch input if on a platform that supports it
            TouchPanel.EnabledGestures = GestureType.Tap;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            SoundManager.LoadContent(Content);

            // Start at the Menu Scene
            SceneManager.ChangeScene(new MenuScene(this, GraphicsDevice));
        }

        protected override void UnloadContent()
        {
            // Unload non-ContentManager content if any
        }

        protected override void Update(GameTime gameTime)
        {
            // Handle input events
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (SceneManager.CurrentScene is not MenuScene)
                {
                    SceneManager.ChangeScene(new MenuScene(this, GraphicsDevice));
                }
            }

            // Update input state before updating the current scene
            InputManager.Update();

            SceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            SceneManager.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
