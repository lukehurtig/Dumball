using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Dumball.Collisions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dumball.SpawnableObjects
{
    public class Hazard : Component
    {
        private Texture2D texture;

        private SoundEffect gameOver;

        private Vector2 position;

        private float scrollingSpeed;

        private BoundingCircle bounds;

        public BoundingCircle Bounds => bounds;

        public bool isOffScreen
        {
            get { return position.X < -150; }
        }

        public Hazard(Vector2 p, float speed)
        {
            position = p;
            scrollingSpeed = speed;
            bounds = new BoundingCircle(position + new Vector2(40, 40), 40);
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("SpawningObjects/Hazard");
            gameOver = content.Load<SoundEffect>("Sfx/Death");
        }

        public void PlaySound()
        {
            gameOver.Play();
        }

        public void SpawnPosition(Vector2 p)
        {
            position = p;
            bounds.Center = position + new Vector2(40, 40);
        }

        public void UpdateSpeed(float newSpeed)
        {
            scrollingSpeed = newSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            float distance = scrollingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            position.X -= distance;
            bounds.Center.X -= distance;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, new Vector2(0, 0), 2.5f, SpriteEffects.None, 0);
        }
    }
}
