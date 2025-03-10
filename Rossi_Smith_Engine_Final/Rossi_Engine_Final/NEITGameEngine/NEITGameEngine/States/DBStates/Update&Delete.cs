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
    public class Update_Delete : BaseGameState
    {
        SpriteFont _font;
        private Texture2D _editorBG;
        public FetchRequest fetch;
        int x = 125;
        int y = 400;

        //text/typing vars
        private KeyboardState previousKeyboardState;
        bool editingName;
        bool editingFirstName;
        bool editingLastName;
        bool editingScore;
        bool editingID;
        string nameText = "Enter New Username";
        string fnameText = "Enter New First Name";
        string lnameText = "Enter New Last name";
        string scoreText = "Enter New Score";
        string idText = "Please Enter ID";
        public override void HandleInput(GameTime gameTime)
        {

        }

        public override void LoadContent(ContentManager contentManager)
        {
            Translation = Matrix.Identity;
            _font = contentManager.Load<SpriteFont>("gameFont");
            _editorBG = contentManager.Load<Texture2D>("EditorBG");
            fetch = new FetchRequest();
        }

        public override void UnloadContent(ContentManager contentManager)
        {

        }
        public override void Update(GameTime gameTime)
        {
            HandleKeyboardInput();
            HandleOptionButtonCLicks();
            HandleTextButtonClicks();
        }
        private void HandleButtons(SpriteBatch spriteBatch)
        {
            int buttonX = x;
            int buttonY = y;
            string[] optionButtons = { "Update Player", "Delete Player", "Main Menu" };

            foreach (string button in optionButtons)
            {
                spriteBatch.Draw(_editorBG, new Rectangle(buttonX, buttonY, 160, 30), Color.Gray);
                spriteBatch.DrawString(_font, button, new Vector2(buttonX + 35, buttonY + 5), Color.White);
                buttonX += 200;
            }
            int bX = 325;
            int bY = 100;
            for (int i = 0; i < 5; i++)
            {
                spriteBatch.Draw(_editorBG, new Rectangle(bX, bY, 160, 30), Color.Gray);
                bY += 45;
            }
            spriteBatch.DrawString(_font, idText, new Vector2(330,105), Color.White);
            spriteBatch.DrawString(_font, nameText, new Vector2(330,150), Color.White);
            spriteBatch.DrawString(_font, fnameText, new Vector2(330,195), Color.White);
            spriteBatch.DrawString(_font, lnameText, new Vector2(330,240), Color.White);
            spriteBatch.DrawString(_font, scoreText, new Vector2(330,285), Color.White);
        }
        private void HandleTextButtonClicks()
        {
            var mouseState = Mouse.GetState();
            int bX = 325;
            int bY = 100;
            if (mouseState.LeftButton == ButtonState.Pressed
                   && mouseState.X >= bX && mouseState.X <= bX + 160
                   && mouseState.Y >= bY && mouseState.Y <= bY + 30)
            {
                editingID = true;
                editingName = false;
                editingFirstName = false;
                editingLastName = false;
                editingScore = false;
                idText = "";
            }
            if (mouseState.LeftButton == ButtonState.Pressed
                   && mouseState.X >= bX && mouseState.X <= bX + 160
                   && mouseState.Y >= bY + 45 && mouseState.Y <= bY + 75)
            {
                editingID = false;
                editingName = true;
                editingFirstName = false;
                editingLastName = false;
                editingScore = false;
                nameText = "";
            }
            if (mouseState.LeftButton == ButtonState.Pressed
                   && mouseState.X >= bX && mouseState.X <= bX + 160
                   && mouseState.Y >= bY + 90 && mouseState.Y <= bY + 120)
            {
                editingID = false;
                editingName = false;
                editingFirstName = true;
                editingLastName = false;
                editingScore = false;
                fnameText = "";
            }
            if (mouseState.LeftButton == ButtonState.Pressed
                   && mouseState.X >= bX && mouseState.X <= bX + 160
                   && mouseState.Y >= bY + 135 && mouseState.Y <= bY + 165)
            {
                editingID = false;
                editingName = false;
                editingFirstName = false;
                editingLastName = true;
                editingScore = false;
                lnameText = "";
            }
            if (mouseState.LeftButton == ButtonState.Pressed
                   && mouseState.X >= bX && mouseState.X <= bX + 160
                   && mouseState.Y >= bY + 180 && mouseState.Y <= bY + 210)
            {
                editingID = false;
                editingName = false;
                editingFirstName = false;
                editingLastName = false;
                editingScore = true;
                scoreText = "";
            }
        }
        private void HandleOptionButtonCLicks()
        {
            var mouseState = Mouse.GetState();
            int buttonX = x;
            int buttonY = y;
            if (mouseState.LeftButton == ButtonState.Pressed
                   && mouseState.X >= buttonX && mouseState.X <= buttonX + 160
                   && mouseState.Y >= buttonY && mouseState.Y <= buttonY + 30)
            {
                 _ = fetch.UpdatePlayerData("http://localhost:3000/updatePlayer", idText, nameText, fnameText, lnameText, Convert.ToInt32(scoreText));               
            }
            if (mouseState.LeftButton == ButtonState.Pressed
                   && mouseState.X >= buttonX + 200 && mouseState.X <= buttonX + 360
                   && mouseState.Y >= buttonY && mouseState.Y <= buttonY + 30)
            {
                _ = fetch.DeletePlayerData("http://localhost:3000/deletePlayer", idText);
            }
            if (mouseState.LeftButton == ButtonState.Pressed
                   && mouseState.X >= buttonX + 400 && mouseState.X <= buttonX + 560
                   && mouseState.Y >= buttonY && mouseState.Y <= buttonY + 30)
            {
                SwitchState(new DBMainMenu());
            }
        }
        
        private void HandleKeyboardInput()
        {
            var currentKeyboardState = Keyboard.GetState();
            if (editingID)
            {
                foreach (var key in currentKeyboardState.GetPressedKeys())
                {
                    if (!previousKeyboardState.IsKeyDown(key))
                    {
                        if (key >= Keys.A && key <= Keys.Z)//handles letters and numbers
                        {

                            if (currentKeyboardState.IsKeyDown(Keys.LeftShift))
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
                            editingID = false;
                        }
                    }
                }
            }
            //Check to see if we are editing width
            if (editingName)
            {
                foreach (var key in currentKeyboardState.GetPressedKeys())
                {
                    if (!previousKeyboardState.IsKeyDown(key))
                    {
                        if (key >= Keys.A && key <= Keys.Z)//handles letters and numbers
                        {

                            if (currentKeyboardState.IsKeyDown(Keys.LeftShift))
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
                            editingName = false;
                        }
                    }
                }
            }
            if (editingFirstName)
            {
                foreach (var key in currentKeyboardState.GetPressedKeys())
                {
                    if (!previousKeyboardState.IsKeyDown(key))
                    {
                        if (key >= Keys.A && key <= Keys.Z)//handles letters and numbers
                        {

                            if (currentKeyboardState.IsKeyDown(Keys.LeftShift))
                            {
                                fnameText += key.ToString();
                            }
                            else
                            {
                                fnameText += key.ToString().ToLower();
                            }
                        }
                        if (key >= Keys.D0 && key <= Keys.D9)//Detects Numeric Keys 0-9
                        {
                            fnameText += (key - Keys.D0).ToString();
                        }
                        else if (key == Keys.Back && fnameText.Length > 0)//Handles backspace and deleting values
                        {
                            fnameText = nameText.Substring(0, fnameText.Length - 1);
                        }
                        else if (key == Keys.Enter)
                        {
                            editingFirstName = false;
                        }
                    }
                }
            }
            if (editingLastName)
            {
                foreach (var key in currentKeyboardState.GetPressedKeys())
                {
                    if (!previousKeyboardState.IsKeyDown(key))
                    {
                        if (key >= Keys.A && key <= Keys.Z)//handles letters and numbers
                        {

                            if (currentKeyboardState.IsKeyDown(Keys.LeftShift))
                            {
                                lnameText += key.ToString();
                            }
                            else
                            {
                                lnameText += key.ToString().ToLower();
                            }
                        }
                        if (key >= Keys.D0 && key <= Keys.D9)//Detects Numeric Keys 0-9
                        {
                            lnameText += (key - Keys.D0).ToString();
                        }
                        else if (key == Keys.Back && lnameText.Length > 0)//Handles backspace and deleting values
                        {
                            lnameText = nameText.Substring(0, lnameText.Length - 1);
                        }
                        else if (key == Keys.Enter)
                        {
                            editingLastName = false;
                        }
                    }
                }
            }
            if (editingScore)
            {
                foreach (var key in currentKeyboardState.GetPressedKeys())
                {
                    if (!previousKeyboardState.IsKeyDown(key))
                    {
                        if (key >= Keys.A && key <= Keys.Z)//handles letters and numbers
                        {

                            if (currentKeyboardState.IsKeyDown(Keys.LeftShift))
                            {
                                scoreText += key.ToString();
                            }
                            else
                            {
                                scoreText += key.ToString().ToLower();
                            }
                        }
                        if (key >= Keys.D0 && key <= Keys.D9)//Detects Numeric Keys 0-9
                        {
                            scoreText += (key - Keys.D0).ToString();
                        }
                        else if (key == Keys.Back && scoreText.Length > 0)//Handles backspace and deleting values
                        {
                            scoreText = nameText.Substring(0, scoreText.Length - 1);
                        }
                        else if (key == Keys.Enter)
                        {
                            editingScore = false;
                        }
                    }
                }
            }
            previousKeyboardState = currentKeyboardState;
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "Enter new data", new Vector2(150,435), Color.White);
            spriteBatch.DrawString(_font, "into all fields to update", new Vector2(128,450), Color.White);
            spriteBatch.DrawString(_font, "Enter Player ID", new Vector2(353,435), Color.White);
            spriteBatch.DrawString(_font, "only to delete", new Vector2(358,450), Color.White);

            HandleButtons(spriteBatch);
            base.Render(spriteBatch);
        }
    }
}
