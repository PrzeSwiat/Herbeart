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

        public Matrix KeyInput(World world, float cosAngle,float tanAngle, Matrix worldMatrix)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.A))
            {
                SetPosition(position - new Vector3(movementSpeedRightLeft, 0, -movementSpeedRightLeft * cosAngle));

                worldMatrix = world.WorldMove(worldMatrix, new Vector3(movementSpeedRightLeft, 0, -movementSpeedRightLeft * cosAngle));

            }
            if (state.IsKeyDown(Keys.D))
            {

                SetPosition(position - new Vector3(-movementSpeedRightLeft, 0, movementSpeedRightLeft * cosAngle));

                worldMatrix = world.WorldMove(worldMatrix, new Vector3(-movementSpeedRightLeft, 0, movementSpeedRightLeft * cosAngle));
            }
            if (state.IsKeyDown(Keys.W))
            {

                SetPosition(position - new Vector3(movementSpeedUpDown / tanAngle, 0, movementSpeedUpDown));

                worldMatrix = world.WorldMove(worldMatrix, new Vector3(movementSpeedUpDown / tanAngle, 0, movementSpeedUpDown));
            }
            if (state.IsKeyDown(Keys.S))
            {

                SetPosition(position - new Vector3(-movementSpeedUpDown / tanAngle, 0, -movementSpeedUpDown));

                worldMatrix = world.WorldMove(worldMatrix, new Vector3(-movementSpeedUpDown / tanAngle, 0, -movementSpeedUpDown));
            }

            return worldMatrix;
        }

        /*
        public void MouseMovement()
        {
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
            {

                if (mouseState.X < WindowWidth / 2 && mouseState.Y < WindowHeight / 2)
                {

                    // moveX = 2.5f*direction.X;
                    //moveZ = 4 * direction.Z;
                    //moveX = (float)Math.Tan(68 * (Math.PI / 180)) * direction.X;
                    //moveZ = (float)Math.Tan(76 * (Math.PI / 180)) * direction.Z;

                    Debug.Write("1" + "\n");
                }
                if (mouseState.X > WindowWidth / 2 && mouseState.Y < WindowHeight / 2)
                {

                    //moveX = -3f * direction.X;
                    //moveZ = 3f * direction.Z;
                    //moveX = -(float)Math.Tan(71 * (Math.PI / 180)) * direction.X;
                    //moveZ = (float)Math.Tan(71 * (Math.PI / 180)) * direction.Z;

                    Debug.Write("2" + "\n");
                }
                if (mouseState.X < WindowWidth / 2 && mouseState.Y > WindowHeight / 2)
                {
                    //moveX = 2.3f * direction.X;
                    //moveZ = -4.6f * direction.Z;
                    //moveX = (float)Math.Tan(65 * (Math.PI / 180)) * direction.X;
                    //moveZ = -(float)Math.Tan(78 * (Math.PI / 180)) * direction.Z;

                    Debug.Write("3" + "\n");
                }
                if (mouseState.X > WindowWidth / 2 && mouseState.Y > WindowHeight / 2)
                {
                    //moveX = -2.3f * direction.X;
                    //moveZ = -4f * direction.Z;
                    //moveX = -(float)Math.Tan(66 * (Math.PI / 180)) * direction.X;
                    //moveZ = -(float)Math.Tan(76 * (Math.PI / 180)) * direction.Z;

                    Debug.Write("4" + "\n");
                }

                //Debug.Write(WindowWidth/2 - state.X + " : " + (WindowHeight / 2 - state.Y) + "\n");
                //world.GetSceneObjectList()[world.GetSceneObjectList().Count-1].SetPosition(new Vector3(direction.X, 0, direction.Z));

                //Debug.Write(worldMatrix + "\n");
            }

            if (mouseState.RightButton == ButtonState.Released && prevMouseState.RightButton == ButtonState.Pressed)
            {

            }

        }
        */

    }
}
