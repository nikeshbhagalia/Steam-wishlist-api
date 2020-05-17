using Microsoft.AspNetCore.Mvc;
using SteamWishlistApi.Actions;
using SteamWishlistApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamWishlistApi.Controllers
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
