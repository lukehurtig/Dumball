using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dumball.Collisions;

namespace Dumball
{
    public class Floor : Component
    {
        protected Texture2D texture;

        public Vector2 Position;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            }
        }

        public Floor(Texture2D t)
        {
            texture = t;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }
    }
}
