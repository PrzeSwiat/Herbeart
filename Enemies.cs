using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private List<Vector2> obstaclePositions;

        public Enemies()
        {
            enemiesList = new List<Enemy>();
        }

        public void AddEnemy(Enemy enemy)
        {
            enemiesList.Add(enemy);
        }

        public void AddEnemies(List<Enemy> enemies)
        {
            foreach (Enemy enemy in enemies)
            {
                enemiesList.Add(enemy);
            }
        }

        public void SetObstaclePositions(List<System.Numerics.Vector2> obstacles)
        {
            this.obstaclePositions = new List<Vector2>();
            foreach (Vector2 obstacle in obstacles)
            {
                this.obstaclePositions.Add(new Vector2(obstacle.X, obstacle.Y));
            }
        }


        public void Move(float deltaTime, Player player)
        {
            Vector3 playerPosition = player.GetPosition();
            foreach (Enemy enemy in enemiesList)
            {
                if (enemy.GetType() == typeof(Nettle))
                {
                    enemy.Update();
                    continue;
                }
                // avoid other enemies and chase player
                //Vector2 alignVel = AlignBehaviour(enemy, 20, 0.1f);
                Vector2 towardsPlayerVel = enemy.CalculateDirectionTowardsTarget(playerPosition);
                if (enemy.Collides)
                {
                    enemy.Direction = towardsPlayerVel;
                }
                else if (Vector3.Distance(enemy.GetPosition(), playerPosition) < enemy.visionRange)
                {
                    Vector2 flockVel = FlockBehaviour(enemy, 20, 0.7f);
                    Vector2 avoidOthersVelocity = AvoidanceBehaviour(enemy, 9, 0.5f);
                    Vector2 avoidObstaclesVelocity = AvoidObstacles(enemy, 10, 0.4f);
                    enemy.Direction = flockVel + towardsPlayerVel + avoidOthersVelocity + avoidObstaclesVelocity;
                    enemy.ActualSpeed = enemy.MaxSpeed;
                }
                else
                {
                    enemy.ActualSpeed = 0;
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

        private Vector2 AvoidObstacles(Enemy enemy, float distance, float power)
        {
            Vector2 output = Vector2.Zero;
            Vector3 currentPositionV3 = enemy.GetPosition();
            Vector2 currentPosition = new Vector2(currentPositionV3.X, currentPositionV3.Z);
            float detectionDistanceSquared = (float)Math.Pow(distance, 2);
            foreach (Vector2 obstaclePosition in obstaclePositions)
            {
                if (Vector2.DistanceSquared(currentPosition, obstaclePosition) > detectionDistanceSquared)
                {
                    continue;
                }
                float distanceToObstacle = Vector2.Distance(currentPosition, obstaclePosition);
                float closeness = distance - distanceToObstacle;
                output += new Vector2(currentPosition.X - obstaclePosition.X, currentPosition.Y - obstaclePosition.Y) * closeness;

            }

            return output * power;
        }

        private Vector2 AvoidanceBehaviour(Enemy enemy, float distance, float power)
        {
            List<Enemy> neighbors = enemiesList.Where(x => x.GetDistance(enemy) < distance).ToList();
            if (neighbors.Count <= 1)
            {
                return Vector2.Zero;
            }
            if (neighbors.Contains(enemy))
            {
                neighbors.Remove(enemy);
            }
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
                enemiesList.Add(new Enemy(new Vector3(x, 0, z), "Objects/player", "Textures/StarSparrow_Green"));
            }
        }

        public void LoadModels()
        {
            foreach (Enemy enemy in enemiesList)
            {
                enemy.LoadContent();
            }
        }

        public void Draw(Vector3 lightpos)
        {
            foreach (Enemy enemy in enemiesList)
            {
                enemy.DrawPlayer(lightpos);
                if (enemy.GetType() == typeof(AppleTree))
                {
                    AppleTree tree = (AppleTree)enemy;
                    foreach (Apple apple in tree.bullet)
                    {
                        apple.LoadContent();
                        apple.Draw(lightpos);
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

        public void RefreshOnDestroy()
        {
            foreach (Enemy enemy in enemiesList.ToList())
            {
                enemy.OnDestroy += DestroyControl;
            }
        }
    }

}
