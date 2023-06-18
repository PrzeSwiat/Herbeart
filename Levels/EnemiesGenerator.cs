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

        private static EnemyType Mint = new EnemyType("Objects/mint", 0.8);
        private static EnemyType Apple = new EnemyType("Objects/apple", 0.08);
        private static EnemyType Nettle = new EnemyType("Objects/nettle", 0.08);
        private static EnemyType Melissa = new EnemyType("Objects/melissa", 0.04);
        private EnemyType[] enemyTypes = {Mint, Apple, Nettle, Melissa};

        public EnemiesGenerator()
        {
            enemies = new List<Enemy>();
        }


        public void prepareDifficultyLevels(int difficultyLevel)
        {
            switch (difficultyLevel)
            {
                case 2:
                    Mint.probability = 0.6;
                    Apple.probability = 0.15;
                    Nettle.probability = 0.15;
                    Melissa.probability = 0.1;
                    break;
                case 3:
                    Mint.probability = 0.4;
                    Apple.probability = 0.2;
                    Nettle.probability = 0.2;
                    Melissa.probability = 0.2;
                    break;
                case 4:
                    Mint.probability = 0.3;
                    Apple.probability = 0.25;
                    Nettle.probability = 0.25;
                    Melissa.probability = 0.2;
                    break;
            }
        }

        public EnemyType GenerateRandomEnemyTypeWithProbability(int difficultyLevel)
        {
            prepareDifficultyLevels(difficultyLevel);
            double sumOfProbabilities = enemyTypes.Sum(e => e.probability);
            double randomNumber = new Random().NextDouble();

            // Znalezienie elementu, którego prawdopodobieństwo przekracza wylosowaną liczbę
            double cumulativeProbability = 0.0;

            for (int i = 0; i < enemyTypes.Length; i++)
            {
                double probability = enemyTypes[i].probability/ sumOfProbabilities;
                cumulativeProbability += probability;
                if (randomNumber < cumulativeProbability)
                {
                    if (enemyTypes[i].enemyType != "Objects/mint" || difficultyLevel > 2)
                    {
                        enemyTypes[i].probability = enemyTypes[i].probability / 2;

                        double increaseProbability = enemyTypes[i].probability / (enemyTypes.Length - 1);
                        for (int j = 0; j < enemyTypes.Length; j++)
                        {
                            if (j != i)
                            {
                                enemyTypes[j].probability += increaseProbability;
                            }
                        }
                    }

                    return enemyTypes[i];
                }
            }

            return enemyTypes[0];
        }


        public void GenerateRandomEnemies(List<Vector3> enemiesPositions, int difficultyLevel)
        {
            if (enemiesPositions.Count() != 0)
            {
                for (int i = 0; i < enemiesPositions.Count(); i++)
                {
                    string enemyType = GenerateRandomEnemyTypeWithProbability(difficultyLevel).enemyType;
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
