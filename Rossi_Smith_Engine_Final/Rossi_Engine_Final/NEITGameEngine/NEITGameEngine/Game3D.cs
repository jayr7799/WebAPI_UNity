using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NEITGameEngine
{
    public class Game3D:Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        Model model;
        Matrix world = Matrix.Identity;
        Matrix view;
        Matrix projection;

        CubePrimative cube;
        public Game3D()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            model = Content.Load<Model>("Spider-Man-NoMaterial");
            cube = new CubePrimative(GraphicsDevice);
            //Camera setup
            view = Matrix.CreateLookAt(new Vector3(0, 7.5f, 5), Vector3.Zero, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 0.1f, 500.0f);
        }

        protected override void Update(GameTime gameTime)
        {
            float rotateCubeSpeed = 1.0f;
            cube.World *= Matrix.CreateRotationY(rotateCubeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach(ModelMesh mesh in model.Meshes)
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                    //effect.TextureEnabled = true;
                    //effect.Texture = Content.Load<Texture2D>("Spiderman_01_diff");
                }
                mesh.Draw();
            }
            cube.Draw(view, projection);
            base.Draw(gameTime);
        }
    }
}
