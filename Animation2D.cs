using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;

namespace TheGame
{
    internal class Animation2D
    {

        private Texture2D texture;
        private Vector2 position;
        private Vector2 targetPosition;
        private float elapsedTime;
        private float animationDuration;
        private Vector2 scale;
        private float ang = 0f;
        public event EventHandler OnDestroy;
        public bool destroyAnimation = false;
        private SpriteFont ScoreFont;
        private bool bcanSmaller = true;
        public Animation2D(Texture2D texture, Vector3 initialPosition, Vector2 targetPosition, float animationDuration,Viewport viewport)
        {
            Vector3 projectedPosition = viewport.Project(initialPosition,
                    Globals.projectionMatrix, Globals.viewMatrix, Matrix.Identity);
            this.texture = texture;
            this.position = new Vector2(projectedPosition.X, projectedPosition.Y);
            this.targetPosition = targetPosition;
            this.animationDuration = animationDuration;
            this.elapsedTime = 0f;
            this.scale = new Vector2(0.1f,0.1f);
        }
        public Animation2D(Vector3 initialPosition, Vector2 targetPosition, float animationDuration, Viewport viewport)
        {
            Vector3 projectedPosition = viewport.Project(initialPosition,
                    Globals.projectionMatrix, Globals.viewMatrix, Matrix.Identity);
            this.position = new Vector2(projectedPosition.X, projectedPosition.Y) - new Vector2(0,80);
            this.targetPosition = targetPosition;
            this.animationDuration = animationDuration;
            this.elapsedTime = 0f;
            this.scale = new Vector2(0.1f, 0.1f);
            ScoreFont = Globals.content.Load<SpriteFont>("ScoreFont");
        }
        public Animation2D(Texture2D texture, Vector2 initialPosition, Vector2 targetPosition, float animationDuration, Viewport viewport)
        {
            
            this.texture = texture;
            this.position=initialPosition;
            this.targetPosition = targetPosition;
            this.animationDuration = animationDuration;
            this.elapsedTime = 0f;
            this.scale = new Vector2(0.2f, 0.2f);
        }
        public void UpdateScore(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            position = CalculateScore();
            
        }
        public Vector2 CalculateScore()
        {
            if (elapsedTime >= animationDuration)
            {
                EndAnimation();
                destroyAnimation = true;
                return this.position; // Jeśli czas przekracza czas trwania animacji, zwracamy pozycję końcową
            }
            else
            {
                float t = elapsedTime / animationDuration;
                Vector2 newPosition = new Vector2(position.X, position.Y - 1f);
                if (bcanSmaller)
                {
                    scale.X = scale.X + 0.1f;
                    scale.Y = scale.Y + 0.1f;
                    if (scale.X > 0.8f)
                    {
                        bcanSmaller = false;
                    }
                }
                
                return newPosition;
            }
        }
        public void DrawScore()
        {
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.DrawString(ScoreFont, "+ " + (10 * Globals.ScoreMultipler).ToString(), position + new Vector2(-3,3), Color.Black, ang, Vector2.Zero, scale, SpriteEffects.None, 0f);
            Globals.spriteBatch.DrawString(ScoreFont, "+ " + (10 * Globals.ScoreMultipler).ToString(), position,Color.White,  ang, Vector2.Zero,  scale, SpriteEffects.None, 0f);
            Globals.spriteBatch.End();
        }
        public void EndAnimation()
        {
            OnDestroy?.Invoke(this, EventArgs.Empty);
            destroyAnimation = true;
        }
        public Vector2 CalculatePosition(Vector2 startPosition, Vector2 targetPosition, float elapsedTime, float animationDuration,bool bSmaller)
        {
            if (elapsedTime >= animationDuration)
            {
                EndAnimation();
                destroyAnimation = true;
                return targetPosition; // Jeśli czas przekracza czas trwania animacji, zwracamy pozycję końcową
            }
            else
            {
                float t = elapsedTime / animationDuration;
                Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, t);
                if (!bSmaller)
                { scale = Vector2.Lerp(scale, new Vector2(0.4f, 0.4f), t); ang += 0.1f; }
                else
                    scale = Vector2.Lerp(scale, new Vector2(0.15f, 0.15f), t);
                
                return newPosition;
            }
        }

        public void Update(GameTime gameTime,bool bSmaller)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            position = CalculatePosition(position, targetPosition, elapsedTime, animationDuration,bSmaller);
            
        }
        
        public void Draw()
        {
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(texture, position, null, Color.White, ang, Vector2.Zero, scale,SpriteEffects.None, 0f);
            Globals.spriteBatch.End();
        }

    }
}
