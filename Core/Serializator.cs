using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class Serializator
    {
        private string fileName;
        private List<ScoreRecord> records;

        public Serializator(string fileName)
        {
            this.fileName = fileName;
            records = new List<ScoreRecord>();
        }

        
        public void SavePlayerName(string name)
        {
            //File.AppendAllText(fileName,"Name: " + name + " Score: " + Globals.Score + '\n');
            AddRecord(name);
        }
        
        public void AddRecord(string name)
        {
            var record = new ScoreRecord(name, Globals.Score);
            records.Clear();
            records.Add(record);
            SaveToFile();
        }

        private void SaveToFile()
        {
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                foreach (var record in records)
                {
                    writer.WriteLine($"{record.Name},{record.Score}");
                }
            }
        }

        public string LoadFromFile()
        {
            string result = "";
            StringBuilder builder = new StringBuilder();

            if (File.Exists(fileName))
            {
                records.Clear();
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                        {
                            ScoreRecord record = new ScoreRecord(parts[0], score);
                            records.Add(record);
                        }
                    }
                }
                var sortedRecords = records.OrderByDescending(r => r.Score);

                foreach (var record in sortedRecords)
                {
                    builder.Append(record.Name + " : " + record.Score + "\n");
                }
                return builder.ToString();
            }
            return result;
        }

        public void SavePlayer(Player player)
        {
            string savePlayer = JsonConvert.SerializeObject(player);
            File.WriteAllText(fileName, savePlayer);
        }

        public Player LoadPlayer()
        {
            string load = null;
            try
            {
               load = File.ReadAllText(fileName);
            }catch(Exception ex)
            {
                Debug.Write(ex);
            };
            
            if(load != null)
            {
                return JsonConvert.DeserializeObject<Player>(load);
            }
            return null;
            
        }

        class ScoreRecord
        {
            public string Name { get; }
            public int Score { get; }

            public ScoreRecord(string name, int score)
            {
                Name = name;
                Score = score;
            }
        }
    }
}