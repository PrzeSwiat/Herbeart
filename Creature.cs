using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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
        public BoundingBox collisionBox;
        public List<CreatureEffect> currentBadEffect;
        public List<float> badEffectTimes;
        

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
        }

        public void ApplyBadEffect(CreatureEffect effect)
        {
            currentBadEffect.Add(effect);
        }

        public void RemoveBadEffect(CreatureEffect effect)
        {
            currentBadEffect.Remove(effect);
        }

        public void UpdateBadEffects()
        {

        }

        private void DealDamage(Creature creature)
        {
            creature.Hit(this.strenght);
        }

        public void AttackCheck()
        {

        }

        public void Drop()
        {

        }

        


        public bool Destroy() //XD
        {
            return true;
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


    }
}
