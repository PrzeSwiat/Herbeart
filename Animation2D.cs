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
        private float scale = 1f;
        private float ang = 0f;

        public Animation2D(Texture2D texture, Vector2 initialPosition, Vector2 targetPosition, float animationDuration)
        {
            this.texture = texture;
            this.position = initialPosition;
            this.targetPosition = targetPosition;

            this.animationDuration = animationDuration;
            this.elapsedTime = 0f;
        }

        public Vector2 CalculatePosition(Vector2 startPosition, Vector2 targetPosition, float elapsedTime, float animationDuration)
        {
            if (elapsedTime >= animationDuration)
            {
                return targetPosition; // Jeśli czas przekracza czas trwania animacji, zwracamy pozycję końcową
            }
            else
            {
                float t = elapsedTime / animationDuration;
                Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, t);
                return newPosition;
            }
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            position = CalculatePosition(position, targetPosition, elapsedTime, animationDuration);
            ang += 0.1f;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, ang, Vector2.Zero, new Vector2(scale * 0.05f, scale * 0.05f), SpriteEffects.None, 0f);
        }

    }
}
