using Microsoft.AspNetCore.Mvc;
using ShoppingList4.Api.Entities;
using ShoppingList4.Api.Interfaces;
using ShoppingList4.Api.Models;

namespace ShoppingList4.Api.Controllers
{
    [Route("api/shoppinglist")]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;

        public ShoppingListController(IShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }

        [HttpGet]
        public ActionResult<List<ShoppingList>> GetAllShoppingLists()
        {
            var shoppingLists = _shoppingListService.GetAll();

            return Ok(shoppingLists);
        }

        [HttpPost]
        public ActionResult CreateShoppingList([FromBody] ShoppingListDto dto)
        {
            var id = _shoppingListService.Create(dto);

            return Created($"/api/shoppinglist/{id}", null);
        }

        [HttpGet("{id}")]
        public ActionResult GetShoppingList([FromRoute] int id)
        {
            var shoppingList = _shoppingListService.GetById(id);

            return Ok(shoppingList);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteShoppingList([FromRoute] int id)
        {
            _shoppingListService.Delete(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult UpdateShoppingList([FromBody] ShoppingListDto dto, [FromRoute] int id)
        {
            _shoppingListService.Update(id, dto);

            return Ok();
        }
    }
}
