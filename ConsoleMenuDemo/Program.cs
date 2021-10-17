using System;
using ConsoleMenu;
using ConsoleMenu.Processors;

namespace ConsoleMenuDemo
{
    class ConsoleMenuDemo
    {
        static void Main(string[] args)
        {
            var menu = new Menu("Main Menu")
                .Add("Exit", (m) => m.Close())
                .Add("Say Hello", () => Console.WriteLine("Hello, world!"))
                .Add("To Farm", new Menu("Farm")
                    .Add("Go Back", (m) => m.Close())
                    .Add("Feed Animals", () => Console.WriteLine("FeedAnimals")));
                    
            Menu.Processor = new ScrollProcessor();
            menu.Run();
        }
    }

    class MyCustomProcessor : Processor
    {
        private static readonly char[] Letters = {'a', 'b', 'c', 'd' }; // ... and so on...

        public MyCustomProcessor(): base()
        {
            Menu.KeyPressed += OnConsoleInput;
        }

        public override void OnConsoleInput(object sender, ConsoleKeyInfo keyInfo)
        {
            bool found = false;
            for (int i = 0; i < Letters.Length; i++)
            {
                if (keyInfo.KeyChar == Letters[i])
                {
                    found = true;
                    Menu.MenuItems[i].Action?.Invoke();
                    break;
                }
            }

            if (!found)
                Console.WriteLine("Invalid selection.");
        }

        public override void Redraw()
        {
            Console.Clear();
            
            int[] colors = { (int) ConsoleColor.Red, (int) ConsoleColor.White, (int) ConsoleColor.Blue };

            var i = 0;
            foreach (var item in Menu.MenuItems)
            {
                Console.BackgroundColor = (ConsoleColor) colors[i & 3];
                ItemHandler?.Invoke(item);
                Console.ResetColor();
                
                i = (i + 1) % 3;
            }
        }

        public override Action<MenuItem> ItemHandler
        {
            get => (item) => Console.WriteLine($"<{Letters[item.Index]}> {item.Name}");
            set => _itemHandler = value;
        }
    }
}