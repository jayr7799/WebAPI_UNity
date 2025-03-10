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
    public class GetUsers : BaseGameState
    {
        SpriteFont _font;
        private Texture2D _editorBG;
        public FetchRequest fetch;
        int x = 225;
        int y = 425;
        bool drawPlayers = false;
        public override void HandleInput(GameTime gameTime)
        {

        }

        public override async void LoadContent(ContentManager contentManager)
        {
            Translation = Matrix.Identity;
            _font = contentManager.Load<SpriteFont>("gameFont");
            _editorBG = contentManager.Load<Texture2D>("EditorBG");
            fetch = new FetchRequest();
            await fetch.LoadPlayerData("http://localhost:3000/player");

        }

        public override void UnloadContent(ContentManager contentManager)
        {

        }
        public override void Update(GameTime gameTime)
        {
           HandleButtonClicks();
        }
        private void HandleButtons(SpriteBatch spriteBatch)
        {
            int buttonX = x;
            int buttonY = y;
            //spriteBatch.Draw(_editorBG, new Rectangle(buttonX, buttonY, 160, 30), Color.Gray);
            //spriteBatch.DrawString(_font, "Get Users", new Vector2(buttonX + 45, buttonY + 5), Color.White);
            spriteBatch.Draw(_editorBG, new Rectangle(buttonX + 100, buttonY, 160, 30), Color.Gray);
            spriteBatch.DrawString(_font, "Back to Menu", new Vector2(buttonX + 135, buttonY + 5), Color.White);
        }
        private async void HandleButtonClicks()
        {
            var mouseState = Mouse.GetState();
            int buttonX = x;
            int buttonY = y;
            //if (mouseState.LeftButton == ButtonState.Pressed
            //       && mouseState.X >= buttonX && mouseState.X <= buttonX + 160
            //       && mouseState.Y >= buttonY && mouseState.Y <= buttonY + 30)
            //{
            //    drawPlayers = true;
            //}
            if (mouseState.LeftButton == ButtonState.Pressed
                   && mouseState.X >= buttonX + 100 && mouseState.X <= buttonX + 260
                   && mouseState.Y >= buttonY && mouseState.Y <= buttonY + 30)
            {
                SwitchState(new MainMenu());
            }
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            //if (drawPlayers)
            //{
                    fetch.DrawPlayerData(new Vector2(300, 200), spriteBatch, _font);
            //}
            HandleButtons(spriteBatch);
            base.Render(spriteBatch);
        }
    }
}
