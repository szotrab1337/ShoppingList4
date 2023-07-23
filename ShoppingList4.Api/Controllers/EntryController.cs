using Microsoft.AspNetCore.Mvc;
using ShoppingList4.Api.Entities;
using ShoppingList4.Api.Interfaces;
using ShoppingList4.Api.Models;

namespace ShoppingList4.Api.Controllers
{
    [Route("api/entry")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private readonly IEntryService _entryService;

        public EntryController(IEntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpPost]
        public ActionResult CreateEntry([FromBody] CreateEntryDto dto)
        {
            int id = _entryService.Create(dto);

            return Created($"/api/entry/{id}", null);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteEntry([FromRoute] int id)
        {
            _entryService.Delete(id);

            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<Entry> GetEntry([FromRoute] int id)
        {
            var entry = _entryService.GetById(id);

            return Ok(entry);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateEntry([FromBody]UpdateEntryDto dto, [FromRoute]int id)
        {
            _entryService.Update(id, dto);

            return Ok();
        }
    }
}
