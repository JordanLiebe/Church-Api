using Church_Api.Data;
using Church_Api.Data.Interfaces;
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
        public IActionResult GetAction()
        {
            EntityBase test = new EntityBase("12345");

            return Ok(test);
        }
    }
}
