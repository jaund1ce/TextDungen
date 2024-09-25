using System;
using System.Collections.Generic;

namespace TextRPG
{
    public class Store
    {
        private List<Item> itemList;
        private Dictionary<string, bool> soldState;

        public Store(List<Item> items)
        {
            itemList = items;
            soldState = new Dictionary<string, bool>();
            foreach (var item in items)
            {
                soldState[item.Name] = false;
            }
        }

        public void DisplayStore(int type)
        {
            foreach (var item in itemList)
            {
                Console.WriteLine($"{item.Name} - Price: {item.Price} G");
            }
        }

        public bool IsAbleToBuy(int itemIdx)
        {
            return !soldState[itemList[itemIdx].Name];
        }

        public void BuyItem(int itemIdx)
        {
            soldState[itemList[itemIdx].Name] = true;
        }

        public int GetStoreItemCount()
        {
            return itemList.Count;
        }

        public Item GetStoreItem(int itemIdx)
        {
            return itemList[itemIdx];
        }
    }
    }

