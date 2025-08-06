using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Models;
using backend.Data; 

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly ActivityDbContext _context;
        private readonly IProducer<Null, string> _producer;

        public ActivityController(ActivityDbContext context, IProducer<Null, string> producer)
        {
            _context = context;
            _producer = producer;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ActivityEvent activity)
        {
            // Save to PostgreSQL
            // _context.ActivityEvents.Add(activity);
            // await _context.SaveChangesAsync();

            // Send to Kafka
            var json = JsonSerializer.Serialize(activity);
            await _producer.ProduceAsync("activity-events", new Message<Null, string> { Value = json });

            return Ok(new { status = "Saved to DB and sent to Kafka" });
        }
    }
}
