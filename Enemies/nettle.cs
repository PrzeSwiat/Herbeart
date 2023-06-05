using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class Nettle : Enemy
    {
        private DateTime lastAttackTime, actualTime;
        public event EventHandler OnAttack;
        private int attackCounter = 0;
        float armortime = 10;
        int lastHealth;
        public Nettle(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
            AssignParameters(80, 8, 2, 0.7f);
            this.shadow.SetScale(0.95f);
            this.leaf = new Leafs.NettleLeaf(worldPosition, "Objects/mis4", "Textures/StarSparrow_Orange");
            lastHealth = this.Health;
        }
        public override void Update(float deltaTime, Player player)
        {
            base.Update(deltaTime, player);
            this.shadow.UpdatingEnemy(this.GetPosition(), new Vector3(1.8f,0,-1.9f));
        }
        protected override void Attack(Player player)
        {
            actualTime = DateTime.Now;
            TimeSpan time = actualTime - lastAttackTime;
            if (time.TotalSeconds > 1)
            {
                OnAttack?.Invoke(this, EventArgs.Empty);
                lastAttackTime = actualTime;
                player.Hit(this.Strength);
            }
        }
    }
}
