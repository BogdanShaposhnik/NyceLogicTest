using Microsoft.AspNetCore.Http;
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
    public class ItemController : ControllerBase
    {
        private IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<List<Item>>> Get()
        {
            try
            {
                var results = await _itemService.GetItems();
                return Ok(results);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Get/{id}", Name = "Get")]
        public async Task<ActionResult<Item>> Get(int id)
        {
            try
            {
                var result = await _itemService.GetItemById(id);
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
                var item = await _itemService.AddItem(name);
                return Ok("Item added successfully");
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
                var item = await _itemService.UpdateItem(id, NewName);
                return Ok("Item updated successfully");
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
                var item = await _itemService.RemoveItem(id);
                return Ok("Item removed successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
