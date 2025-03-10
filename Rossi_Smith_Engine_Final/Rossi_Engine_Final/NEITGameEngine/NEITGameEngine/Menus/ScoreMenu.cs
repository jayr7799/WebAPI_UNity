using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.Objects;
using NEITGameEngine.Objects.Base;
using NEITGameEngine.World;

namespace NEITGameEngine.Menus
{
    public class ScoreMenu:BaseGameObject
    {
        int _score;
        public Vector2 _position;
        SpriteFont _font;
        public PlayerSprite _playerSprite;

        //timer
        public int waitTime = 30000;
        public int originalWaitTime = 30000;
        public int levelIndex = 0;
        public ScoreMenu(SpriteFont font, Vector2 position, PlayerSprite playerSprite)
        {
            _score = 0;
            _font = font;
            _position = position;
            _playerSprite = playerSprite;
        }

        public void AddScore(int points)
        {
            _score += points;
        }

        public void ResetScore()
        {
            _score = 0;
        }

        public int GetScore()
        {
            return _score;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            string scoreText = $"Score: {_score}";
            spriteBatch.DrawString(_font, scoreText, _playerSprite.Position + _position, Color.OrangeRed);

            string Timer = $"Time: {waitTime / 1000}";
            spriteBatch.DrawString(_font, Timer, _playerSprite.Position + new Vector2(_position.X, _position.Y + 25), Color.OrangeRed);
        }
    }
}
