using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Numerics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using System.Xml.XPath;

namespace TheGame
{
    internal class Levels
    {
        private string _textFile;
        private string[] treeModels = { "tree1", "tree2", "tree3" };
        private string[] grassTextures = {"trawa1", "trawa2", "trawa3"};
        public struct Tile
        {
            public String model;
            public String texture;
            public float height;

            public Tile(String model, String texture, float height)
            {
                this.model = model;
                this.texture = texture;
                this.height = height;
            }
        }

        private List<Tile> _tiles;

        public float returnTileHeight(int index)
        {
            return this._tiles[index].height;
        }

        public String returnTileModel(int index)
        {
            return this._tiles[index].model;
        }

        public String returnTileTexture(int index)
        {
            return this._tiles[index].texture;
        }

        public Levels(string textFile)
        {
            _textFile = textFile;
            LoadScene();
        }
        public List<int> ReadFile()
        {
            List<int> tileList = new List<int>();
            if (File.Exists(_textFile))
            {
                Stream s = new FileStream(_textFile, FileMode.Open);
                while (true)
                {
                    int val = s.ReadByte();
                    if (val < 0)
                        break;
                    if (val != 32)
                    {
                        tileList.Add(val);
                    }
                }
                s.Close();
                return tileList;
                
            }
            else return null;
            
        }

        public void LoadScene()
        {
            List<int> tileList = ReadFile();
            _tiles = new List<Tile>();
        
            for (int i = 0; i < tileList.Count; i++)
            {
                switch(tileList[i]) 
                {
                    case 48: //0
                        string treeModel = GenerateRandomString(treeModels);
                        _tiles.Add(new Tile(treeModel, "green", -2.0f));
                        break;
                    case 97: //a
                        _tiles.Add(new Tile("test", "trawa1", -2.0f));
                        break; 
                    case 98:
                        _tiles.Add(new Tile("test", "trawa2", -2.0f));
                        break;
                    case 99:
                        _tiles.Add(new Tile("test", "trawa3", -2.0f));
                        break;
                }
            }
        }

        public string GenerateRandomString(string[] list)
        {
            Random random = new Random();
            int index = random.Next(list.Length);
            string generatedRandom = list[index];
            return generatedRandom;
        }

    }
}
