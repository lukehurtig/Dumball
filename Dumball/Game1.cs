using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Dumball.Collisions;
using Dumball.SpawnableObjects;

namespace Dumball
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Song titleMusic;
        private Song gameplayMusic;
        private bool menuActive;
        private bool gameActive;
        private bool hazardCollide;
        private float scrollingSpeed = 150f;
        private double randomSpawnTime = 0;

        private Texture2D title;
        private SpriteFont start;
        private SpriteFont exit;
        //private SpriteFont instructions;
        //private SpriteFont score;
        private Texture2D ball;
        private Texture2D rectangle;
        private Texture2D background;
        private ScrollingFloor floor;
        private List<BowlingPin> bowlingPins;
        private List<Hazard> hazards;
        private Ball dumball;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            _graphics.PreferredBackBufferWidth = Constants.GAME_WIDTH;
            _graphics.PreferredBackBufferHeight = Constants.GAME_HEIGHT;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            bowlingPins = new List<BowlingPin>
            {
                new BowlingPin(new Vector2(-100, 0), 0),
                new BowlingPin(new Vector2(-100, 0), 0),
                new BowlingPin(new Vector2(-100, 0), 0),
                new BowlingPin(new Vector2(-100, 0), 0),
                new BowlingPin(new Vector2(-100, 0), 0),
                new BowlingPin(new Vector2(-100, 0), 0)
            };

            hazards = new List<Hazard>
            {
                new Hazard(new Vector2(-100, 0), 0),
                new Hazard(new Vector2(-100, 0), 0),
                new Hazard(new Vector2(-100, 0), 0),
                new Hazard(new Vector2(-100, 0), 0),
                new Hazard(new Vector2(-100, 0), 0),
                new Hazard(new Vector2(-100, 0), 0)
            };

            dumball = new Ball(new Vector2(20, Constants.GAME_HEIGHT - 135));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            dumball.LoadContent(Content);

            foreach (var pin in bowlingPins)
            {
                pin.LoadContent(Content);
            }

            foreach (var hazard in hazards)
            {
                hazard.LoadContent(Content);
            }

            background = Content.Load<Texture2D>("BGG");

            ball = Content.Load<Texture2D>("circle");
            rectangle = Content.Load<Texture2D>("Rectangle");

            floor = new ScrollingFloor(Content.Load<Texture2D>("GroundTile"), scrollingSpeed);

            dumball.Active = true;
            floor.Activated = true;
            gameActive = true;

            titleMusic = Content.Load<Song>("Music/BALLGAMETITLE");
            gameplayMusic = Content.Load<Song>("Music/BALLGAMEPLAY");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(titleMusic);
        }

        public void RandomizeSpawn()
        {
            System.Random rand = new System.Random();

            randomSpawnTime = (rand.NextDouble() * 2) + 1;
            double spawn = rand.NextDouble();

            if (spawn < Constants.HAZARD_SPAWN)
            {
                for (int i = 0; i < hazards.Count; i++)
                {
                    if (hazards[i].isOffScreen)
                    {
                        hazards[i].SpawnPosition(new Vector2(Constants.GAME_WIDTH + 20, Constants.GAME_HEIGHT - 125));
                        hazards[i].UpdateSpeed(scrollingSpeed);
                        i = hazards.Count;
                    }
                }
            }
            else
            {
                for (int i = 0; i < bowlingPins.Count; i++)
                {
                    if (bowlingPins[i].isOffScreen)
                    {
                        bowlingPins[i].SpawnPosition(new Vector2(Constants.GAME_WIDTH + 20, Constants.GAME_HEIGHT - 115));
                        bowlingPins[i].UpdateSpeed(scrollingSpeed);
                        i = bowlingPins.Count;
                    }
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (menuActive)
            {
                scrollingSpeed = 150f;
                gameActive = true;
                dumball.Active = true;
                dumball.GameOver = false;
                menuActive = false;
            }

            else if (gameActive)
            {
                randomSpawnTime -= gameTime.ElapsedGameTime.TotalSeconds;

                if (randomSpawnTime <= 0)
                {
                    RandomizeSpawn();
                }

                if (dumball.Active)
                {
                    scrollingSpeed += 0.07f;

                    floor.UpdateSpeed(scrollingSpeed);

                    dumball.Update(gameTime);

                    if (!dumball.GameOver)
                    {
                        floor.Update(gameTime);

                        #region Hazard Updates
                        foreach (Hazard hazard in hazards)
                        {
                            if (!hazard.isOffScreen) hazard.UpdateSpeed(scrollingSpeed);
                            else hazard.UpdateSpeed(0);

                            if (!hazard.isOffScreen) hazard.Update(gameTime);

                            if (dumball.Bounds.CollidesWith(hazard.Bounds))
                            {
                                if (!hazardCollide) hazard.PlaySound();
                                hazardCollide = true;
                                dumball.GameOver = true;
                            }
                            else hazardCollide = false;
                        }
                        #endregion Hazard Updates

                        #region Bowling Pin Updates
                        bool PinCollide = false;

                        foreach (BowlingPin pin in bowlingPins)
                        {
                            if (!pin.isOffScreen) pin.UpdateSpeed(scrollingSpeed);
                            else pin.UpdateSpeed(0);

                            if (!pin.isOffScreen) pin.Update(gameTime);

                            if (dumball.Bounds.CollidesWith(pin.Bounds))
                            {
                                PinCollide = true;
                                if (!pin.PinCollide) pin.PlaySound();
                                pin.PinCollide = true;
                            }
                            else
                            {
                                pin.PinCollide = false;
                            }

                            dumball.PinHit = PinCollide;
                        }


                        #endregion Bowling Pin Updates   
                    }
                }
                else
                {
                    foreach (var pin in bowlingPins)
                    {
                        pin.SpawnPosition(new Vector2(-100, 0));
                    }
                    foreach (var hazard in hazards)
                    {
                        hazard.SpawnPosition(new Vector2(-100, 0));
                    }

                    gameActive = false;
                    menuActive = true;
                    MediaPlayer.Play(titleMusic);
                }
            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            floor.Draw(gameTime, _spriteBatch);
            foreach (var pin in bowlingPins)
            {
                pin.Draw(gameTime, _spriteBatch);
            }
            foreach (var hazard in hazards)
            {
                hazard.Draw(gameTime, _spriteBatch);
            }
            dumball.Draw(gameTime, _spriteBatch);

            #region Collision Drawing
            /*var rect = new Rectangle((int)(dumball.Bounds.Center.X - dumball.Bounds.Radius),
                                         (int)(dumball.Bounds.Center.Y - dumball.Bounds.Radius),
                                         (int)(2 * dumball.Bounds.Radius), (int)(2 * dumball.Bounds.Radius));
            _spriteBatch.Draw(ball, rect, Color.White);
            var rect2 = new Rectangle();
            foreach (var pin in bowlingPins)
            {
                rect2 = new Rectangle((int)pin.Bounds.Left, (int)pin.Bounds.Top,
                                         (int)(pin.Bounds.Width), (int)(pin.Bounds.Height));
                _spriteBatch.Draw(rectangle, rect2, Color.White);
            }

            var rect3 = new Rectangle();
            foreach (var hazard in hazards)
            {
                rect3 = new Rectangle((int)(hazard.Bounds.Center.X - hazard.Bounds.Radius),
                                         (int)(hazard.Bounds.Center.Y - hazard.Bounds.Radius),
                                         (int)(2 * hazard.Bounds.Radius), (int)(2 * hazard.Bounds.Radius));

                _spriteBatch.Draw(ball, rect3, Color.White);
            }*/
            #endregion Collision Drawing

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
