using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
            foreach (Enemy enemy in enemiesList)
            {
                enemy.Update(deltaTime, player);
                Vector2 flockVel = FlockBehaviour(enemy, 50, 0.003);
                Vector2 avoidVel = AvoidBehaviour(enemy, 7, 0.0003);
                //enemy.Direction = flockVel + avoidVel;
                //enemy.Direction.Normalize();
                //enemy.MoveForwards();
            }
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

        private Vector2 FlockBehaviour(Enemy enemy, double distance, double power)
        {
            IEnumerable<Enemy> query = enemiesList.Where(x => x.GetDistance(enemy) < distance);
            List<Enemy> neighbors = query.ToList();
            double meanX = neighbors.Sum(x => x.GetPosition().X) / neighbors.Count();
            double meanZ = neighbors.Sum(x => x.GetPosition().Z) / neighbors.Count();
            double deltaCenterX = meanX - enemy.GetPosition().X;
            double deltaCenterZ = meanZ - enemy.GetPosition().Z;
            Vector2 output = new Vector2((float)deltaCenterX, (float)deltaCenterZ) * (float)power;
            //System.Diagnostics.Debug.WriteLine(output.ToString());
            return output;
        }

        private Vector2 AvoidBehaviour(Enemy enemy, double distance, double power)
        {

            IEnumerable<Enemy> query = enemiesList.Where(x => x.GetDistance(enemy) < distance);
            List<Enemy> neighbors = query.ToList();
            (double sumClosenessX, double sumClosenessY) = (0, 0);
            foreach (var neighbor in neighbors)
            {
                double closeness = distance - enemy.GetDistance(neighbor);
                sumClosenessX += (enemy.GetPosition().X - neighbor.GetPosition().X) * closeness;
                sumClosenessY += (enemy.GetPosition().Z - neighbor.GetPosition().Z) * closeness;
            }
            Vector2 output = new Vector2((float)sumClosenessX, (float)sumClosenessY) * (float)power;
            //System.Diagnostics.Debug.WriteLine(output.ToString());
            return output;
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
