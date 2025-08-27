using Microsoft.Xna.Framework;

namespace HitTheMouse.Core
{
    public static class Extensions
    {
        public static bool ContainsPoint(this Rectangle rect, Vector2 point)
        {
            return (point.X >= rect.X && point.X <= rect.X + rect.Width &&
                    point.Y >= rect.Y && point.Y <= rect.Y + rect.Height);
        }
    }
}
