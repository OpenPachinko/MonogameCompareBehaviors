using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MRT
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Effect _clearGBufferEffect;

        private RenderTarget2D _colorRT;
        private RenderTarget2D _normalRT;
        private RenderTarget2D _depthRT;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;

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

            _clearGBufferEffect = emContent.Load<Effect>("ClearGBuffer");

            // back buffer
            int w = gd.PresentationParameters.BackBufferWidth;
            int h = gd.PresentationParameters.BackBufferHeight;

            _colorRT  = new RenderTarget2D(gd, w, h, false, SurfaceFormat.Color , DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
            _normalRT = new RenderTarget2D(gd, w, h, false, SurfaceFormat.Color , DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
            _depthRT  = new RenderTarget2D(gd, w, h, false, SurfaceFormat.Single, DepthFormat.None   , 0, RenderTargetUsage.PreserveContents);

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

            // clear G-Buffer
            gd.SetRenderTargets(_colorRT, _normalRT, _depthRT);
            foreach (var pass in _clearGBufferEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                DrawQuad(gd, new Vector2(-1, -1), new Vector2(+1, +1));
            }

            // change render target
            gd.SetRenderTarget(null);
            gd.Clear(Color.White);

            int w = gd.PresentationParameters.BackBufferWidth / 2;
            int h = gd.PresentationParameters.BackBufferHeight / 2;
            _spriteBatch.Begin();
            _spriteBatch.Draw(_colorRT , new Rectangle(0, 0, w, h), Color.White);
            _spriteBatch.Draw(_normalRT, new Rectangle(0, h, w, h), Color.White);
            _spriteBatch.Draw(_depthRT , new Rectangle(w, 0, w, h), Color.White);
            _spriteBatch.End();
        }

        static readonly VertexPositionTexture[] QuadVertexes = new VertexPositionTexture[]
        {
            new VertexPositionTexture(new Vector3(0,0,0), new Vector2(1,1)),
            new VertexPositionTexture(new Vector3(0,0,0), new Vector2(0,1)),
            new VertexPositionTexture(new Vector3(0,0,0), new Vector2(0,0)),
            new VertexPositionTexture(new Vector3(0,0,0), new Vector2(0,0)),
            new VertexPositionTexture(new Vector3(0,0,0), new Vector2(1,0)),
            new VertexPositionTexture(new Vector3(0,0,0), new Vector2(1,1)),
        };

        public static void DrawQuad(GraphicsDevice graphicsDevice, Vector2 v1, Vector2 v2)
        {
            QuadVertexes[0].Position.X = v2.X;
            QuadVertexes[0].Position.Y = v1.Y;

            QuadVertexes[1].Position.X = v1.X;
            QuadVertexes[1].Position.Y = v1.Y;

            QuadVertexes[2].Position.X = v1.X;
            QuadVertexes[2].Position.Y = v2.Y;

            QuadVertexes[3].Position.X = v1.X;
            QuadVertexes[3].Position.Y = v2.Y;

            QuadVertexes[4].Position.X = v2.X;
            QuadVertexes[4].Position.Y = v2.Y;

            QuadVertexes[5].Position.X = v2.X;
            QuadVertexes[5].Position.Y = v1.Y;

            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, QuadVertexes, 0, 2);
        }
    }
}
