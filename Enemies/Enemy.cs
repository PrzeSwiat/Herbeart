using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TheGame
{
    internal class Enemy : Creature
    {
        private DateTime lastAttackTime, actualTime;
        public event EventHandler OnAttack;


        public Enemy(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
            AssignParameters(100, 10, 2);
            Direction = new Vector2(0, 0);
            lastAttackTime = DateTime.Now;
            actualTime = lastAttackTime;
            
        }

        public virtual void Update(float deltaTime, Player player)
        {
            Update();
            if (!(this.boundingSphere.Intersects(player.boundingBox)))
            { FollowPlayer(player.GetPosition(), deltaTime,0); }
            else { 

            
            {
                actualTime = DateTime.Now;
                TimeSpan time = actualTime - lastAttackTime;
                if (time.TotalSeconds > 1)
                {
                        OnAttack?.Invoke(this, EventArgs.Empty);
                        lastAttackTime = actualTime;
                }

            }
            }

        }

        public virtual  void FollowPlayer(Vector3 playerPosition, float deltaTime, int dir)
        {
            float speed = this.Speed;
            if (dir ==1 ) { speed = -speed; }
            Direction = new Vector2(playerPosition.X - GetPosition().X, playerPosition.Z - GetPosition().Z);
            NormalizeDirection();
            float ishowspeed = speed * deltaTime;
            Vector3 movement = new Vector3(Direction.X * ishowspeed, 0, Direction.Y * ishowspeed);
            SetPosition(GetPosition() + movement);
            boundingSphere.Center = boundingSphere.Center + movement;
            boundingBox.Min = boundingBox.Min + movement;
            boundingBox.Max = boundingBox.Max + movement;
            
            Vector2 w1 = new Vector2(0, 1);    // wektor wyjsciowy od ktorego obliczam kat czyli ten do dolu
            Vector2 w2 = new Vector2(Direction.X, Direction.Y);

            float rotation = angle(w2, w1);
            rotateSphere(rotation - this.GetRotation().Y);

            this.SetRotation(0, rotation, 0);
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

    }
}
