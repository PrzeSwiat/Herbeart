using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace TheGame
{
    [Serializable]
    internal class Creature : SceneObject
    {
        private int health;
        private int strenght;
        private float speed;
        private float attackSpeed;
        public BoundingSphere attackShpere;
        public List<CreatureEffect> currentBadEffect;
        public List<float> badEffectTimes;

        public event EventHandler OnDestroy;

        private Vector2 direction;
        private DateTime lastAttackTime, actualTime;

        public Creature(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {

        }

        public void AssignParameters(int health, int strenght, float speed)
        {
            this.health = health;
            this.strenght = strenght;
            this.speed = speed;
        }

        public void Hit(int damage)
        {
            health -= damage;

            if(health<0)
            {
                OnDestroy?.Invoke(this,EventArgs.Empty);
            }
            else
            {
                color = Color.Red;
            }
        }

        public void ApplyBadEffect(CreatureEffect effect)
        {
            currentBadEffect.Add(effect);
        }

        public void RemoveBadEffect(CreatureEffect effect)
        {
            currentBadEffect.Remove(effect);
        }

        


        public void Drop()
        {

        }

        // -------------- G E T T E R S --------------------

        public int Health
        {
            get { return this.health; }
            set { this.health = value; }
        }

        public int Strength
        {
            get { return this.strenght; }
            set { this.strenght = value; }
        }

        public float Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }

        public Vector2 Direction
        {
            get { return this.direction; }
            set { this.direction = value; }
        }

        public void setDirectionX(float x)
        {
            this.direction.X = x;
        }

        public void setDirectionY(float y)
        {
            this.direction.Y = y;
        }

        public void NormalizeDirection()
        {
            this.direction.Normalize();
        }


    }
}
