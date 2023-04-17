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

        public Enemy(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
            AssignParameters(100, 10, 2);
            Direction = new Vector2(0, 0);
            lastAttackTime = DateTime.Now;
            actualTime = lastAttackTime;
            
        }

        public void Update(float deltaTime, Player player)
        {
            Update();
            FollowPlayer(player.GetPosition(),deltaTime);
            

            if (Math.Round(player.GetPosition().X, 2) == Math.Round(GetPosition().X, 2)
                && Math.Round(player.GetPosition().Z, 2) == Math.Round(GetPosition().Z, 2))
            {
                actualTime = DateTime.Now;
                TimeSpan time = actualTime - lastAttackTime;
                if (time.TotalSeconds > 1)
                {
                    player.Hit(Strength);
                    lastAttackTime = actualTime;
                }

            }

        }

        private void FollowPlayer(Vector3 playerPosition, float deltaTime)
        {
            Direction = new Vector2(playerPosition.X - GetPosition().X, playerPosition.Z - GetPosition().Z);
            NormalizeDirection();
            float ishowspeed = Speed * deltaTime;
            SetPosition(GetPosition() + new Vector3(Direction.X * ishowspeed, 0, Direction.Y * ishowspeed));
        }


    }
}
