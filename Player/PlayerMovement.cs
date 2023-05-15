using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TheGame
{
    internal class PlayerMovement
    {
        private Player player;
        private Boolean isCraftingTea;
        private Boolean padButtonAClicked;
        private Boolean padButtonBClicked;
        private Boolean padButtonXClicked;
        private Boolean padButtonYClicked;

        MouseState lastMouseState, currentMouseState;

        public PlayerMovement(Player player) 
        { 
            this.player = player;
            isCraftingTea = false;
            padButtonAClicked = false;
            padButtonYClicked = false;
            padButtonXClicked = false;
        }

        public void UpdatePlayerMovement(World world, float deltaTime)
        {
            float rotation = 0;
            //GameControler 
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (capabilities.IsConnected)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                GamePadClick(capabilities, gamePadState);

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
                    player.NormalizeDirection();

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

                lastMouseState = currentMouseState;
                // Get the mouse state relevant for this frame
                currentMouseState = Mouse.GetState();

                // Recognize a single click of the left mouse button
                if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    player.Attack();
                }

                
            }
            float Sphereang = rotation - player.GetRotation().Y;
            rotateSphere(Sphereang);
            if (player.getcanMove())
            {
                Vector3 wec = new Vector3(player.Direction.X, 0, player.Direction.Y);
                UpdateBB(0, world.GetWorldList(), new Vector3(player.Direction.X * deltaTime * player.ActualSpeed, 0, player.Direction.Y * deltaTime * player.ActualSpeed), wec);

            }

            player.SetRotation(0, rotation, 0);
        }

        public void UpdateBB(float ang, List<SceneObject> worldList, Vector3 moveVec, Vector3 normal)
        {
            Vector3 center = ((player.boundingBox.Min + player.boundingBox.Max) / 2);
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


            // check kolizji na X
            BoundingBox boksik = player.boundingBox;
            Vector3 helpVec = new Vector3(moveVec.X, 0, 0);
            player.boundingBox = new BoundingBox(player.boundingBox.Min - helpVec, player.boundingBox.Max - helpVec);
            // Wyznacz punkt środka boxa

            if (this.collision(worldList))
            {
                player.boundingBox = boksik;
                /*float movedDistance = CalculatePenetrationDepthX(worldList);
                if (movedDistance != 0)
                {
                    movedDistance += 0.01f;
                    player.boundingBox.Min.X += movedDistance * normal.X;
                    player.boundingBox.Max.X += movedDistance * normal.X;
                    player.SetPosition((player.boundingBox.Min + player.boundingBox.Max) / 2);

                    helpVec.X -= movedDistance * normal.X;
                    player.boundingSphere.Center -= helpVec;
                }*/
            }
            else
            {
                player.SetPosition(player.GetPosition() - helpVec);
                player.boundingSphere.Center -= helpVec;
            }

            // check kolizji na Z
            boksik = player.boundingBox;
            helpVec = new Vector3(0, 0, moveVec.Z);
            player.boundingBox = new BoundingBox(player.boundingBox.Min - helpVec, player.boundingBox.Max - helpVec);

            if (this.collision(worldList))
            {
                player.boundingBox = boksik;
                /*float movedDistance = CalculatePenetrationDepthZ(worldList);
                if (movedDistance != 0)
                {
                    movedDistance += 0.01f;
                    player.boundingBox.Min.Z += movedDistance * normal.Z;
                    player.boundingBox.Max.Z += movedDistance * normal.Z;
                    player.SetPosition((player.boundingBox.Min + player.boundingBox.Max) / 2);
                    helpVec.Z -= movedDistance * normal.Z;
                    player.boundingSphere.Center -= helpVec;
                }*/
            }
            else
            {
                player.SetPosition(player.GetPosition() - helpVec);
                player.boundingSphere.Center -= helpVec;
            }



        }

        public void GamePadClick(GamePadCapabilities capabilities, GamePadState gamePadState)
        {
            if (capabilities.IsConnected)
            {
                // Button A
                if (capabilities.HasAButton)
                {
                    if (gamePadState.IsButtonDown(Buttons.A))
                    {
                        if (isCraftingTea && !padButtonAClicked) // W momencie jak lewy triger jest wcisniety
                        {
                            player.AddIngredientA();
                            padButtonAClicked = true;

                        }
                        else // normalny atak gracza
                        {
                            player.Attack();
                        }

                    }
                    else if (gamePadState.IsButtonUp(Buttons.A))
                    {
                        padButtonAClicked = false;
                    }
                }

                // Button B
                if (capabilities.HasBButton)
                {
                    if (gamePadState.IsButtonDown(Buttons.B))
                    {
                        if (isCraftingTea && !padButtonBClicked)
                        {
                            player.AddIngredientB();
                            padButtonBClicked = true;
                        }
                        else
                        {
                            // tutaj ewentualny dash/przewrot
                        }

                    }
                    else if (gamePadState.IsButtonUp(Buttons.B))
                    {
                        padButtonBClicked = false;
                    }
                }
                if (capabilities.HasYButton)
                {
                    if (gamePadState.IsButtonDown(Buttons.Y))
                    {
                        if (isCraftingTea && !padButtonYClicked)
                        {
                            player.AddIngredientY();
                            padButtonYClicked = true;
                        }
                        else
                        {
                            // tutaj ewentualny dash/przewrot
                        }

                    }
                    else if (gamePadState.IsButtonUp(Buttons.Y))
                    {
                        padButtonYClicked = false;
                    }
                }
                if (capabilities.HasXButton)
                {
                    if (gamePadState.IsButtonDown(Buttons.X))
                    {
                        if (isCraftingTea && !padButtonXClicked)
                        {
                            player.AddIngredientX();
                            padButtonXClicked = true;
                        }
                        else
                        {
                            // tutaj ewentualny dash/przewrot
                        }

                    }
                    else if (gamePadState.IsButtonUp(Buttons.X))
                    {
                        padButtonXClicked = false;
                    }
                }


                if (gamePadState.IsButtonDown(Buttons.LeftTrigger))
                {
                    isCraftingTea = true;
                }
                else if (gamePadState.IsButtonUp(Buttons.LeftTrigger))
                {
                    this.player.Crafting.cleanRecepture(this.player.Inventory);
                    isCraftingTea = false;
                }




            }
            /*else          DO STEROWANIA WSAD
            {
                lastMouseState = currentMouseState;

                // Get the mouse state relevant for this frame
                currentMouseState = Mouse.GetState();

                // Recognize a single click of the left mouse button
                if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    player.Attack();
                }

            }*/
        }

        float CalculatePenetrationDepthX(List<SceneObject> worldList)
        {
            float penetrationDepth = 0;
            foreach (SceneObject obj in worldList)
            {
                if (player.boundingBox.Intersects(obj.boundingBox))
                {
                    penetrationDepth = Math.Min(player.boundingBox.Max.X - obj.boundingBox.Min.X, obj.boundingBox.Max.X - player.boundingBox.Min.X);
                  
                }
            }
            
            return penetrationDepth;
        }

        float CalculatePenetrationDepthZ(List<SceneObject> worldList)
        {
            float penetrationDepth = 0;

            foreach (SceneObject obj in worldList)
            {
                if (player.boundingBox.Intersects(obj.boundingBox))
                {
                    penetrationDepth = Math.Min(player.boundingBox.Max.Z - obj.boundingBox.Min.Z, obj.boundingBox.Max.Z - player.boundingBox.Min.Z);
                }
            }

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
