using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using Color = Microsoft.Xna.Framework.Color;

namespace TheGame
{
    public class Game1 : Game
    {
        //DON'T TOUCH IT MORTALS
        int WindowWidth = 1920;
        int WindowHeight = 1080;
        private SpriteBatch _spriteBatch;
        Camera camera;
        EffectHandler effectHandler;
        EffectHandler effectPlayerHandler;
        Serializator serializator;
        AudioMenager audioMenager;
        //.................


        private BasicEffect basicEffect;
        World world;
        HUD hud;
        InteractionEventHandler interactionEventHandler;
        Player player;
        Player animacyjnaPacynka;
        Enemies enemies;
        LeafList Leafs;
        SoundActorPlayer soundActorPlayer;
        AnimationMenager animationMenager;
        Viewport viewport;

        public Game1()
        {
            Globals._graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Leafs = new LeafList();
            enemies = new Enemies();
            IsMouseVisible = true;
            Globals._graphics.PreferredBackBufferWidth = WindowWidth;
            Globals._graphics.PreferredBackBufferHeight = WindowHeight;
            Globals._graphics.ApplyChanges();
            Globals.Pause = true;

        }


        protected override void Initialize()
        {

            //DON'T TOUCH IT MORTALS
            camera = new Camera();

            Globals.projectionMatrix = Matrix.CreateOrthographicOffCenter(-(WindowWidth / 50), (WindowWidth / 50), -(WindowHeight / 50), (WindowHeight / 50), 1f, 100f);      // orthographic view 
                                                                                                                                                                              //projectionMatrix = Matrix.CreateOrthographic(20, 20, 1f, 1000f);                      // second type orthographic view

            // PERSPECTIVE point of view
            //projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f),GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f); // render range (from 1 near playing to 1000 far playing)
            // ................................................................................................................

            // tells the world of our orientantion
            // (the same as:
            // Vector3(0,1,0) - up and down is along y axis)
            Globals.worldMatrix = Matrix.CreateWorld(camera.CamTarget, Vector3.Forward, Vector3.Up);
            //.................

            

            Globals.effectHandler = new EffectHandler(Content.Load<Effect>("ShaderOne"));
            Globals.effectHandler1 = new EffectHandler(Content.Load<Effect>("MainShader"));
           
            hud = new HUD("Textures/forest2", WindowWidth, WindowHeight);
            world = new World();
            player = new Player(new Vector3(30,0,50), "Objects/mis", "Textures/MisTexture");
            animacyjnaPacynka = new Player(new Vector3(0, 15, 30), "Objects/mis", "Textures/tekstura");
            serializator = new Serializator("zapis.txt");
            interactionEventHandler = new InteractionEventHandler(player, enemies.EnemiesList);
            audioMenager = new AudioMenager(Content);
            soundActorPlayer = new SoundActorPlayer(Content, player, enemies.EnemiesList);
            animationMenager = new AnimationMenager(Content, animacyjnaPacynka, enemies.EnemiesList);
            viewport = GraphicsDevice.Viewport;
            /*AppleTree apple = new AppleTree(new Vector3(25, 0, 25), "player", "StarSparrow_Green");
            Mint mint = new Mint(new Vector3(20, 0, 20), "player", "StarSparrow_Green");
            Melissa apple2 = new Melissa(new Vector3(23, 0, 23), "player", "StarSparrow_Green");
            Nettle apple3 = new Nettle(new Vector3(22, 0, 21), "player", "StarSparrow_Green");
            enemies.AddEnemy(apple);
            enemies.AddEnemy(mint);
            enemies.AddEnemy(apple2);
            enemies.AddEnemy(apple3);*/

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.content = this.Content;

            world.LoadContent();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            hud.LoadContent();
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Projection = Globals.projectionMatrix;
            player.LoadContent();
            enemies.LoadModels();
            Leafs.LoadModels();
            animacyjnaPacynka.LoadContent();

            audioMenager.LoadContent();
            soundActorPlayer.LoadContent();
            animationMenager.LoadContent();

        }

        protected override void Update(GameTime gameTime)
        {
            hud.MainMenuCheck();
            audioMenager.MainPlay();
            //if (!Globals.Pause)
            //{
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();
                var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Globals.time += delta;
                camera.CamPosition = player.GetPosition() + camera.CamPositionState;
                camera.nextpos = player.GetPosition();
                Globals.viewMatrix = Matrix.CreateLookAt(camera.CamPosition, player.GetPosition(), Vector3.Up);
                basicEffect.View = Matrix.CreateLookAt(camera.CamPosition, camera.camTracker, Vector3.Up);
                player.Update(world, delta, enemies);
                enemies.AddEnemies(world.returnEnemiesList(player.GetPosition().X, player.GetPosition().Z));  // czemu w update ???
                enemies.Move(delta, player);    // i po co 3 funkcje a nie 1
                enemies.RefreshOnDestroy();
                Leafs.RefreshInventory(this.player);
                Leafs.UpdateScene(enemies.EnemiesList);
                camera.Update1(player.GetPosition());
                hud.Update(camera.CamPosition, player.Inventory.returnLeafs());
                
                interactionEventHandler.Update(enemies.EnemiesList);
                viewport = GraphicsDevice.Viewport;
                animationMenager.Update(gameTime);
                SaveControl();
                base.Update(gameTime);
           // }
            //else
            //
               //Globals.time = 0;
            //
            
        }

        protected override void Draw(GameTime gameTime)
        {
           // if (!Globals.Pause)
            //{
                GraphicsDevice.Clear(Color.Black);
                base.Draw(gameTime);
                hud.DrawBackground(_spriteBatch);
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                
                world.Draw(player.GetPosition());
                player.DrawPlayer(player.GetPosition());
            
                enemies.Draw(player.GetPosition());
                
                Leafs.Draw(player.GetPosition());
                animationMenager.DrawAnimation(GraphicsDevice);
                hud.DrawFrontground(_spriteBatch, player.Health, enemies.EnemiesList, viewport);  //hud jako OSTATNI koniecznie



            //}
            // else
            // {
            // hud.DrawMainMenu(_spriteBatch);
            // }
            //DrawBoundingBoxes();
        }

        #region DrawingBB
        public void DrawBoundingBoxes()
        {
            basicEffect.CurrentTechnique.Passes[0].Apply();

            foreach (SceneObject obj in world.GetWorldList())
            {
                //obj.DrawBB();
            }
            foreach (Enemy enemy in enemies.EnemiesList)
            {
                enemy.DrawBB();
                if (enemy.GetType() == typeof(AppleTree))
                {
                    AppleTree apple1 = (AppleTree)enemy;
                    foreach (Apple apple in apple1.bullet)
                    {
                        //apple.DrawBB();
                    }
                }
                //DrawBS(enemy.boundingSphere.Center, enemy.boundingSphere.Radius);
            }

            player.DrawBB();
            DrawBS(player.boundingSphere.Center, player.boundingSphere.Radius);
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
                    player.LoadContent();
                }

            }
        }

        void DestroyControl(object obj, EventArgs e)
        {
            if (obj is Player)
            {
                Exit();
                // GAME OVER HERE;
            }

        }
        #endregion

    }

}