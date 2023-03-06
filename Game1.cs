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

        List<SceneObject> sceneObjectsArray = new List<SceneObject>();
        Vector3 cameraMove;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
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

            //ObjectInitializer();
            WorldCreator(2.15f,4,4,"test", "StarSparrow_Green", "ShaderOne");

            cameraMove = new Vector3(0, 0, 0);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            //cameraMove.X += 0.01f;
            //cameraMove.Z += 0.01f;
            //worldMatrix = Matrix.CreateTranslation(cameraMove);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue); //change if u want to
            GraphicsDevice.Clear(Color.Black);           //........

            base.Draw(gameTime);

            //DrawModelWithEffect(model, worldMatrix , viewMatrix, projectionMatrix, texture);
            foreach (SceneObject sceneObject in sceneObjectsArray)
            {
                sceneObject.DrawModelWithEffect(sceneObject.GetModel(),worldMatrix * Matrix.CreateScale(sceneObject.GetScale()) *
                    Matrix.CreateRotationX(sceneObject.GetRotation().X) * Matrix.CreateRotationY(sceneObject.GetRotation().Y) *
                    Matrix.CreateRotationZ(sceneObject.GetRotation().Z) * Matrix.CreateTranslation(sceneObject.GetPosition().X,
                    sceneObject.GetPosition().Y, sceneObject.GetPosition().Z), viewMatrix, projectionMatrix, sceneObject.GetTexture2D());
            }

            // TODO: Add your drawing code here

        }


        private void ObjectInitializer()
        {
            //sceneObjectsArray.Add(new SceneObject(Content,new Vector3(0, 0, 2), "test", "StarSparrow_Green", "ShaderOne"));
            //sceneObjectsArray.Add(new SceneObject(Content,new Vector3(2.15f, 0, 0), "maja", "StarSparrow_Green", "ShaderOne"));
        }

        private void WorldCreator(float blockSeparation, float worldWidth, float worldLenght, string modelFileName, string textureFileName, string effectFileName)   //simple creator (develop it later)
        {
            float _blockSeparation = blockSeparation;
            int _worldWidth = (int)worldWidth/2;
            int _minusWidth = -_worldWidth;

            int _worldLenght = (int)worldLenght/2;
            int _minusLenght = -_worldLenght;
            float level = -4.0f;

            for(int i = _minusLenght; i <=_worldLenght; i++)
            {
                for(int j= _minusLenght; j<=_worldWidth; j++)
                {
                   sceneObjectsArray.Add(new SceneObject(Content,new Vector3(j*blockSeparation, level, i*blockSeparation), modelFileName, textureFileName, effectFileName));
                }
            }
        }

    }
}