using System;

namespace ConsoleMenu.Processors
{
    public class ScrollProcessor : Processor
    {
        public override void HandleInput(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.DownArrow:
                case ConsoleKey.J:
                    if (Menu.SelectedItem.Index + 1 <= Menu.MenuItems.Count - 1)
                    {
                        Menu.SelectedItem = Menu.MenuItems[Menu.SelectedItem.Index + 1];
                    }
                    break;
                case ConsoleKey.UpArrow:
                case ConsoleKey.K:
                    if (Menu.SelectedItem.Index - 1 >= 0)
                    {
                        Menu.SelectedItem = Menu.MenuItems[Menu.SelectedItem.Index - 1];
                    }
                    break;
                case ConsoleKey.Enter:
                case ConsoleKey.L:
                    ReadyToInvoke = true;
                    break;
            }
        }

        public override void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine(Menu.Title);

            foreach (var item in Menu.MenuItems)
            {
                if (item.Index == Menu.SelectedItem.Index)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    ItemHandler?.Invoke(item);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    ItemHandler?.Invoke(item);
                }
            }
        }

        public override Action<MenuItem> ItemHandler
        { 
            get 
            {
                _itemHandler ??= (item) => Console.WriteLine(
                    Menu.SelectedItem == item ? 
                        $"==> {item.Name}" : 
                        $"    {item.Name}");

                return _itemHandler;
            }
            set
            {
                _itemHandler = value;
            }
        }
    }
}