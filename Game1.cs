using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace TheGame
{
    public class Game1 : Game
    {
        //DON'T TOUCH IT MORTALS
        int WindowWidth = 1280;
        int WindowHeight = 720;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;
        Camera camera;
        EffectHandler effectHandler;
        Serializator serializator;
        //.................
        World world;
        Player player;
        HUD hud;

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
            //projectionMatrix = Matrix.CreateOrthographicOffCenter(-(WindowWidth / 100), (WindowWidth / 100), -(WindowHeight / 50), (WindowHeight / 100), 1f, 100f);      // orthographic view 
            projectionMatrix = Matrix.CreateOrthographic(20, 20, 1f, 1000f);                      // second type orthographic view

                // PERSPECTIVE point of view
                //projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f),GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f); // render range (from 1 near playing to 1000 far playing)
                // ................................................................................................................

                 // tells the world of our orientantion
                                                                                      // (the same as:
                                                                                      // Vector3(0,1,0) - up and down is along y axis)
                worldMatrix = Matrix.CreateWorld(camera.CamTarget, Vector3.Forward, Vector3.Up);
            //.................
            Effect one;
            one = Content.Load<Effect>("ShaderOne");
            effectHandler = new EffectHandler(one);

            hud = new HUD("sky", WindowWidth, WindowHeight);

            //effectHandler.AddLight(new Vector3(0,0,0));

            world = new World(WindowWidth,WindowHeight,Content,2f,30,30,"test", "StarSparrow_Green");
            player = new Player(new Vector3(0,2,0), "player", "StarSparrow_Orange");
            


           

            serializator = new Serializator("zapis.txt");


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(Content);
            world.ObjectInitializer(Content);
            hud.LoadContent(Content);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            camera.CamPosition = player.GetPosition() + camera.CamPositionState;
            camera.nextpos = player.GetPosition();
            viewMatrix = Matrix.CreateLookAt(camera.CamPosition, camera.camTracker , Vector3.Up);
            //viewMatrix = Matrix.CreateLookAt(camera.CamPosition, player.GetPosition(), Vector3.Up);

            player.PlayerMovement(world,camera.CosAngle, camera.SinAngle, camera.TanAngle);
            camera.Update();
            hud.Update(camera.CamPosition);
            
            SaveControl();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue); //change if u want to
            GraphicsDevice.Clear(Color.Black);           //........

            base.Draw(gameTime);

            hud.DrawBackground(_spriteBatch);

            foreach (SceneObject sceneObject in world.GetWorldList())
            {
                effectHandler.BasicDraw(sceneObject.GetModel(),worldMatrix * Matrix.CreateScale(sceneObject.GetScale())
                     * Matrix.CreateTranslation(sceneObject.GetPosition().X,sceneObject.GetPosition().Y, sceneObject.GetPosition().Z)
                     *Matrix.CreateRotationX(sceneObject.GetRotation().X) * Matrix.CreateRotationY(sceneObject.GetRotation().Y) *
                    Matrix.CreateRotationZ(sceneObject.GetRotation().Z), viewMatrix, projectionMatrix, sceneObject.GetTexture2D());
            }



            effectHandler.BasicDraw(player.GetModel(), worldMatrix * Matrix.CreateScale(player.GetScale()) * 
                    Matrix.CreateRotationX(player.GetRotation().X) * Matrix.CreateRotationY(player.GetRotation().Y) *
                    Matrix.CreateRotationZ(player.GetRotation().Z)*
                    Matrix.CreateTranslation(player.GetPosition().X,player.GetPosition().Y, player.GetPosition().Z), viewMatrix, projectionMatrix, player.GetTexture2D());

            hud.DrawFrontground(_spriteBatch);

        }

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
                if(copied!=null)
                {
                    player = copied;
                    player.LoadContent(Content);
                }
               
            }
        }
    }
}