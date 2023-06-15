using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Timers;

namespace TheGame
{
    internal class Enemy : Creature
    {
        public event EventHandler OnAttack;
        public bool isStun = false;
        private bool isSlow = false;
        public float stunTime = 0, elapsedStunTime = 0;
        public float slowTime = 1, elapsedSlowTime = 0;
        public Shadow shadow;
        private DateTime lastAttackTime, actualTime;
        private bool collides = false;
        public float visionRange = 0f;


        public Enemy(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
            shadow = new Shadow(worldPosition);
            Direction = new Vector2(1, 1);
            lastAttackTime = DateTime.Now;
            actualTime = lastAttackTime;
            SetScale(1.5f);
            //this.setBSRadius(3);
        }

        protected virtual void Attack(Player player)
        {
            actualTime = DateTime.Now;
            TimeSpan time = actualTime - lastAttackTime;
            if (time.TotalSeconds > this.ActualAttackSpeed)
            {
                OnAttack?.Invoke(this, EventArgs.Empty);
                lastAttackTime = actualTime;
            }
        }
        public override void LoadContent()
        {
            shadow.LoadContent();
            base.LoadContent();
            
        }
        public override void DrawPlayer(Vector3 lightpos)
        {
            base.DrawPlayer(lightpos);
            shadow.Draw(lightpos);

        }

        protected void checkCollision(Player player)
        {
            if (this.boundingSphere.Intersects(player.boundingBox) == true) collides = true;
            else collides = false;
        }

        public virtual void Update(float deltaTime, Player player)
        {
            Update();
            if (isStun)
            {
                elapsedStunTime += deltaTime;
                if (elapsedStunTime >= stunTime)
                {
                    isStun = false;
                    elapsedStunTime = 0;
                }
            }
            else
            {
                if (isSlow)
                {
                    elapsedSlowTime += deltaTime;
                    if (elapsedSlowTime >= slowTime)
                    {
                        SetNormalSpeed();
                    }
                }

                checkCollision(player);
                RotateTowardsCurrentDirection();
                if (collides)
                {
                    Attack(player);
                }
                else
                {
                    MoveForwards(deltaTime, true);
                }
            }
            
        }

        public void Stun(int time)
        {
            isStun = true;
            stunTime = time;
        }

        public void Slow (float slowMultiplier)
        {
            isSlow = true;
            this.ActualAttackSpeed = this.AttackSpeed * slowMultiplier;
            this.ActualSpeed = this.MaxSpeed * slowMultiplier;
        }

        private void SetNormalSpeed()
        {
            this.ActualAttackSpeed = this.AttackSpeed;
            this.ActualSpeed = this.MaxSpeed;
            isSlow = false;
            elapsedSlowTime = 0;
        }


        public Vector2 CalculateDirectionTowardsTarget(Vector3 targetPosition)
        {
            return new Vector2(targetPosition.X - GetPosition().X, targetPosition.Z - GetPosition().Z);
        }



        protected void RotateTowardsCurrentDirection()
        {
            Vector2 w1 = new Vector2(0, 1);    // wektor wyjsciowy od ktorego obliczam kat czyli ten do dolu
            Vector2 w2 = new Vector2(Direction.X, Direction.Y);

            float rotation = angle(w2, w1);

            // rotate bounding box
            rotateSphere(rotation - this.GetRotation().Y);

            // rotate model
            this.SetRotation(0, rotation, 0);
        }

        private void MoveBoundingBoxForwards(float speed)
        {
            Vector3 movement = new Vector3(Direction.X * speed, 0, Direction.Y * speed);
            boundingSphere.Center = boundingSphere.Center + movement;
            boundingBox.Min = boundingBox.Min + movement;
            boundingBox.Max = boundingBox.Max + movement;
        }

        public void MoveForwards(float deltaTime, bool shouldChase)
        {
            float currentSpeed = this.ActualSpeed;
            if (!shouldChase) { currentSpeed *= -1; }


            MoveModelForwards(currentSpeed * deltaTime);
            MoveBoundingBoxForwards(currentSpeed * deltaTime);
        }

        
        public void rotateSphere(float Angle)
        {
            Matrix rotationMatrix = Matrix.CreateRotationY(Angle);
            boundingSphere.Center -= this.GetPosition();
            boundingSphere.Center = Vector3.Transform(boundingSphere.Center, rotationMatrix);
            boundingSphere.Center += this.GetPosition();
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

        public bool Collides
        {
            get { return this.collides; }
        }

    }
}
