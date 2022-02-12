using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Dumball.Collisions;

namespace Dumball
{
    public class Ball : Component
    {
        private Texture2D texture;        

        private float jumpHeight = 180;

        private float originalYPos;

        private Vector2 position;

        private Vector2 velocity;

        private BoundingCircle bounds;

        public BoundingCircle Bounds => bounds;

        public bool Active;

        public bool hasJumped;

        public Ball(Texture2D t, Vector2 p)
        {
            texture = t;
            position = p;
            bounds = new BoundingCircle(p, 40);
            originalYPos = p.Y;
            hasJumped = true;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;

            if (GamePad.GetState(0).ThumbSticks.Left.X > 0) position.X = GamePad.GetState(0).ThumbSticks.Left.X * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                position.X += 3f;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                position.X -= 3f;
            }
            
            if ((Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(0).IsButtonDown(Buttons.A)) && !hasJumped)
            {
                position.Y -= 10f;
                velocity.Y = -5f;
                hasJumped = true;
            }

            if (hasJumped || position.Y > originalYPos)
            {
                float i = 0.88f;
                velocity.Y += 0.15f * i;
            }

            if (position.Y + 180 >= originalYPos + jumpHeight)
                hasJumped = false;

            if (!hasJumped)
                velocity.Y = 0f;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(Active) spriteBatch.Draw(texture, position, new Rectangle(0, 0, 64, 64), Color.White, 0.1f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
            else spriteBatch.Draw(texture, position, new Rectangle(0, 0, 64, 64), Color.White, 0f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
        }
    }    
}
