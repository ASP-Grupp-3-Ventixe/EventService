﻿using EventApp.Interfaces;
using EventApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController(IEventService eventService) : ControllerBase
    {
        private readonly IEventService _eventService = eventService;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _eventService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID.");

            var result = await _eventService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventDto model)
        {
            if (!ModelState.IsValid)
            {
                var errorDetails = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .Select(ms => new
                    {
                        Field = ms.Key,
                        Errors = ms.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    });

                foreach (var error in errorDetails)
                {
                    Console.WriteLine($"🔴 FIELD: {error.Field}");
                    foreach (var e in error.Errors)
                    {
                        Console.WriteLine($"    ❌ ERROR: {e}");
                    }
                }

                return BadRequest(errorDetails);
            }

            var success = await _eventService.CreateAsync(model);
            return success ? Ok() : BadRequest("Event creation failed in service.");
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEventDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
        public async Task<IActionResult> UploadImage(int eventId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            // AllowedTypes inklistrad från ChatGpt
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType))
                return BadRequest("Invalid image type.");


            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "event-images");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var savePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var result = await _eventService.ReplaceEventImageAsync(eventId, fileName);

            return result
                ? Ok(new { fileName, imageUrl = $"https://localhost:7101/event-images/{fileName}" })
                : NotFound("Event not found");
        }
    }


}
