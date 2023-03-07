using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace TheGame
{
    public class Game1 : Game
    {
        //DON'T TOUCH IT MORTALS
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;
        Effect effect;
        //.................
        World world;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            //DON'T TOUCH IT MORTALS
                camTarget = new Vector3(0f, 0f, 0f);
                camPosition = new Vector3(10f, 20f, 20f);

                projectionMatrix = Matrix.CreateOrthographicOffCenter(-10, 10, -10, 5, 1f, 1000f);      // orthographic view 
                                                                                                        // projectionMatrix = Matrix.CreateOrthographic(20, 20, 1f, 1000f);                      // second type orthographic view

                // PERSPECTIVE point of view
                //projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                //    MathHelper.ToRadians(45f),
                //    GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f); // render range (from 1 near playing to 1000 far playing)
                // ................................................................................................................

                viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up); // tells the world of our orientantion
                                                                                      // (the same as:
                                                                                      // Vector3(0,1,0) - up and down is along y axis)
                worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);
            //.................

            world = new World(Content,2.15f,4,4,"test", "StarSparrow_Green", "ShaderOne");
            world.ObjectInitializer(Content);


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

            //worldMatrix = world.WorldMove(worldMatrix, new Vector3(0.1f, 0, 0));
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue); //change if u want to
            GraphicsDevice.Clear(Color.Black);           //........

            base.Draw(gameTime);

            foreach (SceneObject sceneObject in world.GetSceneObjectList())
            {
                sceneObject.DrawModelWithEffect(sceneObject.GetModel(),worldMatrix * Matrix.CreateScale(sceneObject.GetScale()) *
                    Matrix.CreateRotationX(sceneObject.GetRotation().X) * Matrix.CreateRotationY(sceneObject.GetRotation().Y) *
                    Matrix.CreateRotationZ(sceneObject.GetRotation().Z) * Matrix.CreateTranslation(sceneObject.GetPosition().X,
                    sceneObject.GetPosition().Y, sceneObject.GetPosition().Z), viewMatrix, projectionMatrix, sceneObject.GetTexture2D());
            }


        }

    }
}