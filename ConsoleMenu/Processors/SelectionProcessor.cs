using System;

namespace ConsoleMenu.Processors
{
    public class SelectionProcessor : IProcessor
    {
        private Action<MenuItem> _itemHandler;

        public void HandleInput(ConsoleKeyInfo keyInfo)
        {
            if (int.TryParse(keyInfo.KeyChar.ToString(), out int idx) && idx < Menu.MenuItems.Count)
            {
                Menu.SelectedItem = Menu.MenuItems[idx];
                ReadyToInvoke = true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid selection!");
                Console.ResetColor();
            }
        }

        public void PrintMenu(Menu menu)
        {
            if (ReadyToInvoke)
            {
                Console.WriteLine();
                ReadyToInvoke = false;
                Menu.SelectedItem.Action?.Invoke();
                
                if (Menu.IsRunning)
                {
                    return;
                }
            }

            Menu = menu;

            Console.Clear();
            Console.WriteLine(Menu.Title);
            Console.WriteLine(new string('=', Menu.SeparatorWidth));

            foreach (var item in Menu.MenuItems)
            {
                ItemHandler?.Invoke(item);
            }
        }

        public bool ReadyToInvoke { get; private set; }

        public Menu Menu { get; set; }

        public Action<MenuItem> ItemHandler
        {
            get =>_itemHandler ?? (_itemHandler = (item) => Console.WriteLine(string.Format("{0,2}: {1}", item.Index, item.Name)));
            set => _itemHandler = value;
        }
    }
}