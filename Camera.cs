using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ModelsAndVerts
{
    public class Camera
    {
        // using z as up vector here
        private readonly Vector3 forwardVector = new Vector3(0, -1, 0);
        private readonly Vector3 rightVector = new Vector3(-1, 0, 0);
        private readonly Vector3 upVector = new Vector3(0, 0, 1);

        private float _xAngle = 0;
        private float _zAngle = 0;
        private MouseState _mouseState;
        private GraphicsDevice _graphicsDevice;

        public Camera(GraphicsDevice graphicsDevice, Vector3 position)
        {
            _graphicsDevice = graphicsDevice;
            Position = position;
            _mouseState = Mouse.GetState();
        }


        // Field of view measures how wide of a view our camera has.
        // Increasing this value means it has a wider view, making everything
        // on screen smaller. This is conceptually the same as "zooming out".
        public float FieldOfView { get; set; } = MathHelper.PiOver4;
        // Anything closer than this will not be drawn (will be clipped)
        public float NearClippingPlane { get; set; } = 1;
        // Anything further than this will not be drawn (will be clipped)
        public float FarClippingPlane { get; set; } = 1000;
        // Let's start at X = 0 so we're looking at things head-on
        public Vector3 Position { get; set; }
        public float MoveSpeed { get; set; } = 10f;
        public float RotateSpeed { get; set; } = 3;

        // camera position and orientation
        public Matrix ViewMatrix
        {
            get
            {
                var lookAtVector = new Vector3(0, -1, -0.5f);
                // We'll create a rotation matrix using our angle
                var xRotationMatrix = Matrix.CreateRotationX(_xAngle);
                var zRotationMatrix = Matrix.CreateRotationZ(_zAngle);

                // Then we'll modify the vector using this matrix:
                lookAtVector = Vector3.Transform(lookAtVector, xRotationMatrix * zRotationMatrix);
                lookAtVector += Position;

                return Matrix.CreateLookAt(Position, lookAtVector, Up);
            }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                // We want the aspect ratio of our display to match
                // the entire screen's aspect ratio:
                float aspectRatio = _graphicsDevice.Viewport.Width / _graphicsDevice.Viewport.Height;
                return Matrix.CreatePerspectiveFieldOfView(FieldOfView, aspectRatio, NearClippingPlane, FarClippingPlane);
            }
        }

        public Vector3 Forward
        {
            get
            {
                Matrix xRotationMatrix = Matrix.CreateRotationX(_xAngle);
                Matrix zRotationMatrix = Matrix.CreateRotationZ(_zAngle);
                return Vector3.Transform(forwardVector, xRotationMatrix * zRotationMatrix);
            }
        }

        public Vector3 Right
        {
            get
            {
                Matrix zRotationMatrix = Matrix.CreateRotationZ(_zAngle);
                return Vector3.Transform(rightVector, zRotationMatrix);
            }
        }

        public Vector3 Up
        {
            get
            {
                Matrix xRotationMatrix = Matrix.CreateRotationX(_xAngle);
                Matrix zRotationMatrix = Matrix.CreateRotationZ(_zAngle);
                return Vector3.Transform(upVector, xRotationMatrix * zRotationMatrix);
            }
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                MoveInFreeCameraMode(gameTime, mouseState);
            }
            _mouseState = mouseState;
        }

        private void MoveInFreeCameraMode(GameTime gameTime, MouseState mouseCurrentState)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            float moveSpeed = keyboardState.IsKeyDown(Keys.LeftShift) ? MoveSpeed * 3 : MoveSpeed;
            
            if (keyboardState.IsKeyDown(Keys.Q))
            {
                Position -= upVector * moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (keyboardState.IsKeyDown(Keys.E))
            {
                Position += upVector * moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                Position -= Right * moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            else if (keyboardState.IsKeyDown(Keys.D))
            {
                Position += Right * moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (keyboardState.IsKeyDown(Keys.W))
            {
                Position += Forward * moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                Position -= Forward * moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }


            float horizontalMovement = mouseCurrentState.X - _mouseState.X;
            if (horizontalMovement != 0)
            {
                _zAngle -= Math.Sign(horizontalMovement) * RotateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds; // MathHelper.ToRadians(-_rotateSpeed *horizontalMovement * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            float verticalMovement = mouseCurrentState.Y - _mouseState.Y;
            if (verticalMovement != 0)
            {
                _xAngle += Math.Sign(verticalMovement) * RotateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds; //MathHelper.ToRadians(verticalMovement * _rotateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

    }
}
