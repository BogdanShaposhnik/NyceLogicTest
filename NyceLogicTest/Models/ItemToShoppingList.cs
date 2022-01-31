using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NyceLogicTest.Models
{
    public class ItemToShoppingList
    {
        public int Id { get; set; }
        public int ItemID { get; set; }
        public int ListId { get; set; }
        public int Value { get; set; }
    }
}
