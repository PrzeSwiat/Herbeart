using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace TheGame
{
    [Serializable]
    internal class Player : SceneObject
    {
        private float movementSpeedUpDown;
        private float movementSpeedRightLeft = 0.15f;
        private int lastrotationtochcek = 0;
        public Player(Vector3 Position, string modelFileName, string textureFileName) : base(Position,modelFileName,textureFileName)
        {
            movementSpeedUpDown = movementSpeedRightLeft * 2;
            SetScale(1f);

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

        //.................


        public void PlayerMovement(World world, float cosAngle,float sinAngle,float tanAngle)
        {

          //position += new Vector3(0,-0.1f,0);


            //GameControler 
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if(capabilities.IsConnected)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                if (capabilities.HasLeftXThumbStick)
                {
                    
                    if (gamePadState.ThumbSticks.Left.X < -0.5f)
                    {
                       SetPosition(position - new Vector3(movementSpeedRightLeft, 0, -movementSpeedRightLeft * cosAngle));
                        
                    }
                    if (gamePadState.ThumbSticks.Left.X > 0.5f)
                    {
                        SetPosition(position - new Vector3(-movementSpeedRightLeft, 0, movementSpeedRightLeft * cosAngle));
                        
                    }
                    if (gamePadState.ThumbSticks.Left.Y > 0.5f)
                    {
                        SetPosition(position - new Vector3(movementSpeedUpDown / tanAngle, 0, movementSpeedUpDown));
                        
                    }
                    if (gamePadState.ThumbSticks.Left.Y < -0.5f)
                    {
                        SetPosition(position - new Vector3(-movementSpeedUpDown / tanAngle, 0, -movementSpeedUpDown));
                        
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
                    ///Right THUMBSTICK
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
                    ///

                }
            }

            //Debug.Write(collision(world.GetWorldList()));
            //Debug.Write(boundingBox + "\n");

            //Keyboard
            float angleLeft = (float)Math.PI * 3 / 2;
            float angleRight = (float)Math.PI * 1 / 2;
            float angleUp = (float)Math.PI;
            float angleDown = 0;
            float lastcount = this.rotation.Y;

            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.A))
            {
                float ang = angleLeft - lastcount;
                if (lastrotationtochcek == 1) ang = 0.0f;
                Vector3 left = new Vector3(movementSpeedRightLeft, 0, 0);
                
                UpdateBB(ang, world, left);

                SetRotation(0,angleLeft,0);
                lastrotationtochcek = 1;
            }
            if (state.IsKeyDown(Keys.D))
            {

                float ang = angleRight - lastcount;
                if (lastrotationtochcek == 2) ang = 0.0f;
                Vector3 right = new Vector3(-movementSpeedRightLeft, 0, 0);
                UpdateBB(ang, world, right);

                SetRotation(0, angleRight, 0);

                lastrotationtochcek = 2;
                //Camera movement acccording to player D move 
            }
            if (state.IsKeyDown(Keys.W))
            {
                float ang = angleUp - lastcount;
                if (lastrotationtochcek == 3) ang = 0.0f;

                Vector3 up = new Vector3(0, 0, movementSpeedUpDown);
                UpdateBB(ang, world, up);

                lastrotationtochcek = 3;
                SetRotation(0, angleUp, 0);


                //Camera movement acccording to player W move 
            }
            if (state.IsKeyDown(Keys.S))
            {
                float ang = angleDown - lastcount;
                if (lastrotationtochcek == 4) ang = 0.0f;

                Vector3 down = new Vector3(0, 0, -movementSpeedUpDown);
                UpdateBB(ang, world, down);

                SetRotation(0, angleDown, 0);
                lastrotationtochcek = 4;
                //Camera movement acccording to player S move 
            }

        }
        public bool collision(List<SceneObject> objects)
        {

            foreach (SceneObject obj in objects)
            {
                if (this.boundingBox.Intersects(obj.boundingBox))
                {
                    return true;
                }


            }
            return false;
        }

        public Matrix UpdateBB(float ang, World world, Vector3 moveVec)
        {
            Vector3 center = new Vector3((boundingBox.Min.X + boundingBox.Max.X) / 2, (boundingBox.Min.Y + boundingBox.Max.Y) / 2, (boundingBox.Min.Z + boundingBox.Max.Z) / 2);
            // Przenieś punkt środka boxa do punktu (0, 0, 0)
            Matrix translation4 = Matrix.CreateTranslation(moveVec);
            Matrix translation1 = Matrix.CreateTranslation(-center);
            Matrix rotation = Matrix.CreateRotationY(ang);
            Matrix translation2 = Matrix.CreateTranslation(center);
            Matrix translation3 = Matrix.CreateTranslation(moveVec);
            Matrix transform = translation1 * rotation * translation2;
            Vector3 min = Vector3.Transform(boundingBox.Min, transform);
            Vector3 max = Vector3.Transform(boundingBox.Max, transform);


            if (min.X > max.X)
            {

                float bufor = min.X;
                min.X = max.X;
                max.X = bufor;
            }
            if (min.Y > max.Y)
            {
                float bufor = min.Y;
                min.Y = max.Y;
                max.Y = bufor;
            }
            if (min.Z > max.Z)
            {
                float bufor = min.Z;
                min.Z = max.Z;
                max.Z = bufor;
            }

            boundingBox = new BoundingBox(min, max);
            //Debug.Write(ang);
            BoundingBox bb = new BoundingBox(boundingBox.Min - moveVec, boundingBox.Max - moveVec);
            // Wyznacz punkt środka boxa

            lastrotationtochcek = 1;
            //Debug.Write(bb.Min + " " + bb.Max + "\n");
            BoundingBox boksik = boundingBox;
            boundingBox = bb;

            if (!(this.collision(world.GetWorldList())))
            {
                SetPosition(position - moveVec);

            }
            else
            {
                boundingBox = boksik;

            }

            return transform;
        }



    }
}
