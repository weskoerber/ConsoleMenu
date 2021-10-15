using System;
using System.Collections.Generic;
using System.Timers;

namespace ConsoleMenu
{
    internal static class ConsoleMenu
    {
        private static bool IsRunning = true;

        private static bool ReadyToInvoke = false;
        
        private static int SelectedItem = 0;

        private static readonly List<MenuItem> _menuItems = new List<MenuItem>();

        public static Timer clearInterval = new Timer(3000);

        static void Main(string[] args)
        {
            Add("Test", () => Console.WriteLine("Test!"));
            Add("Use Scroll", () => {
                ConsoleSettings.selectionType = ConsoleSettings.SelectionType.Scroll; 
                Console.Clear();
                Console.CursorVisible = false;
            });
            Add("Use Numeric", () => {
                ConsoleSettings.selectionType = ConsoleSettings.SelectionType.Numeric;
                Console.CursorVisible = true;
            });

            while (IsRunning)
            {
                Render();
                WaitForSelection();
            }

            if (!IsRunning)
            {
                HandleExit();
            }
        }

        public static void InitMenu()
        {
            _menuItems.Clear();
        }

        public static void Add(string name, Action action)
        {
            _menuItems.Add(new MenuItem(_menuItems.Count, name, action));
        }
         public static void Render()
        {
            if (ConsoleSettings.selectionType == ConsoleSettings.SelectionType.Scroll)
            {
                for (int i = 0; i < _menuItems.Count; i++)
                {
                    if (i == SelectedItem)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        PrintItem(_menuItems[i]);
                        Console.ForegroundColor = ConsoleColor.White;

                        SelectedItem = _menuItems[i].index;
                    }
                    else
                    {
                        PrintItem(_menuItems[i]);
                    }
                }

                if (ReadyToInvoke)
                {
                    Console.WriteLine();
                    _menuItems[SelectedItem].action?.Invoke();
                    ReadyToInvoke = false;
                    HandleTimer(true);
                }
            }
            else
            {
                foreach (var item in _menuItems)
                {
                    PrintItem(item);
                }

                Console.Write("\n> ");
            }
        }

        public static void PrintItem(MenuItem item)
        {
            if (ConsoleSettings.selectionType == ConsoleSettings.SelectionType.Numeric)
            {
                Console.WriteLine($"{item.index}: {item.name}");
            }
            else
            {
                if (SelectedItem == item.index)
                {
                    Console.WriteLine($"==> {item.name}");
                }
                else
                {
                    Console.WriteLine($"    {item.name}");
                }
            }
        }

        public static ConsoleKeyInfo WaitForSelection()
        {
            var input = Console.ReadKey(true);

            if (ConsoleSettings.selectionType == ConsoleSettings.SelectionType.Numeric)
            {
                Console.WriteLine();
            }

            ProcessInput(input);

            return input;
        }

        public static void ProcessInput(ConsoleKeyInfo key)
        {
            if (ConsoleSettings.selectionType == ConsoleSettings.SelectionType.Numeric)
            {
                HandleTimer(false);

                if (key.Key == ConsoleKey.X)
                {
                    IsRunning = false;
                }

                if (int.TryParse(key.KeyChar.ToString(), out int idx) && idx < _menuItems.Count)
                {
                    _menuItems[idx].action?.Invoke();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid selection!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else
            {
                Console.Clear();

                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.J:
                        if (SelectedItem + 1 <= _menuItems.Count - 1)
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

        public static void HandleTimer(bool enable)
        {
            if (enable)
            {
                clearInterval.Enabled = true;
                clearInterval.Elapsed += TimerElapsed;

                clearInterval.Start();
            }
            else
            {
                clearInterval.Stop();
            }
        }

        public static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Console.Clear();
            clearInterval.Stop();
            Render();
        }

        public static void HandleExit()
        {
            Console.WriteLine("Exiting gracefully...");
        }
    }
}
