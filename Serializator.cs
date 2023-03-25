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
        public Serializator(string fileName)
        {
            this.fileName = fileName;
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

    }
}