namespace EM.Web.Controllers
{
    using EM.Services.Cities;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static EM.Common.RoleConstants;

    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService cityService;

        public CitiesController(ICityService cityService)
        {
            this.cityService = cityService;
        }

        [HttpGet("{action}/{input}")]
        [Authorize(Roles = Admin)]
        public async Task<IActionResult> Find(string input)
        {
            var model = await this.cityService.GetSimilarCityNames(input, 5);

            return this.Ok(model);
        }
    }
}
