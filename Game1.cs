using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

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
            camPosition = new Vector3(0f, 20f, 20f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45f),
                GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f); // render range (from 1 near playing to 1000 far playing)

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up); // tells the world of our orientantion
                                                                                  // (the same as:
                                                                                  // Vector3(0,1,0) - up and down is along y axis)
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);
            //.................

            //ObjectInitializer();
            WorldCreator(2.15f,4,4,"maja", "StarSparrow_Green");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            foreach(SceneObject sceneObject in sceneObjectsArray)
            {
                sceneObject.SetModel(Content.Load<Model>(sceneObject.GetModelFileName()));
                sceneObject.SetTexture(Content.Load<Texture2D>(sceneObject.GetTextureFileName()));
            }
            Debug.WriteLine(sceneObjectsArray.Count);
            effect = Content.Load<Effect>("ShaderOne");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

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
                DrawModelWithEffect(sceneObject.GetModel(),worldMatrix * Matrix.CreateScale(sceneObject.GetScale()) *
                    Matrix.CreateRotationX(sceneObject.GetRotation().X) * Matrix.CreateRotationY(sceneObject.GetRotation().Y) *
                    Matrix.CreateRotationZ(sceneObject.GetRotation().Z) * Matrix.CreateTranslation(sceneObject.GetPosition().X,
                    sceneObject.GetPosition().Y, sceneObject.GetPosition().Z), viewMatrix, projectionMatrix, sceneObject.GetTexture2D());
            }

            // TODO: Add your drawing code here

        }

        private void DrawModelWithEffect(Model model, Matrix world, Matrix view, Matrix projection, Texture2D texture2D)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
                    effect.Parameters["WorldInverseTransposeMatrix"].SetValue(worldInverseTransposeMatrix);
                    //effect.Parameters["AmbienceColor"].SetValue(Color.Gray.ToVector4());      //uncomment if needed
                    effect.Parameters["DiffuseColor"].SetValue(Color.White.ToVector4());
                    effect.Parameters["DiffuseLightPower"].SetValue(1.5f);
                    effect.Parameters["DiffuseLightDirection"].SetValue(new Vector3(1.0f, 1.0f, 1.0f));
                    effect.Parameters["ModelTexture"].SetValue(texture2D);
                    effect.Parameters["WorldMatrix"].SetValue(world * mesh.ParentBone.Transform);
                    effect.Parameters["ViewMatrix"].SetValue(view);
                    effect.Parameters["ProjectionMatrix"].SetValue(projection);

                }

                mesh.Draw();
            }
        }

        private void ObjectInitializer()
        {
            //sceneObjectsArray.Add(new SceneObject(new Vector3(0, 0, 0), "maja", "StarSparrow_Green"));
            //sceneObjectsArray.Add(new SceneObject(new Vector3(2.15f, 0, 0), "maja", "StarSparrow_Green"));
        }

        private void WorldCreator(float blockSeparation, float worldWidth, float worldLenght, string modelFileName, string textureFileName)   //simple creator (develop it later)
        {
            float _blockSeparation = blockSeparation;
            int _worldWidth = (int)worldWidth/2;
            int _minusWidth = -_worldWidth;

            int _worldLenght = (int)worldLenght/2;
            int _minusLenght = -_worldLenght;
            float level = 0.0f;

            for(int i = _minusLenght; i <=_worldLenght; i++)
            {
                for(int j= _minusLenght; j<=_worldWidth; j++)
                {
                    sceneObjectsArray.Add(new SceneObject(new Vector3(j*blockSeparation, level, i*blockSeparation), modelFileName, textureFileName));
                }
            }
        }

    }
}