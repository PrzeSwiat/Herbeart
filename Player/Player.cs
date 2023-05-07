﻿using Microsoft.Xna.Framework;
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
        public List<Leaf> inventory;
        private bool canMove = true;
        private float StunTimer = 0;
        private DateTime lastAttackTime, actualTime;


        private PlayerMovement playerMovement;
        private PlayerEffectHandler playerEffects;
        public event EventHandler OnAttackPressed;

        private Boolean isCraftingTea;
        private Boolean padButtonAClicked;
        private Boolean padButtonBClicked;
        private Boolean padButtonXClicked;
        private Boolean padButtonYClicked;

        public Player(Vector3 Position, string modelFileName, string textureFileName) : base(Position, modelFileName, textureFileName)
        {
            SetScale(1.7f);

            AssignParameters(200, 20, 20);
            playerMovement = new PlayerMovement(this);
            isCraftingTea = false;
            padButtonAClicked = false;

            // Dołączenie metody, która będzie wykonywana przy każdym ticku timera
            playerEffects = new PlayerEffectHandler(this);
            // Uruchomienie timera
            playerEffects.Start();
        }

        public void Update(World world, float deltaTime) //Logic player here
        {
            Update();
            if (canMove) { playerMovement.UpdatePlayerMovement(world, deltaTime); StunTimer = 2; }
            else
            {
                StunTimer = StunTimer - deltaTime;
                if (StunTimer < 0) { canMove = true; }
            }
            GamePadClick();
        }

        public void setStun(bool bStun)
        {
            this.canMove = bStun;
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


        public void PlayerRegenarateHealth(int hp, int time)
        {
            playerEffects.RegenarateHP(hp, time);
        }


        public void AddLeaf(Leaf leaf)
        {
            inventory.Add(leaf);
        }

        public void RemoveLeaf(Leaf leaf)
        {
            inventory.Remove(leaf);
        }


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
                            // tutaj dodac skladnik do tworzenia herbaty pod A
                            padButtonAClicked = true;

                        } else // normalny atak gracza
                        {
                            actualTime = DateTime.Now;
                            TimeSpan time = actualTime - lastAttackTime;
                            if (time.TotalSeconds > 1)
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
                            // tutaj dodac skladnik do tworzenia herbaty pod B
                            padButtonBClicked = true;
                            Debug.Write("Dupa");

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


                if (gamePadState.IsButtonDown(Buttons.LeftTrigger)) 
                { 
                    isCraftingTea = true;
                } else if (gamePadState.IsButtonUp(Buttons.LeftTrigger))
                {
                    isCraftingTea = false;
                }




            }
        }

    }
}