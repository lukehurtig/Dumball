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
        private const float DUST_ANIMATION_SPEED = 0.1f;

        private bool forwardMovement;

        private bool backMovement;

        private bool hasJumped;

        private double dustAnimationTimer;

        private int dustAnimation = 0;

        private Texture2D texture;

        private Texture2D dust;              

        private float jumpHeight = 180;

        private float originalYPos;

        private Vector2 position;

        private Vector2 velocity;

        private SoundEffect jumpSound;

        private SoundEffect powerUpSound;

        private BoundingCircle bounds;

        public BoundingCircle Bounds => bounds;

        public bool Active;

        public Ball(Texture2D t, Texture2D d, Vector2 p, SoundEffect j, SoundEffect pow)
        {
            texture = t;
            dust = d;
            position = p;
            jumpSound = j;
            powerUpSound = pow;
            bounds = new BoundingCircle(p, 40);
            originalYPos = p.Y;
            hasJumped = true;
        }

        public override void Update(GameTime gameTime)
        {

            if (Active)
            {
                position += velocity;

                float rightMovement = position.X;

                if (GamePad.GetState(0).ThumbSticks.Left.X > 0)
                {
                    forwardMovement = true;
                    backMovement = false;
                    if (position.X < 660) position.X += 3f;
                }
                else if (GamePad.GetState(0).ThumbSticks.Left.X < 0 && position.X > 20)
                {
                    forwardMovement = false;
                    backMovement = true;
                    position.X -= 3f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    forwardMovement = true;
                    backMovement = false;
                    if (position.X < 660) position.X += 3f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left) && position.X > 20)
                {
                    forwardMovement = false;
                    backMovement = true;
                    position.X -= 3f;
                }
                else if (position.X > 20)
                {
                    forwardMovement = false;
                    backMovement = false;
                    position.X -= 1f;
                }

                if ((Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(0).IsButtonDown(Buttons.A)) && !hasJumped)
                {
                    position.Y -= 10f;
                    velocity.Y = -5f;
                    hasJumped = true;
                    jumpSound.Play();
                }

                if (hasJumped || position.Y > originalYPos)
                {
                    float i = 0.88f;
                    velocity.Y += 0.15f * i;
                }

                if (position.Y + 180 >= originalYPos + jumpHeight)
                    hasJumped = false;

                if (!hasJumped)
                {
                    velocity.Y = 0f;

                    dustAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                    if (dustAnimationTimer > DUST_ANIMATION_SPEED)
                    {
                        dustAnimation = 1 - dustAnimation;
                        dustAnimationTimer -= DUST_ANIMATION_SPEED;
                    }
                }
            }            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (forwardMovement && Active) spriteBatch.Draw(texture, position, new Rectangle(0, 0, 64, 64), Color.White, 0.1f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
            else if (backMovement && Active) spriteBatch.Draw(texture, new Vector2(position.X, position.Y + 5), new Rectangle(0, 0, 64, 64), Color.White, -0.1f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
            else spriteBatch.Draw(texture, position, new Rectangle(0, 0, 64, 64), Color.White, 0f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
            if (!hasJumped && forwardMovement) spriteBatch.Draw(dust, new Vector2(position.X - 30, position.Y + 18), new Rectangle(32 * dustAnimation, 0, 32, 32), 
                Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0);
        }
    }    
}
