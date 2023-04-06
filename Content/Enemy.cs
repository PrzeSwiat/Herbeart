using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TheGame.Content
{
    internal class Enemy : Creature
    {
        private Vector2 direction;
        public Enemy(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
            this.AssignParameters(100, 10, 2);
            this.direction = new Vector2(0, 0);
        }

        public void Update(float deltaTime)
        {
            this.SetPosition(this.GetPosition() + new Vector3(this.Speed * deltaTime, 0, 0));
        }

        private void FollowPlayer()
        {

        }

    }
}
