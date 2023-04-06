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
        private BasicEffect basicEffect;

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
            Effect one;
            one = Content.Load<Effect>("ShaderOne");
            effectHandler = new EffectHandler(one);

            hud = new HUD("sky", WindowWidth, WindowHeight);

            //effectHandler.AddLight(new Vector3(0,0,0));

            world = new World(WindowWidth,WindowHeight,Content,2f,3,3,"test", "StarSparrow_Green");
            player = new Player(new Vector3(5,2,5), "player", "StarSparrow_Orange");
            
            

            serializator = new Serializator("zapis.txt");


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(Content);
            world.ObjectInitializer(Content);
            hud.LoadContent(Content);
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Projection = projectionMatrix;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            camera.CamPosition = player.GetPosition() + camera.CamPositionState;
            camera.nextpos = player.GetPosition();
            viewMatrix = Matrix.CreateLookAt(camera.CamPosition, camera.camTracker , Vector3.Up);
            //viewMatrix = Matrix.CreateLookAt(camera.CamPosition, player.GetPosition(), Vector3.Up);
            basicEffect.View = Matrix.CreateLookAt(camera.CamPosition, camera.camTracker, Vector3.Up);
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
                        * Matrix.CreateRotationX(sceneObject.GetRotation().X) * Matrix.CreateRotationY(sceneObject.GetRotation().Y) *
                        Matrix.CreateRotationZ(sceneObject.GetRotation().Z)
                        * Matrix.CreateTranslation(sceneObject.GetPosition().X,sceneObject.GetPosition().Y, sceneObject.GetPosition().Z)
                         , viewMatrix, projectionMatrix, sceneObject.GetTexture2D());

                
            }



           effectHandler.BasicDraw(player.GetModel(), worldMatrix * Matrix.CreateScale(player.GetScale())
                    * Matrix.CreateRotationX(player.GetRotation().X) * Matrix.CreateRotationY(player.GetRotation().Y) *
                    Matrix.CreateRotationZ(player.GetRotation().Z) * Matrix.CreateTranslation(player.GetPosition().X, player.GetPosition().Y, player.GetPosition().Z), 
                    viewMatrix, projectionMatrix, player.GetTexture2D());
            //Debug.Write(worldMatrix + "\n");


            hud.DrawFrontground(_spriteBatch);

            DrawBoundingBoxes();


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


        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }

        public void DrawBoundingBoxes()
        {
            basicEffect.CurrentTechnique.Passes[0].Apply();
            
            foreach(SceneObject obj in world.GetWorldList())
            {
                DrawBB(obj.boundingBox.GetCorners());
            }

            //DrawBB(player.boundingBox.GetCorners());

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
    }
}