using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public class Item
    {
        public string Name { get; private set; }
        public int Type { get; private set; } // 0 for defense, 1 for attack
        public int Value { get; private set; }
        public int Price { get; private set; }
        public string Description { get; private set; }
        public bool IsEquipped { get; set; }

        public Item(string name, int type, int value, int price, string description)
        {
            Name = name;
            Type = type;
            Value = value;
            Price = price;
            Description = description;
            IsEquipped = false;
        }
    }
}
