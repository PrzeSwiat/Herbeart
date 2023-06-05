using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGame.Core;

namespace TheGame
{
    

    internal class Shadows
    {
        Texture2D PlayerShadow, MelissaShadow, NettleShadow, MintShadow, AppleShadow;
        int WindowWidth, WindowHeight;

        public Shadows(int windowWidth, int windowHeight) 
        {
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
        }

        public void LoadContent()
        {
            LoadedModels models = LoadedModels.Instance;

        }

        public void Draw(Viewport viewport,SpriteBatch spriteBatch,Vector3 position)
        {
            DrawPlayerShadow(viewport, spriteBatch, position);
        }

        public void DrawPlayerShadow(Viewport viewport, SpriteBatch spriteBatch, Vector3 pos)
        {
            Vector3 projectedPosition = viewport.Project(pos,
                    Globals.projectionMatrix, Globals.viewMatrix, Matrix.Identity);
            Rectangle rect = new Rectangle((int)projectedPosition.X - 33, (int)projectedPosition.Y - 80, 160, 100);
            spriteBatch.Draw(PlayerShadow, rect, Color.Gray);
        }
    }
}
