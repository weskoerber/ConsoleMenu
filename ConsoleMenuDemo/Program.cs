using System;
using ConsoleMenu;
using ConsoleMenu.Processors;

namespace ConsoleMenuDemo
{
    class ConsoleMenuDemo
    {
        static void Main(string[] args)
        {
            // var mainMenu = new Menu("Main Menu")
            //     .Add("Exit", (m) => m.Close())
            //     .Add("Use Numeric", () => ConsoleSettings.Nav = ConsoleSettings.Navigation.Numeric)
            //     .Add("Use Scroll", () => ConsoleSettings.Nav = ConsoleSettings.Navigation.Scroll)
            //     .Add("Sub Menu", new Menu("Sub Menu")
            //         .Add("Go Back", (m) => m.Close())
            //         .Add("Kick Rocks", () => Console.WriteLine("Kicking rocks..."))
            //         .Add("Say Hello", () => Console.WriteLine("Hello, world!"))
            //         .Add("Another Sub Menu", new Menu("Sub Menu 2")
            //             .Add("Go Back", (m) => m.Close())));

            // mainMenu.Closing += (object s, EventArgs e) => Console.WriteLine($"Closing {(s as Menu).Title}");

            // Menu.ItemHandler = (MenuItem i) => Console.WriteLine($"({i.Index}) > {i.Name}");            
            // Menu.RenderHandler = (Menu m) => {
            //     int[] colors = { (int) ConsoleColor.Red, (int) ConsoleColor.White, (int) ConsoleColor.Blue };

            //     var i = 0;
            //     foreach (var item in m.MenuItems)
            //     {
            //         Console.BackgroundColor = (ConsoleColor) colors[i & 3];
            //         Menu.ItemHandler?.Invoke(item);
            //         Console.ResetColor();
                    
            //         i = (i + 1) % 3;
            //     }
            // };
            // mainMenu.Run();

            // ConsoleSettings.Nav = ConsoleSettings.Navigation.Scroll;
            var menu = new Menu("Main Menu")
                .Add("Exit", (m) => m.Close())
                .Add("Say Hello", () => Console.WriteLine("Hello, world!"))
                .Add("To Farm", new Menu("Farm")
                    .Add("Go Back", (m) => m.Close())
                    .Add("Feed Animals", () => Console.WriteLine("FeedAnimals")));

            // Menu.Processor = new ScrollProcessor();
            menu.Run();
        }
    }
}