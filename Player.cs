using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class Player
    {
        Vector3 position;
        private Vector3 rotation = new Vector3(0.0f, 0.0f, 0.0f);
        private float scale = 0.5f;
        private Model model;
        private Texture2D texture2D;
        private Effect effect;
        private string _modelFileName;
        private string _textureFileName;
        private string _effectFileName;
        private float movementSpeedUpDown = 0.2f;
        private float movementSpeedRightLeft = 0.1f;


        float rot;

        public Player(ContentManager content, Vector3 worldPosition, string modelFileName, string textureFileName, string effectFileName)
        {
            position = worldPosition;
            _modelFileName = modelFileName;
            _textureFileName = textureFileName;
            _effectFileName = effectFileName;

            SetModel(content.Load<Model>(modelFileName));
            SetTexture(content.Load<Texture2D>(textureFileName));
            SetEffect(content.Load<Effect>(effectFileName));
        }

        //GET'ERS
        public float GetmovementSpeedUpDown()
        {
            return movementSpeedUpDown;
        }
        public float GetmovementSpeedRightLeft()
        {
            return movementSpeedRightLeft;
        }
        public Vector3 GetPosition()
        {
            return position;
        }
        public Vector3 GetRotation()
        {
            return rotation;
        }
        public float GetScale()
        {
            return scale;
        }
        public Model GetModel()
        {
            return model;
        }
        public Texture2D GetTexture2D()
        {
            return texture2D;
        }
        public string GetModelFileName()
        {
            return _modelFileName;
        }
        public string GetTextureFileName()
        {
            return _textureFileName;
        }
        //.................

        //SET'ERS
        public void SetmovementSpeedUpDown(float speed)
        {
            movementSpeedUpDown = speed;
        }
        public void SetmovementSpeedRightLeft(float speed)
        {
            movementSpeedRightLeft = speed;
        }
        public void SetPosition(float x, float y, float z)
        {
            position = new Vector3(x, y, z);
        }
        public void SetPosition(Vector3 pos)
        {
            position = pos;
        }
        public void SetRotation(Vector3 rot)
        {
            rotation = rot;
        }
        public void SetRotation(float x, float y, float z)
        {
            rotation = new Vector3(x, y, z);
        }
        public void SetScale(float scl)
        {
            scale = scl;
        }
        public void SetModel(Model mod)
        {
            model = mod;
        }
        public void SetTexture(Texture2D tex)
        {
            texture2D = tex;
        }
        public void SetEffect(Effect eff)
        {
            effect = eff;
        }
        //.................

        public void DrawModelWithEffect(Model model, Matrix world, Matrix view, Matrix projection, Texture2D texture2D)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
                    effect.Parameters["WorldInverseTransposeMatrix"].SetValue(worldInverseTransposeMatrix);
                    //effect.Parameters["AmbienceColor"].SetValue(Color.Gray.ToVector4());      //uncomment if ambient needed
                    effect.Parameters["DiffuseColor"].SetValue(Color.White.ToVector4());
                    effect.Parameters["DiffuseLightPower"].SetValue(2);              // diffuse light power 
                    effect.Parameters["DiffuseLightDirection"].SetValue(new Vector3(2.0f, 4.0f, 1.0f));
                    effect.Parameters["ModelTexture"].SetValue(texture2D);
                    effect.Parameters["WorldMatrix"].SetValue(world * mesh.ParentBone.Transform);
                    effect.Parameters["ViewMatrix"].SetValue(view);
                    effect.Parameters["ProjectionMatrix"].SetValue(projection);

                }

                mesh.Draw();
            }
        }
        public void DrawModelWithEffect2(Model model, Matrix world, Matrix view, Matrix projection, Texture2D texture2D, Vector3 lightpos)
        {
            // point light (fire light?)
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform);
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["DiffuseColor"].SetValue((Color.DarkOrange.ToVector4() + Color.Yellow.ToVector4()) / 3);
                    effect.Parameters["AmbientColor"].SetValue(Color.White.ToVector4());
                    effect.Parameters["AmbientIntensity"].SetValue(0.3f);

                    effect.Parameters["DiffuseIntensity"].SetValue(3f);
                    effect.Parameters["ModelTexture"].SetValue(texture2D);
                    effect.Parameters["Attenuation"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
                    effect.Parameters["LightRange"].SetValue(20.0f);
                    effect.Parameters["LightPosition"].SetValue(lightpos + new Vector3(10f, 2f, 0f));

                    //Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * world));
                    //effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                }
                mesh.Draw();
            }
        }

        public Matrix PlayerMovement(World world, float cosAngle,float sinAngle,float tanAngle, Matrix worldMatrix)
        {
            //GameControler 
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if(capabilities.IsConnected)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                if (capabilities.HasLeftXThumbStick)
                {
                    
                    if (gamePadState.ThumbSticks.Left.X < -0.5f)
                    {
                        //SetRotation(0, gamePadState.ThumbSticks.Left.X * cosAngle, 0);
                       SetPosition(position - new Vector3(movementSpeedRightLeft, 0, -movementSpeedRightLeft * cosAngle));
                        worldMatrix = world.WorldMove(worldMatrix, new Vector3(movementSpeedRightLeft * 2, 0, -movementSpeedRightLeft * cosAngle * 2));
                    }
                    if (gamePadState.ThumbSticks.Left.X > 0.5f)
                    {
                       // SetRotation(0, gamePadState.ThumbSticks.Left.X * cosAngle * 3, 0);
                        SetPosition(position - new Vector3(-movementSpeedRightLeft, 0, movementSpeedRightLeft * cosAngle));
                        worldMatrix = world.WorldMove(worldMatrix, new Vector3(-movementSpeedRightLeft * 2, 0, movementSpeedRightLeft * cosAngle * 2));
                    }
                    if (gamePadState.ThumbSticks.Left.Y > 0.5f)
                    {
                        // SetRotation(0, gamePadState.ThumbSticks.Left.Y*cosAngle * 5.5f, 0);
                        SetPosition(position - new Vector3(movementSpeedUpDown / tanAngle, 0, movementSpeedUpDown));
                        worldMatrix = world.WorldMove(worldMatrix, new Vector3(movementSpeedUpDown / tanAngle * 2, 0, movementSpeedUpDown * 2));
                    }
                    if (gamePadState.ThumbSticks.Left.Y < -0.5f)
                    {
                       // SetRotation(0, gamePadState.ThumbSticks.Left.Y*cosAngle, 0);
                        SetPosition(position - new Vector3(-movementSpeedUpDown / tanAngle, 0, -movementSpeedUpDown));
                        worldMatrix = world.WorldMove(worldMatrix, new Vector3(-movementSpeedUpDown / tanAngle * 2, 0, -movementSpeedUpDown * 2));
                    }

                    // UPOŚLEDZONY RUCH KAMERĄ LEFT FUKIN THUMBSTICK
                    
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == 1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == -1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 1 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == 1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == 0)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 1 / 2, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == 1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == 1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 3 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == 0 && Math.Round(gamePadState.ThumbSticks.Left.Y) == 1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == -1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == 1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 5 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == -1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == 0)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 3 / 2, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == -1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == -1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI*7/4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == 0 && Math.Round(gamePadState.ThumbSticks.Left.Y) == -1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 0, 0);
                    }
                    //////////////////????////////////////////////////////////////
                    ///LEFT THUMBSTICK
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == 1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == -1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 1 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == 1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == 0)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 1 / 2, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == 1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == 1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 3 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == 0 && Math.Round(gamePadState.ThumbSticks.Right.Y) == 1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == -1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == 1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 5 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == -1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == 0)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 3 / 2, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == -1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == -1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 7 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == 0 && Math.Round(gamePadState.ThumbSticks.Right.Y) == -1)
                    {
                        SetRotation(0, ((float)Math.PI * 2f) + cosAngle + (float)Math.PI * 0, 0);
                    }
                    /////////////
                }
            }


            //Keyboard
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.A))
            {
                SetPosition(position - new Vector3(movementSpeedRightLeft, 0, -movementSpeedRightLeft * cosAngle));
               SetRotation(0, -cosAngle, 0);
                //Camera movement acccording to player A move 
                worldMatrix = world.WorldMove(worldMatrix, new Vector3(movementSpeedRightLeft*2, 0, -movementSpeedRightLeft * cosAngle*2));

            }
            if (state.IsKeyDown(Keys.D))
            {

                SetPosition(position - new Vector3(-movementSpeedRightLeft, 0, movementSpeedRightLeft * cosAngle));
                SetRotation(0, cosAngle*3, 0);
                //Camera movement acccording to player D move 
                worldMatrix = world.WorldMove(worldMatrix, new Vector3(-movementSpeedRightLeft*2, 0, movementSpeedRightLeft * cosAngle * 2));
            }
            if (state.IsKeyDown(Keys.W))
            {

                SetPosition(position - new Vector3(movementSpeedUpDown / tanAngle, 0, movementSpeedUpDown));
                SetRotation(0, cosAngle * 5.5f, 0);
                //Camera movement acccording to player W move 
                worldMatrix = world.WorldMove(worldMatrix, new Vector3(movementSpeedUpDown / tanAngle * 2, 0, movementSpeedUpDown * 2));
            }
            if (state.IsKeyDown(Keys.S))
            {

                SetPosition(position - new Vector3(-movementSpeedUpDown / tanAngle, 0, -movementSpeedUpDown));
                SetRotation(0, cosAngle, 0);
                //Camera movement acccording to player S move 
                worldMatrix = world.WorldMove(worldMatrix, new Vector3(-movementSpeedUpDown / tanAngle * 2, 0, -movementSpeedUpDown * 2));
            }

            return worldMatrix;
        }

        

    }
}
