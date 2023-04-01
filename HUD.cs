using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class HUD
    {
        private string _texFilename;
        private SpriteFont Font;
        private Texture2D skyTex, red, itemFrame;
        private Rectangle skyRec;
        //private int MoveSpeed = 2;
        private int BackgroundWidth, BackgroundHeight, WindowWidth, WindowHeight;
        private int BackgroundX, BackgroundY;
        private int health = 200;

        public HUD(string texFilename, int windowWidth, int windowHeight)
        {
            _texFilename = texFilename;
            BackgroundWidth = 50;
            BackgroundHeight = 50;
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            skyRec = new Rectangle(0,0, BackgroundWidth, BackgroundHeight);
        }

        public void LoadContent(ContentManager content)
        {
            skyTex = content.Load<Texture2D>(_texFilename);
            red = content.Load<Texture2D>("Red");
            itemFrame = content.Load<Texture2D>("itemFrame");
            Font = content.Load<SpriteFont>("Fonts");
        }

        public void Update(Vector3 camPos)
        {
            BackgroundX = (int)camPos.X % BackgroundWidth - BackgroundWidth;
            BackgroundY = (int)camPos.Z % BackgroundHeight - BackgroundHeight;
        }

        public void DrawBackground(SpriteBatch spriteBatch)
        {
            int numberOfWidthBG = WindowWidth / BackgroundWidth;
            int numberOfHeightBG = WindowHeight / BackgroundHeight;

            spriteBatch.Begin();
            
            for(int i = 0; i <= numberOfWidthBG + 2; i++)
            {
                skyRec.X = BackgroundX + i * BackgroundWidth;
                for (int j = 0; j <= numberOfHeightBG + 1; j++)
                {   
                    
                    skyRec.Y = BackgroundY + j * BackgroundHeight;
                    spriteBatch.Draw(skyTex, skyRec, Color.Gray);
                }
            }

            spriteBatch.End();
        }

        public void DrawFrontground(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            int[] numbers = { 1, 2, 3 };

            DrawInventory(spriteBatch, numbers);
            DrawHealthBar(spriteBatch, health);
            health -= 1;
            if (health < 0) health = 200;

            spriteBatch.End();
        }

        private void DrawHealthBar(SpriteBatch spriteBatch, int health)
        {
            Rectangle rect = new Rectangle(10, 10, health, 20);
            spriteBatch.Draw(red, rect, Color.Red);
        }

        public void DrawInventory(SpriteBatch spriteBatch, int[] numberOfItems)
        {
            Rectangle rect = new Rectangle(0, 650, 50, 50);
            for (int i = 0; i < 3; i++)
            {
                rect.X = 20 + i * 50;
                spriteBatch.Draw(itemFrame, rect, Color.Gray);
                spriteBatch.DrawString(Font, numberOfItems[i].ToString(), new Vector2(50 + i * 50, 660), Color.Black);
            }
        }



    }
}
