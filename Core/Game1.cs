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
        int WindowWidth = 1280;
        int WindowHeight = 900;
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
        Enemies enemies;
        LeafList Leafs;
        EffectHandler effectPrzemyslaw;
        EffectHandler effectWiktor;

        public Game1()
        {
            Leafs = new LeafList();
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
            
            projectionMatrix = Matrix.CreateOrthographicOffCenter(-(WindowWidth / 50), (WindowWidth / 50), -(WindowHeight / 50), (WindowHeight / 50), 1f, 100f);      // orthographic view 
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
            AppleTree apple = new AppleTree(new Vector3(25, 2, 25), "player", "StarSparrow_Green");
            Mint apple1 = new Mint(new Vector3(20, 2, 20), "player", "StarSparrow_Green");
            Melissa apple2 = new Melissa(new Vector3(23, 2, 23), "player", "StarSparrow_Green");
            Nettle apple3 = new Nettle(new Vector3(22, 2, 21), "player", "StarSparrow_Green");
           // enemies.AddEnemy(apple);
           // enemies.AddEnemy(apple1);
            //  enemies.AddEnemy(apple2);
           // enemies.AddEnemy(apple3);
            player = new Player(new Vector3(30,0,30), "mis", "MisTexture");

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
            Leafs.LoadModels(Content);
            //player.OnDestroy += DestroyControl;

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            camera.CamPosition = player.GetPosition() + camera.CamPositionState;
            camera.nextpos = player.GetPosition();
            //viewMatrix = Matrix.CreateLookAt(camera.CamPosition, camera.camTracker , Vector3.Up);
            viewMatrix = Matrix.CreateLookAt(camera.CamPosition, player.GetPosition(), Vector3.Up);
            basicEffect.View = Matrix.CreateLookAt(camera.CamPosition, camera.camTracker, Vector3.Up);
            player.Update(world, delta);

            //Leafs.RefreshInventory(this.player);
            //Leafs.RefreshOnCreate(enemies.EnemiesList);
            //Leafs.RefreshOnDestroy();

            enemies.AddEnemies(world.returnEnemiesList(player.position.X, player.position.Z));
            enemies.Move(delta, player);
            enemies.RefreshOnDestroy();


            Leafs.RefreshInventory(this.player);
            Leafs.UpdateScene(enemies.EnemiesList);
            // Debug.Write(player.Inventory.appleLeafNumber + "\n");
            camera.Update1(player.position);
            hud.Update(camera.CamPosition);

            interactionEventHandler.Update(enemies.EnemiesList);
            SaveControl();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            base.Draw(gameTime);

            hud.DrawBackground(_spriteBatch);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            world.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix, player.position.X, player.position.Z);

            enemies.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix, Content);
            player.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix, player.color);


            Leafs.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix, Content);
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
                obj.DrawBB(_graphics.GraphicsDevice);
            }
            foreach (Enemy enemy in enemies.EnemiesList)
            {
                //enemy.DrawBB(_graphics.GraphicsDevice);
                if(enemy.GetType() == typeof(AppleTree))
                {
                    AppleTree apple1 = (AppleTree)enemy;
                    foreach(Apple apple in apple1.bullet)
                    {
                        //apple.DrawBB(GraphicsDevice);
                    }
                }
                //DrawBS(enemy.boundingSphere.Center, enemy.boundingSphere.Radius);
            }

            player.DrawBB(_graphics.GraphicsDevice);
            //DrawBS(player.boundingSphere.Center, player.boundingSphere.Radius);
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