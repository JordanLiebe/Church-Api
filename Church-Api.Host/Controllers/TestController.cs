using Church_Api.Data.Interfaces;
using Church_Api.Data.Implementations;
using Microsoft.AspNetCore.Mvc;
using Church_Api.Domain;

namespace Church_Api.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IEntityStore _entityStore;
        public TestController(IEntityStore entityStore)
        {
            _entityStore = entityStore;
        }

        [HttpGet]
        public async Task<IActionResult> GetManyAction(string? query)
        {
            var response = await _entityStore.GetManyAsync<BaseEntity>("Test", query);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAction(string id)
        {
            var response = await _entityStore.GetAsync<BaseEntity>("Test", id);

            BaseEntity? test = response.Data;

            JsonResource<BaseEntity> resource = new JsonResource<BaseEntity>(test);

            return Ok(resource);
        }

        [HttpPost]
        public async Task<IActionResult> PostAction(BaseEntity obj)
        {
            var response = await _entityStore.CreateAsync("Test", obj);

            return Ok(response);
        }

        [HttpPost("batch")]
        public async Task<IActionResult> PostAction(IEnumerable<BaseEntity> batchEntites)
        {
            var response = await _entityStore.CreateManyAsync("Test", batchEntites);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAction(string id)
        {
            List<string> deleteIds = new List<string> { "Jordan123", "Jacob123", id };

            var response = await _entityStore.DeleteManyAsync<BaseEntity>("Test", deleteIds);

            return Ok(response);
        }
    }
}
