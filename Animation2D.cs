using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void EndAnimation()
        {
            OnDestroy?.Invoke(this, EventArgs.Empty);
            destroyAnimation = true;
        }
        public Vector2 CalculatePosition(Vector2 startPosition, Vector2 targetPosition, float elapsedTime, float animationDuration)
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
                scale = Vector2.Lerp(scale, new Vector2(0.4f, 0.4f), t);
                ang += 0.1f;
                return newPosition;
            }
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            position = CalculatePosition(position, targetPosition, elapsedTime, animationDuration);
            
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, null, Color.White, ang, Vector2.Zero, scale,SpriteEffects.None, 0f);
            spriteBatch.End();
        }

    }
}
