using System;

namespace ConsoleMenu.Processors
{
    public class ScrollProcessor : Processor
    {
        public ScrollProcessor(): base()
        {
            Menu.KeyPressed += OnConsoleInput;
        }

        public override void OnConsoleInput(object sender, ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.DownArrow:
                case ConsoleKey.J:
                    if (Menu.SelectedItem.Index + 1 <= Menu.MenuItems.Count - 1)
                    {
                        Menu.SelectedItem = Menu.MenuItems[Menu.SelectedItem.Index + 1];
                    }
                    Redraw();
                    break;
                case ConsoleKey.UpArrow:
                case ConsoleKey.K:
                    if (Menu.SelectedItem.Index - 1 >= 0)
                    {
                        Menu.SelectedItem = Menu.MenuItems[Menu.SelectedItem.Index - 1];
                    }
                    Redraw();
                    break;
                case ConsoleKey.Enter:
                case ConsoleKey.L:
                    Menu.SelectedItem.Action?.Invoke();
                    break;
            }
        }

        public override void Redraw()
        {
            if (Menu.SelectedItem == null)
                Menu.SelectedItem = Menu.MenuItems[0];

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