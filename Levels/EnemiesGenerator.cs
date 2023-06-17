using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class EnemiesGenerator
    {
        private List<Enemy> enemies;
        private int enemiesCount;
        private List<Vector3> enemiesPositions;

        private static EnemyType Mint = new EnemyType("Objects/mint", 0.5);
        private static EnemyType Apple = new EnemyType("Objects/apple", 0.2);
        private static EnemyType Nettle = new EnemyType("Objects/nettle", 0.2);
        private static EnemyType Melissa = new EnemyType("Objects/melissa", 0.1);
        private EnemyType[] enemyTypes = {Mint, Apple, Nettle, Melissa};

        public EnemiesGenerator(List<Vector3> enemiesPositions)
        {
            this.enemiesCount = enemiesPositions.Count();
            this.enemiesPositions = enemiesPositions;

            enemies = new List<Enemy>();
            GenerateRandomEnemies();
        }

        public struct EnemyType
        {
            public string enemyType;
            public double probability;

            public EnemyType(string enemyType, double probability)
            {
                this.enemyType = enemyType;
                this.probability = probability;
            }
        }


        public EnemyType GenerateRandomEnemyTypeWithProbability()
        {
            Random random = new Random();

            double randomNumber = random.NextDouble();

            // Znalezienie elementu, którego prawdopodobieństwo przekracza wylosowaną liczbę
            double cumulativeProbability = 0;

            for (int i = 0; i < enemyTypes.Count(); i++)
            {
                cumulativeProbability += enemyTypes[i].probability;
                if (randomNumber < cumulativeProbability)
                {
                    return enemyTypes[i];
                }
            }

            return enemyTypes[0];
        }


        public void GenerateRandomEnemies()
        {
            if (this.enemiesCount != 0)
            {
                for (int i = 0; i < this.enemiesCount; i++)
                {
                    string enemyType = GenerateRandomEnemyTypeWithProbability().enemyType;
                    Vector3 enemyPosition = enemiesPositions[i];
                    GenerateEnemy(enemyType, enemyPosition);
                }
            }
        }

        public void GenerateEnemy(string enemyType, Vector3 enemyPosition)
        {
            switch (enemyType)
            {
                case "Objects/apple":
                    AppleTree apple = new AppleTree(enemyPosition, "Objects/jablon", "Textures/drzewotekstur");
                    apple.LoadContent();
                    enemies.Add(apple);
                    break;
                case "Objects/melissa":
                    Melissa melissa = new Melissa(enemyPosition, "Objects/melisa", "Textures/melisa");
                    melissa.LoadContent();
                    enemies.Add(melissa);
                    break;
                case "Objects/nettle":
                    Nettle nettle = new Nettle(enemyPosition, "Objects/pokrzywa", "Textures/pokrzyw");
                    nettle.LoadContent();
                    enemies.Add(nettle);
                    break;
                case "Objects/mint":
                    Mint mint = new Mint(enemyPosition, "Objects/mieta", "Textures/mieta");
                    mint.LoadContent();
                    enemies.Add(mint);
                    break;
            }
        }

        public List<Enemy> returnEnemies()
        {
            return enemies;
        }

    }
}
