using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Linq;

namespace HitTheMouse.Core
{
    public static class InputManager
    {
        private static MouseState _previousMouseState;
        private static MouseState _currentMouseState;

        private static TouchCollection _previousTouchState;
        private static TouchCollection _currentTouchState;

        public static void Update()
        {
            // Update mouse states
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            // Update touch states
            _previousTouchState = _currentTouchState;
            _currentTouchState = TouchPanel.GetState();
        }

        /// <summary>
        /// Returns the click/tap position in screen coordinates if there was a click this frame.
        /// If no click, returns null.
        /// </summary>
        public static Vector2? GetClickPosition()
        {
            // Check for mouse click
            if (_previousMouseState.LeftButton == ButtonState.Pressed &&
                _currentMouseState.LeftButton == ButtonState.Released)
            {
                return new Vector2(_currentMouseState.X, _currentMouseState.Y);
            }

            // Check for touch release (tap)
            foreach (var touch in _currentTouchState)
            {
                // Find corresponding previous touch
                var previousTouch = _previousTouchState.FirstOrDefault(t => t.Id == touch.Id);
                if (touch.State == TouchLocationState.Released &&
                    (previousTouch.State == TouchLocationState.Pressed ||
                     previousTouch.State == TouchLocationState.Moved))
                {
                    return touch.Position;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns current mouse or touch position (if any touch). If no input, returns null.
        /// This is more for hover or dragging scenarios.
        /// </summary>
        public static Vector2? GetCurrentPointerPosition()
        {
            if (_currentTouchState.Count > 0)
            {
                // Return the position of the first touch point
                return _currentTouchState[0].Position;
            }

            // Return mouse position
            return new Vector2(_currentMouseState.X, _currentMouseState.Y);
        }
    }
}
