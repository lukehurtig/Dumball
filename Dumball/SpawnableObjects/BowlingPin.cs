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
    class BowlingPin : Component
    {
        private Texture2D texture;

        private SoundEffect knockDown;

        private Vector2 position;

        private float scrollingSpeed;

        public bool PinCollide;

        private BoundingRectangle bounds;

        public BoundingRectangle Bounds => bounds;

        public bool isOffScreen
        {
            get { return position.X < -150; }
        }

        public BowlingPin(Vector2 p, float speed)
        {
            position = p;
            scrollingSpeed = speed;
            bounds = new BoundingRectangle(position.X + 4, position.Y, 32, 60);
        } 

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("SpawningObjects/Bowling_Pin");
            knockDown = content.Load<SoundEffect>("Sfx/KnockDown");
        }

        public void PlaySound()
        {
            knockDown.Play();
        }

        public void SpawnPosition(Vector2 p)
        {
            position = p;
            bounds.X = position.X + 4;
            bounds.Y = position.Y;
        }

        public void UpdateSpeed(float newSpeed)
        {
            scrollingSpeed = newSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            float distance = scrollingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            position.X -= distance;
            bounds.X -= distance;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, new Vector2(0, 0), 2.5f, SpriteEffects.None, 0);
        }
    }
}
