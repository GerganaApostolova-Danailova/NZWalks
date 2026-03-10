using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();

            try
            {
            // Get all regions from the Web API
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7179/api/regions");

                httpResponseMessage.EnsureSuccessStatusCode();

                    response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());

                ViewBag.Response = response;

                //var regions = System.Text.Json.JsonSerializer.Deserialize<List<Models.DTO.RegionDto>>(response, new System.Text.Json.JsonSerializerOptions
                //{
                //    PropertyNameCaseInsensitive = true
                //});
            }
            catch (Exception ex)
            {

                throw;
            }



            return View(response);
        }
    }
}
