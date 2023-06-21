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
            AssignParameters(6, 12, 2, 1f);
            this.setBSRadius(2.5f);
            this.shadow.SetScale(0.95f);
            this.visionRange = 30f;
            this.leaf = new Leafs.NettleLeaf(worldPosition, "Objects/nettle_pickup", "Textures/nettle_pickup");
            lastHealth = this.Health;
        }
        public override void Update(float deltaTime, Player player)
        {
            base.Update(deltaTime, player);
            this.shadow.UpdatingEnemy(this.GetPosition(), new Vector3(1.8f,0,-1.9f));
            this.SetPositionY(this.GetPosition().Y + 2);
        }
        public override void LoadContent()
        {
            base.LoadContent();
            float vlll = 2f;
            BoundingBox helper;
            helper.Min = new Vector3(-vlll, 0, -vlll);
            helper.Max = new Vector3(vlll, 4, vlll);

            SetBoundingBox(new BoundingBox(helper.Min + this.GetPosition(), helper.Max + this.GetPosition()));
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
