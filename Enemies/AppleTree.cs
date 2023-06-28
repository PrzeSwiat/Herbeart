using Liru3D.Animations;
using Liru3D.Models;
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
            AssignParameters(4, 12, 2, 1.5f);
            this.setBSRadius(3);
            this.visionRange = 30f;
            this.leaf = new Leafs.AppleLeaf(worldPosition, "Objects/apple_pickup", "Textures/apple_pickup");
            this.shadow.SetScale(1.05f);
        }


        public void RemoveBullet(object sender,EventArgs e)
        {
            bullet.Remove((Apple)sender);
        }

        public  void Throw(int damage,Player player)
        {
            base.OnAttackGo();
            Vector3 pos = (this.boundingBox.Min + this.boundingBox.Max) / 2;

            bullet.Add(new Apple(pos, "Objects/japco", "Textures/appleTexture", player.GetPosition()));
        }


        public override void Update(float deltaTime, Player player)
        {
            Update();

            AAttack.Update(Globals.gameTime);
            AIdle.Update(Globals.gameTime);
            ARun.Update(Globals.gameTime);

            this.shadow.UpdatingEnemy(this.GetPosition(), new Vector3(1.8f, 0, -1.9f));
            foreach (Apple apple in bullet.ToList())
            {

                apple.Update(deltaTime, player);
                apple.OnDestroy += RemoveBullet;
            }
            if (isStunned)
            {
                elapsedStunTime += deltaTime;
                if (elapsedStunTime >= stunTime)
                {
                    isStunned = false;
                    elapsedStunTime = 0;
                }
            } else
            {
                Vector3 difference = this.GetPosition() - player.GetPosition();
                float distance = difference.LengthSquared();
                timecounter -= deltaTime;
                timetoshoot -= deltaTime;

                this.Direction = Vector2.Zero;
                Vector3 playerPosition = player.GetPosition();
                this.Direction = CalculateDirectionTowardsTarget(playerPosition);
                RotateTowardsCurrentDirection();

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

                        this.MoveForwards(deltaTime/10, true);
                        difference = this.GetPosition() - player.GetPosition();
                        distance = difference.LengthSquared();

                        if (PossibledistanceToPlayer * PossibledistanceToPlayer >= distance) {
                                timecounter = 10f;
                            }

                    }
                    else if(PossibledistanceToPlayer * PossibledistanceToPlayer >= distance ) {


                        this.MoveForwards(deltaTime/10, false);
                        difference = this.GetPosition() - player.GetPosition();
                        distance = difference.LengthSquared();
                        if (PossibledistanceToPlayer * PossibledistanceToPlayer <= distance)
                        {
                            timecounter = 10f;
                        }
                    }
                }
                if((this.GetPosition()-player.GetPosition()).Length()<PossibledistanceToPlayer)
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
                        this.Throw(12, player);
                        timetoshoot = this.ActualAttackSpeed;
                    }

                }
                
            }
            

            

        }
        public override void LoadContent()
        {
            Idle = Globals.content.Load<SkinnedModel>("Animations/drzewko_idle4");
            Atak = Globals.content.Load<SkinnedModel>("Animations/drzewko_atak2");
            Run = Globals.content.Load<SkinnedModel>("Animations/drzewko_idle4");
            SpecialAttack = Globals.content.Load<SkinnedModel>("Animations/mis_bieg_2");

            //Idle
            AIdle = new AnimationPlayer(Idle);
            AIdle.Animation = Idle.Animations[0];
            AIdle.PlaybackSpeed = 0.5f;
            AIdle.IsPlaying = true;
            AIdle.IsLooping = false;
            AIdle.CurrentTime = 1.0f;
            AIdle.CurrentTick = Idle.Animations[0].DurationInTicks;
            //Attack
            AAttack = new AnimationPlayer(Atak);
            AAttack.Animation = Atak.Animations[0];
            AAttack.PlaybackSpeed = this.AttackSpeed;
            AAttack.IsPlaying = false;
            AAttack.IsLooping = false;
            AAttack.CurrentTime = 1.0f;
            AAttack.CurrentTick = Atak.Animations[0].DurationInTicks;
            //Run
            ARun = new AnimationPlayer(Run);
            ARun.Animation = Run.Animations[0];
            ARun.PlaybackSpeed = 0.5f;
            ARun.IsPlaying = false;
            ARun.IsLooping = false;
            ARun.CurrentTime = 1.0f;
            ARun.CurrentTick = Run.Animations[0].DurationInTicks;
            //SpecialAtack
            ASpecialAttack = new AnimationPlayer(SpecialAttack);
            ASpecialAttack.Animation = SpecialAttack.Animations[0];
            ASpecialAttack.PlaybackSpeed = 1f;
            ASpecialAttack.IsPlaying = false;
            ASpecialAttack.IsLooping = false;
            ASpecialAttack.CurrentTime = 1.0f;
            ASpecialAttack.CurrentTick = SpecialAttack.Animations[0].DurationInTicks;

            base.LoadContent();
            float vlll = 2f;
            BoundingBox helper;
            helper.Min = new Vector3(-vlll, 0, -vlll);
            helper.Max = new Vector3(vlll, 6, vlll);

            SetBoundingBox(new BoundingBox(helper.Min + this.GetPosition(), helper.Max + this.GetPosition()));
        }
    }

    internal class Apple : SceneObject
    {
        int dmg = 30;
        Vector3 velocity;
        float speed = 30f;
        private float time = 0, maxTime = 2;
        public event EventHandler OnDestroy;
        public Apple(Vector3 worldPosition, string modelFileName, string textureFileName, Vector3 endPosition) : base(worldPosition, modelFileName, textureFileName)
        {
            Vector3 startPosition = this.GetPosition();
            startPosition.Y = 4;
            this.SetPosition(startPosition);
            
            Vector2 direction = new Vector2(endPosition.X - startPosition.X, endPosition.Z - startPosition.Z);
            direction = Vector2.Normalize(direction);
           
            velocity = new Vector3(direction.X * speed, 0, direction.Y * speed);
            SetScale(1.4f);
        }

        public void Update(float deltaTime, Player player)
        {
            // Move the bullet
            time += deltaTime;
            this.Move(velocity * deltaTime);


            // Check if the bullet has collided with the player
            if (this.boundingBox.Intersects(player.boundingBox))
            {
                player.HitWithParticle(dmg);

                OnDestroy?.Invoke(this, EventArgs.Empty);
            }

            // Check if the bullet has hit the ground
            if (time >= maxTime)
            {
                OnDestroy?.Invoke(this, EventArgs.Empty);
            }

            // Decrease the time to live of the bullet

        }



    }
}
