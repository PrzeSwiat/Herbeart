using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class MapType
    {
        public string name;
        public string type;
        public double probability;

        //Maps directions
        public static string straight = "straight";
        public static string up_down = "up_down";
        public static string up_right = "up_right";
        public static string down_up = "down_up";
        public static string down_right = "down_right";
        public static string left_down = "left_down";
        public static string left_up = "left_up";


        public MapType(string name, string type)
        {
            this.name = name;
            this.type = type;
            this.probability = 1;
        }

        //Spawn Maps
        public static MapType Map_spawn = new MapType("Maps/map1.txt", straight);

        //Straight Maps
        public static MapType Map_straight_1 = new MapType("Maps/map2.txt", straight);
        public static MapType Map_straight_2 = new MapType("Maps/map3.txt", straight);
        public static MapType Map_straight_3 = new MapType("Maps/map4.txt", straight);
        public static MapType Map_straight_4 = new MapType("Maps/map5.txt", straight);
        public static MapType[] Maps_straight = new MapType[] { Map_straight_1, Map_straight_2, Map_straight_3, Map_straight_4 };

        //Maps from Up to Down
        public static MapType Map_up_down_1 = new MapType("Maps/map_up_down_1.txt", up_down);

        //Maps from Down to Up
        public static MapType Map_down_up_1 = new MapType("Maps/map_down_up_1.txt", down_up);

        //Maps from Up to Right
        public static MapType Map_up_right_1= new MapType("Maps/map_up_right.txt", up_right);

        //Maps from Down to Right
        public static MapType Map_down_right_1 = new MapType("Maps/map_down_right_1.txt", down_right);

        //Maps from Left to Down
        public static MapType Map_left_down_1 = new MapType("Maps/map_left_down_1.txt", left_down);
        public static MapType Map_left_down_2 = new MapType("Maps/map_left_down_2.txt", left_down);
        public static MapType Map_left_down_3 = new MapType("Maps/map_left_down_3.txt", left_down);
        public static MapType[] Maps_left_down = new MapType[] { Map_left_down_1, Map_left_down_2, Map_left_down_3 };

        //Maps from Left to Up
        public static MapType Map_left_up_1 = new MapType("Maps/map_left_up_1.txt", left_up);
        public static MapType Map_left_up_2 = new MapType("Maps/map_left_up_2.txt", left_up);
        public static MapType Map_left_up_3 = new MapType("Maps/map_left_up_3.txt", left_up);
        public static MapType[] Maps_left_up = new MapType[] { Map_left_up_1, Map_left_up_2, Map_left_up_3 };

        //Loot and Forest Maps
        public static MapType Map_loot_forest_straight_1 = new MapType("Maps/map_loot_forest_straight_1.txt", straight);
        public static MapType Map_loot_forest_straight_2 = new MapType("Maps/map_loot_forest_straight_2.txt", straight);
        public static MapType Map_loot_up_down_1 = new MapType("Maps/map_loot_up_down_1.txt", up_down);
        public static MapType Map_loot_down_up_1 = new MapType("Maps/map_loot_up_down_1.txt", down_up);

        //All Maps
        public static MapType[] All_Maps = new MapType[] { Map_left_down_1, Map_left_down_2, Map_left_down_3, Map_left_up_1, Map_left_up_2, Map_left_up_3, Map_straight_1, Map_straight_2, Map_straight_3, Map_straight_4, Map_up_down_1, Map_down_up_1, Map_up_right_1, Map_down_right_1, Map_loot_forest_straight_1, Map_loot_forest_straight_2, Map_loot_up_down_1, Map_loot_down_up_1 };
        
        //Party Maps
        public static MapType Map_party_left_up = new MapType("Maps/map_party_left_up.txt", left_up);
        public static MapType Map_party_left_down = new MapType("Maps/map_party_left_down.txt", left_down);
        public static MapType Map_party_down_up = new MapType("Maps/map_party_down_up.txt", down_up);
        public static MapType Map_party_down_right = new MapType("Maps/map_party_down_right.txt", down_right);
        public static MapType Map_party_up_right = new MapType("Maps/map_party_up_right.txt", up_right);
        public static MapType Map_party_up_down = new MapType("Maps/map_party_up_down.txt", up_down);
        public static MapType[] Maps_party = new MapType[] {Map_party_left_up, Map_party_left_down, Map_party_down_up, Map_party_down_right, Map_party_up_right, Map_party_up_down };



        public static MapType GenerateRandomMap(List<MapType> maps)
        {
            double sumOfProbabilities = maps.Sum(e => e.probability);
            double randomNumber = new Random().NextDouble();

            // Znalezienie elementu, którego prawdopodobieństwo przekracza wylosowaną liczbę
            double cumulativeProbability = 0.0;

            for (int i = 0; i < maps.Count; i++)
            {
                double probability = maps[i].probability / sumOfProbabilities;
                cumulativeProbability += probability;
                if (randomNumber < cumulativeProbability)
                {
                    maps[i].probability = maps[i].probability / 2;

                    double increaseProbability = maps[i].probability / (maps.Count - 1);
                    for (int j = 0; j < maps.Count; j++)
                    {
                        if (j != i)
                        {
                            maps[j].probability += increaseProbability;
                        }
                    }

                    return maps[i];
                }
            }
            return maps[0];
        }

        public static void SetInitialProbabilities()
        {
            //Dla map z weselami
            foreach (MapType map in Maps_party)
            {
                map.probability = 1.0 / Maps_party.Length;
            }

            //Dla zwykłych map
            foreach (MapType map in All_Maps)
            {
                map.probability = 1.0 / All_Maps.Length;
            }
        }

    }
}
