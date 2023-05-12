﻿using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TheGame
{
    internal class PlayerMovement
    {
        private Player player;
        public PlayerMovement(Player player) { this.player = player; }

        public void UpdatePlayerMovement(World world, float deltaTime)
        {
            float rotation = 0;
            //GameControler 
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (capabilities.IsConnected)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                if (capabilities.HasLeftXThumbStick)
                {
                    double thumbLeftX = -Math.Round(gamePadState.ThumbSticks.Left.X);
                    double thumbLeftY = Math.Round(gamePadState.ThumbSticks.Left.Y);
                    float LeftjoystickX = -gamePadState.ThumbSticks.Left.X;
                    float LeftjoystickY = gamePadState.ThumbSticks.Left.Y;
                    float RightjoystickX = gamePadState.ThumbSticks.Right.X;
                    float RightjoystickY = gamePadState.ThumbSticks.Right.Y;

                    Vector2 w1 = new Vector2(0, -1);    // wektor wyjsciowy od ktorego obliczam kat czyli ten do dolu
                    Vector2 w2 = new Vector2(-LeftjoystickX, LeftjoystickY);

                    rotation = angle(w1, w2);
                    player.Direction = new Vector2(LeftjoystickX, LeftjoystickY);

                    // JUZ WCALE NIE UPOŚLEDZONY RUCH KAMERĄ LEFT FUKIN THUMBSTICK
                    if (thumbLeftX == 0 && thumbLeftY == 0)
                    {
                        player.Direction = new Vector2(0, 0);
                        rotation = player.GetRotation().Y;
                    }

                    ///Right THUMBSTICK
                    if (RightjoystickX != 0 || RightjoystickY != 0)
                    {
                        w2 = new Vector2(RightjoystickX, RightjoystickY);
                        rotation = angle(w1, w2);
                    }
                }
            }
            else
            {
                KeyboardState state = Keyboard.GetState();
                Boolean anyButtonClicked = false;

                player.Direction = new Vector2(0, 0);
                if (state.IsKeyDown(Keys.A))
                {
                    player.setDirectionX(1.0f);
                    anyButtonClicked = true;
                }
                if (state.IsKeyDown(Keys.D))
                {
                    player.setDirectionX(-1.0f);
                    anyButtonClicked = true;
                }
                if (state.IsKeyDown(Keys.W))
                {
                    player.setDirectionY(1.0f);
                    anyButtonClicked = true;
                }
                if (state.IsKeyDown(Keys.S))
                {
                    player.setDirectionY(-1.0f);
                    anyButtonClicked = true;
                }

                Vector2 w3 = new Vector2(0, -1);    // wektor wyjsciowy od ktorego obliczam kat czyli ten do dolu
                Vector2 w4 = new Vector2(-player.Direction.X, player.Direction.Y);

                if (!anyButtonClicked)
                {
                    rotation = player.GetRotation().Y;
                }
                else
                {
                    rotation = angle(w3, w4);
                }
            }
            float Sphereang = rotation - player.GetRotation().Y;
            rotateSphere(Sphereang);
            if (player.canMove)
            {
                Vector3 wec = new Vector3(player.Direction.X, 0, player.Direction.Y);
                //UpdateBB(0, world, new Vector3(player.Direction.X * deltaTime * player.MaxSpeed, 0, player.Direction.Y * deltaTime * player.MaxSpeed), wec);
                UpdateBB1(0, world, new Vector3(player.Direction.X * deltaTime * player.MaxSpeed, 0, 0), player.Direction);
                UpdateBB2(0, world, new Vector3(0, 0, player.Direction.Y * deltaTime * player.MaxSpeed), player.Direction);
            }

            player.SetRotation(0, rotation, 0);
        }

        public void UpdateBB(float ang, World world, Vector3 moveVec, Vector3 normal)
        {
            Vector3 center = ((player.boundingBox.Min + player.boundingBox.Max) / 2);
            // Przenieś punkt środka boxa do punktu (0, 0, 0)
            Matrix translation1 = Matrix.CreateTranslation(-center);
            Matrix rotation = Matrix.CreateRotationY(ang);
            Matrix translation2 = Matrix.CreateTranslation(center);
            Matrix transform = translation1 * rotation * translation2;
            Vector3[] vertices = player.boundingBox.GetCorners();

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

            player.boundingBox = new BoundingBox(min, max);

            BoundingBox boksik = player.boundingBox;
            player.boundingBox = new BoundingBox(player.boundingBox.Min - moveVec, player.boundingBox.Max - moveVec);
            // Wyznacz punkt środka boxa

            if (this.collision(world.GetWorldList()))
            {
                float movedDistance = CalculatePenetrationDepth(world);
                if (movedDistance != 0)
                {
                    movedDistance += 0.01f;
                    Debug.Write(CalculatePenetrationDepth(world) + "\n");
                    player.boundingBox.Min += movedDistance * normal;
                    player.boundingBox.Max += movedDistance * normal;
                    player.SetPosition((player.boundingBox.Min + player.boundingBox.Max) / 2);
                    //player.boundingSphere.Center = ;
                }
            }
            else
            {
                player.SetPosition(player.GetPosition() - moveVec);
                player.boundingSphere.Center -= moveVec;
            }


        }

        public void UpdateBB1(float ang, World world, Vector3 moveVec, Vector2 normal)
        {
            Vector3 center = ((player.boundingBox.Min + player.boundingBox.Max) / 2);
            // Przenieś punkt środka boxa do punktu (0, 0, 0)
            Matrix translation1 = Matrix.CreateTranslation(-center);
            Matrix rotation = Matrix.CreateRotationY(ang);
            Matrix translation2 = Matrix.CreateTranslation(center);
            Matrix transform = translation1 * rotation * translation2;
            Vector3[] vertices = player.boundingBox.GetCorners();

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

            player.boundingBox = new BoundingBox(min, max);

            BoundingBox boksik = player.boundingBox;
            player.boundingBox = new BoundingBox(player.boundingBox.Min - moveVec, player.boundingBox.Max - moveVec);
            // Wyznacz punkt środka boxa

            if (this.collision(world.GetWorldList()))
            {
                float movedDistance = CalculatePenetrationDepth(world);
                if (movedDistance != 0)
                {
                    movedDistance += 0.01f;
                    Debug.Write(CalculatePenetrationDepth(world) + "\n");
                    player.boundingBox.Min.X += movedDistance * normal.X;
                    player.boundingBox.Max.X += movedDistance * normal.X;
                    player.SetPosition((player.boundingBox.Min + player.boundingBox.Max) / 2);
                    //player.boundingSphere.Center = ;
                }
            } else
            {
                player.SetPosition(player.GetPosition() - moveVec);
                player.boundingSphere.Center -= moveVec;
            }
            

        }

        public void UpdateBB2(float ang, World world, Vector3 moveVec, Vector2 normal)
        {
            Vector3 center = ((player.boundingBox.Min + player.boundingBox.Max) / 2);
            // Przenieś punkt środka boxa do punktu (0, 0, 0)
            Matrix translation1 = Matrix.CreateTranslation(-center);
            Matrix rotation = Matrix.CreateRotationY(ang);
            Matrix translation2 = Matrix.CreateTranslation(center);
            Matrix transform = translation1 * rotation * translation2;
            Vector3[] vertices = player.boundingBox.GetCorners();

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

            player.boundingBox = new BoundingBox(min, max);

            BoundingBox boksik = player.boundingBox;
            player.boundingBox = new BoundingBox(player.boundingBox.Min - moveVec, player.boundingBox.Max - moveVec);
            // Wyznacz punkt środka boxa

            if (this.collision(world.GetWorldList()))
            {
                float movedDistance = CalculatePenetrationDepth(world);
                if (movedDistance != 0) 
                {
                    movedDistance += 0.01f;
                    player.boundingBox.Min.Z += movedDistance * normal.Y;
                    player.boundingBox.Max.Z += movedDistance * normal.Y;
                    player.SetPosition((player.boundingBox.Min + player.boundingBox.Max) / 2);
                }
                //player.boundingBox = boksik;
            }
            else
            {
                player.SetPosition(player.GetPosition() - moveVec);
                player.boundingSphere.Center -= moveVec;
            }


        }

        float CalculatePenetrationDepth(World world)
        {
            float penetrationDepth;
            float dx = 0, dz = 0;

            foreach (SceneObject obj in world.GetWorldList())
            {
                if (player.boundingBox.Intersects(obj.boundingBox))
                {
                    dx = Math.Min(player.boundingBox.Max.X - obj.boundingBox.Min.X, obj.boundingBox.Max.X - player.boundingBox.Min.X);
                    dz = Math.Min(player.boundingBox.Max.Z - obj.boundingBox.Min.Z, obj.boundingBox.Max.Z - player.boundingBox.Min.Z);
                }
            }
            
            penetrationDepth = Math.Min(dx, dz);
            
            return penetrationDepth;
        }

        public void rotateSphere(float Angle)
        {
            Matrix rotationMatrix = Matrix.CreateRotationY(Angle);
            player.boundingSphere.Center -= player.GetPosition();
            player.boundingSphere.Center = Vector3.Transform(player.boundingSphere.Center, rotationMatrix);
            player.boundingSphere.Center += player.GetPosition();
        }

        public bool collision(List<SceneObject> objects)
        {
            foreach (SceneObject obj in objects)
            {
                if (player.boundingBox.Intersects(obj.boundingBox))
                {
                    return true;
                }
            }
            return false;
        }

        public static float angle(Vector2 a, Vector2 b)
        {
            float dot = dotProduct(a, b);
            float det = a.X * b.Y - a.Y * b.X;
            return (float)Math.Atan2(det, dot);
        }

        public static float dotProduct(Vector2 vector, Vector2 vector1)
        {
            float result = vector.X * vector1.X + vector.Y * vector1.Y;

            return result;
        }

    }
}
