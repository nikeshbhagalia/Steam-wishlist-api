﻿using Newtonsoft.Json;
using SteamWishlistApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SteamWishlistApi.Actions
{
    public class SteamActions : ISteamActions
    {
        private const string PageTotalKey = "var g_nAdditionalPages = ";

        private readonly HttpClient _httpClient;

        public SteamActions()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Game>> GetWishlist(string id)
        {
            var sourceCode = string.Empty;
            using (var webclient = new WebClient())
            {
                sourceCode = webclient.DownloadString(@"https://store.steampowered.com/wishlist/profiles/" + id);
            }

            var pageIndex = sourceCode.IndexOf(PageTotalKey);
            sourceCode = sourceCode.Substring(pageIndex + PageTotalKey.Length);
            var pages = Int32.Parse(sourceCode.Substring(0, sourceCode.IndexOf(";")));

            var urls = new string[pages];
            for (var pageNumber = 0; pageNumber < pages; pageNumber++)
            {
                urls[pageNumber] = @"https://store.steampowered.com/wishlist/profiles/" + id + "/wishlistdata?p=" + pageNumber;
            }

            var requests = urls.Select(url => _httpClient.GetAsync(url)).ToList();

            await Task.WhenAll(requests);

            var responses = requests.Select(task => task.Result);

            var games = new List<Game>();

            foreach (var response in responses)
            {
                var gamesResponseString = await response.Content.ReadAsStringAsync();
                var dict = JsonConvert.DeserializeObject<Dictionary<string, Game>>(gamesResponseString);
                games.AddRange(dict.Values);
            }

            return games;
        }
    }
}
