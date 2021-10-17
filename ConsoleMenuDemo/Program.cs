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
                    
            menu.Run();
        }
    }
}