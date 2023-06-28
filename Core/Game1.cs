using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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
        DateTime death_timer;
        DateTime score_timer;
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
            //Globals._graphics.IsFullScreen = true;
            Globals._graphics.PreferredBackBufferWidth = Globals.WindowWidth; // Szerokość ekranu
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
            score_timer = DateTime.Now;                                                                                                                                  //projectionMatrix = Matrix.CreateOrthographic(20, 20, 1f, 1000f);                      // second type orthographic view

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
            Globals.ScoreMultiplier = 1;
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
        }

        protected override void Update(GameTime gameTime)
        {
            Globals.gameTime = gameTime;
            Globals.HPpercent = player.calculateHPPercent();

            if (Globals.Start)
            {
                hud.name = "";
                if(Globals.Tutorial)
                {
                    TurorialCheck();
                }
                else if (Globals.Credits)
                {
                    CreditsCheck();
                }
                else if(Globals.LeaderBoardSumup)
                {
                    LeaderBoardSumupCheck();
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
                            TimeSpan timeSpan = DateTime.Now - death_timer;
                            animationMenager.DeathAnimationUpdate(gameTime);
                            if (timeSpan.TotalSeconds > 4)
                            {
                                DeathMenuCheck();
                            }
                           
                        }
                    }
                    else
                    {
                        TutorialPauseCheck(Globals.time);
                        if (!Globals.TutorialPause)
                        {
                            TimeSpan scoredeltatime = DateTime.Now - score_timer;
                            if (scoredeltatime.TotalSeconds > 1) { Globals.Score++; score_timer = DateTime.Now; }
                            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                            Globals.time += delta;
                            player.Update(world, delta, enemies, gameTime);
                            camera.CamPosition = player.GetPosition() + camera.CamPositionState;
                            camera.nextpos = player.GetPosition();
                            Globals.viewMatrix = Matrix.CreateLookAt(camera.CamPosition, player.GetPosition(), Vector3.Up);
                            basicEffect.View = Matrix.CreateLookAt(camera.CamPosition, camera.camTracker, Vector3.Up);
                            world.PrepareRandomMap(player.GetPosition().X, player.GetPosition().Z);
                            enemies.AddEnemies(world.returnEnemiesList(player.GetPosition().X, player.GetPosition().Z));  // czemu w update ???
                            enemies.SetObstaclePositions(world.GetEnemiesColliders(player.GetPosition().X, player.GetPosition().Z));
                            enemies.Move(delta, player,gameTime);    // i po co 3 funkcje a nie 1
                            enemies.RefreshOnDestroy();
                            Leafs.RefreshInventory(this.player);
                            Leafs.UpdateScene(enemies.EnemiesList, gameTime);
                            camera.Update1(player.GetPosition());
                            soundActorPlayer.Update();

                            interactionEventHandler.Update(enemies.EnemiesList);
                            Globals.viewport = GraphicsDevice.Viewport;
                            ParticleSystem particleSystem = ParticleSystem.Instance;
                            particleSystem.Update();

                            if (world.ifPlayerOnPartyModule(player.GetPosition()))
                            {
                                player.Stop();
                                if (!Globals.playerActiveEffects.Contains("immortal2"))
                                {
                                    Globals.playerActiveEffects.Add("immortal2");
                                }
                            }
                            else
                            {
                                player.Start();
                                Globals.playerActiveEffects.Remove("immortal2");
                            }

                            bool check = world.ifPlayerIsCloseToShop(player.GetPosition());
                            if (progressSystem.canDraw==true && check==false)
                            {
                                
                                progressSystem.canDraw = true;
                                player.Stop();
                            }
                            else if(progressSystem.canDraw==false && check == false) 
                            { 
                                progressSystem.canDraw = false;
                            }
                            else if(progressSystem.canDraw == false && check == true) 
                            {
                                pause_timer = DateTime.Now;
                                progressSystem.canDraw=true;
                                player.Stop();
                            }
                            else if(progressSystem.canDraw == true && check == true)
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
            Globals.prevPauseState = GamePad.GetState(PlayerIndex.One);
            Globals.prevLeaderState = GamePad.GetState(PlayerIndex.One);
            Globals.prevTutorialState = GamePad.GetState(PlayerIndex.One);

            Globals.prevKeyBoardTutorialState = Keyboard.GetState();
            Globals.prevKeyBoardState = Keyboard.GetState();
            Globals.prevKeyBoardLeaderState = Keyboard.GetState();
            Globals.prevKeyBoardDeathState = Keyboard.GetState();
            Globals.prevKeyBoardPauseState = Keyboard.GetState();

            if (Globals.Start)
            {
                
                if(Globals.Tutorial)
                {
                    hud.DrawTutorialMenu();
                }
                else if (Globals.Credits)
                {
                    hud.DrawCredits();
                }
                else if (Globals.LeaderBoardSumup)
                {
                    hud.DrawLeaderBoardSumup(Globals.LeaderBoardSting);
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
                            GraphicsDevice.Clear(Color.Black);
                            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                            TimeSpan timeSpan = DateTime.Now - death_timer;
                            player.SetRotation(new Vector3(0,0,0));
                            animationMenager.DeathAnimationDraw();
                            if (timeSpan.TotalSeconds > 4)
                            {
                                hud.DrawDeathMenu();
                            }
                        }
                    }
                    else
                    {

                        GraphicsDevice.Clear(Color.Black);
                        base.Draw(gameTime);


                        GraphicsDevice.BlendState = BlendState.AlphaBlend;
                        GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                        animationMenager.DrawAnimations();
                        world.Draw(new System.Numerics.Vector3(player.GetPosition().X, player.GetPosition().Y, player.GetPosition().Z));
                        player.DrawPlayer(player.GetPosition());
                        enemies.Draw(player.GetPosition());
                        Leafs.Draw(player.GetPosition());
                        player.DrawEffectsShadow(player.GetPosition());
                        GraphicsDevice.BlendState = BlendState.Opaque;
                        
                        hud.Update(player.Inventory.returnLeafs(), player.isCrafting(), player.isThrowing(), player.Crafting.returnRecepture(), player.getRotationY());
                        hud.DrawFrontground(player.Health, enemies.EnemiesList);  //hud jako OSTATNI koniecznie
                        ParticleSystem particleSystem = ParticleSystem.Instance;
                        particleSystem.Draw();
                        Leafs.DrawHud();//Koniecznie ostatnie nawet za Hudem
                        player.DrawAnimation();
                        progressSystem.drawSelectMenu(pause_timer);
                        enemies.DrawHud();


                        if (Globals.TutorialPause && !progressSystem.canDraw)
                        {
                            if (player.Health <= player.maxHealth * 0.80 && Globals.counter == 0)    // 80% zycia 
                            {
                                hud.DrawTutorial(2);
                            }
                            if (Globals.Module2 && Globals.moduleCounter == 0) // 1 attack_tutorial
                            {
                                hud.DrawTutorial(1);
                            }
                            if (Globals.Module4 && Globals.moduleCounter == 1) // 4 efekty miotane pierwsze
                            {
                                hud.DrawTutorial(4);
                            }
                            if (Globals.Module5 && Globals.moduleCounter == 2) // 5 wesele
                            {
                                hud.DrawTutorial(5);
                            }
                        }

                    }

                }
                else
                {
                    hud.DrawPause();
                }
            }
        }

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
                if (Globals.Module2 && Globals.moduleCounter == 0) // 1 attack_tutorial
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

                if (Globals.Module4 && Globals.moduleCounter == 1)   
                {
                    player.Stop();
                    Globals.TutorialPause = true;
                    if (!Globals.tutorialDone[2])
                    {
                        Globals.tutorialDone[2] = true;
                        pause_timer = DateTime.Now;
                    }

                }
                if (Globals.Module5 && Globals.moduleCounter == 2)
                {
                    player.Stop();
                    Globals.TutorialPause = true;
                    if (!Globals.tutorialDone[3])
                    {
                        Globals.tutorialDone[3] = true;
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
                        if (Globals.Module4 && Globals.moduleCounter == 1)
                        {
                            Globals.moduleCounter += 1;
                        }
                        if (Globals.Module5 && Globals.moduleCounter == 2)
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

        #region MORE_CHECKS
        void LeaderBoardSumupCheck()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState state = Keyboard.GetState();

            if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardState.IsKeyUp(Keys.Enter))) //ACCEPT
            {
                Globals.LeaderBoardSumup = false;
            }

            
        }

        void CreditsCheck()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState state = Keyboard.GetState();

            if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardState.IsKeyUp(Keys.Enter))) //ACCEPT
            {
                Globals.Credits = false;
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
                if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevTutorialState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardTutorialState.IsKeyUp(Keys.Enter)) && hud.MenuOption == 1) //leader board
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
                if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevTutorialState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardTutorialState.IsKeyUp(Keys.Enter)) && hud.MenuOption == 2) //try again 
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
                    if (Globals.Hard)
                    {
                        Globals.ScoreMultiplier++;
                    }
                    player.Start();

                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevTutorialState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardTutorialState.IsKeyUp(Keys.Enter)) && hud.MenuOption == 3) // main menu
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
                if (hud.MenuOption > 4)
                {
                    hud.MenuOption = 4;
                }
                Globals.prevState = gamePadState;
                Globals.prevKeyBoardState = state;
                if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevDeathState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardDeathState.IsKeyUp(Keys.Enter)) && hud.MenuOption == 1)
                {
                    Globals.Tutorial = true;
                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevDeathState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardDeathState.IsKeyUp(Keys.Enter)) && hud.MenuOption == 2)
                {
                    Globals.Credits = true;
                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevDeathState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardDeathState.IsKeyUp(Keys.Enter)) && hud.MenuOption == 3)
                {
                    Globals.LeaderBoardSumup = true;
                    Globals.LeaderBoardSting = PlayerNames.LoadFromFile();

                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed || state.IsKeyDown(Keys.Enter)) && hud.MenuOption == 4)
                {
                    Exit();
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

                if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevState.Buttons.A == ButtonState.Released|| (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardState.IsKeyUp(Keys.Enter)) && hud.MenuOption == 1)
                {
                    player.Start();
                    Globals.Start = false;
                    Globals.Easy = true;
                    Globals.Pause = false;
                }
                if ((gamePadState.Buttons.A == ButtonState.Pressed && Globals.prevState.Buttons.A == ButtonState.Released || (state.IsKeyDown(Keys.Enter)) && Globals.prevKeyBoardState.IsKeyUp(Keys.Enter)) && hud.MenuOption == 2)
                {
                    player.Start();
                    Globals.Start = false;
                    Globals.Hard = true;
                    Globals.ScoreMultiplier ++;
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
                death_timer = DateTime.Now;
                Globals.Death = true;
                // GAME OVER HERE;
            }

        }
        #endregion

    }
}