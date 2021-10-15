using System;
using System.Collections.Generic;
using System.Timers;

namespace ConsoleMenu
{
    public static class Menu
    {
        private static bool IsRunning { get; set; }

        private static bool ReadyToInvoke { get; set; }
        
        private static int SelectedItem { get; set; }

        private static readonly List<MenuItem> MenuItems = new();

        private static readonly Timer ClearInterval = new(3000);

        public static void Run()
        {
            Console.CursorVisible = false;
            
            IsRunning = true;
            
            while (IsRunning)
            {
                Render();
                WaitForSelection();
            }
        }

        public static void InitMenu()
        {
            MenuItems.Clear();
            ConsoleSettings.NavChanged += OnNavChanged;
        }

        public static void Add(string name, Action action)
        {
            MenuItems.Add(new MenuItem(MenuItems.Count, name, action));
        }

        public static void Add(List<MenuItem> items)
        {
            foreach (var item in items)
            {
                MenuItems.Add(new MenuItem(MenuItems.Count, item.Name, item.Action));
            }
        }
         private static void Render()
        {
            if (ConsoleSettings.Nav == ConsoleSettings.Navigation.Scroll)
            {
                for (var i = 0; i < MenuItems.Count; i++)
                {
                    if (i == SelectedItem)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        PrintItem(MenuItems[i]);
                        Console.ForegroundColor = ConsoleColor.Gray;

                        SelectedItem = MenuItems[i].Index;
                    }
                    else
                    {
                        PrintItem(MenuItems[i]);
                    }
                }
            }
            else
            {
                foreach (var item in MenuItems)
                {
                    PrintItem(item);
                }
            }
            
            if (ReadyToInvoke)
            {
                Console.WriteLine();
                MenuItems[SelectedItem].Action?.Invoke();
                ReadyToInvoke = false;
                HandleTimer(true);
            }
        }

        private static void PrintItem(MenuItem item)
        {
            if (ConsoleSettings.Nav == ConsoleSettings.Navigation.Numeric)
            {
                Console.WriteLine($"{item.Index}: {item.Name}");
            }
            else
            {
                Console.WriteLine(SelectedItem == item.Index ? $"==> {item.Name}" : $"    {item.Name}");
            }
        }

        private static void WaitForSelection()
        {
            var input = Console.ReadKey(true);

            if (ConsoleSettings.Nav == ConsoleSettings.Navigation.Numeric)
            {
                Console.WriteLine();
            }

            ProcessInput(input);
        }

        private static void ProcessInput(ConsoleKeyInfo key)
        {
            Console.Clear();
            
            if (ConsoleSettings.Nav == ConsoleSettings.Navigation.Numeric)
            {
                HandleTimer(false);

                if (key.Key == ConsoleKey.X)
                {
                    IsRunning = false;
                }

                if (int.TryParse(key.KeyChar.ToString(), out int idx) && idx < MenuItems.Count)
                {
                    SelectedItem = idx;
                    ReadyToInvoke = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid selection!");
                    Console.ResetColor();
                }
            }
            else
            {
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.J:
                        if (SelectedItem + 1 <= MenuItems.Count - 1)
                        {
                            SelectedItem++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.K:
                        if (SelectedItem - 1 >= 0)
                        {
                            SelectedItem--;
                        }
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.L:
                        ReadyToInvoke = true;
                        break;
                }
            }
        }

        private static void HandleTimer(bool enable)
        {
            if (enable)
            {
                ClearInterval.Enabled = true;
                ClearInterval.Elapsed += TimerElapsed;

                ClearInterval.Start();
            }
            else
            {
                ClearInterval.Stop();
            }
        }

        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Console.Clear();
            ClearInterval.Stop();
            Render();
        }

        private static void OnNavChanged(object sender, NavChangedEventArgs e)
        {
            HandleTimer(false);
            
            Console.Clear();
            if (e.Nav == ConsoleSettings.Navigation.Numeric)
            {
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Render();
        }
    }
}
