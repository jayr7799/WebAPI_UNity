using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NEITGameEngine.States.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEITGameEngine.States.DBStates
{
    public class GetOneUser : BaseGameState
    {
        SpriteFont _font;
        private Texture2D _editorBG;
        public FetchRequest fetch;
        int x = 225;
        int y = 425;
        string idText = "Enter Player ID Here";
        bool editing = false;
        private KeyboardState previousKeyboardState;
        bool userFound = false;
        public override void HandleInput(GameTime gameTime)
        {
            
        }

        public override void LoadContent(ContentManager contentManager)
        {
            Translation = Matrix.Identity;
            _font = contentManager.Load<SpriteFont>("gameFont");
            _editorBG = contentManager.Load<Texture2D>("EditorBG");
            fetch = new FetchRequest();
            previousKeyboardState = Keyboard.GetState();
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            
        }
        public override void Update(GameTime gameTime)
        {
            HandleButtonClicks();
            HandleKeyboardInput();
        }
        private void HandleButtons(SpriteBatch spriteBatch)
        {
            int buttonX = x;
            int buttonY = y;
            spriteBatch.Draw(_editorBG, new Rectangle(buttonX, buttonY, 160, 30), Color.Gray);
            spriteBatch.DrawString(_font, "Get One Users", new Vector2(buttonX + 45, buttonY + 5), Color.White);
            spriteBatch.Draw(_editorBG, new Rectangle(buttonX + 200, buttonY, 160, 30), Color.Gray);
            spriteBatch.DrawString(_font, "Back to Menu", new Vector2(buttonX + 235, buttonY + 5), Color.White);
            spriteBatch.Draw(_editorBG, new Rectangle(buttonX + 100, buttonY - 45, 160, 30), Color.Gray);
            spriteBatch.DrawString(_font, idText, new Vector2(buttonX + 110, buttonY - 40), Color.White);
        }
        private void HandleButtonClicks()
        {
            var mouseState = Mouse.GetState();
            int buttonX = x;
            int buttonY = y;
            if (mouseState.LeftButton == ButtonState.Pressed
                   && mouseState.X >= buttonX && mouseState.X <= buttonX + 160
                   && mouseState.Y >= buttonY && mouseState.Y <= buttonY + 30)
            {
                _ = fetch.LoadOnePlayerData("http://localhost:3000/player" + "/" + idText);
                userFound = true;
            }
            if (mouseState.LeftButton == ButtonState.Pressed
                   && mouseState.X >= buttonX + 200 && mouseState.X <= buttonX + 360
                   && mouseState.Y >= buttonY && mouseState.Y <= buttonY + 30)
            {
                SwitchState(new DBMainMenu());
                userFound = false;
            }
            if (mouseState.LeftButton == ButtonState.Pressed
               && mouseState.X >= buttonX + 100 && mouseState.X <= buttonX + 260
               && mouseState.Y >= buttonY - 45 && mouseState.Y <= buttonY + - 15)
            {
                editing = true;
                idText = "";
            }
        }
        private void HandleKeyboardInput()
        {
            var currentKeyboardState = Keyboard.GetState();

            //Check to see if we are editing width
            if (editing)
            {
                foreach (var key in currentKeyboardState.GetPressedKeys())
                {
                    if (!previousKeyboardState.IsKeyDown(key))
                    {
                        if (key >= Keys.A && key <= Keys.Z)//handles letters and numbers
                        {
                            
                            if(currentKeyboardState.IsKeyDown(Keys.LeftShift))
                            {
                                idText += key.ToString();
                            }
                            else
                            {
                                idText += key.ToString().ToLower();
                            }
                        }
                        if (key >= Keys.D0 && key <= Keys.D9)//Detects Numeric Keys 0-9
                        {
                            idText += (key - Keys.D0).ToString();
                        }
                        else if (key == Keys.Back && idText.Length > 0)//Handles backspace and deleting values
                        {
                            idText = idText.Substring(0, idText.Length - 1);
                        }
                        else if (key == Keys.Enter)
                        {
                            editing = false;
                        }
                    }
                }
            }
            previousKeyboardState = currentKeyboardState;
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            if(userFound) spriteBatch.DrawString(_font, $"Screen Name:{fetch.player.name}, User: {fetch.player.firstName}  {fetch.player.lastName}, Joined: {fetch.player.joined}, Score: {fetch.player.score}", new Vector2(100, 100), Color.White);

            HandleButtons(spriteBatch);
            base.Render(spriteBatch);
        }
    }
}
