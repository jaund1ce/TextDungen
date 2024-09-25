using System;
using System.Collections.Generic;

namespace TextRPG
{
    internal class Program
    {
        static Player player;
        static Store store;
        static int startState = 0;

        static void Main(string[] args)
        {
            InitPlayerInfo();
            InitStore();

            while (true)
            {
                // 1: 상태 보기, 2: 인벤토리, 3: 상점
                if (startState == 0) DisplayStartState();
                else if (startState == 1) DisplayPlayerInfo();
                else if (startState == 2) DisplayInventoryInfo();
                else DisplayStore();
            }
        }
        static void DisplayStore()
        {
            Console.Clear();
            Console.WriteLine("=== 상점 ===");
            Console.WriteLine("필요한 아이템을 구매할 수 있습니다.");
            Console.WriteLine();

            player.DisplayMoney();  // 플레이어의 현재 골드 출력

            store.DisplayStore(0);  // Store에서 아이템 목록을 출력

            Console.WriteLine();
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("0. 뒤로 가기");

            int select = GetPlayerSelect(0, 1);
            if (select == 1)
            {
                BuyItem();  // 아이템 구매 함수 호출
            }
            else if (select == 0)
            {
                startState = 0;  // 메인 화면으로 돌아감
            }
        }

        static int GetPlayerSelect(int start, int end)
        {
            int selection = -1;
            bool validInput = false;

            while (!validInput)
            {
                Console.WriteLine($"옵션을 선택하세요 ({start} ~ {end}): ");
                string input = Console.ReadLine();

                // 입력이 숫자인지 확인하고, 범위 내에 있는지 확인
                if (int.TryParse(input, out selection) && selection >= start && selection <= end)
                {
                    validInput = true;  // 유효한 입력이면 반복 종료
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
                }
            }

            return selection;
        }
        static void DisplayStartState()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine();
            ("1").PrintWithColor(ConsoleColor.Magenta, false);
            Console.WriteLine(". 상태 보기");
            ("2").PrintWithColor(ConsoleColor.Magenta, false);
            Console.WriteLine(". 인벤토리");
            ("3").PrintWithColor(ConsoleColor.Magenta, false);
            Console.WriteLine(". 상점");
            Console.WriteLine();
            startState = GetPlayerSelect(1, 3);
        }

        static void DisplayPlayerInfo()
        {
            Console.Clear();
            Console.WriteLine("=== 플레이어 상태 ===");
            Console.WriteLine($"이름: {player.Name}");
            Console.WriteLine($"체력: {player.Hp}");
            Console.WriteLine($"공격력: {player.Power}");
            Console.WriteLine($"방어력: {player.Shield}");
            Console.WriteLine($"골드: {player.Money}");

            Console.WriteLine();
            Console.WriteLine("=== 장착한 아이템 ===");

            foreach (var item in player.Inventory)
            {
                if (item.IsEquipped)
                {
                    Console.WriteLine($"- {item.Name} (효과: {item.Value})");
                }
            }

            Console.WriteLine();
            Console.WriteLine("0. 뒤로 가기");

            // 뒤로 가기 입력 처리
            int select = GetPlayerSelect(0, 0);
            if (select == 0)
            {
                startState = 0;  // 시작 상태로 돌아감
            }
        }
        static void DisplayInventoryInfo()
        {
            Console.Clear();
            Console.WriteLine("=== 인벤토리 ===");

            if (player.GetItemCount() == 0)
            {
                Console.WriteLine("보유하고 있는 아이템이 없습니다.");
            }
            else
            {
                int index = 1;
                foreach (var item in player.Inventory)
                {
                    string equippedText = item.IsEquipped ? "[장착됨]" : "";
                    Console.WriteLine($"{index}. {item.Name} (효과: {item.Value}) {equippedText}");
                    index++;
                }
            }

            Console.WriteLine();
            Console.WriteLine("1. 아이템 장착/해제");
            Console.WriteLine("0. 뒤로 가기");

            int select = GetPlayerSelect(0, 1);
            if (select == 1 && player.GetItemCount() > 0)
            {
                ManagementItemInventory();  // 아이템 장착/해제 관리
            }
            else if (select == 0)
            {
                startState = 0;  // 시작 상태로 돌아감
            }
        }


        static void BuyItem()
        {
            Console.WriteLine("구매할 아이템 번호를 입력하세요:");
            int itemIndex = int.Parse(Console.ReadLine()) - 1;

            if (store.IsAbleToBuy(itemIndex))
            {
                Item selectedItem = store.GetStoreItem(itemIndex);
                if (player.IsAbleToBuy(selectedItem.Price))
                {
                    store.BuyItem(itemIndex);  // 상점에서 아이템 구매 처리
                    player.BuyItem(selectedItem);  // 플레이어 인벤토리에 추가 및 골드 감소
                    Console.WriteLine($"{selectedItem.Name}을(를) 구매했습니다.");
                }
                else
                {
                    Console.WriteLine("골드가 부족합니다.");
                }
            }
            else
            {
                Console.WriteLine("이미 구매한 아이템입니다.");
            }

            Console.WriteLine();
            Console.WriteLine("계속하려면 아무 키나 누르세요...");
            Console.ReadKey();
        }
        static void InitPlayerInfo()
        {
            Console.Write("Player 닉네임을 입력해주세요: ");
            string playerName = Console.ReadLine();
            player = new Player(playerName, 100, 10, 20, 500, new List<Item>());

            player.InitItemList(GetItemFromDB(0));
        }

        static void InitStore()
        {
            List<Item> storeItems = new List<Item>
            {
                GetItemFromDB(0),  // 인덱스를 0으로 수정
                GetItemFromDB(1)   // 인덱스 1까지만 접근
            };
            store = new Store(storeItems);
        }

        static Item GetItemFromDB(int itemIdx)
        {
            List<string[]> itemDB = new List<string[]>
            {
                new string[] { "Iron Armor", "0", "5", "500", "A strong iron armor" },
                new string[] { "Old Sword", "1", "10", "600", "A worn-out sword" }
            };

            if (itemIdx >= 0 && itemIdx < itemDB.Count)  // 인덱스 유효성 검사 추가
            {
                string[] itemData = itemDB[itemIdx];
                return new Item(itemData[0], int.Parse(itemData[1]), int.Parse(itemData[2]), int.Parse(itemData[3]), itemData[4]);
            }

            throw new ArgumentOutOfRangeException("잘못된 아이템 인덱스입니다.");
        }

        static void ManagementItemInventory()
        {
            bool isExit = false;
            while (!isExit)
    {
        Console.Clear();
        Console.WriteLine("=== 인벤토리 - 장착 관리 ===");
        Console.WriteLine("보유 중인 아이템을 장착하거나 해제할 수 있습니다.");
        Console.WriteLine();

        // 현재 플레이어의 아이템 목록 출력
        int index = 1;
        foreach (var item in player.Inventory)
        {
            string equippedText = item.IsEquipped ? "[장착됨]" : "[미장착]";
            Console.WriteLine($"{index}. {item.Name} (효과: {item.Value}) {equippedText}");
            index++;
        }

        Console.WriteLine("0. 나가기");
        int selectedItem = GetPlayerSelect(0, player.GetItemCount());

        if (selectedItem == 0)
        {
            isExit = true;
        }
        else
        {
            // 아이템 장착 또는 해제
            player.EquipItem(selectedItem - 1);
            Console.WriteLine("아이템 장착 상태가 변경되었습니다.");
        }
    }
}
    }
}
