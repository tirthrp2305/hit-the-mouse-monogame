using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace HitTheMouse.Entities
{
    public class MoleHole
    {
        private Texture2D _spriteSheet;
        private Rectangle[] _frames;
        private MoleState _currentState;
        private float _stateTimer;
        private Vector2 _position;
        private float _stayTime;

        private float _frameDuration; // Duration of each animation frame (seconds)

        // Use a shared static Random instance
        private static readonly Random _rand = new Random();

        // Spawn interval ranges (in seconds)
        private float _spawnIntervalMin;
        private float _spawnIntervalMax;
        private float _nextSpawnTime;

        // Bounding box for hit detection
        public Rectangle HitBox { get; private set; }

        public bool IsActive => _currentState != MoleState.Ideal;

        public MoleHole(Texture2D spriteSheet, Vector2 position, Rectangle[] frames, float frameDuration, float spawnIntervalMin, float spawnIntervalMax)
        {
            _spriteSheet = spriteSheet;
            _position = position;
            _frames = frames; // Expected to have 6 frames: 0..5
            _frameDuration = frameDuration;
            _currentState = MoleState.Ideal;
            _stateTimer = 0f;

            HitBox = new Rectangle((int)position.X, (int)position.Y, _frames[0].Width, _frames[0].Height);

            // Initialize spawn intervals
            _spawnIntervalMin = spawnIntervalMin;
            _spawnIntervalMax = spawnIntervalMax;
            _nextSpawnTime = GetRandomSpawnTime();
        }

        public void Update(GameTime gameTime, float stayTime)
        {
            _stayTime = stayTime; // Assign the stayTime
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _stateTimer += dt;

            switch (_currentState)
            {
                case MoleState.Ideal:
                    if (_stateTimer >= _nextSpawnTime)
                    {
                        SpawnMouse();
                    }
                    break;

                case MoleState.GettingOut1:
                    if (_stateTimer >= _frameDuration)
                    {
                        TransitionToState(MoleState.GettingOut2);
                    }
                    break;

                case MoleState.GettingOut2:
                    if (_stateTimer >= _frameDuration)
                    {
                        TransitionToState(MoleState.FullyOut);
                        Debug.WriteLine($"Mole at {_position} is fully out.");
                    }
                    break;

                case MoleState.FullyOut:
                    if (_stateTimer >= _stayTime)
                    {
                        TransitionToState(MoleState.GettingIn1);
                        Debug.WriteLine($"Mole at {_position} starts retreating.");
                    }
                    break;

                case MoleState.GettingIn1:
                    if (_stateTimer >= _frameDuration)
                    {
                        TransitionToState(MoleState.GettingIn2);
                    }
                    break;

                case MoleState.GettingIn2:
                    if (_stateTimer >= _frameDuration)
                    {
                        TransitionToState(MoleState.Ideal);
                        // After retreating, set a new spawn interval
                        _nextSpawnTime = GetRandomSpawnTime();
                        Debug.WriteLine($"Mole at {_position} has retreated to ideal state.");
                    }
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRect = GetSourceRectangle();

            spriteBatch.Draw(_spriteSheet, _position, sourceRect, Color.White);
        }

        /// <summary>
        /// Checks if the mole was hit based on the click position.
        /// If hit, transitions the mole back to the Ideal state immediately.
        /// </summary>
        public bool CheckHit(Vector2 clickPosition)
        {
            if (!IsActive)
                return false;

            if (HitBox.Contains(clickPosition.ToPoint()) && _currentState == MoleState.FullyOut)
            {
                TransitionToState(MoleState.Ideal);
                // After being hit, set a new spawn interval
                _nextSpawnTime = GetRandomSpawnTime();
                Debug.WriteLine($"Mole at {_position} was hit and transitioned to Ideal state.");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Transitions the mole to a new state and resets the state timer.
        /// </summary>
        private void TransitionToState(MoleState newState)
        {
            _currentState = newState;
            _stateTimer = 0f;
            Debug.WriteLine($"Mole at {_position} transitioned to {_currentState}.");
        }

        /// <summary>
        /// Spawns the mole by transitioning it to the GettingOut1 state.
        /// </summary>
        private void SpawnMouse()
        {
            TransitionToState(MoleState.GettingOut1);
            Debug.WriteLine($"Mole at {_position} is spawning.");
        }

        /// <summary>
        /// Generates a random spawn time within the specified interval.
        /// </summary>
        private float GetRandomSpawnTime()
        {
            return (float)(_rand.NextDouble() * (_spawnIntervalMax - _spawnIntervalMin) + _spawnIntervalMin);
        }

        /// <summary>
        /// Determines the source rectangle based on the current state.
        /// </summary>
        private Rectangle GetSourceRectangle()
        {
            switch (_currentState)
            {
                case MoleState.Ideal:
                    return _frames[5]; // Empty hole
                case MoleState.GettingOut1:
                    return _frames[0]; // Starting to pop out
                case MoleState.GettingOut2:
                    return _frames[1]; // Continuing to pop out
                case MoleState.FullyOut:
                    return _frames[2]; // Fully visible
                case MoleState.GettingIn1:
                    return _frames[3]; // Starting to retreat
                case MoleState.GettingIn2:
                    return _frames[4]; // Continuing to retreat
                default:
                    return _frames[5];
            }
        }
    }
}
