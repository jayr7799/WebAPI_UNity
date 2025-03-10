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
    public class AddUser : BaseGameState
    {
        SpriteFont _font;
        private Texture2D _editorBG;
        public FetchRequest fetch;
        int x = 225;
        int y = 425;
        string nameText = "Enter Player ID Here";
        bool editing = false;
        private KeyboardState previousKeyboardState;
        bool submitted = false;
        int finalScore;
        
        public AddUser(int score)
        {
            this.finalScore = score;
        }

        public override void HandleInput(GameTime gameTime)
        {
            
        }
        public override void UnloadContent(ContentManager contentManager)
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

        public override void Update(GameTime gameTime)
        {
            if (submitted)
            {
                _ = fetch.SendPlayerData("http://localhost:3000/sentdatatodb", nameText, finalScore);
                submitted = false;
            }
            HandleButtonClicks();
            HandleKeyboardInput();           
        }
        private void HandleButtons(SpriteBatch spriteBatch)
        {
            int buttonX = x;
            int buttonY = y;
            spriteBatch.Draw(_editorBG, new Rectangle(buttonX, buttonY, 160, 30), Color.Gray);
            spriteBatch.DrawString(_font, "Submit", new Vector2(buttonX + 45, buttonY + 5), Color.White);
            spriteBatch.Draw(_editorBG, new Rectangle(buttonX + 200, buttonY, 160, 30), Color.Gray);
            spriteBatch.DrawString(_font, "Back to Menu", new Vector2(buttonX + 235, buttonY + 5), Color.White);            
            
            spriteBatch.Draw(_editorBG, new Rectangle(buttonX + 400, buttonY, 160, 30), Color.Gray);
            spriteBatch.DrawString(_font, "Leaderboards", new Vector2(buttonX + 435, buttonY + 5), Color.White);

            spriteBatch.Draw(_editorBG, new Rectangle(buttonX + 100, buttonY - 45, 160, 30), Color.Gray);
            spriteBatch.DrawString(_font, nameText, new Vector2(buttonX + 110, buttonY - 40), Color.White);

            spriteBatch.DrawString(_font, $"Score: {finalScore}", new Vector2(buttonX + 110, buttonY - 100), Color.White);
            spriteBatch.DrawString(_font, "Please Enter name", new Vector2(buttonX + 110, buttonY - 75), Color.White);
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
                submitted = true;
            }
            if (mouseState.LeftButton == ButtonState.Pressed
                   && mouseState.X >= buttonX + 200 && mouseState.X <= buttonX + 360
                   && mouseState.Y >= buttonY && mouseState.Y <= buttonY + 30)
            {
                SwitchState(new MainMenu());
            }
            if (mouseState.LeftButton == ButtonState.Pressed
                   && mouseState.X >= buttonX + 400 && mouseState.X <= buttonX + 560
                   && mouseState.Y >= buttonY && mouseState.Y <= buttonY + 30)
            {
                SwitchState(new GetUsers());
            }
            if (mouseState.LeftButton == ButtonState.Pressed
               && mouseState.X >= buttonX + 100 && mouseState.X <= buttonX + 260
               && mouseState.Y >= buttonY - 45 && mouseState.Y <= buttonY + - 15)
            {
                editing = true;
                nameText = "";
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
                                nameText += key.ToString();
                            }
                            else
                            {
                                nameText += key.ToString().ToLower();
                            }
                        }
                        if (key >= Keys.D0 && key <= Keys.D9)//Detects Numeric Keys 0-9
                        {
                            nameText += (key - Keys.D0).ToString();
                        }
                        else if (key == Keys.Back && nameText.Length > 0)//Handles backspace and deleting values
                        {
                            nameText = nameText.Substring(0, nameText.Length - 1);
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
            HandleButtons(spriteBatch);
            base.Render(spriteBatch);
        }
    }
}
