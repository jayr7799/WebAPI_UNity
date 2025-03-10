using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace NEITGameEngine
{
    public class CubePrimative
    {
        VertexPositionColor[] vertices;
        int[] indicies;
        BasicEffect effect;
        GraphicsDevice _graphics;

        public Matrix World { get; set; } = Matrix.Identity;

        public CubePrimative(GraphicsDevice graphicsDevice)
        {
            this._graphics = graphicsDevice;
            effect = new BasicEffect(graphicsDevice)
            { 
                VertexColorEnabled = true
            };
            InitializeCube();

        }

        private void InitializeCube()
        {
            vertices = new VertexPositionColor[]
            {
                //front face
                new VertexPositionColor(new Vector3(-1,-1,-1), Color.Red),
                new VertexPositionColor(new Vector3(-1,1,-1), Color.Green),
                new VertexPositionColor(new Vector3(1,1,-1), Color.Blue),
                new VertexPositionColor(new Vector3(1,-1,-1), Color.Yellow),

                //back face
                new VertexPositionColor(new Vector3(-1,-1,1), Color.Red),
                new VertexPositionColor(new Vector3(-1,1,1), Color.Green),
                new VertexPositionColor(new Vector3(1,1,1), Color.Blue),
                new VertexPositionColor(new Vector3(1,-1,1), Color.Yellow),
            };
            indicies = new int[]
            {
                0,1,2,0,2,3, //front face
                4,6,5,4,7,6, //back face
                0,5,1,0,4,5, //left face
                3,2,7,2,6,7, //right face
                1,5,6,1,6,2, //top face
                0,3,4,3,7,4  //bottom face
            };
        }
        public void Draw(Matrix view, Matrix projection)
        {
            effect.World = World;
            effect.View = view;
            effect.Projection = projection;
            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphics.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indicies, 0, indicies.Length/3);
            }
        }
    }

}
