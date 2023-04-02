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
        public float health;
        public float strenght;
        public float speed;
        public float attackSpeed;
        public BoundingSphere attackShpere;
        public BoundingBox collisionBox;
        public List<CreatureEffect> currentBadEffect;
        public List<float> badEffectTimes;
        public List<Leaf> inventory;

        public Creature(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {

        }

        public void Hit(float damage)
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
            creature.health -= this.strenght;
        }

        public void AttackCheck()
        {

        }

        public void Drop()
        {

        }

        public void AddLeaf(Leaf leaf)
        {
            inventory.Add(leaf);
        }

        public void RemoveLeaf(Leaf leaf)
        {
            inventory.Remove(leaf);
        }


        public bool Destroy() //XD
        {
            return true;
        }

    }
}
