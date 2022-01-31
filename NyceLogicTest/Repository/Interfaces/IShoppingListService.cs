using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NyceLogicTest.Models;

namespace NyceLogicTest.Repository.Interfaces
{
    public interface IShoppingListService
    {
        public abstract Task<List<ShoppingList>> GetLists();
        public abstract Task<UserShoppingList> GetListById(int id);
        public abstract Task<ShoppingList> AddList(string name);
        public abstract Task<ShoppingList> UpdateList(int id, string name);
        public abstract Task<ShoppingList> RemoveList(int id);
        public abstract Task<ShoppingList> RemoveItemFromList(int id, string productName);
        public abstract Task<ShoppingList> AddItemToList(int id, string productName, int value);
    }
}
