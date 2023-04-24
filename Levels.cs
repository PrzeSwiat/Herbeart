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

namespace TheGame
{
    internal class Levels
    {
        private string _textFile; //= "D:\\Projects\\Herbeart\\map1.txt"
        private List<string> models;
        private List<string> textures;
        private Rectangle tileRect;

        public Levels(string textFile)
        {
            _textFile = textFile;
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
/*                for (int i = 0; i < tileList.Count; i++)
                {
                    Debug.Write(tileList[i]);
                    Debug.Write("\n");
                }*/
                return tileList;
            }
            else return null;
            
        }

        public Tuple<List<string>, List<string>, List<float>> DrawScene()
        {
            List<int> tileList = ReadFile();
            List<string> models = new List<string>();
            List<string> textures = new List<string>();
            List<float> level = new List<float>();
            for (int i = 0; i < tileList.Count; i++)
            {
                if (tileList[i] == 48) //0 - las
                {
                    models.Add("tree1");
                    textures.Add("green");
                    level.Add(-3.0f);
                }
                if (tileList[i] == 49) //1 - las
                {
                    models.Add("tree2");
                    textures.Add("green");
                    level.Add(-3.0f);
                }
                if (tileList[i] == 50) //2 - las
                {
                    models.Add("tree3");
                    textures.Add("green");
                    level.Add(-3.0f);
                }
                if (tileList[i] == 51) //3 - las
                {
                    models.Add("tree4");
                    textures.Add("green");
                    level.Add(-3.0f);
                }
                if (tileList[i] == 97) //a
                {
                    models.Add("test");
                    textures.Add("trawa1");
                    level.Add(-2.0f);
                }
                if (tileList[i] == 98) //b
                {
                    models.Add("test");
                    textures.Add("trawa2");
                    level.Add(-2.0f);
                }
                if (tileList[i] == 99) //c
                {
                    models.Add("test");
                    textures.Add("trawa3");
                    level.Add(-2.0f);
                }
            }
            return Tuple.Create(models,textures, level);
        }


/*        public void DrawBackground(SpriteBatch spriteBatch, int WindowWidth, int WindowHeight)
        {
            int numberOfWidthBG = WindowWidth / 100;
            int numberOfHeightBG = WindowHeight / 100;

            spriteBatch.Begin();

            for (int i = 0; i <= numberOfWidthBG + 2; i++)
            {
                tileRect.X = BackgroundX + i * BackgroundWidth;
                for (int j = 0; j <= numberOfHeightBG + 1; j++)
                {

                    tileRect.Y = BackgroundY + j * BackgroundHeight;
                    spriteBatch.Draw(skyTex, tileRect, Color.Gray);
                }
            }

            spriteBatch.End();
        }*/
    }
}
