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
    public class DBMainMenu : BaseGameState
    {
        SpriteFont _font;
        private Texture2D _editorBG;
        public FetchRequest fetch;
        int buttonIndex;
        int x = 325;
        int y = 200;

        public override void HandleInput(GameTime gameTime)
        {
            
        }

        public override void LoadContent(ContentManager contentManager)
        {
            Translation = Matrix.Identity;
            _font = contentManager.Load<SpriteFont>("gameFont");
            _editorBG = contentManager.Load<Texture2D>("EditorBG");
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            
        }
        public override void Update(GameTime gameTime)
        {
            HandleButtonClicks();
            base.Update(gameTime);
        }
        private void HandleButtons(SpriteBatch spriteBatch)
        {
            int buttonX = x;
            int buttonY = y;
            string[] buttonText = { "Get All Users", "Get One User", "Update Or Delete" };
            foreach (var button in buttonText) 
            {
                spriteBatch.Draw(_editorBG, new Rectangle(buttonX, buttonY, 160, 30), Color.Gray);
                spriteBatch.DrawString(_font, button, new Vector2(buttonX + 10, buttonY + 5), Color.White);
                buttonY += 45;
            }
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
                SwitchState(new GetUsers());
            }
            if (mouseState.LeftButton == ButtonState.Pressed
                   && mouseState.X >= buttonX && mouseState.X <= buttonX + 160
                   && mouseState.Y >= buttonY + 45 && mouseState.Y <= buttonY + 75)
            {
                SwitchState(new GetOneUser());
            }
            if (mouseState.LeftButton == ButtonState.Pressed
                   && mouseState.X >= buttonX && mouseState.X <= buttonX + 160
                   && mouseState.Y >= buttonY + 90 && mouseState.Y <= buttonY + 120)
            {
                SwitchState(new Update_Delete());
            }
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            HandleButtons(spriteBatch);
            base.Render(spriteBatch);
        }
    }
}
