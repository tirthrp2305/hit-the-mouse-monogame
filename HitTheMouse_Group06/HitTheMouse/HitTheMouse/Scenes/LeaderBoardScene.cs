using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HitTheMouse.UI;
using HitTheMouse.Core;
using HitTheMouse.Logic;
using System.Linq;
using System.Collections.Generic;

namespace HitTheMouse.Scenes
{
    public class LeaderBoardScene : BaseScene
    {
        private SpriteFont _font;

        private Texture2D _whiteTexture;

        public LeaderBoardScene(Game game, GraphicsDevice graphicsDevice) : base(game, graphicsDevice)
        {
        }

        public override void Load()
        {
            _font = Game.Content.Load<SpriteFont>("Fonts/main_font");

            // Create a 1x1 white texture for drawing colored rectangles
            _whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            _whiteTexture.SetData(new[] { Color.White });

            // Initialize back button
            int centerX = GraphicsDevice.Viewport.Width / 2;
            int buttonWidth = 250;  // Increased width for better visibility
            int buttonHeight = 60;  // Increased height for better visibility

            Rectangle buttonRect = new Rectangle(
                centerX - buttonWidth / 2,
                GraphicsDevice.Viewport.Height - 100,
                buttonWidth,
                buttonHeight
            );

            // Load high scores
            ScoreManager.LoadHighScoresAsync();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();

            // Draw Background
            spriteBatch.Draw(Game.Content.Load<Texture2D>("Backgrounds/background_leaderboard"), GraphicsDevice.Viewport.Bounds, Color.White);

            // Draw title with shadow
            string title = "Leaderboard";
            Vector2 titleSize = _font.MeasureString(title);
            Vector2 titlePos = new Vector2((GraphicsDevice.Viewport.Width - titleSize.X) / 2, 50);

            // Draw shadow
            spriteBatch.DrawString(_font, title, titlePos + new Vector2(2, 2), Color.Black);

            // Draw main title
            spriteBatch.DrawString(_font, title, titlePos, Color.White);

            // Retrieve and display high scores
            var scores = ScoreManager.GetHighScores().Take(10).ToList();

            if (scores != null && scores.Count > 0)
            {
                float startY = 150;
                float spacing = 60;

                for (int i = 0; i < scores.Count; i++)
                {
                    int score = scores[i];
                    string line = $"{i + 1}. {score}";

                    Vector2 lineSize = _font.MeasureString(line);
                    Vector2 linePos = new Vector2((GraphicsDevice.Viewport.Width - lineSize.X) / 2, startY + i * spacing);

                    // Draw semi-transparent black background behind the text
                    Rectangle backgroundRect = new Rectangle(
                        (int)(linePos.X - 10),
                        (int)(linePos.Y - 5),
                        (int)(lineSize.X + 20),
                        (int)(lineSize.Y + 10)
                    );
                    spriteBatch.Draw(_whiteTexture, backgroundRect, Color.Black * 0.5f); // 50% opacity

                    // Draw the score line
                    spriteBatch.DrawString(_font, line, linePos, Color.White);
                }
            }
            else
            {
                // Display a message if there are no high scores
                string noScores = "No high scores yet!";
                Vector2 noScoresSize = _font.MeasureString(noScores);
                Vector2 noScoresPos = new Vector2((GraphicsDevice.Viewport.Width - noScoresSize.X) / 2, 250);
                spriteBatch.DrawString(_font, noScores, noScoresPos, Color.White);
            }

            spriteBatch.End();
        }
    }
}
