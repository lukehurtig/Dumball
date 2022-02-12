using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Dumball.Collisions;

namespace Dumball
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Song backgroundMusic;
        private bool _menuActive;
        private bool _gameActive;
        private bool _menuItemSelect;
        

        private Texture2D title;
        private SpriteFont start;
        private SpriteFont exit;
        private Texture2D background;
        private ScrollingFloor floor;
        private Ball dumball;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>("BGG");
            floor = new ScrollingFloor(Content.Load<Texture2D>("GroundTile"), _graphics.PreferredBackBufferHeight + 1);
            floor.Activated = true;
            dumball = new Ball(Content.Load<Texture2D>("Dumball"), new Vector2(30, _graphics.PreferredBackBufferHeight - 135));
            backgroundMusic = Content.Load<Song>("Music/BALLGAMETITLE");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            floor.Update(gameTime);
            dumball.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            floor.Draw(gameTime, _spriteBatch);
            dumball.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
