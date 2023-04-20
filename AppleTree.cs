using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{


    internal class Apple : SceneObject
    {
        float dmg;
        Vector3 velocity;
        float speed =0.3f;

        public Apple(Vector3 worldPosition, string modelFileName, string textureFileName, Vector3 endPosition) : base(worldPosition, modelFileName, textureFileName)
        {
            Vector3 startPosition = this.GetPosition();
            Vector3 direction = Vector3.Normalize(endPosition - startPosition);
            position = startPosition;
            velocity = Vector3.Normalize(direction) * speed;
            
        }
        public void Update(GameTime gameTime, Player player)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move the bullet
            position += velocity * deltaTime;

            // Check if the bullet has collided with the player
            if (this.boundingSphere.Intersects(player.boundingBox))
            {
                player.Health -= 5;
            }

            // Check if the bullet has hit the ground
            if (position.Y <= 0)
            {
                // Destroy the bullet
                // ...
                Debug.Write("chuj \n");
            }

            // Decrease the time to live of the bullet
           
        }


    }

    internal class AppleTree : Enemy
    {
        float PossibledistanceToPlayer = 20f;
        bool canCorrectPosition = true;
        bool canShoot = true;
        float timecounter = 0;
        float timetoshoot = 0;
        List<Apple> bullet = new List<Apple>(); 
        public AppleTree(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {

        }



        public  void Throw(int damage,Player player)
        {
          bullet.Add(new Apple(this.boundingSphere.Center,"mis4","green",player.GetPosition()));
            
        }


        public override void Update(float deltaTime, Player player)
        {

            
            Vector3 difference = this.GetPosition() - player.GetPosition();
            float distance = difference.LengthSquared();
            timecounter -= deltaTime;
            timetoshoot -= deltaTime;
            Debug.Write(timetoshoot.ToString() + "\n");

            if (timecounter > 0)
            {
                canCorrectPosition = false;
            }
            else if (timecounter <= 0)
            {
                canCorrectPosition = true;
            }

            if (canCorrectPosition)
            { 
                if (PossibledistanceToPlayer * PossibledistanceToPlayer < distance )
                {
                    this.FollowPlayer(player.GetPosition(), deltaTime, 0);
                    difference = this.GetPosition() - player.GetPosition();
                    distance = difference.LengthSquared();

                    if (PossibledistanceToPlayer * PossibledistanceToPlayer >= distance) {
                            timecounter = 10f;
                        }

                }
            else if(PossibledistanceToPlayer * PossibledistanceToPlayer >= distance ) {


                    this.FollowPlayer(player.GetPosition(), deltaTime,1);
                    difference = this.GetPosition() - player.GetPosition();
                    distance = difference.LengthSquared();
                    if (PossibledistanceToPlayer * PossibledistanceToPlayer <= distance)
                    {
                        timecounter = 10f;
                    }
                }
            }
            else
            {
                if (timetoshoot > 0)
                {
                    canShoot = false;
                }
                else if (timetoshoot <= 0)
                {
                    canShoot = true;
                }
                if (canShoot) { 
                this.Throw(5, player);
                    timetoshoot = 5;
                }

            }
            foreach (Apple apple in bullet)
            {
                apple.Update();
            }



        }
    }
}
