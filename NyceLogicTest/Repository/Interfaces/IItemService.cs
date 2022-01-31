using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NyceLogicTest.Models;

namespace NyceLogicTest.Repository.Interfaces
{
    public interface IItemService
    {
        public abstract Task<List<Item>> GetItems();
        public abstract Task<Item> GetItemById(int id);
        public abstract Task<Item> AddItem(string name);
        public abstract Task<Item> UpdateItem(int id, string name);
        public abstract Task<Item> RemoveItem(int id);
    }
}
