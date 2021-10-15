using System;
using System.Collections.Generic;
using ConsoleMenu;

namespace ConsoleMenuDemo
{
    class ConsoleMenuDemo
    {
        static void Main(string[] args)
        {
            Menu.InitMenu();

            Menu.Add(new List<MenuItem>
                {
                    new ("Exit", () => Environment.Exit(0)),
                    new ("Say Hello", () => Console.WriteLine("Hello, world!")),
                    new ("Use Numeric", () => ConsoleSettings.Nav = ConsoleSettings.Navigation.Numeric),
                    new ("Use Scroll", () => ConsoleSettings.Nav = ConsoleSettings.Navigation.Scroll),
                }
            );
            
            Menu.Run();
        }
    }
}