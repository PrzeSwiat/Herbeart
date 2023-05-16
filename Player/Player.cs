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
        public Crafting Crafting;
        private DateTime lastAttackTime;
        private DateTime actualTime;
        private PlayerMovement playerMovement;
        private PlayerEffectHandler playerEffects;
        public event EventHandler OnAttackPressed;

        private bool canMove = true;
        

        public Player(Vector3 Position, string modelFileName, string textureFileName) : base(Position, modelFileName, textureFileName)
        {
            SetScale(1.5f);
            AssignParameters(200, 20, 20);

            playerEffects = new PlayerEffectHandler(this);
            Inventory = new Inventory();
            Crafting = new Crafting(playerEffects);
            playerMovement = new PlayerMovement(this);
           /* isCraftingTea = false;
            padButtonAClicked = false;
            padButtonYClicked = false;
            padButtonXClicked = false;*/
            
            // Uruchomienie timera
            playerEffects.Start();
            this.setRadius(3);
        }

        public void Update(World world, float deltaTime) //Logic player here
        {
            Update();
            playerMovement.UpdatePlayerMovement(world, deltaTime);
        }

        public void Attack()
        {
            actualTime = DateTime.Now;
            TimeSpan time = actualTime - lastAttackTime;
            if (time.TotalSeconds > this.getAttackSpeed())
            {
                OnAttackPressed?.Invoke(this, EventArgs.Empty);
                lastAttackTime = actualTime;
            }
        }

        public void AddIngredientA()
        {
            Crafting.addIngredient('A', Inventory);
        }
        public void AddIngredientB()
        {
            Crafting.addIngredient('B', Inventory);
        }
        public void AddIngredientX()
        {
            Crafting.addIngredient('X', Inventory);
        }
        public void AddIngredientY()
        {
            Crafting.addIngredient('Y', Inventory);
        }

        public void setOriginalSpeed()
        {
            this.ActualSpeed = MaxSpeed;
        }

        public void setStun(int time)
        {
            playerEffects.Stun(time);
        }

        public void setOriginalStrenght()
        {
            this.Strength = this.MaxStrength;
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
        public bool getcanMove()
        {
            return this.canMove;
        }
        public void setcanMove(bool can)
        {
            this.canMove=can;
        }

        

    }
}