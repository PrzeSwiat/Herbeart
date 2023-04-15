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
        public List<Leaf> inventory;
        public Player(Vector3 Position, string modelFileName, string textureFileName) : base(Position, modelFileName, textureFileName)
        {
            movementSpeedUpDown = movementSpeedRightLeft * 2;
            SetScale(1.7f);

            AssignParameters(200, 20, 3);
        }

        public void getDamage(int amount)
        {
            Health -= amount;
            if (Health < 0) { Health = 0; }
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


        public void PlayerMovement(World world, float cosAngle, float sinAngle, float tanAngle)
        {
            //GameControler 
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (capabilities.IsConnected)
            {

                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                if (capabilities.HasLeftXThumbStick)
                {
                    float lastcount = this.boundingboxrotation.Y;

                    double thumbLeftX = Math.Round(gamePadState.ThumbSticks.Left.X);
                    double thumbLeftY = Math.Round(gamePadState.ThumbSticks.Left.Y);
                    float rotation = 0;
                    float rotationBB = 0;
                    float Sphereang = 0;

                    // UPOŚLEDZONY RUCH KAMERĄ LEFT FUKIN THUMBSTICK
                    if (thumbLeftX == 0 && thumbLeftY == 0) 
                    {
                        Direction = new Vector2(0, 0);
                        rotation = this.GetRotation().Y;
                        Sphereang = 0;
                    }
                    if (thumbLeftX == 1 && thumbLeftY == -1) //prawy dol
                    {
                        Direction = new Vector2(1, -1);
                        Sphereang = (float)Math.PI * 1 / 4 - this.GetRotation().Y;
                        rotation = (float)Math.PI * 1 / 4;
                        rotationBB = (float)Math.PI * 1 / 2;
                        boundingboxrotation = new Vector3(0, (float)Math.PI * 1 / 2, 0);
                    }

                    if (thumbLeftX == 1 && thumbLeftY == 0) // prawo
                    {
                        Direction = new Vector2(1, 0);
                        Sphereang = (float)Math.PI * 1 / 2 - this.GetRotation().Y;
                        rotation = (float)Math.PI * 1 / 2;
                        rotationBB = (float)Math.PI * 1 / 2;
                        boundingboxrotation = new Vector3(0, (float)Math.PI * 1 / 2, 0);
                    }
                    if (thumbLeftX == 1 && thumbLeftY == 1) //prawa gora
                    {
                        Direction = new Vector2(1, 1);
                        Sphereang = (float)Math.PI * 3 / 4 - this.GetRotation().Y;
                        rotation = (float)Math.PI * 3 / 4;
                        rotationBB = (float)Math.PI * 1 / 2;
                        boundingboxrotation = new Vector3(0, (float)Math.PI * 1 / 2, 0);
                    }
                    if (thumbLeftX == 0 && thumbLeftY == 1) // gora
                    {
                        Direction = new Vector2(0, 1);
                        Sphereang = (float)Math.PI - this.GetRotation().Y;
                        rotation = (float)Math.PI;
                        rotationBB = (float)Math.PI;
                        boundingboxrotation = new Vector3(0, (float)Math.PI, 0);
                    }
                    if (thumbLeftX == -1 && thumbLeftY == 1) //lewa gora
                    {
                        Direction = new Vector2(-1, 1);
                        Sphereang = (float)Math.PI * 5 / 4 - this.GetRotation().Y;
                        rotation = (float)Math.PI * 5 / 4;
                        rotationBB = (float)Math.PI * 3 / 2;
                        boundingboxrotation = new Vector3(0, (float)Math.PI * 3 / 2, 0);
                    }
                    if (thumbLeftX == -1 && thumbLeftY == 0) //lewo 
                    {
                        Direction = new Vector2(-1, 0);
                        Sphereang = (float)Math.PI * 3 / 2 - this.GetRotation().Y;
                        rotation = (float)Math.PI * 3 / 2;
                        rotationBB = (float)Math.PI * 3 / 2;
                        boundingboxrotation = new Vector3(0, (float)Math.PI * 3 / 2, 0);
                    }
                    if (thumbLeftX == -1 && thumbLeftY == -1) //lewy dol
                    {
                        Direction = new Vector2(-1, -1);
                        Sphereang = (float)Math.PI * 7 / 4 - this.GetRotation().Y;
                        rotation = (float)Math.PI * 7 / 4;
                        rotationBB = (float)Math.PI * 3 / 2;
                        boundingboxrotation = new Vector3(0, (float)Math.PI * 1 / 2, 0);
                    }
                    if (thumbLeftX == 0 && thumbLeftY == -1) // dol
                    {
                        Direction = new Vector2(0, -1);
                        Sphereang = 0 - this.GetRotation().Y;
                        rotation = 0;
                        boundingboxrotation = new Vector3(0, 0, 0);
                        rotationBB = 0;
                    }

                    //////////////////????////////////////////////////////////////
                    ///Right THUMBSTICK
                    if (Math.Round(gamePadState.ThumbSticks.Right.X) == 1 && Math.Round(gamePadState.ThumbSticks.Right.Y) == -1)
                    {
                        SetRotation(0, (float)Math.PI * 1 / 4, 0);
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
                        SetRotation(0, (float)Math.PI, 0);
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
                    //Vector2 dr = Direction;
                    //dr.Normalize();
                    rotateSphere(Sphereang);
                    this.UpdateBB(0, world, new Vector3(-Direction.X * 0.2f, 0, 0));
                    this.UpdateBB(0, world, new Vector3(0, 0, Direction.Y * 0.2f));
                    SetRotation(0, rotation, 0);

                }
            }

            //Keyboard
            float angleLeft = (float)Math.PI * 3 / 2;
            float angleRight = (float)Math.PI * 1 / 2;
            float angleUp = (float)Math.PI;
            float angleDown = 0;

            KeyboardState state = Keyboard.GetState();
            float ang = 0;
            Direction = new Vector2(0, 0);
            if (state.IsKeyDown(Keys.A))
            {
                ang = angleLeft - this.GetRotation().Y;
                if (lastrotationtochcek == 1) ang = 0.0f;
                setDirectionX(1.0f);
                SetRotation(0, angleLeft, 0);
                lastrotationtochcek = 1;
            }
            if (state.IsKeyDown(Keys.D))
            {
                ang = angleRight - this.GetRotation().Y;
                if (lastrotationtochcek == 2) ang = 0.0f;

                setDirectionX(-1);
                SetRotation(0, angleRight, 0);
                lastrotationtochcek = 2;
                //Camera movement acccording to player D move 
            }
            if (state.IsKeyDown(Keys.W))
            {
                ang = angleUp - this.GetRotation().Y;
                if (lastrotationtochcek == 3) ang = 0.0f;
                setDirectionY(1);
                lastrotationtochcek = 3;
                SetRotation(0, angleUp, 0);

                //Camera movement acccording to player W move 
            }
            if (state.IsKeyDown(Keys.S))
            {
                ang = angleDown - this.GetRotation().Y;
                if (lastrotationtochcek == 4) ang = 0.0f;
                setDirectionY(-1);
                SetRotation(0, angleDown, 0);
                lastrotationtochcek = 4;
                //Camera movement acccording to player S move 
            }
            //NormalizeDirection();
            UpdateBB(ang, world, new Vector3(Direction.X * 0.2f, 0, Direction.Y * 0.2f));

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

            BoundingBox bb = new BoundingBox(boundingBox.Min - moveVec, boundingBox.Max - moveVec);
            // Wyznacz punkt środka boxa

            lastrotationtochcek = 1;
            BoundingBox boksik = boundingBox;
            boundingBox = bb;

            if (!(this.collision(world.GetWorldList())))
            {
                
                SetPosition(GetPosition() - moveVec);
                this.boundingSphere.Center -= moveVec;

            } 
            else
            {
                boundingBox = boksik;

            }

        }
        public void AddLeaf(Leaf leaf)
        {
            inventory.Add(leaf);
        }

        public void RemoveLeaf(Leaf leaf)
        {
            inventory.Remove(leaf);
        }

        public void rotateSphere(float Angle)
        {
            Matrix rotationMatrix = Matrix.CreateRotationY(Angle);
            boundingSphere.Center -= this.GetPosition();
            boundingSphere.Center = Vector3.Transform(boundingSphere.Center, rotationMatrix);
            boundingSphere.Center += this.GetPosition();
        }


    }
}
