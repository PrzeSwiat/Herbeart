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
        float speed =10f;
        public event EventHandler OnDestroy;
        public Apple(Vector3 worldPosition, string modelFileName, string textureFileName, Vector3 endPosition) : base(worldPosition, modelFileName, textureFileName)
        {
           
            Vector3 startPosition = this.GetPosition();
            startPosition.Y += 3;
            Vector3 direction = Vector3.Normalize(endPosition - startPosition);
            position = startPosition;
            velocity = Vector3.Normalize(direction);
            velocity.X = velocity.X * speed;
            velocity.Y = velocity.Y * 6 ;
            velocity.Z = velocity.Z * speed;
            


        }
        public void Update(float deltaTime, Player player)
        {
 
            // Move the bullet
            position += velocity * deltaTime;
           // position.Y = 0.2f;
            SetPosition(position);

            // Check if the bullet has collided with the player
            if (this.boundingSphere.Intersects(player.boundingBox))
            {
                player.Health -= 5;
                
                
                OnDestroy?.Invoke(this, EventArgs.Empty);
              //  Debug.Write("dupa \n");
            }

            // Check if the bullet has hit the ground
            if (position.Y <= 0)
            {
                
                
                OnDestroy?.Invoke(this, EventArgs.Empty);
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
        public List<Apple> bullet = new List<Apple>(); 
        public AppleTree(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
           
        }

        public void RemoveBullet(object sender,EventArgs e)
        {
            bullet.Remove((Apple)sender);
            Debug.Write("Hitler");
        }

        public  void Throw(int damage,Player player)
        {
          bullet.Add(new Apple(this.boundingSphere.Center,"mis4", "StarSparrow_Orange", player.GetPosition()));
            
            
        }


        public override void Update(float deltaTime, Player player)
        {
            Update();
            
            Vector3 difference = this.GetPosition() - player.GetPosition();
            float distance = difference.LengthSquared();
            timecounter -= deltaTime;
            timetoshoot -= deltaTime;
            //Debug.Write(timetoshoot.ToString() + "\n");

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
            foreach (Apple apple in bullet.ToList())
            {
                apple.Update(deltaTime,  player);
                apple.OnDestroy += RemoveBullet;
               //ebug.Write(bullet.Count()

            }

            

        }
    }
}
