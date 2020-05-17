using HtmlAgilityPack;
using Newtonsoft.Json;
using Steam_wishlist_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Steam_wishlist_api.Actions
{
    public class SteamActions: ISteamActions
    {
        private readonly HtmlWeb _htmlWeb;

        public SteamActions()
        {
            _htmlWeb = new HtmlWeb();
        }

        public async Task<List<Game>> GetWishlist(string id)
        {
            var webclient = new WebClient();
            var sourceCode = webclient.DownloadString(@"https://store.steampowered.com/wishlist/profiles/" + id);
            var pageVaribaleName = "var g_nAdditionalPages = ";
            var pageIndex = sourceCode.IndexOf(pageVaribaleName);
            sourceCode = sourceCode.Substring(pageIndex + pageVaribaleName.Length);
            var pages = Int32.Parse(sourceCode.Substring(0, sourceCode.IndexOf(";")));

            var client = new HttpClient();

            var urls = new string[pages];
            for (var pageNumber = 0; pageNumber < pages; pageNumber++)
            {
                urls[pageNumber] = @"https://store.steampowered.com/wishlist/profiles/" + id + "/wishlistdata?p=" + pageNumber;
            }

            var requests = urls.Select(url => client.GetAsync(url)).ToList();

            await Task.WhenAll(requests);

            var responses = requests.Select(task => task.Result);

            var games = new List<Game>();

            foreach (var r in responses)
            {
                var str = await r.Content.ReadAsStringAsync();
                var dict = JsonConvert.DeserializeObject<Dictionary<string, Game>>(str);
                games.AddRange(dict.Values);
            }

            return games;
        }
    }
}
