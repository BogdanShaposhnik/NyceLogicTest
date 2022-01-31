using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NyceLogicTest.Context;
using NyceLogicTest.Models;
using NyceLogicTest.Repository.Interfaces;

namespace NyceLogicTest.Repository
{
    public class ItemService : IItemService
    {
        private readonly ShopContext _dbContext;
        public ItemService(ShopContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Item> GetItemById(int id)
        {
            Item item = await _dbContext.Item.FirstOrDefaultAsync(i => i.Id == id);
            if (item == null)
            {
                throw new Exception("Wrong id. No item found");
            }
            return item;
        }
        public async Task<List<Item>> GetItems()
        {
            var items = await _dbContext.Item.ToListAsync();
            return items;
        }
        public async Task<Item> AddItem(string name)
        {
            var itemToCheck = await _dbContext.Item.FirstOrDefaultAsync(i => i.Name == name);
            if (itemToCheck != null)
            {
                throw new Exception("Item with this id already exists");
            }
            _dbContext.Item.Add(new Item { Name = name });
            await _dbContext.SaveChangesAsync();
            var item = _dbContext.Item.OrderByDescending(i => i.Id).FirstOrDefault(); ;
            return item;
        }
        public async Task<Item> UpdateItem(int id, string name)
        {
            var item = await _dbContext.Item.FirstOrDefaultAsync(i => i.Id == id);
            if (item == null)
            {
                throw new Exception("Wrong id. No item found");
            }
            item.Name = name;
            _dbContext.Update(item);
            await _dbContext.SaveChangesAsync();
            return await _dbContext.Item.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<Item> RemoveItem(int id)
        {
            var item = await _dbContext.Item.Where(r => r.Id == id).FirstOrDefaultAsync();

            if (item == null)
            {
                throw new Exception("Wrong id. No item found");
            }

            _dbContext.Item.Remove(item);
            await _dbContext.SaveChangesAsync();
            return await _dbContext.Item.Where(r => r.Id == id).FirstOrDefaultAsync();
        }
    }
}
