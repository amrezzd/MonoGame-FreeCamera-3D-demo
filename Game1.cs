using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ModelsAndVerts
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private VertexPositionTexture[] _floorVerts;
        private BasicEffect _effect;
        private Texture2D _checkerboardTexture;
        private Robot _robot;
        private Camera _camera;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _floorVerts = new VertexPositionTexture[6];
            _floorVerts[0].Position = new Vector3(-20, -20, 0);
            _floorVerts[1].Position = new Vector3(-20, 20, 0);
            _floorVerts[2].Position = new Vector3(20, -20, 0);
            _floorVerts[3].Position = _floorVerts[1].Position;
            _floorVerts[4].Position = new Vector3(20, 20, 0);
            _floorVerts[5].Position = _floorVerts[2].Position;

            _floorVerts[0].TextureCoordinate = new Vector2(0, 0);
            _floorVerts[1].TextureCoordinate = new Vector2(0, 20);
            _floorVerts[2].TextureCoordinate = new Vector2(20, 0);
            _floorVerts[3].TextureCoordinate = _floorVerts[1].TextureCoordinate;
            _floorVerts[4].TextureCoordinate = new Vector2(20, 20);
            _floorVerts[5].TextureCoordinate = _floorVerts[2].TextureCoordinate;

            _effect = new BasicEffect(_graphics.GraphicsDevice);

            _robot = new Robot();
            _robot.Initialize(Content);

            _camera = new Camera(_graphics.GraphicsDevice, new Vector3(0, 20, 10));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            using (var stream = TitleContainer.OpenStream("Content/checkerboard.png"))
            {
                _checkerboardTexture = Texture2D.FromStream(this.GraphicsDevice, stream);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _robot.Update(gameTime);
            _camera.Update(gameTime);

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            DrawGround();

            _robot.Draw(_camera);

            base.Draw(gameTime);
        }
        
        void DrawGround()
        {
            _effect.View = _camera.ViewMatrix;

            _effect.Projection = _camera.ProjectionMatrix;

            _effect.TextureEnabled = true;
            _effect.Texture = _checkerboardTexture;

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                _graphics.GraphicsDevice.DrawUserPrimitives(
                    // We’ll be rendering two trinalges
                    PrimitiveType.TriangleList,
                    // The array of verts that we want to render
                    _floorVerts,
                    // The offset, which is 0 since we want to start 
                    // at the beginning of the floorVerts array
                    0,
                    // The number of triangles to draw
                    2);
            }
        }
    }
}
