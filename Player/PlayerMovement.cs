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
        public Boolean isCraftingTea;
        public Boolean isThrowing;
        private Boolean padButtonAClicked;
        private Boolean padButtonBClicked;
        private Boolean padButtonXClicked;
        private Boolean padButtonYClicked;
        private Boolean leftShoulderClicked;
        private Boolean rightShoulderClicked;
        private Boolean rightStickClicked;

        public bool isMoving;

        MouseState lastMouseState, currentMouseState;

        private float dashTime = 0.2f, elapsedDashTime = 0;     // dash
        private float timeToDash = 2, elapsedLastDashTime = 3;  // dash delay
        private float dashSpeed = 50f;
        private float deltaTime;
        private Vector2 dashDirection;
        bool isDashing = false;
        bool canDash = true;

        public PlayerMovement(Player player) 
        { 
            this.player = player;
            isCraftingTea = false;
            isThrowing = false;
            padButtonAClicked = false;
            padButtonBClicked = false;
            padButtonYClicked = false;
            padButtonXClicked = false;
            leftShoulderClicked = false;
            rightShoulderClicked = false;
            rightStickClicked = false;
    }

        public void UpdatePlayerMovement(World world, float deltaTime)
        {
            float rotation = 0;
            this.deltaTime = deltaTime;
            elapsedLastDashTime += this.deltaTime;
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
                    if (player.Direction.X != 0 || player.Direction.Y != 0)
                    {
                        isMoving = true;
                    }
                    else
                    {
                        isMoving = false;
                    }
                    ///Right THUMBSTICK
                    /*if (RightjoystickX != 0 || RightjoystickY != 0)
                    {
                        w2 = new Vector2(RightjoystickX, RightjoystickY);
                        rotation = angle(w1, w2);
                       
                    }*/
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

            if (isDashing && player.getcanMove() && isMoving)
            {
                elapsedDashTime += deltaTime;

                if (elapsedDashTime < dashTime)
                {
                    Vector3 wec = new Vector3(dashDirection.X, 0, dashDirection.Y);
                    UpdateBB(0, world.GetWorldList(), new Vector3(dashDirection.X * deltaTime * dashSpeed, 0, dashDirection.Y * deltaTime * dashSpeed), wec);
                }
                else 
                {
                    isDashing = false;
                    elapsedDashTime = 0;
                }
            } else // MOVING
            {
                float Sphereang = rotation - player.GetRotation().Y;
                rotateSphere(Sphereang);
                if (player.getcanMove())
                {
                    Vector3 wec = new Vector3(player.Direction.X, 0, player.Direction.Y);
                    UpdateBB(0, world.GetWorldList(), new Vector3(player.Direction.X * deltaTime * player.ActualSpeed, 0, player.Direction.Y * deltaTime * player.ActualSpeed), wec);

                }

                player.SetRotation(0, rotation, 0);
            }

            
        }


        public void GamePadClick(GamePadCapabilities capabilities, GamePadState gamePadState)
        {
            if (capabilities.IsConnected)
            {
                // Button A
                if (capabilities.HasAButton)
                {
                    if (gamePadState.IsButtonDown(Buttons.RightStick) && isCraftingTea && !rightStickClicked)
                    {
                        rightStickClicked = true;
                        player.Crafting.cleanRecepture();
                    } else if (gamePadState.IsButtonDown(Buttons.RightStick) && isCraftingTea && rightStickClicked)
                    {
                        rightStickClicked = false;
                    }


                        if (gamePadState.IsButtonDown(Buttons.A) && !padButtonAClicked)
                    {
                        if (isCraftingTea && !isThrowing)           // Tworzenie herbatek
                        {
                            player.AddIngredientA();
                            padButtonAClicked = true;
                        } 
                        else if (!isCraftingTea && isThrowing)    // Rzucane
                        {
                            player.ThrowMint();
                            padButtonAClicked = true;
                        } 
                        else                                      // normalny atak gracza
                        {
                            player.Attack();
                            padButtonAClicked = true;
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
                    if (gamePadState.IsButtonDown(Buttons.B) && !padButtonBClicked)
                    {
                        if (isCraftingTea && !isThrowing)
                        {
                            player.AddIngredientB();
                            padButtonBClicked = true;
                        } 
                        else if (!isCraftingTea && isThrowing)
                        {
                            player.ThrowNettle();
                            padButtonBClicked = true;
                        }
                        else 
                        {
                            padButtonBClicked = true;
                            Dash();
                        }
                    }
                    else if (gamePadState.IsButtonUp(Buttons.B))
                    {
                        padButtonBClicked = false;
                    }
                }
                if (capabilities.HasYButton)
                {
                    if (gamePadState.IsButtonDown(Buttons.Y) && !padButtonYClicked)
                    {
                        if (isCraftingTea && !isThrowing)
                        {
                            player.AddIngredientY();
                            padButtonYClicked = true;
                        } else if (!isCraftingTea && isThrowing)
                        {
                            padButtonYClicked = true;
                            player.ThrowApple();
                        }
                        else
                        {
                            padButtonYClicked = true;
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
                    if (gamePadState.IsButtonDown(Buttons.X) && !padButtonXClicked)
                    {
                        if (isCraftingTea && !isThrowing)
                        {
                            player.AddIngredientX();
                            padButtonXClicked = true;
                        } 
                        else if (!isCraftingTea && isThrowing)
                        {
                            player.ThrowMelise();
                            padButtonXClicked = true;
                        }
                        else
                        {
                            padButtonXClicked = true;
                            // tutaj ewentualny dash/przewrot
                        }
                    }
                    else if (gamePadState.IsButtonUp(Buttons.X))
                    {
                        padButtonXClicked = false;
                    }
                }

                // Przelaczanie receptur
                if (gamePadState.IsButtonDown(Buttons.LeftShoulder) && !leftShoulderClicked)
                {
                    leftShoulderClicked = true;
                    if (Globals.numberOfRecepture - 1 < 0)
                    {
                        Globals.numberOfRecepture = Globals.maxReceptures - 1;
                    } else
                    {
                        Globals.numberOfRecepture -= 1;
                    }
                } else if (gamePadState.IsButtonUp(Buttons.LeftShoulder)) {
                    leftShoulderClicked = false;
                }

                if (gamePadState.IsButtonDown(Buttons.RightShoulder) && !rightShoulderClicked)
                {
                    rightShoulderClicked = true;
                    if (Globals.numberOfRecepture + 1 >= Globals.maxReceptures)
                    {
                        Globals.numberOfRecepture = 0;
                    }
                    else
                    {
                        Globals.numberOfRecepture += 1;
                    }
                }
                else if (gamePadState.IsButtonUp(Buttons.RightShoulder))
                {
                    rightShoulderClicked = false;
                }

                // Trigery = Rzucanie i Tworzenie herb
                if (gamePadState.IsButtonDown(Buttons.LeftTrigger))
                {
                    isCraftingTea = true;
                }
                else if (gamePadState.IsButtonUp(Buttons.LeftTrigger))
                {
                    this.player.Crafting.makeTea();
                    isCraftingTea = false;
                }

                if (gamePadState.IsButtonDown(Buttons.RightTrigger))
                {
                    isThrowing = true;
                }
                else if (gamePadState.IsButtonUp(Buttons.RightTrigger))
                {
                    isThrowing = false;
                }



            }
        }

        private void Dash()
        {
            if (elapsedLastDashTime >= timeToDash)
            {
                isDashing = true;
                dashDirection = player.getLookingDirection();
                elapsedLastDashTime = 0;
            }
            
        }

        public void UpdateBB(float ang, List<SceneObject> worldList, Vector3 moveVec, Vector3 normal)
        {
            BoundingBox boksik = player.boundingBox;
            Vector3 helpVec = new Vector3(moveVec.X, 0, 0);
            boksik.Min -= helpVec;
            boksik.Max -= helpVec;

            // check kolizji na X
            if (!collide(boksik, worldList))
            {
                player.SetBoundingBox(boksik);
                player.MoveBoundingSphere(helpVec);
            }

            // check kolizji na Z
            boksik = player.boundingBox;
            helpVec = new Vector3(0, 0, moveVec.Z);
            boksik.Min -= helpVec;
            boksik.Max -= helpVec;

            if (!collide(boksik, worldList))
            {
                player.SetBoundingBox(boksik);
                player.MoveBoundingSphere(helpVec);
            }

        }
        public void rotateSphere(float Angle)
        {
            Matrix rotationMatrix = Matrix.CreateRotationY(Angle);
            player.boundingSphere.Center -= player.GetPosition();
            player.boundingSphere.Center = Vector3.Transform(player.boundingSphere.Center, rotationMatrix);
            player.boundingSphere.Center += player.GetPosition();
        }

        public bool collide(BoundingBox box, List<SceneObject> objects)
        {
            foreach (SceneObject obj in objects)
            {
                if (box.Intersects(obj.boundingBox))
                {
                    return true;
                }
            }
            return false;
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
