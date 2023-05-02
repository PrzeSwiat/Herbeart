using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace TheGame
{
    internal class Enemies
    {
        private List<Enemy> enemiesList;

        public Enemies()
        {
            enemiesList = new List<Enemy>();
        }

        public void AddEnemy(Enemy enemy)
        {
            enemiesList.Add(enemy);
        }


        public void Move(float deltaTime, Player player)
        {
            Vector3 playerPosition = player.GetPosition();
            foreach (Enemy enemy in enemiesList)
            {
                Vector2 flockVel = FlockBehaviour(enemy, 20, 0.7f);
                Vector2 avoidVel = AvoidanceBehaviour(enemy, 9, 0.5f);
                Vector2 alignVel = AlignBehaviour(enemy, 20, 0.1f);
                Vector2 towardsPlayerVel = enemy.CalculateDirectionTowardsTarget(playerPosition);
                if (enemy.Collides)
                {
                    enemy.Direction = towardsPlayerVel;
                }
                else
                {
                    enemy.Direction = flockVel + towardsPlayerVel + avoidVel;
                }
                enemy.NormalizeDirection();
                enemy.Update(deltaTime, player);
            }
        }

        private Vector2 FlockBehaviour(Enemy enemy, float distance, float power)
        {
            List<Enemy> neighbors = enemiesList.Where(x => x.GetDistance(enemy) < distance).ToList();
            if (neighbors.Count <= 1) return Vector2.Zero;
            neighbors.Remove(enemy);
            float meanX = neighbors.Sum(x => x.GetPosition().X) / neighbors.Count();
            float meanZ = neighbors.Sum(x => x.GetPosition().Z) / neighbors.Count();
            float deltaCenterX = meanX - enemy.GetPosition().X;
            float deltaCenterZ = meanZ - enemy.GetPosition().Z;
            Vector2 output = new Vector2(deltaCenterX, deltaCenterZ) * power;
            return output;
        }

        private Vector2 AvoidanceBehaviour(Enemy enemy, float distance, float power)
        {
            List<Enemy> neighbors = enemiesList.Where(x => x.GetDistance(enemy) < distance).ToList();
            if (neighbors.Count <= 1) return Vector2.Zero;
            neighbors.Remove(enemy);
            (float sumClosenessX, float sumClosenessY) = (0, 0);
            foreach (var neighbor in neighbors)
            {
                float closeness = distance - (float)enemy.GetDistance(neighbor);
                sumClosenessX += (enemy.GetPosition().X - neighbor.GetPosition().X) * closeness;
                sumClosenessY += (enemy.GetPosition().Z - neighbor.GetPosition().Z) * closeness;
            }
            Vector2 output = new Vector2(sumClosenessX, sumClosenessY) * power;
            return output;
        }

        private Vector2 AlignBehaviour(Enemy enemy, double distance, float power)
        {
            List<Enemy> neighbors = enemiesList.Where(x => x.GetDistance(enemy) < distance).ToList();
            if (neighbors.Count <= 1) return Vector2.Zero;
            neighbors.Remove(enemy);
            float meanXvel = neighbors.Sum(x => x.Direction.X) / neighbors.Count();
            float meanYvel = neighbors.Sum(x => x.Direction.Y) / neighbors.Count();
            float dXvel = meanXvel - enemy.Direction.X;
            float dYvel = meanYvel - enemy.Direction.Y;
            Vector2 output = new Vector2(dXvel * power, dYvel * power);
            return output;
        }

        public void SpawnEnemies(int count, float distanceFroMCenterX, float distanceFroMCenterZ)
        {
            for (int i = 0; i < count; i++)
            {
                System.Random random = new System.Random();
                float x = (float)(random.NextDouble() * 2 * distanceFroMCenterX - distanceFroMCenterX);
                float z = (float)(random.NextDouble() * 2 * distanceFroMCenterZ - distanceFroMCenterZ);
                enemiesList.Add(new Enemy(new Vector3(x, 2, z), "player", "StarSparrow_Green"));
            }
        }

        public void LoadModels(ContentManager content)
        {
            foreach (Enemy enemy in enemiesList)
            {
                enemy.LoadContent(content);
                enemy.OnDestroy += DestroyControl;
            }
        }

        public void Draw(EffectHandler effectHandler, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, ContentManager content)
        {
            foreach (Enemy enemy in enemiesList)
            {
                enemy.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix, enemy.color);
                if (enemy.GetType() == typeof(AppleTree))
                {
                    AppleTree tree = (AppleTree)enemy;
                    foreach (Apple apple in tree.bullet)
                    {
                        apple.LoadContent(content);
                        apple.Draw(effectHandler, worldMatrix, viewMatrix, projectionMatrix, apple.color);
                    }
                }
            }
        }


        private void DestroyControl(object obj, EventArgs e)
        {
            enemiesList.Remove((Enemy)obj);
        }

        public List<Enemy> EnemiesList
        {
            get { return enemiesList; }
        }
    }

}
