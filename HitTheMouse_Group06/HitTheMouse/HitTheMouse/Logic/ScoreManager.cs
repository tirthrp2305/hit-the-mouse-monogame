using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace HitTheMouse.Logic
{
    public static class ScoreManager
    {
        public static int CurrentScore { get; private set; } = 0;
        private static List<int> _highScores = new List<int>();
        private static readonly string _fileName = "highscores.json";

        /// <summary>
        /// Adds the current score to the high scores list and saves it.
        /// </summary>
        public static void SaveHighScore()
        {
            _highScores.Add(CurrentScore);
            _highScores.Sort((a, b) => b.CompareTo(a)); // Sort descending

            // Limit to top 10 scores
            if (_highScores.Count > 10)
                _highScores.RemoveAt(_highScores.Count - 1);

            SaveHighScoresAsync();
        }

        /// <summary>
        /// Resets the current score to zero.
        /// </summary>
        public static void ResetScore()
        {
            CurrentScore = 0;
        }

        /// <summary>
        /// Increments the current score by the specified points.
        /// </summary>
        /// <param name="points">Points to add.</param>
        public static void AddPoints(int points)
        {
            CurrentScore += points;
        }

        /// <summary>
        /// Retrieves the top high scores.
        /// </summary>
        /// <returns>A read-only list of high scores.</returns>
        public static IReadOnlyList<int> GetHighScores()
        {
            return _highScores.AsReadOnly();
        }

        /// <summary>
        /// Loads high scores from a file asynchronously.
        /// </summary>
        public static async void LoadHighScoresAsync()
        {
            try
            {
                string path = GetFilePath(_fileName);
                if (File.Exists(path))
                {
                    string json = await File.ReadAllTextAsync(path);
                    _highScores = JsonSerializer.Deserialize<List<int>>(json);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log them)
                Debug.WriteLine($"Error loading high scores: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves high scores to a file asynchronously.
        /// </summary>
        private static async void SaveHighScoresAsync()
        {
            try
            {
                string path = GetFilePath(_fileName);
                string json = JsonSerializer.Serialize(_highScores, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(path, json);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log them)
                Debug.WriteLine($"Error saving high scores: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves the appropriate file path based on the platform.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Full path to the file.</returns>
        private static string GetFilePath(string fileName)
        {
            string folderPath;

#if ANDROID
            // For Android, use the internal storage directory
            folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#else
            // For Desktop, use the local application data directory
            folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
#endif

            // Ensure the directory exists
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            return Path.Combine(folderPath, fileName);
        }
    }
}
