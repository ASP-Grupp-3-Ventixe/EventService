using EventApp.Interfaces;
using EventApp.Models;
using EventApp.Services.Media;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController(IEventService eventService, ILogger<EventsController> logger) : ControllerBase
    {
        private readonly IEventService _eventService = eventService;
        private readonly ILogger<EventsController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _eventService.GetAllAsync());


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid: {@errors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(ModelState);

            }

            var success = await _eventService.CreateAsync(model);
            return success ? Ok() : BadRequest("Event creation failed.");
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEventDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid: {@errors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(ModelState);

            }

            var success = await _eventService.UpdateAsync(model);
            return success ? Ok() : NotFound("Event not found.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            var success = await _eventService.DeleteAsync(id);
            return success ? Ok() : NotFound("Event not found.");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("upload-image/{eventId}")]
        public async Task<IActionResult> UploadImage(int eventId, [FromForm] IFormFile file, [FromServices] IImageService imageService)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType))
                return BadRequest("Invalid image type.");

            try
            {
                var result = await imageService.UploadImageAsync(file);

                if (result == null)
                    return StatusCode(500, "Image upload failed.");

                var (fileName, imageUrl) = result.Value;

                var success = await _eventService.ReplaceEventImageAsync(eventId, fileName, imageUrl);

                return success
                    ? Ok(new { fileName, imageUrl })
                    : NotFound("Event not found");      

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UploadImage failed.");
                return StatusCode(500, "Failed to upload image.");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _eventService.GetEventByIdAsync(id);

            if (result == null)
                return NotFound(result);

            return Ok(result);
        }


        [HttpPost("increase-tickets")]
        public async Task<IActionResult> IncreaseTicketsSold(int eventId, int quantity)
        {
            var result = await _eventService.IncreaseTicketsSoldAsync(eventId, quantity);
            if (!result)
                return BadRequest("Failed to increase tickets.");

            return Ok("Tickets updated.");
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> GetForEdit(int id)
        {
            var result = await _eventService.GetEventForEditAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

    }
}



