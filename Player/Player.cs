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
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using TheGame.Core;

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
        private List<Apple> apples = new List<Apple>();

        private bool canMove = true;

        public event EventHandler onMove;



        public Player(Vector3 Position, string modelFileName, string textureFileName) : base(Position, modelFileName, textureFileName)
        {
            SetScale(1.5f);
            AssignParameters(200, 20, 20);

            playerEffects = new PlayerEffectHandler(this);
            Inventory = new Inventory();
            Crafting = new Crafting(playerEffects);
            playerMovement = new PlayerMovement(this);
            
            // Uruchomienie timera
            playerEffects.Start();
            this.setRadius(3);
        }

        public void Update(World world, float deltaTime, Enemies enemies) //Logic player here
        {
            Update();
            foreach (Apple apple in apples.ToList())
            {
                apple.Update(deltaTime, enemies);
            }
            playerMovement.UpdatePlayerMovement(world, deltaTime);

            if(playerMovement.isMoving)
            {
                onMove?.Invoke(this, EventArgs.Empty);
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Draw(Vector3 lightpos)
        {
            base.Draw(lightpos);

            foreach(Apple apple in apples)
            {
                apple.Draw(lightpos);
                //apple.DrawBB(); why not working ???
            }
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

        public void ThrowableY()
        {
            if(Inventory.checkAppleLeafNumber())
            {
                Inventory.removeAppleLeaf();
                Apple apple = new Apple(this.GetPosition(), this.getLookingDirection());
                apple.LoadContent();
                apple.OnDestroy += RemoveBullet;
                apples.Add(apple);
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

        public void RemoveBullet(object sender, EventArgs e)
        {
            apples.Remove((Apple)sender);
        }

        internal class Apple : SceneObject
        {
            private Vector3 velocity;
            private int dmg = 100;
            private float speed = 20f;
            private float time = 0, maxTime = 4;

            public event EventHandler OnDestroy;

            public Apple(Vector3 Position, Vector2 direction) : base(Position, "Objects/Apple", "Textures/appleTexture")
            {
                Vector3 startPosition = Position;
                startPosition.Y = 2;
                this.SetPosition(startPosition);
                velocity = new Vector3(-direction.X * speed, 0, -direction.Y * speed);
            }

            public void Update(float deltaTime, Enemies enemies)
            {
                time += deltaTime;
                // Move the bullet
                this.Move(velocity * deltaTime);

                // Check if the bullet has collided with the player
                foreach (Enemy enemy in enemies.EnemiesList.ToList())
                {
                    if (this.boundingBox.Intersects(enemy.boundingBox))
                    {
                        enemy.Hit(dmg);
                        OnDestroy?.Invoke(this, EventArgs.Empty);
                    }
                }

                // Check if the bullet has hit the ground
                if (time >= maxTime)
                {
                    OnDestroy?.Invoke(this, EventArgs.Empty);
                }
            }
        }


    }
}