using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NEITGameEngine.States.Base;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using System.Reflection.Metadata;

namespace NEITGameEngine.States
{
    public class FetchRequest
    {
        SpriteFont _font;
        private Texture2D _editorBG;
        public List<PlayerData> _players = new List<PlayerData>();
        public PlayerData player;
        public async Task LoadPlayerData(string url) //url is api endpoint passed to this function
        {
            using HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync(url); //like get request
                response.EnsureSuccessStatusCode(); //gives error if there is one
                string jsonResponse = await response.Content.ReadAsStringAsync();

                _players = JsonSerializer.Deserialize<List<PlayerData>>(jsonResponse); //use for sorting

                Debug.WriteLine(jsonResponse);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Fetching player data:{ex.Message}");
            }
        }
        public void DrawPlayerData(Vector2 startPos, SpriteBatch spriteBatch, SpriteFont font)
        {
            int index = 1;
            Vector2 position = startPos;
            var sortedPlayers = _players.OrderBy(player => player.name).ToList();
            spriteBatch.DrawString(font, "LEADERBOARD", new Vector2(position.X + 15, position.Y - 45), Color.White);

            foreach (var player in sortedPlayers)
            {
                spriteBatch.DrawString(font, $"{index}. {player.name} , Score:{player.score}", position, Color.White);//User: {player.firstName}  {player.lastName}, Date Joined: {player.joined}, Score:{player.score}", position, Color.White);
                position.Y += 25;
                index++;
            }
        }
        public async Task<bool> SendPlayerData(string url, string name, int score)
        {
            var playerData = new { name, score }; //quick set instead of playerData.name = name, etc.
            string json = JsonSerializer.Serialize(playerData);
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            using HttpClient client = new HttpClient();
            try {
                HttpResponseMessage response = await client.PostAsync(url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error sending player data: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdatePlayerData(string url, string playerid, string name, string firstName, string lastName, int score)
        {
            var playerData = new { playerid, name, firstName, lastName, score }; //quick set instead of playerData.name = name, etc.
            string json = JsonSerializer.Serialize(playerData);
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            using HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Successfully updated {name}");
                    return true;
                }
                else
                {
                    Debug.WriteLine($"Failed to update {name}: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating player data: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeletePlayerData(string url, string playerid)
        {
            var playerData = new { playerid };            
            string json = JsonSerializer.Serialize(playerData);
            using HttpClient client = new HttpClient();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, url)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Successfully Deleted {playerid}");
                    return true;
                }
                else
                {
                    Debug.WriteLine($"Failed to delete {playerid}: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting player data: {ex.Message}");
                return false;
            }
        }
        public async Task LoadOnePlayerData(string url) //url is api endpoint passed to this function
        {
            player = new PlayerData();
            using HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync(url); //like get request
                response.EnsureSuccessStatusCode(); //gives error if there is one
                string jsonResponse = await response.Content.ReadAsStringAsync();

                player = JsonSerializer.Deserialize<PlayerData>(jsonResponse); //use for sorting

                Debug.WriteLine(jsonResponse);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Fetching player data:{ex.Message}");
            }
        }
        //public override async void LoadContent(ContentManager contentManager)
        //{
        //    Translation = Matrix.Identity;
        //    _font = contentManager.Load<SpriteFont>("gameFont");
        //    _editorBG = contentManager.Load<Texture2D>("EditorBG");

        //    //get player data
        //     await LoadPlayerData("http://localhost:3000/player"); //initial load
        //    //send data
        //    //await SendPlayerData("http://localhost:3000/sentdataToDB", "Rachel", 510, 5);
        //}

        //public override void UnloadContent(ContentManager contentManager)
        //{

        //}
        //public override void HandleInput(GameTime gameTime)
        //{

        //}

        //public override void Update(GameTime gameTime)
        //{
        //    if(Keyboard.GetState().IsKeyDown(Keys.U))
        //    {
        //        _ = UpdatePlayerData("http://localhost:3000/updatePlayer", "l1VS3IDS", "Gup", "Jay", "Rossi", 10);
        //        _ = LoadPlayerData("http://localhost:3000/player"); //refresh
        //    }
        //    if (Keyboard.GetState().IsKeyDown(Keys.D))
        //    {
        //        _ = DeletePlayerData("http://localhost:3000/deletePlayer", "0acQ2wmr");
        //        _ = LoadPlayerData("http://localhost:3000/player"); //refresh
        //    }
        //    base.Update(gameTime);
        //}

        //public override void Render(SpriteBatch spriteBatch)
        //{
        //    DrawPlayerData(new Vector2(50,20), spriteBatch);
        //    base.Render(spriteBatch);
        //}

    }
}

public class PlayerData
{
    public string playerid { get; set; }
    public string name { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public int score { get; set; }
    public DateTime joined { get; set; }  
}
