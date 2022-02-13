using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dumball
{
    public class Menu : Component
    {
        private readonly Texture2D title;
        private SpriteFont start;
        private SpriteFont exit;
        private Vector2 position;
        private readonly float yPosition;
        private bool menuSelection;
        private KeyboardState keyboardState;
        private KeyboardState priorKeyboardState;
        private GamePadState gamePadState;
        private GamePadState priorGamePadState;
        public bool Active;
        public bool Exit;
        //private SpriteFont instructions;

        public Menu(Vector2 p)
        {
            position = p;
            yPosition = p.Y;
        }

        public void LoadContent(ContentManager content)
        {
            content.Load<Texture2D>("Dumball_Logo");
        }

        public override void Update(GameTime gameTime)
        {
            if (Active && position.Y < yPosition)
            {
                position.Y += 50f;
            }
            else if (!Active && position.Y > yPosition - 200)
            {
                position.Y -= 50f;
            }

            if (Active && position.Y >= yPosition)
            {
                priorGamePadState = gamePadState;
                gamePadState = GamePad.GetState(0);
                priorKeyboardState = keyboardState;
                keyboardState = Keyboard.GetState();

                if (gamePadState.ThumbSticks.Left.Y != 0 && priorGamePadState.ThumbSticks.Left.Y == 0) menuSelection = !menuSelection;
                else if (keyboardState.IsKeyDown(Keys.Up) && priorKeyboardState.IsKeyUp(Keys.Up)) menuSelection = !menuSelection;
                else if (keyboardState.IsKeyDown(Keys.Down) && priorKeyboardState.IsKeyUp(Keys.Down)) menuSelection = !menuSelection;

                if ((gamePadState.IsButtonDown(Buttons.A) || keyboardState.IsKeyDown(Keys.Enter)) && menuSelection) Active = false;
                else if ((gamePadState.IsButtonDown(Buttons.A) || keyboardState.IsKeyDown(Keys.Enter)) && !menuSelection) Exit = true;
            }
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

    }
}
