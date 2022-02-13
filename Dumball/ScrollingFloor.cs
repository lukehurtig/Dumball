using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dumball
{
    public class ScrollingFloor : Component
    {
        private readonly List<Floor> tiles;

        private readonly float textureWidth;

        private float scrollingSpeed;

        public bool Activated;        

        public ScrollingFloor(Texture2D texture, float initialSpeed)
        {
            tiles = new List<Floor>();
            scrollingSpeed = initialSpeed;
            textureWidth = (float)(texture.Width * 1.5);

            for ( int i = 0; i < 6; i++ )
            {
                tiles.Add(new Floor(texture)
                {
                    Position = new Vector2((float)(i * textureWidth - Math.Min(i, i + 1)), (float)(Constants.GAME_HEIGHT - (texture.Height * 1.5)))
                });
            }
        }

        public void LoadContent(ContentManager content)
        {

        }

        private void Scroll(GameTime gameTime)
        {
            if ( Activated )
            {
                foreach (var tile in tiles)
                {
                    tile.Position.X -= (float)(scrollingSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                }
            }            
        }

        private void CheckPosition()
        {
            for ( int i = 0; i < 6; i++ )
            {
                if ( tiles[i].Position.X + textureWidth <= 0 )
                {
                    if (i == 0) tiles[i].Position.X = tiles[tiles.Count - 1].Position.X + textureWidth;
                    else tiles[i].Position.X = tiles[i - 1].Position.X + textureWidth;
                }
            }
        }

        public void UpdateSpeed(float newSpeed)
        {
            scrollingSpeed = newSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            Scroll(gameTime);
            CheckPosition();
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var tile in tiles)
                tile.Draw(gameTime, spriteBatch);
        }
    }
}
