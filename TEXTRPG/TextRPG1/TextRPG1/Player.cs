using System;
using System.Collections.Generic;

namespace TextRPG
{
    public class Player
    {
        public string Name { get; private set; }
        public int Hp { get; private set; }
        public int Shield { get; private set; }
        public int Power { get; private set; }
        public int Money { get; private set; }
        public List<Item> Inventory { get; private set; }

        public Player(string name, int hp, int shield, int power, int money, List<Item> inventory)
        {
            Name = name;
            Hp = hp;
            Shield = shield;
            Power = power;
            Money = money;
            Inventory = inventory;
        }

        public void InitItemList(Item item)
        {
            Inventory.Add(item);
        }

        public int GetItemCount()
        {
            return Inventory.Count;
        }

        public void DisplayPlayerInfo()
        {
            Console.WriteLine($"Name: {Name}, HP: {Hp}, Shield: {Shield}, Power: {Power}, Gold: {Money}");
            // Further info for each equipped item
        }

        public void EquipItem(int itemIdx)
        {
            Inventory[itemIdx].IsEquipped = !Inventory[itemIdx].IsEquipped;
        }

        public void BuyItem(Item item)
        {
            Inventory.Add(item);
            Money -= item.Price;
        }

        public bool IsAbleToBuy(int itemPrice)
        {
            return itemPrice <= Money;
        }

        public void ArrangeItemInventory(int pivot)
        {
            // Sorting logic based on pivot (name, equipped status, attack power, defense power)
        }

        public void DisplayMoney()
        {
            Console.WriteLine($"Current Gold: {Money}");
        }
    }
}
