using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class Mint : Enemy
    {

        private DateTime lastAttackTime, actualTime;
        public event EventHandler OnAttack;

        public Mint(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
            AssignParameters(2, 15, 2, 3f);
            this.shadow.SetScale(0.7f);
            this.setBSRadius(2);

            this.leaf = new Leafs.MintLeaf(worldPosition, "Objects/mint_pickup", "Textures/mint_pickup");
        }

        public override void LoadContent()
        {
            base.LoadContent();
            float vlll = 1f;
            BoundingBox helper;
            helper.Min = new Vector3(-vlll, 0, -vlll);
            helper.Max = new Vector3(vlll, 4, vlll);

            SetBoundingBox(new BoundingBox(helper.Min + this.GetPosition(), helper.Max + this.GetPosition()));
        }

        public override void Update(float deltaTime, Player player)
        {
            base.Update(deltaTime, player);
            this.shadow.UpdatingEnemy(this.GetPosition(), new Vector3(1.2f,0,-1));
            this.shadow.SetPositionY(-0.5f);

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
