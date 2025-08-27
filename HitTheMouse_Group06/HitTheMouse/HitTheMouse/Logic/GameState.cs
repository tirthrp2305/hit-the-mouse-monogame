using HitTheMouse.Core; // For Config or LevelsProvider

namespace HitTheMouse.Logic
{
    public class GameState
    {
        public int CurrentLevelIndex { get; private set; } = 0;
        public bool GameOver { get; private set; } = false;

        public void StartGame()
        {
            CurrentLevelIndex = 0;
            GameOver = false;
            ScoreManager.ResetScore();
        }

        public Config.LevelData GetCurrentLevelData()
        {
            if (CurrentLevelIndex < Config.Levels.Length)
                return Config.Levels[CurrentLevelIndex];
            // If out of range, return the last level or handle gracefully
            return Config.Levels[Config.Levels.Length - 1];
        }

        /// <summary>
        /// Advances to the next level. Returns true if there are more levels, false otherwise.
        /// </summary>
        public bool NextLevel()
        {
            CurrentLevelIndex++;
            if (CurrentLevelIndex >= Config.Levels.Length)
            {
                GameOver = true;
                ScoreManager.AddPoints(CurrentLevelIndex);
                return false;
            }
            return true;
        }
    }
}
