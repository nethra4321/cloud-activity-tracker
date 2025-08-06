using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly IProducer<Null, string> _producer;

        public ActivityController(IProducer<Null, string> producer)
        {
            _producer = producer;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ActivityEvent activity)
        {
            var json = JsonSerializer.Serialize(activity);
            await _producer.ProduceAsync("activity-events", new Message<Null, string> { Value = json });
            return Ok(new { status = "Event sent to Kafka" });
        }
    }
}
