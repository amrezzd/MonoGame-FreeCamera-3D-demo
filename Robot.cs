using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ModelsAndVerts
{
    public class Robot
    {
        private Model _model;
        private float _angle;

        public void Initialize(ContentManager contentManager)
        {
            _model = contentManager.Load<Model>("robot");
        }

        public void Update(GameTime gameTime)
        {
            _angle += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(Camera camera)
        {
            // A model is composed of "Meshes" which are
            // parts of the model which can be positioned
            // independently, which can use different textures,
            // and which can have different rendering states
            // such as lighting applied.
            foreach (var mesh in _model.Meshes)
            {
                // "Effect" refers to a shader. Each mesh may
                // have multiple shaders applied to it for more
                // advanced visuals. 
                foreach (BasicEffect effect in mesh.Effects)
                {
                    // We could set up custom lights, but this
                    // is the quickest way to get somethign on screen:
                    effect.EnableDefaultLighting();
                    // This makes lighting look more realistic on
                    // round surfaces, but at a slight performance cost:
                    effect.PreferPerPixelLighting = true;

                    // The world matrix can be used to position, rotate
                    // or resize (scale) the model. Matrix.Identity means that
                    // the model is unrotated, drawn at the origin, and
                    // its size is unchanged from the loaded content file.
                    // We’ll be doing our calculations here...
                    effect.World = GetWorldMatrix();

                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;

                }

                // Now that we've assigned our properties on the effects we can
                // draw the entire mesh
                mesh.Draw();
            }

            Matrix GetWorldMatrix()
            {
                const float circleRadius = 8;
                const float heightOffGround = 3;

                // this matrix moves the model "out" from the origin
                Matrix translationMatrix = Matrix.CreateTranslation(
                    circleRadius, 0, heightOffGround);

                // this matrix rotates everything around the origin
                Matrix rotationMatrix = Matrix.CreateRotationZ(_angle);

                // We combine the two to have the model move in a circle:
                Matrix combined = translationMatrix * rotationMatrix;

                return combined;
            }
        }
    }
}
