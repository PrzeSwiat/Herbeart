using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
        
        Player player;
        List<Enemy> enemies;

        EffectHandler effectPrzemyslaw;

        public Game1()
        {
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
            Levels levels = new Levels("../../../map1.txt");
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
            enemies = new List<Enemy>();


            effectHandler = new EffectHandler(Content.Load<Effect>("ShaderOne"));
            effectPrzemyslaw = new EffectHandler(Content.Load<Effect>("Przemyslaw"));


            hud = new HUD("forest2", WindowWidth, WindowHeight);
            //world = new World(WindowWidth,WindowHeight,Content,2f,3,3,"test", "StarSparrow_Green");
            Tuple<List<string>, List<string>, List<float>> models_textures = levels.DrawScene();
            List<string> models = models_textures.Item1;
            List<string> textures = models_textures.Item2;
            List<float> level = models_textures.Item3;
            world = new World(WindowWidth, WindowHeight, Content, 3f, 20, 20, models, textures, level);

            player = new Player(new Vector3(5,0,5), "mis4", "StarSparrow_Orange");
            Enemy enemy = new Enemy(new Vector3(10, 2, 5), "player", "StarSparrow_Green");
            Enemy enemy2 = new Enemy(new Vector3(0, 2, 30), "player", "StarSparrow_Green");
            AppleTree apple = new AppleTree(new Vector3(30, 2, 30), "player", "StarSparrow_Green");

           //  enemies.Add(enemy);
           // enemies.Add(enemy2);
            enemies.Add(apple);
            serializator = new Serializator("zapis.txt");
            interactionEventHandler = new InteractionEventHandler(player,enemies);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            world.ObjectInitializer(Content);
            hud.LoadContent(Content);
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Projection = projectionMatrix;
            player.LoadContent(Content);

            foreach (Enemy enemy in enemies)
            {
                enemy.LoadContent(Content);
                enemy.OnDestroy += DestroyControl;
            }

            player.OnDestroy += DestroyControl;
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            camera.CamPosition = player.GetPosition() + camera.CamPositionState;
            camera.nextpos = player.GetPosition();
            viewMatrix = Matrix.CreateLookAt(camera.CamPosition, camera.camTracker , Vector3.Up);
            //viewMatrix = Matrix.CreateLookAt(camera.CamPosition, player.GetPosition(), Vector3.Up);
            basicEffect.View = Matrix.CreateLookAt(camera.CamPosition, camera.camTracker, Vector3.Up);
            player.Update(world, delta);

            foreach(Enemy enemy in enemies)
            {
                enemy.Update(delta, player);
                
            }

            camera.Update();
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
            foreach (SceneObject sceneObject in world.GetWorldList())
            {
                sceneObject.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix,sceneObject.color);
            }
            foreach(Enemy enemy in enemies)
            {
                enemy.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix,enemy.color);
            }
            foreach (Enemy enemy in enemies )
            {
                if(enemy.GetType() == typeof(AppleTree))
                {
                    AppleTree tree = (AppleTree)enemy;
                    foreach(Apple apple in tree.bullet) 
                    {
                        apple.LoadContent(Content);
                        apple.Draw(effectHandler,worldMatrix, viewMatrix, projectionMatrix,apple.color); 
                    }
                   
                }
            }
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
            foreach(Enemy enemy in enemies)
            {
                DrawBB(enemy.boundingBox.GetCorners());
            }
            //DrawBB(player.boundingBox.GetCorners());
            foreach (Enemy enemy in enemies)
            {
                DrawBS(enemy.boundingSphere.Center, enemy.boundingSphere.Radius);
            }
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
            enemies.Remove((Enemy)obj);
        }
        #endregion

    }

}