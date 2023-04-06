using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
    internal class Player : Creature
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

                    float lastcount = this.boundingboxrotation.Y;
                    // UPOŚLEDZONY RUCH KAMERĄ LEFT FUKIN THUMBSTICK       
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == 1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == -1) //prawy dol
                    {
                        this.UpdateBB((float)Math.PI * 1 / 2 - lastcount,world ,new Vector3(-movementSpeedRightLeft, 0, -movementSpeedUpDown));
                        SetRotation(0,(float)Math.PI * 1 / 4, 0);
                        boundingboxrotation = new Vector3(0, (float)Math.PI * 1 / 2, 0);
                    }
                    
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == 1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == 0) // prawo
                    {
                        this.UpdateBB((float)Math.PI * 1 / 2 - lastcount, world, new Vector3(-movementSpeedRightLeft, 0, 0));
                        SetRotation(0, (float)Math.PI * 1 / 2, 0);
                        boundingboxrotation = new Vector3(0, (float)Math.PI * 1 / 2, 0);
                    } 
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == 1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == 1) //prawa gora
                    {
                        this.UpdateBB((float)Math.PI * 1 / 2 - lastcount, world, new Vector3(-movementSpeedRightLeft, 0, movementSpeedUpDown));
                        SetRotation(0, (float)Math.PI * 3 / 4, 0);
                        boundingboxrotation = new Vector3(0, (float)Math.PI * 1 / 2, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == 0 && Math.Round(gamePadState.ThumbSticks.Left.Y) == 1) // gora
                    {
                        this.UpdateBB((float)Math.PI - lastcount, world, new Vector3(0, 0, movementSpeedUpDown));
                        SetRotation(0,(float)Math.PI, 0);
                        boundingboxrotation = new Vector3(0, (float)Math.PI, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == -1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == 1) //lewa gora
                    {
                        this.UpdateBB((float)Math.PI * 3 / 2 - lastcount, world, new Vector3(movementSpeedRightLeft, 0, movementSpeedUpDown));
                        SetRotation(0, (float)Math.PI * 5 / 4, 0);
                        boundingboxrotation = new Vector3(0, (float)Math.PI * 3 / 2, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == -1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == 0) //lewo 
                    {
                        this.UpdateBB((float)Math.PI * 3 / 2 - lastcount, world, new Vector3(movementSpeedRightLeft, 0, 0));
                        SetRotation(0, (float)Math.PI * 3 / 2, 0);
                        boundingboxrotation = new Vector3(0, (float)Math.PI * 3 / 2, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == -1 && Math.Round(gamePadState.ThumbSticks.Left.Y) == -1) //lewy dol
                    {
                        this.UpdateBB((float)Math.PI * 3 / 2- lastcount, world, new Vector3(movementSpeedRightLeft, 0, -movementSpeedUpDown));
                        SetRotation(0, (float)Math.PI * 7 / 4, 0);
                        boundingboxrotation = new Vector3(0, (float)Math.PI * 1 / 2, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Left.X) == 0 && Math.Round(gamePadState.ThumbSticks.Left.Y) == -1)// dol
                    {
                        this.UpdateBB(0 - lastcount, world, new Vector3(0, 0, -movementSpeedUpDown));
                        SetRotation(0, 0, 0);
                    }
                    //////////////////????////////////////////////////////////////
                    ///Right THUMBSTICK
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == 1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == -1)
                    {
                        SetRotation(0,  (float)Math.PI * 1 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == 1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == 0)
                    {
                        SetRotation(0, (float)Math.PI * 1 / 2, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == 1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == 1)
                    {
                        SetRotation(0, (float)Math.PI * 3 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == 0 && Math.Round(gamePadState.ThumbSticks.Right.Y) == 1)
                    {
                        SetRotation(0,  (float)Math.PI, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == -1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == 1)
                    {
                        SetRotation(0, (float)Math.PI * 5 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == -1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == 0)
                    {
                        SetRotation(0, (float)Math.PI * 3 / 2, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == -1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == -1)
                    {
                        SetRotation(0, (float)Math.PI * 7 / 4, 0);
                    }
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == 0 && Math.Round(gamePadState.ThumbSticks.Right.Y) == -1)
                    {
                        SetRotation(0, (float)Math.PI * 0, 0);
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
           

            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.A))
            {
                float ang = angleLeft - this.GetRotation().Y;
                if (lastrotationtochcek == 1) ang = 0.0f;
                Vector3 left = new Vector3(movementSpeedRightLeft, 0, 0);
                
                UpdateBB(ang, world, left);

                SetRotation(0,angleLeft,0);
                lastrotationtochcek = 1;
            }
            if (state.IsKeyDown(Keys.D))
            {

                float ang = angleRight - this.GetRotation().Y;
                if (lastrotationtochcek == 2) ang = 0.0f;
                Vector3 right = new Vector3(-movementSpeedRightLeft, 0, 0);
                UpdateBB(ang, world, right);

                SetRotation(0, angleRight, 0);

                lastrotationtochcek = 2;
                //Camera movement acccording to player D move 
            }
            if (state.IsKeyDown(Keys.W))
            {
                float ang = angleUp - this.GetRotation().Y;
                if (lastrotationtochcek == 3) ang = 0.0f;

                Vector3 up = new Vector3(0, 0, movementSpeedUpDown);
                UpdateBB(ang, world, up);

                lastrotationtochcek = 3;
                SetRotation(0, angleUp, 0);


                //Camera movement acccording to player W move 
            }
            if (state.IsKeyDown(Keys.S))
            {
                float ang = angleDown - this.GetRotation().Y;
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

        public void UpdateBB(float ang, World world, Vector3 moveVec)
        {
            Vector3 center = ((boundingBox.Min + boundingBox.Max) / 2);
            // Przenieś punkt środka boxa do punktu (0, 0, 0)
            Matrix translation1 = Matrix.CreateTranslation(-center);
            Matrix rotation = Matrix.CreateRotationY(ang);
            Matrix translation2 = Matrix.CreateTranslation(center);
            Matrix transform = translation1 * rotation * translation2;
            Vector3[] vertices = boundingBox.GetCorners();

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Vector3.Transform(vertices[i], transform);
            }
            Vector3 min = vertices[0];
            Vector3 max = vertices[0];
            for (int i = 1; i < vertices.Length; i++)
            {
                min = Vector3.Min(min, vertices[i]);
                max = Vector3.Max(max, vertices[i]);
            }

            boundingBox = new BoundingBox(min, max);

            //boundingBox =  BoundingBox.CreateFromPoints(vertices);

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

        }
        



    }
}
