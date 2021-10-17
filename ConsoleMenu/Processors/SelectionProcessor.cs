using System;

namespace ConsoleMenu.Processors
{
    public class SelectionProcessor : Processor
    {
        public SelectionProcessor(): base()
        {
            Menu.KeyPressed += OnConsoleInput;
        }

        public override void OnConsoleInput(object sender, ConsoleKeyInfo keyInfo)
        {
            if (int.TryParse(keyInfo.KeyChar.ToString(), out int idx) && idx < Menu.MenuItems.Count)
            {
                Menu.SelectedItem = Menu.MenuItems[idx];
                Menu.SelectedItem.Action?.Invoke();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid selection!");
                Console.ResetColor();
            }
        }

        public override void Redraw()
        {
            Console.Clear();
            Console.WriteLine(Menu.Title);

            foreach (var item in Menu.MenuItems)
            {
                ItemHandler?.Invoke(item);
            }
        }

        public override Action<MenuItem> ItemHandler
        {
            get =>_itemHandler ?? (_itemHandler = (item) => Console.WriteLine(string.Format("{0,2}: {1}", item.Index, item.Name)));
            set => _itemHandler = value;
        }
    }
}