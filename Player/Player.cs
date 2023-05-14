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
using System.Timers;
using System.Xml;

namespace TheGame
{
    [Serializable]
    internal class Player : Creature
    {
        public Inventory Inventory;
        private Crafting Crafting;
        public bool canMove = true;
        private DateTime lastAttackTime, actualTime;
        private float atackSpeed = 0.5f;

        //public string test = "";
        private PlayerMovement playerMovement;
        private PlayerEffectHandler playerEffects;
        public event EventHandler OnAttackPressed;

        private Boolean isCraftingTea;
        private Boolean padButtonAClicked;
        private Boolean padButtonBClicked;
        private Boolean padButtonXClicked;
        private Boolean padButtonYClicked;

        MouseState lastMouseState, currentMouseState;

        public Player(Vector3 Position, string modelFileName, string textureFileName) : base(Position, modelFileName, textureFileName)
        {
            SetScale(1f);
            AssignParameters(200, 20, 20);

            Inventory = new Inventory();
            Crafting = new Crafting();
            playerMovement = new PlayerMovement(this);
            isCraftingTea = false;
            padButtonAClicked = false;
            padButtonYClicked = false;
            padButtonXClicked = false;
            
            //Matrix bs = Matrix.Transpose();
            //this.boundingSphere.Transform()
            //this.boundingSphere.Radius = 
            // Dołączenie metody, która będzie wykonywana przy każdym ticku timera
            playerEffects = new PlayerEffectHandler(this);
            // Uruchomienie timera
            playerEffects.Start();
        }

        public void Update(World world, float deltaTime) //Logic player here
        {
            Update();

            playerMovement.UpdatePlayerMovement(world, deltaTime);

            GamePadClick();
        }

        public void setStun(int time)
        {
            playerEffects.Stun(time);
        }

        public void PlayerRegenarateHealth(int hp, int time)
        {
            playerEffects.RegenarateHP(hp, time);
        }

        public void AddHealth(int amount)
        {
            if (this.Health + amount > this.MaxHealth)
            {
                this.Health = this.MaxHealth;
            }
            else { this.Health += amount; }
        }

        public void SubstractHealth(int amount)
        {
            if (this.Health - amount < 0)
            {
                this.Health = 0;
            }
            else { this.Health -= amount; }
        }



        #region Getters
        //GET'ERS

        //.................
        #endregion

        #region Setters
        //SET'ERS

        #endregion
        //.................


        


      


        public void GamePadClick()
        {
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (capabilities.IsConnected)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                // Button A
                if (capabilities.HasAButton)
                {
                    if (gamePadState.IsButtonDown(Buttons.A))
                    {
                        if (isCraftingTea && !padButtonAClicked) // W momencie jak lewy triger jest wcisniety
                        {
                            Crafting.addIngredient('A', Inventory);
                            Debug.Write(Crafting.getRecepture() + "\n");
                            padButtonAClicked = true;

                        } else // normalny atak gracza
                        {

                            actualTime = DateTime.Now;
                            TimeSpan time = actualTime - lastAttackTime;
                            if (time.TotalSeconds > atackSpeed)
                            {
                                OnAttackPressed?.Invoke(this, EventArgs.Empty);
                                lastAttackTime = actualTime;
                            }
                        }
                        
                    } else if (gamePadState.IsButtonUp(Buttons.A)) {
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
                            Crafting.addIngredient('B', Inventory);
                            Debug.Write(Crafting.getRecepture() + "\n");
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
                            Crafting.addIngredient('Y', Inventory);
                            Debug.Write(Crafting.getRecepture() + "\n");
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
                            Crafting.addIngredient('X', Inventory);
                            Debug.Write(Crafting.getRecepture() + "\n");
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
                } else if (gamePadState.IsButtonUp(Buttons.LeftTrigger))
                {
                    Crafting.makeTea(this.Inventory);
                    isCraftingTea = false;
                }




            } else
            {
                lastMouseState = currentMouseState;

                // Get the mouse state relevant for this frame
                currentMouseState = Mouse.GetState();

                // Recognize a single click of the left mouse button
                if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    actualTime = DateTime.Now;
                    TimeSpan time = actualTime - lastAttackTime;
                    if (time.TotalSeconds > atackSpeed)
                    {
                        OnAttackPressed?.Invoke(this, EventArgs.Empty);
                        lastAttackTime = actualTime;
                    }
                }

            }
        }

    }
}