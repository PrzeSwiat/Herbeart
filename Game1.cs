using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Reflection;
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
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;
        float cosAngle = 0;
        float tanAngle = 0;
        float sinAngle = 0;
        Effect effect;
        //.................
        World world;
        Player player;
        MouseState prevMouseState;
        MouseState mouseState;

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
                camTarget = new Vector3(0f, 0f, 0f);
                camPosition = new Vector3(30f, 30f, 30f);

                //projectionMatrix = Matrix.CreateOrthographicOffCenter(-(WindowWidth / 100), (WindowWidth / 100), -(WindowHeight / 50), (WindowHeight / 100), 1f, 100f);      // orthographic view 
                  projectionMatrix = Matrix.CreateOrthographic(20, 20, 1f, 1000f);                      // second type orthographic view

                // PERSPECTIVE point of view
                //projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f),GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f); // render range (from 1 near playing to 1000 far playing)
                // ................................................................................................................

                viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up); // tells the world of our orientantion
                                                                                      // (the same as:
                                                                                      // Vector3(0,1,0) - up and down is along y axis)
                worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);
            //.................

            world = new World(WindowWidth,WindowHeight,Content,2f,14,14,"test", "StarSparrow_Green", "ShaderOne");
            player = new Player(Content,new Vector3(0,2,0), "player", "StarSparrow_Orange", "ShaderOne");
            world.ObjectInitializer(Content);

            cosAngle =  camPosition.X / (float)Math.Sqrt(Math.Pow(camPosition.X, 2) + Math.Pow(camPosition.Z, 2));
            sinAngle = camPosition.Z / (float)Math.Sqrt(Math.Pow(camPosition.X, 2) + Math.Pow(camPosition.Z, 2));
            tanAngle = camPosition.Z / camPosition.X;




            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            worldMatrix = player.PlayerMovement(world,cosAngle,tanAngle,worldMatrix);
            //MouseMovement();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue); //change if u want to
            GraphicsDevice.Clear(Color.Black);           //........

            base.Draw(gameTime);

            foreach (SceneObject sceneObject in world.GetWorldList())
            {
                sceneObject.DrawModelWithEffect(sceneObject.GetModel(),worldMatrix * Matrix.CreateScale(sceneObject.GetScale())
                     * Matrix.CreateTranslation(sceneObject.GetPosition().X,sceneObject.GetPosition().Y, sceneObject.GetPosition().Z)
                     *Matrix.CreateRotationX(sceneObject.GetRotation().X) * Matrix.CreateRotationY(sceneObject.GetRotation().Y) *
                    Matrix.CreateRotationZ(sceneObject.GetRotation().Z), viewMatrix, projectionMatrix, sceneObject.GetTexture2D());
            }



            player.DrawModelWithEffect(player.GetModel(), worldMatrix * Matrix.CreateScale(player.GetScale()) *
                    Matrix.CreateTranslation(player.GetPosition().X,player.GetPosition().Y, player.GetPosition().Z) *
                    Matrix.CreateRotationX(player.GetRotation().X) * Matrix.CreateRotationY(player.GetRotation().Y) *
                    Matrix.CreateRotationZ(player.GetRotation().Z), viewMatrix, projectionMatrix, player.GetTexture2D());

        }

    }
}