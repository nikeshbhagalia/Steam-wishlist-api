using Steam_wishlist_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Steam_wishlist_api.Actions
{
    public interface ISteamActions
    {
        Task<List<Game>> GetWishlist(string id);
    }
}
