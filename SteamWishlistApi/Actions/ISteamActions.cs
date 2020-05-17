using SteamWishlistApi.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamWishlistApi.Actions
{
    public interface ISteamActions
    {
        Task<List<Game>> GetWishlist(string id);
    }
}
