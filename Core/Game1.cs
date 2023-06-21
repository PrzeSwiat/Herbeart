using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using TheGame.Core;
using Color = Microsoft.Xna.Framework.Color;

namespace TheGame
{
    public class Game1 : Game
    {
        //DON'T TOUCH IT MORTALS
        Camera camera;
        EffectHandler effectHandler;
        EffectHandler effectPlayerHandler;
        Serializator serializator;
        Serializator PlayerNames;
        AudioMenager audioMenager;
        ParticleSystem particleSystem;
        DateTime pause_timer;
        //.................


        private BasicEffect basicEffect;
        World world;
        HUD hud;
        InteractionEventHandler interactionEventHandler;
        Player player;
        Enemies enemies;
        LeafList Leafs;
        SoundActorPlayer soundActorPlayer;
        AnimationMenager animationMenager;
        ProgressSystem progressSystem;


        public Game1()
        {
            Globals._graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Globals.WindowWidth = 1920;
            Globals.WindowHeight = 1080;
            Globals._graphics.PreferredBackBufferWidth = Globals.WindowWidth;
            Globals._graphics.PreferredBackBufferHeight = Globals.WindowHeight;
            Globals._graphics.ApplyChanges();
            Globals.Pause = false;
            Globals.Start = true;
            Globals.Tutorial = false;

        }


        protected override void Initialize()
        {
            //DON'T TOUCH IT MORTALS
            camera = new Camera();
            Leafs = new LeafList();
            enemies = new Enemies();
            Globals.projectionMatrix = Matrix.CreateOrthographicOffCenter(-(Globals.WindowWidth / 65), (Globals.WindowWidth / 65), -(Globals.WindowHeight / 65f), (Globals.WindowHeight / 65f), -10f, 100f);      // orthographic view 
                                                                                                                                                            //projectionMatrix = Matrix.CreateOrthographic(20, 20, 1f, 1000f);                      // second type orthographic view

            // PERSPECTIVE point of view
            //projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f),GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f); // render range (from 1 near playing to 1000 far playing)
            // ................................................................................................................

            // tells the world of our orientantion
            // (the same as:
            // Vector3(0,1,0) - up and down is along y axis)
            Globals.worldMatrix = Matrix.CreateWorld(camera.CamTarget, Vector3.Forward, Vector3.Up);
            //.................

            Globals.tutorialDone = new bool[7];
            for(int i=0;i<Globals.tutorialDone.Length;i++)
            {
                Globals.tutorialDone[i] = false;
            }
            pause_timer = new DateTime();
            Globals.effectHandler = new EffectHandler(Content.Load<Effect>("ShaderOne"));
            Globals.effectHandler1 = new EffectHandler(Content.Load<Effect>("MainShader"));
            Globals.Score = 0;
            Globals.ScoreMultipler = 1;
            Globals.learnedRecepture = new List<String>();
            Globals.learnedRecepture.Add("AAA");
            hud = new HUD();
            world = new World();
            player = new Player(new Vector3(50,0,60), "Objects/mis", "Textures/MisTexture");
            serializator = new Serializator("zapis.txt");
            PlayerNames = new Serializator("PlayerNames.txt");
            interactionEventHandler = new InteractionEventHandler(player, enemies.EnemiesList);
            audioMenager = new AudioMenager(Content);
            soundActorPlayer = new SoundActorPlayer(Content, player, enemies.EnemiesList);
            animationMenager = new AnimationMenager(Content, player, enemies.EnemiesList);
            Globals.numberOfRecepture = 0;
            Globals.viewport = GraphicsDevice.Viewport;
            
            base.Initialize();

        }

        protected override void LoadContent()
        {
            Globals.content = this.Content;

            world.LoadContent();
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);
            hud.LoadContent();
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Projection = Globals.projectionMatrix;
            player.LoadContent();
            enemies.LoadModels();
            Leafs.LoadModels();
            audioMenager.LoadContent();
            soundActorPlayer.LoadContent();
            animationMenager.LoadContent();
            player.OnDestroy += DestroyControl;
            progressSystem = new ProgressSystem(player);


            //List<Texture2D> textures = new List<Texture2D>();
            //textures.Add(Content.Load<Texture2D>("Particles/Textures/circle"));
            //textures.Add(Content.Load<Texture2D>("Particles/Textures/star"));
            //textures.Add(Content.Load<Texture2D>("diamond"));
            //particleSystem = new ParticleSystem(textures, new Vector2(400, 240));
        }

        protected override void Update(GameTime gameTime)
        {
            //particleSystem.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            //particleSystem.Update();
            if (Globals.Start)
            {
                hud.name = "";
                if(Globals.Tutorial)
                {
                    TurorialCheck();
                }
                else
                {
                    MainMenuCheck();
                }
                Globals.time = 0;
                audioMenager.MainPlay();
            }
            else
            {
                PauseCheck();
                audioMenager.MainPlay();
                if (!Globals.Pause)
                {
                    if (Globals.Death)
                    {
                        if (Globals.LeaderBoard)
                        {
                            LeaderBoardCheck();
                        }
                        else
                        {

                            DeathMenuCheck();
                        }
                    }
                    else
                    {
                        TutorialPauseCheck(Globals.time);
                        if (!Globals.TutorialPause)
                        {
                            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                            Globals.time += delta;
                            player.Update(world, delta, enemies, gameTime);
                            camera.CamPosition = player.GetPosition() + camera.CamPositionState;
                            camera.nextpos = player.GetPosition();
                            Globals.viewMatrix = Matrix.CreateLookAt(camera.CamPosition, player.GetPosition(), Vector3.Up);
                            basicEffect.View = Matrix.CreateLookAt(camera.CamPosition, camera.camTracker, Vector3.Up);
                            
                            enemies.AddEnemies(world.returnEnemiesList(player.GetPosition().X, player.GetPosition().Z));  // czemu w update ???
                            enemies.SetObstaclePositions(world.GetEnemiesColliders(player.GetPosition().X, player.GetPosition().Z));
                            enemies.Move(delta, player);    // i po co 3 funkcje a nie 1
                            enemies.RefreshOnDestroy();
                            Leafs.RefreshInventory(this.player);
                            Leafs.UpdateScene(enemies.EnemiesList, gameTime);
                            camera.Update1(player.GetPosition());
                            world.PrepareRandomMap(player.GetPosition().X, player.GetPosition().Z);

                            interactionEventHandler.Update(enemies.EnemiesList);
                            Globals.viewport = GraphicsDevice.Viewport;
                            if(progressSystem.canDraw==true && world.ifPlayerOnPartyModule(player.GetPosition().X, player.GetPosition().Z)==false)
                            {
                                
                                progressSystem.canDraw = true;
                                player.Stop();
                            }
                            else if(progressSystem.canDraw==false && world.ifPlayerOnPartyModule(player.GetPosition().X, player.GetPosition().Z) == false) 
                            { 
                                progressSystem.canDraw = false;
                            }
                            else if(progressSystem.canDraw == false && world.ifPlayerOnPartyModule(player.GetPosition().X, player.GetPosition().Z) == true) 
                            {
                                pause_timer = DateTime.Now;
                                progressSystem.canDraw=true;
                                player.Stop();
                            }
                            else if(progressSystem.canDraw == true && world.ifPlayerOnPartyModule(player.GetPosition().X, player.GetPosition().Z) == true)
                            {
                                
                                progressSystem.canDraw=true;
                                player.Stop();
                            }
                            animationMenager.Update(gameTime);
                            SaveControl();
                            base.Update(gameTime);
                        }
                    }
                }
                
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            Globals.prevState = GamePad.GetState(PlayerIndex.One);
            Globals.prevDeathState = GamePad.GetState(PlayerIndex.One);
            Globals.prevKeyBoardState = Keyboard.GetState();
            Globals.prevKeyBoardDeathState = Keyboard.GetState();
            Globals.prevPauseState = GamePad.GetState(PlayerIndex.One);
            Globals.prevKeyBoardPauseState = Keyboard.GetState();
            if (Globals.Start)
            {
                
                if(Globals.Tutorial)
                {
                    hud.DrawTutorialMenu();
                }
                else
                {
                    hud.DrawMainMenu();
                }
            }
            else
            {
                if (!Globals.Pause)
                {
                    if(Globals.Death)
                    {
                        if(Globals.LeaderBoard)
                        {
                            hud.DrawLeaderBoard();
                        }
                        else
                        {
                            hud.DrawDeathMenu();
                        }
                    }
                    else
                    {

                        GraphicsDevice.Clear(Color.Black);
                        base.Draw(gameTime);
                        GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                        world.Draw(new System.Numerics.Vector3(player.GetPosition().X, player.GetPosition().Y, player.GetPosition().Z));
                        player.DrawPlayer(player.GetPosition());

                        enemies.Draw(player.GetPosition());

                        Leafs.Draw(player.GetPosition());
                        animationMenager.DrawAnimations();
                        player.DrawEffectsShadow(player.GetPosition());

                        hud.Update(player.Inventory.returnLeafs(), player.isCrafting(), player.isThrowing(), player.Crafting.returnRecepture(), player.getRotationY());
                        hud.DrawFrontground(player.Health, enemies.EnemiesList);  //hud jako OSTATNI koniecznie
                        Leafs.DrawHud();//Koniecznie ostatnie nawet za Hudem
                        player.DrawAnimation();
                        progressSystem.drawSelectMenu(pause_timer);


                        if (Globals.TutorialPause && !progressSystem.canDraw)
                        {
                            if (player.Health <= player.maxHealth * 0.80 && Globals.counter == 0)    // 80% zycia 
                            {
                                hud.DrawTutorial(2);


                            }
                            if (Globals.Module2 && Globals.moduleCounter == 0)
                            {
                                hud.DrawTutorial(1);
                            }
                            if (Globals.Module3 && Globals.moduleCounter == 1)
                            {
                                hud.DrawTutorial(3);
                            }
                            if (Globals.Module4 && Globals.moduleCounter == 2)
                            {
                                hud.DrawTutorial(4);
                            }
                            if(Globals.Module6 && Globals.moduleCounter == 3)
                            {
                                hud.DrawTutorial(6);
                            }
                            if (Globals.Module9 && Globals.moduleCounter == 4)
                            {
                                hud.DrawTutorial(9);
                            }

                        }

                    }

                }
                else
                {
                    hud.DrawPause();
                }
                //DrawBoundingBoxes();
            }

            //particleSystem.Draw(Globals.spriteBatch);
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
                        apple.DrawBB();
                    }
                }
                DrawBS(enemy.boundingSphere.Center, enemy.boundingSphere.Radius);
            }

            player.DrawBB();
            /*foreach (BoundingBox bb in player.returnApplesBB())
            {
                SceneObject.DrawBB(bb);
            }*/
            //DrawBS(player.)
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

        #region PAUSE_CHECK
        void PauseCheck()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState state = Keyboard.GetState();
            if ((gamePadState.Buttons.Start == ButtonState.Pressed && Globals.prevState.Buttons.Start == ButtonState.Released) || (state.IsKeyDown(Keys.Escape) && Globals.prevKeyBoardState.IsKeyUp(Keys.Escape)))
            {
                Globals.Pause = true;
                player.Stop();
            }
            Globals.prevState = gamePadState;
            Globals.prevKeyBoardState = state;

            if (Globals.Pause)
            {
                if ((gamePadState.ThumbSticks.Left.Y >= 0.5f && !(Globals.prevPauseState.ThumbSticks.Left.Y >= 0.5f)) || (state.IsKeyDown(Keys.W) && !Globals.prevKeyBoardPauseState.IsKeyDown(Keys.W)))
                {
                    hud.MenuOption -= 1;
                }
                if ((gamePadState.ThumbSticks.Left.Y <= -0.5f && !(Globals.prevPauseState.ThumbSticks.Left.Y <= -0.5f)) || (state.IsKeyDown(Keys.S) && !Globals.prevKeyBoardPauseState.IsKeyDown(Keys.S)))
                {
                    hud.MenuOption += 1;
                }
                if (hud.MenuOption < 1)
                {
                    hud.MenuOption = 1;
                }
                if (hud.MenuOption > 3)
                {
                    hud.MenuOption = 3;
                }

                Globals.prevPauseState = gamePadState;
                Globals.prevKeyBoardPauseState = state;

                if ((gamePadState.Buttons.A == ButtonState.Pressed || state.IsKeyDown(Keys.Enter)) && hud.MenuOption == 1) //reasume
                {
                    Globals.Pause = false;
                    player.Start();
                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed || state.IsKeyDown(Keys.Enter)) && hud.MenuOption == 2) //mainmenu
                {
                    Globals.Death = false;
                    Globals.Tutorial = false;
                    Globals.TutorialPause = false;
                    Globals.Module2 = false;
                    Globals.Module3 = false;
                    Globals.Module4 = false;
                    Globals.Module5 = false;
                    Globals.Module6 = false;
                    Globals.Module9 = false;
                    Globals.counter = 0;
                    Globals.moduleCounter = 0;
                    LoadContent();
                    Initialize();
                    
                    Globals.Start = true;
                    Globals.Easy = false;
                    Globals.Hard = false;
                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed || state.IsKeyDown(Keys.Enter)) && hud.MenuOption == 3) //exit 
                {
                    Exit();
                }
                
            }
           
        }
        #endregion

        #region TUTORIALPAUSE_CHECK
        void TutorialPauseCheck(float time)
        {
            if(Globals.Easy)
            {
                if (Globals.Module2 && Globals.moduleCounter == 0) 
                {
                    player.Stop();
                    Globals.TutorialPause = true;
                    if (!Globals.tutorialDone[0])
                    {
                        Globals.tutorialDone[0] = true;
                        pause_timer = DateTime.Now;
                    }
                    
                }

                if (player.Health <= player.maxHealth * 0.80 && Globals.counter == 0)    // 80% zycia 
                {
                    player.Stop();
                    Globals.TutorialPause = true;
                    if (!Globals.tutorialDone[1])
                    {
                        Globals.tutorialDone[1] = true;
                        pause_timer = DateTime.Now;
                    }
                }

                if(Globals.Module3 && Globals.moduleCounter == 1)    
                {
                    player.Stop();
                    Globals.TutorialPause = true;
                    if (!Globals.tutorialDone[2])
                    {
                        Globals.tutorialDone[2] = true;
                        pause_timer = DateTime.Now;
                    }
                    //ODBLOKOWAĆ NOWY SKŁADNIK POKRZYWA
                }
                if (Globals.Module4 && Globals.moduleCounter == 2)   
                {
                    player.Stop();
                    Globals.TutorialPause = true;
                    if (!Globals.tutorialDone[3])
                    {
                        Globals.tutorialDone[3] = true;
                        pause_timer = DateTime.Now;
                    }

                }
                if (Globals.Module6 && Globals.moduleCounter == 3)
                {
                    player.Stop();
                    Globals.TutorialPause = true;
                    if (!Globals.tutorialDone[4])
                    {
                        Globals.tutorialDone[4] = true;
                        pause_timer = DateTime.Now;
                    }
                    //ODBLOKOWAĆ NOWY SKŁADNIK  JABŁOŃ

                }
                if (Globals.Module9 && Globals.moduleCounter == 4)
                {
                    player.Stop();
                    Globals.TutorialPause = true;
                    if (!Globals.tutorialDone[5])
                    {
                        Globals.tutorialDone[5] = true;
                        pause_timer = DateTime.Now;
                    }
                    //ODBLOKOWAĆ NOWY SKŁADNIK  JABŁOŃ

                }


                if (Globals.TutorialPause)
                {
                    GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                    KeyboardState state = Keyboard.GetState();

                    if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevPauseState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter) && Globals.prevKeyBoardPauseState.IsKeyUp(Keys.Enter)))) //ACCEPT
                    {
                        
                        TimeSpan deltatime_Pause = DateTime.Now - pause_timer;
                        if(deltatime_Pause.TotalSeconds > 1) { 
                        if (Globals.Module2 && Globals.moduleCounter == 0)
                        {
                            Globals.moduleCounter += 1;
                        }
                        if (player.Health <= player.maxHealth * 0.80 && Globals.counter == 0)    // 80% zycia 
                        {
                            Globals.counter += 1;
                        }
                        if (Globals.Module3 && Globals.moduleCounter == 1)
                        {
                            Globals.moduleCounter += 1;
                        }
                        if (Globals.Module4 && Globals.moduleCounter == 2)
                        {
                            Globals.moduleCounter += 1;
                        }
                        if (Globals.Module6 && Globals.moduleCounter == 3)
                        {
                            Globals.moduleCounter += 1;
                        }
                        if (Globals.Module9 && Globals.moduleCounter == 4)
                        {
                            Globals.moduleCounter += 1;
                        }

                        player.Start();
                        Globals.TutorialPause = false;
                        }
                    }
                }
            }
        }
        #endregion

        #region LEADERBOARD_CHECK
        void LeaderBoardCheck()
        {
            if (Globals.LeaderBoard)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                KeyboardState state = Keyboard.GetState();

                Globals.prevLeaderState = gamePadState;
                Globals.prevKeyBoardLeaderState = state;


                if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevDeathState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter) && Globals.prevKeyBoardDeathState.IsKeyUp(Keys.Enter)))) //ACCEPT
                {
                    Window.TextInput -= hud.TextInputHandler;
                    PlayerNames.SavePlayerName(hud.name);
                    Globals.LeaderBoard = false;
                    Globals.Death = false;
                    Globals.Tutorial = false;
                    Globals.Easy = false;
                    Globals.Hard = false;
                    LoadContent();
                    Initialize();
                    Globals.Pause = false;
                    Globals.Start = true;
                }
            }
        }
        #endregion

        #region DEATH_CHECK
        void DeathMenuCheck()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState state = Keyboard.GetState();
            if ((gamePadState.ThumbSticks.Left.Y >= 0.5f && !(Globals.prevDeathState.ThumbSticks.Left.Y >= 0.5f)) || (state.IsKeyDown(Keys.W) && !Globals.prevKeyBoardDeathState.IsKeyDown(Keys.W)))
            {
                hud.MenuOption -= 1;
            }
            if ((gamePadState.ThumbSticks.Left.Y <= -0.5f && !(Globals.prevDeathState.ThumbSticks.Left.Y <= -0.5f)) || (state.IsKeyDown(Keys.S) && !Globals.prevKeyBoardDeathState.IsKeyDown(Keys.S)))
            {
                hud.MenuOption += 1;
            }
            if (hud.MenuOption < 1)
            {
                hud.MenuOption = 1;
            }
            if (hud.MenuOption > 4)
            {
                hud.MenuOption = 4;
            }

            Globals.prevDeathState = gamePadState;
            Globals.prevKeyBoardDeathState = state;

            TimeSpan deltatime_Pause = DateTime.Now - pause_timer;
            if (deltatime_Pause.TotalSeconds >= 1)
            {
                if ((gamePadState.Buttons.A == ButtonState.Pressed || state.IsKeyDown(Keys.Enter)) && hud.MenuOption == 1) //leader board
                {

                    Globals.TutorialPause = false;
                    Globals.Module2 = false;
                    Globals.Module3 = false;
                    Globals.Module4 = false;
                    Globals.Module5 = false;
                    Globals.Module6 = false;
                    Globals.Module9 = false;
                    Globals.counter = 0;
                    Globals.moduleCounter = 0;
                    Window.TextInput += hud.TextInputHandler;
                    Globals.LeaderBoard = true;

                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed || state.IsKeyDown(Keys.Enter)) && hud.MenuOption == 2) //try again 
                {
                    Globals.TutorialPause = false;
                    Globals.Module2 = false;
                    Globals.Module3 = false;
                    Globals.Module4 = false;
                    Globals.Module5 = false;
                    Globals.Module6 = false;
                    Globals.Module9 = false;
                    Globals.Death = false;
                    Globals.Tutorial = false;

                    LoadContent();
                    Initialize();
                    player.Start();

                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed || state.IsKeyDown(Keys.Enter)) && hud.MenuOption == 3) // main menu
                {
                    Globals.TutorialPause = false;
                    Globals.Module2 = false;
                    Globals.Module3 = false;
                    Globals.Module4 = false;
                    Globals.Module5 = false;
                    Globals.Module6 = false;
                    Globals.Module9 = false;
                    Globals.counter = 0;
                    Globals.moduleCounter = 0;
                    Globals.Death = false;
                    Globals.Tutorial = false;
                    Globals.Easy = false;
                    Globals.Hard = false;
                    LoadContent();
                    Initialize();
                    Globals.Pause = false;
                    Globals.Start = true;
                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed || state.IsKeyDown(Keys.Enter)) && hud.MenuOption == 4) // exit
                {
                    Exit();
                }
            }
        }
        #endregion

        #region MENU_CHECK
        void MainMenuCheck()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState state = Keyboard.GetState();
            if(Globals.Start)
            {
                if ((gamePadState.ThumbSticks.Left.Y >= 0.5f && !(Globals.prevState.ThumbSticks.Left.Y >= 0.5f)) || (state.IsKeyDown(Keys.W) && !Globals.prevKeyBoardState.IsKeyDown(Keys.W)))
                {
                    hud.MenuOption -= 1;
                }
                if ((gamePadState.ThumbSticks.Left.Y <= -0.5f && !(Globals.prevState.ThumbSticks.Left.Y <= -0.5f)) || (state.IsKeyDown(Keys.S) && !Globals.prevKeyBoardState.IsKeyDown(Keys.S)))
                {
                    hud.MenuOption += 1;
                }
                if (hud.MenuOption < 1)
                {
                    hud.MenuOption = 1;
                }
                if (hud.MenuOption > 3)
                {
                    hud.MenuOption = 3;
                }
                Globals.prevState = gamePadState;
                Globals.prevKeyBoardState = state;
                if ((gamePadState.Buttons.A == ButtonState.Pressed || state.IsKeyDown(Keys.Enter)) && hud.MenuOption == 1)
                {
                    Globals.Tutorial = true;
                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed || state.IsKeyDown(Keys.Enter)) && hud.MenuOption == 3)
                {
                    Exit();
                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed || state.IsKeyDown(Keys.Enter)) && hud.MenuOption == 2)
                {
                    // notimplemented
                }

                
            }

        }
        #endregion

        #region TUTORIAL_CHECK
        void TurorialCheck()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState state = Keyboard.GetState();

            if (Globals.Tutorial)
            {
                if ((gamePadState.ThumbSticks.Left.Y >= 0.5f && !(Globals.prevTutorialState.ThumbSticks.Left.Y >= 0.5f)) || (state.IsKeyDown(Keys.W) && !Globals.prevKeyBoardTutorialState.IsKeyDown(Keys.W)))
                {
                    hud.MenuOption -= 1;
                }
                if ((gamePadState.ThumbSticks.Left.Y <= -0.5f && !(Globals.prevTutorialState.ThumbSticks.Left.Y <= -0.5f)) || (state.IsKeyDown(Keys.S) && !Globals.prevKeyBoardTutorialState.IsKeyDown(Keys.S)))
                {
                    hud.MenuOption += 1;
                }
                if (hud.MenuOption < 1)
                {
                    hud.MenuOption = 1;
                }
                if (hud.MenuOption > 2)
                {
                    hud.MenuOption = 2;
                }
                Globals.prevTutorialState = gamePadState;
                Globals.prevKeyBoardTutorialState = state;

                if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevState.Buttons.A == ButtonState.Released|| (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardDeathState.IsKeyUp(Keys.Enter)) && hud.MenuOption == 1)
                {
                    player.Start();
                    Globals.Start = false;
                    Globals.Easy = true;
                    Globals.Pause = false;
                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed || state.IsKeyDown(Keys.Enter)) && hud.MenuOption == 2)
                {
                    player.Start();
                    Globals.Start = false;
                    Globals.Hard = true;
                    Globals.Pause = false;
                }
            }

        }
        #endregion 

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
                //Exit();
                pause_timer=DateTime.Now;
                Globals.Death = true;
                // GAME OVER HERE;
            }

        }
        #endregion

        private Vector2 ConvertToXnaVector2(System.Numerics.Vector2 systemVector2)
        {
            return new Vector2(systemVector2.X, systemVector2.Y);
        }

    }

}