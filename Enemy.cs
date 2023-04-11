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
        private Vector2 direction;
        private DateTime lastAttackTime, actualTime;

        public Enemy(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
            AssignParameters(100, 10, 2);
            direction = new Vector2(0, 0);
            lastAttackTime = DateTime.Now;
            actualTime = lastAttackTime;
        }

        public void Update(float deltaTime, Player player)
        {
            FollowPlayer(player.GetPosition());
            float ishowspeed = Speed * deltaTime;
            SetPosition(GetPosition() + new Vector3(direction.X * ishowspeed, 0, direction.Y * ishowspeed));

            Debug.Write(Math.Round(player.GetPosition().X, 2) + "  " + GetPosition().X + "  ");
            if (Math.Round(player.GetPosition().X, 2) == Math.Round(GetPosition().X, 2)
                && Math.Round(player.GetPosition().Z, 2) == Math.Round(GetPosition().Z, 2))
            {
                actualTime = DateTime.Now;
                TimeSpan time = actualTime - lastAttackTime;
                if (time.TotalSeconds > 1)
                {
                    player.getDamage(Strength);
                    lastAttackTime = actualTime;
                }

            }

        }

        private void FollowPlayer(Vector3 playerPosition)
        {
            direction.X = playerPosition.X - GetPosition().X;
            direction.Y = playerPosition.Z - GetPosition().Z;
            direction.Normalize();
        }


    }
}
