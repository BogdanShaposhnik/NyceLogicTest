using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NyceLogicTest.Models
{
    public class UserShoppingList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<NameValue> Items { get; set; }
    }
    public class NameValue
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
