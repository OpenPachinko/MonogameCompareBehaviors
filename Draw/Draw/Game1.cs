using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Draw
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Effect _singleEffect;
        private Effect _instanceEffect;

        private RenderTarget2D _singleRT;
        private RenderTarget2D _instanceRT;

        VertexPositionColor[] vertices;
        VertexBuffer vertexBuffer;

        short[] indeces;
        IndexBuffer indexBuffer;

        VertexDynamicInstance[] instances;
        DynamicVertexBuffer instanceBuffer;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            var gd = GraphicsDevice;
            _spriteBatch = new SpriteBatch(gd);

            // TODO: use this.Content to load your game content here

            var emContent = new EmbeddedResourceContentManager(this.Services);

            _singleEffect = emContent.Load<Effect>("DrawSingle");
            _instanceEffect = emContent.Load<Effect>("DrawInstance");

            // back buffer
            int w = gd.PresentationParameters.BackBufferWidth;
            int h = gd.PresentationParameters.BackBufferHeight;

            _singleRT   = new RenderTarget2D(gd, w, h, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
            _instanceRT = new RenderTarget2D(gd, w, h, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);

            // vertex buffer
            vertices = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(0, 0, 0.1f), Color.Red),
                new VertexPositionColor(new Vector3(0, 0.1f, 0), Color.Blue),
                new VertexPositionColor(new Vector3(0.1f, 0, 0), Color.Green),
            };

            vertexBuffer = new VertexBuffer(gd, VertexPositionColor.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);

            // index buffer
            indeces = new short[] { 0, 1, 2 };
            indexBuffer = new IndexBuffer(gd, IndexElementSize.SixteenBits, indeces.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indeces);

            // instance buffer
            var rand = new System.Random();
            instances = new VertexDynamicInstance[50];
            for (int i = 0; i < instances.Length; i++)
            {
                instances[i].World = Matrix.CreateTranslation(
                    (float)rand.NextDouble() - 0.5f,
                    (float)rand.NextDouble() - 0.5f,
                    0);
            }

            instanceBuffer = new DynamicVertexBuffer(gd, VertexDynamicInstance.VertexDeclaration, instances.Length, BufferUsage.WriteOnly);
            instanceBuffer.SetData(instances);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var gd = GraphicsDevice;
            var view = Matrix.CreateLookAt(new Vector3(0.0f, 1.0f, 1.0f), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, gd.Viewport.AspectRatio, 1.0f, 100.0f);

            // (1) Calling order 
            // DesktopGL: no problem
            // Android  : problem
            //DrawInstance(gd, view, proj);
            //DrawSingle(gd, view, proj);

            // (2) Calling order
            // DesktopGL: problem
            // Android  : problem
            //DrawSingle(gd, view, proj);
            //DrawInstance(gd, view, proj);

            // (3) sinle only
            // DesktopGL: no problem
            // Android  : no problem
            //DrawSingle(gd, view, proj);

            // (4) instance only
            // DesktopGL: no problem
            // Android  : problem
            DrawInstance(gd, view, proj);

            // change render target
            //gd.SetRenderTarget(null);
            //gd.Clear(Color.Blue);

            //int w = gd.PresentationParameters.BackBufferWidth;
            //int h = gd.PresentationParameters.BackBufferHeight;
            //_spriteBatch.Begin();
            //_spriteBatch.Draw(_singleRT, new Rectangle(0, 0, w / 2, h / 2), Color.White);
            //_spriteBatch.Draw(_instanceRT, new Rectangle(w / 2, h / 2, w / 2, h / 2), Color.White);
            //_spriteBatch.End();
        }

        void DrawSingle(GraphicsDevice gd, Matrix view, Matrix proj)
        {
            // change render target
            gd.SetRenderTarget(_singleRT);

            // clear
            gd.Clear(Color.Red);

            // draw
            for (int i = 0; i < instances.Length; i++)
            {
                _singleEffect.Parameters["World"].SetValue(instances[i].World);
                _singleEffect.Parameters["View"].SetValue(view);
                _singleEffect.Parameters["Projection"].SetValue(proj);

                gd.SetVertexBuffers(new VertexBufferBinding(vertexBuffer, 0, 0));
                gd.Indices = indexBuffer;

                foreach (EffectPass pass in _singleEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    gd.DrawIndexedPrimitives(
                        primitiveType: PrimitiveType.TriangleList,
                        baseVertex: 0,
                        startIndex: 0,
                        primitiveCount: vertices.Length / 3);
                }
            }
            
        }

        void DrawInstance(GraphicsDevice gd, Matrix view, Matrix proj)
        {
            // change render target
            gd.SetRenderTarget(null);

            // clear
            gd.Clear(Color.LightPink);

            // draw
            _instanceEffect.Parameters["View"].SetValue(view);
            _instanceEffect.Parameters["Projection"].SetValue(proj);

            gd.SetVertexBuffers(
                new VertexBufferBinding(vertexBuffer, 0, 0),
                new VertexBufferBinding(instanceBuffer, 0, 1));
            gd.Indices = indexBuffer;

            foreach (EffectPass pass in _instanceEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                gd.DrawInstancedPrimitives(
                    primitiveType: PrimitiveType.TriangleList,
                    baseVertex: 0,
                    startIndex: 0,
                    primitiveCount: vertices.Length / 3,
                    instanceCount: instances.Length);
            }
        }
    }
}
