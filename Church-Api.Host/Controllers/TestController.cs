using Church_Api.Data.Interfaces;
using Church_Api.Data.Implementations;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAction(string id)
        {
            var response = await _entityStore.GetAsync<BaseEntity>("Test", id);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> PostAction(string id)
        {
            BaseEntity testEntity = new BaseEntity(id);

            var response = await _entityStore.CreateAsync("Test", testEntity);

            return Ok(response);
        }
    }
}
