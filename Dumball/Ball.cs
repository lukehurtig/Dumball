using Dumball.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dumball
{
    public class Ball : Component
    {
        private const float ANIMATION_SPEED = 0.09f;
        private double dustAnimationTimer;
        private int dustAnimation = 0;
        private double animationTimer;
        private int animationFrame = 0;
        private int animationIndex = 0;
        private bool forwardMovement;
        private bool backMovement;
        private bool hasJumped;

        private KeyboardState currentKeyboardState;
        private GamePadState currentGamePadState1;
        private Texture2D texture;
        private Texture2D dust;
        private Vector2 position;
        private Vector2 velocity;
        private SoundEffect jumpSound;
        private SoundEffect powerUpSound;
        private BoundingCircle bounds;
        public BoundingCircle Bounds => bounds;

        public bool PinHit;
        public bool GameOver;
        public bool Active;
        public int AnimationLoops = 0;

        public Ball(Vector2 p)
        {
            position = p;
            bounds = new BoundingCircle(position + new Vector2(20, 20), 40);
            hasJumped = true;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Dumball");
            dust = content.Load<Texture2D>("DustCloud");
            jumpSound = content.Load<SoundEffect>("Sfx/Jump");
            powerUpSound = content.Load<SoundEffect>("Sfx/Powerup");
        }

        public override void Update(GameTime gameTime)
        {
            if (GameOver && animationFrame != 3)
            {
                animationIndex = 2;

                animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (animationTimer > .13f)
                {
                    animationFrame++;

                    if (animationFrame == 3) Active = false;

                    animationTimer -= .13f;
                }
            }

            else if (PinHit)
            {
                animationIndex = 1;

                animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (animationTimer > ANIMATION_SPEED)
                {
                    if (animationFrame < 3) animationFrame++;                    

                    animationTimer -= ANIMATION_SPEED;
                }
            }
            else if (!PinHit)
            {
                animationFrame = 0;
                animationIndex = 0;
                PinHit = false;
            }

            if (!GameOver)
            {
                currentGamePadState1 = GamePad.GetState(0);
                currentKeyboardState = Keyboard.GetState();

                position += velocity;

                #region Get Right/Left Input
                if (currentGamePadState1.ThumbSticks.Left.X > 0)
                {
                    forwardMovement = true;
                    backMovement = false;
                    if (position.X < 660) position.X += 3f;
                }
                else if (currentGamePadState1.ThumbSticks.Left.X < 0 && position.X > 20)
                {
                    forwardMovement = false;
                    backMovement = true;
                    position.X -= 3f;
                }
                else if (currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    forwardMovement = true;
                    backMovement = false;
                    if (position.X < 660) position.X += 3f;
                }
                else if (currentKeyboardState.IsKeyDown(Keys.Left) && position.X > 20)
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
                #endregion Get Right/Left Input

                #region Jumping Input
                if ((currentKeyboardState.IsKeyDown(Keys.Space) || currentGamePadState1.IsButtonDown(Buttons.A)) && !hasJumped)
                {
                    position.Y -= 10f;
                    velocity.Y = -5.5f;
                    hasJumped = true;
                    jumpSound.Play();
                }

                if (hasJumped || position.Y > Constants.GAME_HEIGHT - 135)
                {
                    float i = 0.88f;
                    velocity.Y += 0.15f * i;
                }

                if (position.Y >= Constants.GAME_HEIGHT - 135 )
                    hasJumped = false;

                if (!hasJumped)
                {
                    velocity.Y = 0f;

                    dustAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                    if (dustAnimationTimer > ANIMATION_SPEED)
                    {
                        dustAnimation = 1 - dustAnimation;
                        dustAnimationTimer -= ANIMATION_SPEED;
                    }
                }
                #endregion Jumping Input

                bounds.Center = position + new Vector2(40, 40);
            }            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!hasJumped && forwardMovement && Active) spriteBatch.Draw(dust, new Vector2(position.X - 30, position.Y + 18), new Rectangle(32 * dustAnimation, 0, 32, 32),
                Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0);
            else if (!hasJumped && !backMovement && Active) spriteBatch.Draw(dust, new Vector2(position.X - 13, position.Y + 33), new Rectangle(32 * dustAnimation, 0, 32, 32),
                Color.White, -0.1f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
            if (forwardMovement && Active) spriteBatch.Draw(texture, position, new Rectangle(animationFrame * 64, animationIndex * 64, 64, 64), Color.White, 0.1f,
                new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
            else if (backMovement && Active) spriteBatch.Draw(texture, new Vector2(position.X, position.Y + 5), new Rectangle(animationFrame * 64, animationIndex * 64, 64, 64),
                Color.White, -0.1f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
            else spriteBatch.Draw(texture, position, new Rectangle(animationFrame * 64, animationIndex * 64, 64, 64), Color.White, 0f, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
            
        }
    }    
}
