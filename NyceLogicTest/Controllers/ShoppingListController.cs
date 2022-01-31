using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NyceLogicTest.Models;
using NyceLogicTest.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace NyceLogicTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingListController : ControllerBase
    {
        private IShoppingListService _shoppingListService;
        public ShoppingListController(IShoppingListService resultService)
        {
            _shoppingListService = resultService;
        }
        [HttpGet("GetAll", Name = "Get lists")]
        public async Task<ActionResult<UserShoppingList>> Get()
        {
            try
            {
                var result = await _shoppingListService.GetLists();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("Get/{id}", Name = "Get list")]
        public async Task<ActionResult<UserShoppingList>> Get(int id)
        {
            try
            {
                var result = await _shoppingListService.GetListById(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("Add")]
        [Produces("application/json")]
        public async Task<IActionResult> Add([FromBody] string name)
        {
            try
            {
                var item = await _shoppingListService.AddList(name);
                return Ok("List added successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] string NewName)
        {
            try
            {
                var item = await _shoppingListService.UpdateList(id, NewName);
                return Ok("List updated successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var item = await _shoppingListService.RemoveList(id);
                return Ok("List removed successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("DeleteItemFromList/{listId}/{productName}")]
        public async Task<IActionResult> DeleteItemFromList(int listId, string productName)
        {
            try
            {
                var item = await _shoppingListService.RemoveItemFromList(listId, productName);
                return Ok("Item removed from list successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("AddItemToList/{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> AddItemToList(int id, [FromBody] NameValue prod)
        {
            try
            {
                var item = await _shoppingListService.AddItemToList(id, prod.Name, prod.Value);
                return Ok("Item added to list successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
