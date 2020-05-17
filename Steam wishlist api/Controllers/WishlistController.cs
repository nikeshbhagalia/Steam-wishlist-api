using Microsoft.AspNetCore.Mvc;
using Steam_wishlist_api.Actions;
using Steam_wishlist_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Steam_wishlist_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly ISteamActions _steamActions;

        public WishlistController(ISteamActions steamActions)
        {
            _steamActions = steamActions;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ICollection<Game>>> Get(string id)
        {
            return await _steamActions.GetWishlist(id);
        }
    }
}
