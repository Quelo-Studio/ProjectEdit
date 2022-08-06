using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ArcaneNebula
{
    public static class Database
    {
        private static readonly HttpClient s_Client = new();
        private static readonly string s_ServerURI = "http://quelostudio.mygamesonline.org/Database";

        public static async Task<List<LevelData>> GetLevels()
        {
            var response = await s_Client.PostAsync($"{s_ServerURI}/PE_GetLevels.php", null);
            string responseStr = await response.Content.ReadAsStringAsync();

            List<LevelData> levelsList = new();

            string[] levels = responseStr.Split(';');
            foreach (string level in levels)
            {
                LevelData lvl = new();

                string[] props = level.Split('+');
                foreach (string prop in props)
                {
                    string[] keyValue = prop.Split(':');

                    switch (keyValue[0])
                    {
                        case "0":
                            lvl.ID = Convert.ToInt32(keyValue[1]);
                            break;
                        case "1":
                            lvl.Name = keyValue[1];
                            break;
                        default:
                            break;
                    }
                }

                levelsList.Add(lvl);
            }

            return levelsList;
        }
    }
}
