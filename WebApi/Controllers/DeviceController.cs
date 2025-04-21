using BusinessLogic.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _service;

        public DevicesController(IDeviceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var device = await _service.GetByIdAsync(id);
            return device is null ? NotFound() : Ok(device);
        }

        [HttpPost]
        [Consumes("application/json", "text/plain")]
        public async Task<IActionResult> Create()
        {
            var contentType = Request.ContentType?.Split(';')[0].Trim();
            Device device;

            if (contentType == "text/plain")
            {
                using var reader = new StreamReader(Request.Body);
                var text = await reader.ReadToEndAsync();
                device = ParseFromPlainText(text);
            }
            else
            {
                device = await HttpContext.Request.ReadFromJsonAsync<Device>()
                         ?? throw new InvalidOperationException("Invalid JSON body.");
            }

            await _service.CreateAsync(device);
            return CreatedAtAction(nameof(GetById), new { id = device.Id }, device);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Device device)
        {
            if (id != device.Id)
            {
                return BadRequest("ID in URL must match ID in body.");
            }

            try
            {
                await _service.UpdateAsync(device);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        private Device ParseFromPlainText(string text)
        {
            var parts = text.Split(';', StringSplitOptions.TrimEntries);
            return new Device
            {
                Id        = parts[0],
                Name      = parts[1],
                IsEnabled = bool.Parse(parts[2])
            };
        }
    }
