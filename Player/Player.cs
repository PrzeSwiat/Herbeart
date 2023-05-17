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
        private Model appleModel;
        private Texture2D appleTexture;

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

        public void Update(World world, float deltaTime, Enemies enemies) //Logic player here
        {
            Update();
            foreach (Apple apple in apples)
            {
                apple.Update(deltaTime, enemies);
            }
            playerMovement.UpdatePlayerMovement(world, deltaTime);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            LoadedModels models = LoadedModels.Instance;
            appleModel = models.getModel("Apple");
            appleTexture = models.getTexture("appleTexture");
        }

        public override void Draw(EffectHandler effectHandler, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, Microsoft.Xna.Framework.Color color)
        {
            base.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix, color);

            foreach(Apple apple in apples)
            {
                apple.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix, color);
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

        public void ThrowableA()
        {
            if(Inventory.checkAppleLeafNumber())
            {
                Inventory.removeAppleLeaf();
                apples.Add(new Apple(this.position, this.getLookingDirection(), appleModel, appleTexture));
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

        internal class Apple : SceneObject
        {
            private int dmg;
            Vector3 velocity;
            private float speed = 10f;
            public event EventHandler OnDestroy;

            public Apple(Vector3 Position, Vector2 direction, Model model, Texture2D texture) : base(Position, "Apple", "appleTexture")
            {
                this.model = model;
                this.texture2D = texture;
                this.boundingBox = CreateBoundingBox(this.model);
                Vector3 startPosition = this.GetPosition();
                startPosition.Y += 3;
                position = startPosition;
                velocity = new Vector3(-direction.X, 0, -direction.Y);
                velocity.X = velocity.X * speed;
                velocity.Z = velocity.Z * speed;
                
            }

            public void Update(float deltaTime, Enemies enemies)
            {
                // Move the bullet
                position += velocity * deltaTime;
                SetPosition(position);

                // Check if the bullet has collided with the player
                foreach (Enemy enemy in enemies.EnemiesList)
                {
                    if (this.boundingBox.Intersects(enemy.boundingBox))
                    {
                        enemy.Hit(dmg);
                        //Debug.Write("SDSD");
                        OnDestroy?.Invoke(this, EventArgs.Empty);
                    }
                }
                

                // Check if the bullet has hit the ground
                if (position.Y <= 0)
                {
                    OnDestroy?.Invoke(this, EventArgs.Empty);
                }

                // Decrease the time to live of the bullet

            }



        }

    }
}