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
    public class ShoppingListService : IShoppingListService
    {
        private readonly ShopContext _dbContext;
        public ShoppingListService(ShopContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<UserShoppingList> GetListById(int id)
        {
            var list = await _dbContext.ShoppingList.FirstOrDefaultAsync(i => i.Id == id);
            if (list == null)
            {
                throw new Exception("Wrong id. No list found with this id");
            }
            UserShoppingList userList = new UserShoppingList();
            userList.Title = list.Title;
            userList.Id = list.Id;
            var itemsToList = await _dbContext.ItemToShoppingList.Where(i => i.ListId == id).ToListAsync();
            userList.Items = new List<NameValue>();
            foreach (var item in itemsToList)
            {
                NameValue nameToValue = new NameValue();
                nameToValue.Value = item.Value;
                var product = await _dbContext.Item.FirstOrDefaultAsync(i => i.Id == item.ItemID);
                nameToValue.Name = product.Name;
                userList.Items.Add(nameToValue); 
            }
            return userList;
        }
        public async Task<ShoppingList> AddList(string title)
        {
            _dbContext.ShoppingList.Add(new ShoppingList { Title = title });
            await _dbContext.SaveChangesAsync();
            var list = _dbContext.ShoppingList.OrderByDescending(i => i.Id).FirstOrDefault(); ;
            return list;
        }
        public async Task<List<ShoppingList>> GetLists()
        {
            var lists = await _dbContext.ShoppingList.ToListAsync();
            return lists;
        }
        public async Task<ShoppingList> UpdateList(int id, string name)
        {
            var list = await _dbContext.ShoppingList.FirstOrDefaultAsync(i => i.Id == id);
            if (list == null)
            {
                throw new Exception("Wrong id. No list found with this id");
            }
            list.Title = name;
            _dbContext.Update(list);
            await _dbContext.SaveChangesAsync();
            return await _dbContext.ShoppingList.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<ShoppingList> RemoveList(int id)
        {
            var list = await _dbContext.ShoppingList.Where(r => r.Id == id).FirstOrDefaultAsync();

            if (list == null)
            {
                throw new Exception("Wrong id. No list found with this id");
            }

            _dbContext.ShoppingList.Remove(list);
            await _dbContext.SaveChangesAsync();
            return await _dbContext.ShoppingList.Where(r => r.Id == id).FirstOrDefaultAsync();
        }
        public async Task<ShoppingList> RemoveItemFromList(int id, string productName)
        {
            var list = await _dbContext.ShoppingList.Where(l => l.Id == id).FirstOrDefaultAsync();
            var item = await _dbContext.Item.Where(i => i.Name == productName).FirstOrDefaultAsync();
            if (list == null || item == null)
            {
                throw new Exception("Wrong id. No list found with this id");
            }
            var itemToList = await _dbContext.ItemToShoppingList.Where(i => i.ListId == list.Id && i.ItemID == item.Id).FirstOrDefaultAsync();

            if (itemToList == null)
            {
                throw new Exception("Wrong product name. No item found");
            }

            _dbContext.ItemToShoppingList.Remove(itemToList);
            await _dbContext.SaveChangesAsync();
            return await _dbContext.ShoppingList.Where(l => l.Id == id).FirstOrDefaultAsync();
        }
        public async Task<ShoppingList> AddItemToList(int id, string productName, int value)
        {
            var list = await _dbContext.ShoppingList.Where(l => l.Id == id).FirstOrDefaultAsync();
            var item = await _dbContext.Item.Where(i => i.Name == productName).FirstOrDefaultAsync();
            if (list == null || item == null)
            {
                throw new Exception("Wrong id. No item found");
            }
            _dbContext.ItemToShoppingList.Add(new ItemToShoppingList { ListId = list.Id, ItemID = item.Id, Value = value });
            await _dbContext.SaveChangesAsync();
            return await _dbContext.ShoppingList.Where(l => l.Id == id).FirstOrDefaultAsync();
        }
    }
}
