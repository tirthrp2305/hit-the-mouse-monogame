using Microsoft.Xna.Framework;

namespace HitTheMouse.Core
{
    public static class Config
    {
        // Screen Dimensions
        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        // Timing and animation configuration
        public static float FrameDuration = 0.1f; // How long each frame lasts in seconds

        // Level configuration data
        public struct LevelData
        {
            public int HoleCount;
            public int Rows;
            public int Columns;
            public float MouseStayTime; // Time mouse stays fully visible (in seconds)
            public float SpawnIntervalMin; // Minimum spawn interval (seconds)
            public float SpawnIntervalMax; // Maximum spawn interval (seconds)
        }

        public static LevelData[] Levels = new[]
        {
            new LevelData
            {
                HoleCount = 3,
                Rows = 1,
                Columns = 3,
                MouseStayTime = 0.4f,
                SpawnIntervalMin = 2.5f,
                SpawnIntervalMax = 4.5f
            },
            new LevelData
            {
                HoleCount = 4,
                Rows = 2,
                Columns = 2,
                MouseStayTime = 0.4f,
                SpawnIntervalMin = 2.0f,
                SpawnIntervalMax = 4.5f
            },
            new LevelData
            {
                HoleCount = 6,
                Rows = 2,
                Columns = 3,
                MouseStayTime = 0.3f,
                SpawnIntervalMin = 2.0f,
                SpawnIntervalMax = 4.4f
            },
            new LevelData
            {
                HoleCount = 8,
                Rows = 2,
                Columns = 4,
                MouseStayTime = 0.2f,
                SpawnIntervalMin = 3.8f,
                SpawnIntervalMax = 4.0f
            }
        };
    }
}
