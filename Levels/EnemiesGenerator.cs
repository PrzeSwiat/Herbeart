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

        private static EnemyType Mint = new EnemyType("Objects/mint", 0.65);
        private static EnemyType Apple = new EnemyType("Objects/apple", 0.15);
        private static EnemyType Nettle = new EnemyType("Objects/nettle", 0.15);
        private static EnemyType Melissa = new EnemyType("Objects/melissa", 0.05);
        private EnemyType[] enemyTypes = {Mint, Apple, Nettle, Melissa};
        public EnemiesGenerator()
        {
            enemies = new List<Enemy>();
        }


        public void prepareDifficultyLevels(int difficultyLevel)
        {
            switch (difficultyLevel)
            {
                case 1:     //Dla modułu 4 ma losować tylko mięte i pokrzywe
                    Mint.probability = 0.8;
                    Apple.probability = 0.0;
                    Nettle.probability = 0.2;
                    Melissa.probability = 0.0;
                    break;
                case 2:     //Dla modułów 6-8 ma wylosować pierwszy raz jabłko, oraz mięte i pokrzywe
                    Mint.probability = 0.75;
                    Apple.probability = 0.05;
                    Nettle.probability = 0.2;
                    Melissa.probability = 0.0;
                    break;
                case 3:     //Dla modułów 9-11 ma wylosować pierwszy raz melisse, oraz mięte, pokrzywe, jabłko
                    Mint.probability = 0.7;
                    Apple.probability = 0.1;
                    Nettle.probability = 0.15;
                    Melissa.probability = 0.05;
                    break;
                case 5:
                    Mint.probability = 0.54;
                    Apple.probability = 0.18;
                    Nettle.probability = 0.18;
                    Melissa.probability = 0.1;
                    break;
                case 6:
                    Mint.probability = 0.4;
                    Apple.probability = 0.2;
                    Nettle.probability = 0.2;
                    Melissa.probability = 0.2;
                    break;
                case 7:
                    Mint.probability = 0.3;
                    Apple.probability = 0.25;
                    Nettle.probability = 0.25;
                    Melissa.probability = 0.2;
                    break;
                case 8:
                    Mint.probability = 1.0;
                    Apple.probability = 0.0;
                    Nettle.probability = 0.0;
                    Melissa.probability = 0.0;
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
                    if (enemyTypes[i].enemyType != "Objects/mint" && (difficultyLevel != 7 || difficultyLevel != 6))
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


        public void GenerateRandomEnemies(int enemyCount, int difficultyLevel, List<Vector3> vectors)
        {
            List<Vector3> enemyPositions = prepareEnemiesPositions(vectors, enemyCount);

            for (int i = 0; i < enemyCount; i++)
            {
                string enemyType;
                if (difficultyLevel == 1 && i == 0)
                {
                    enemyType = "Objects/nettle";
                }
                else if (difficultyLevel == 2 && i == 0)
                {
                    enemyType = "Objects/apple";
                }
                else if (difficultyLevel == 3 && i == 0)
                {
                    enemyType = "Objects/melissa";
                }
                else
                {
                    enemyType = GenerateRandomEnemyTypeWithProbability(difficultyLevel).enemyType;
                }
                Vector3 enemyPosition = enemyPositions[i];
                GenerateEnemy(enemyType, enemyPosition);
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



        public List<Vector3> prepareEnemiesPositions(List<Vector3> vectors, int enemyCount)
        {
            List<EnemiesPositions> enemiesPositions = new List<EnemiesPositions>();
            vectors.Sort((a, b) =>a.Z.CompareTo(b.Z));

            int count = vectors.Count;
            int middleIndex = count / 2;
            double middleProbability = 1.0 / count;

            // Wybierz 20% najbliższych wektorów względem środkowej wartości Z
            int closestCount = (int)Math.Round(count * 0.2);
            List<Vector3> closestVectors = vectors.Take(closestCount).ToList();
            //Stwórz elementy struktury z pozycją i prawdopodobieństwem
            enemiesPositions.AddRange(setProbabilities(closestVectors, 0.6 * middleProbability));
            
            // Wybierz 40% wektorów bardziej oddalonych niż 20%
            int middleCount = (int)Math.Round(count * 0.4);
            List<Vector3> middleVectors = vectors.Skip(closestCount).Take(middleCount).ToList();
            enemiesPositions.AddRange(setProbabilities(middleVectors, 0.3 * middleProbability));

            // Wybierz ostatnie 40% wektorów najbardziej oddalonych
            List<Vector3> lastVectors = vectors.Skip(closestCount + middleCount).ToList();
            enemiesPositions.AddRange(setProbabilities(lastVectors, 0.1 * middleProbability));

            List<Vector3> positions = new List<Vector3>();
            for(int i = 0; i < enemyCount; i++)
            {
                positions.Add(GenerateEnemiesPositions(enemiesPositions).enemyPosition);
            }
            return positions;


        }

        public EnemiesPositions GenerateEnemiesPositions(List<EnemiesPositions> enemiesPositions)
        {
            // Obliczenie sumy prawdopodobieństw
            double totalProbability = 0;
            foreach (var enemy in enemiesPositions)
            {
                totalProbability += enemy.probability;
            }

            // Losowanie liczby z zakresu [0, totalProbability)
            Random random = new Random();
            double randomNumber = random.NextDouble() * totalProbability;

            // Wybieranie elementu na podstawie losowej liczby
            double cumulativeProbability = 0;
            for (int i = 0; i < enemiesPositions.Count; i++)
            {
                cumulativeProbability += enemiesPositions[i].probability;
                if (randomNumber < cumulativeProbability)
                {
                    // Ustawienie probability na 0
                    enemiesPositions[i].probability = 0;
                    return enemiesPositions[i];
                }
            }


            return enemiesPositions[0];
        }

        public List<EnemiesPositions> setProbabilities(List<Vector3> vectors, double probability)
        {
            List<EnemiesPositions> result = new List<EnemiesPositions>();
            for (int i = 0; i < vectors.Count(); i++)
            {
                EnemiesPositions enemyPosition = new EnemiesPositions(vectors[i], probability);
                result.Add(enemyPosition);
            }
            return result;
        }

        public List<Enemy> returnEnemies()
        {
            return enemies;
        }

    }
}
