using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using Color = Microsoft.Xna.Framework.Color;

namespace TheGame
{
    public class Game1 : Game
    {
        //DON'T TOUCH IT MORTALS
        int WindowWidth = 1920;
        int WindowHeight = 1080;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;
        Camera camera;
        EffectHandler effectHandler;
        Serializator serializator;
        //.................

        private BasicEffect basicEffect;
        World world;
        HUD hud;
        InteractionEventHandler interactionEventHandler;
        int i = 0;
        Player player;
        Enemies enemies;

        EffectHandler effectPrzemyslaw;
        EffectHandler effectWiktor;

        public Game1()
        {
            enemies = new Enemies();
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = WindowWidth;
            _graphics.PreferredBackBufferHeight = WindowHeight;
            _graphics.ApplyChanges();
            
        }
        

        protected override void Initialize()
        {
            
            //DON'T TOUCH IT MORTALS
            camera = new Camera();
            
            projectionMatrix = Matrix.CreateOrthographicOffCenter(-(WindowWidth / 100), (WindowWidth / 100), -(WindowHeight / 50), (WindowHeight / 100), 1f, 100f);      // orthographic view 
            //projectionMatrix = Matrix.CreateOrthographic(20, 20, 1f, 1000f);                      // second type orthographic view

                // PERSPECTIVE point of view
                //projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f),GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f); // render range (from 1 near playing to 1000 far playing)
                // ................................................................................................................

                 // tells the world of our orientantion
                                                                                      // (the same as:
                                                                                      // Vector3(0,1,0) - up and down is along y axis)
                worldMatrix = Matrix.CreateWorld(camera.CamTarget, Vector3.Forward, Vector3.Up);
            //.................


            effectHandler = new EffectHandler(Content.Load<Effect>("ShaderOne"));
            effectPrzemyslaw = new EffectHandler(Content.Load<Effect>("Przemyslaw"));
            effectWiktor = new EffectHandler(Content.Load<Effect>("Wiktor"));


            hud = new HUD("forest2", WindowWidth, WindowHeight);
            world = new World(Content);
            //enemies.AddEnemies(world.returnEnemies());
            player = new Player(new Vector3(30,0,30), "mis4", "StarSparrow_Orange");

            serializator = new Serializator("zapis.txt");
            interactionEventHandler = new InteractionEventHandler(player, enemies.EnemiesList);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //world.ObjectInitializer(Content);
            hud.LoadContent(Content);
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Projection = projectionMatrix;
            player.LoadContent(Content);
            enemies.LoadModels(Content);

            //player.OnDestroy += DestroyControl;
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Debug.Write(enemies.EnemiesList.Count);
            Debug.Write("\n");
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            camera.CamPosition = player.GetPosition() + camera.CamPositionState;
            camera.nextpos = player.GetPosition();
            //viewMatrix = Matrix.CreateLookAt(camera.CamPosition, camera.camTracker , Vector3.Up);
            viewMatrix = Matrix.CreateLookAt(camera.CamPosition, player.GetPosition(), Vector3.Up);
            basicEffect.View = Matrix.CreateLookAt(camera.CamPosition, camera.camTracker, Vector3.Up);
            player.Update(world, delta);
            enemies.AddEnemies(world.returnEnemiesList(player.position.X));

            enemies.Move(delta, player);
            camera.Update1(player.position);
            hud.Update(camera.CamPosition);
            SaveControl();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            base.Draw(gameTime);

            hud.DrawBackground(_spriteBatch);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            world.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix, player.position.X);

            enemies.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix, Content);
            player.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix, player.color); 


            
            //player.PrzemyslawDraw(effectPrzemyslaw, worldMatrix, viewMatrix, projectionMatrix, player.color);
            
            hud.DrawFrontground(_spriteBatch, player.Health);

            DrawBoundingBoxes();
        }

        #region DrawingBB
        public void DrawBoundingBoxes()
        {
            basicEffect.CurrentTechnique.Passes[0].Apply();
            
            foreach(SceneObject obj in world.GetWorldList())
            {
                //DrawBB(obj.boundingBox.GetCorners());
            }
            foreach (Enemy enemy in enemies.EnemiesList)
            {
                DrawBB(enemy.boundingBox.GetCorners());
                DrawBS(enemy.boundingSphere.Center, enemy.boundingSphere.Radius);
            }
            //DrawBB(player.boundingBox.GetCorners());
            DrawBS(player.boundingSphere.Center, player.boundingSphere.Radius);
        }

        public void DrawBB(Vector3[] corners)
        {
            var bottom = new[] 
            { 
                new VertexPositionColor(corners[2], Color.White),
                new VertexPositionColor(corners[3], Color.White),
                new VertexPositionColor(corners[7], Color.White),
                new VertexPositionColor(corners[6], Color.White),
                new VertexPositionColor(corners[2], Color.White),
            };

            var top = new[]
            {
                new VertexPositionColor(corners[4], Color.White),
                new VertexPositionColor(corners[5], Color.White),
                new VertexPositionColor(corners[1], Color.White),
                new VertexPositionColor(corners[0], Color.White),
                new VertexPositionColor(corners[4], Color.White),
            };

            var rf = new[]
            {
                new VertexPositionColor(corners[1], Color.White),
                new VertexPositionColor(corners[2], Color.White)
            };
            var lf = new[]
            {
                new VertexPositionColor(corners[3], Color.White),
                new VertexPositionColor(corners[0], Color.White)
            };
            var rb = new[]
            {
                new VertexPositionColor(corners[5], Color.White),
                new VertexPositionColor(corners[6], Color.White)
            };
            var lb = new[]
            {
                new VertexPositionColor(corners[4], Color.White),
                new VertexPositionColor(corners[7], Color.White)
            };

            //Sciany dolna i górna
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, bottom, 0, 4);
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, top, 0,4);
            //boki
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, rf, 0, 1);
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, lf, 0, 1);
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, rb, 0, 1);
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, lb, 0, 1);
        }

        public void DrawBS(Vector3 center, float radius)
        {
            var X = new[]
            {
                new VertexPositionColor(center+new Vector3(radius,0,0), Color.White),
                new VertexPositionColor(center+new Vector3(-radius,0,0), Color.White),

            };

            var Y = new[]
            {
                new VertexPositionColor(center+new Vector3(0,radius,0), Color.White),
                new VertexPositionColor(center+new Vector3(0,-radius,0), Color.White),

            };

            var Z = new[]
            {
                new VertexPositionColor(center+new Vector3(0,0,radius), Color.White),
                new VertexPositionColor(center+new Vector3(0,0,-radius), Color.White),

            };

            var XTOY = new[]
            {
                
                new VertexPositionColor(center+new Vector3(0,radius,0), Color.White),
                new VertexPositionColor(center+new Vector3(-radius,0,0), Color.White),
                new VertexPositionColor(center+new Vector3(0,-radius,0), Color.White),
                new VertexPositionColor(center+new Vector3(radius,0,0), Color.White),
                new VertexPositionColor(center+new Vector3(0,radius,0), Color.White),
            };

            var ZTOY = new[]
            {

                new VertexPositionColor(center+new Vector3(0,radius,0), Color.White),
                new VertexPositionColor(center+new Vector3(0,0,-radius), Color.White),
                new VertexPositionColor(center+new Vector3(0,-radius,0), Color.White),
                new VertexPositionColor(center+new Vector3(0,0,radius), Color.White),
                new VertexPositionColor(center+new Vector3(0,radius,0), Color.White),
            };


            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, X, 0, 1);
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, Y, 0, 1);
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, Z, 0, 1);
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, XTOY, 0, 4);
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, ZTOY, 0, 4);
        }
        #endregion

        #region Controls
        void SaveControl()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.P))
            {
                serializator.SavePlayer(player);
            }
            if (state.IsKeyDown(Keys.O))
            {
                Player copied;
                copied = serializator.LoadPlayer();
                if (copied != null)
                {
                    player = copied;
                    player.LoadContent(Content);
                }

            }
        }

        void DestroyControl(object obj, EventArgs e)
        {
            if(obj is Player)
            {
                Exit();
                // GAME OVER HERE;
            }

        }
        #endregion

    }

}