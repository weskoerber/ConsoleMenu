using System;
using System.Collections.Generic;

namespace ConsoleMenu
{
    public class Menu
    {
        private bool _isRunning;
        private bool _readyToInvoke;        
        private int _selectedItem;
        private int _separatorWidth;
        private List<MenuItem> _menuItems;
        private static Menu Current { get; set; }

        public static Action<MenuItem> ItemHandler { get; set; }
        public static Action<Menu> RenderHandler { get; set; }

        public string Title { get; private set; }
        public Menu Parent { get; set; }
        public IReadOnlyList<MenuItem> MenuItems => _menuItems;
        public event EventHandler Closing;

        public Menu(string title)
        {
            try
            {
                Console.GetCursorPosition();
            }
            catch (System.IO.IOException)
            {
                Environment.Exit(1);
            }

            Title = title;

            RenderHandler ??= (menu) => Render();
            ItemHandler ??= (item) => PrintItem(item);

            _menuItems = new List<MenuItem>();

            ConsoleSettings.NavChanged += OnNavChanged;
        }

        public void Run()
        {
            Current = this;
            Console.CursorVisible = false;
            Console.Clear();

            _isRunning = true;
            
            while (_isRunning)
            {
                Menu.RenderHandler?.Invoke(Current);
                WaitForSelection();
                
                if (_readyToInvoke)
                {
                    Current.MenuItems[_selectedItem].Action?.Invoke();
                    _readyToInvoke = false;
                }
            }
        }

        public void Close()
        {
            _isRunning = false;
            Closing?.Invoke(this, EventArgs.Empty);

            if (Parent != null)
            {
                Current = Parent;
            }            
        }

        public Menu Add(string name, Action action)
        {
            var item = new MenuItem(MenuItems.Count, name, action);

            _menuItems.Add(item);

            SetSeparatorWidth(item);

            return this;
        }

        public Menu Add(string name, Action<Menu> action)
        {
            var item = new MenuItem(MenuItems.Count, name, () => action(this));

            _menuItems.Add(item);

            SetSeparatorWidth(item);

            return this;
        }

        public Menu Add(string name, Menu child)
        {
            child.Parent = this;

            _menuItems.Add(new MenuItem(MenuItems.Count, name, () => child.Run()));

            return this;
        }

        private void SetSeparatorWidth(MenuItem item)
        {
            int width;
            if (ConsoleSettings.Nav == ConsoleSettings.Navigation.Scroll)
            {
                width = $"{MenuItems.Count}: {item.Name}".Length;
            }
            else
            {
                width = $"==> {item.Name}".Length;
            }

            if (width > _separatorWidth)
            {
                _separatorWidth = width;
            }
        }

        private void Render()
        {
            Console.WriteLine(Current.Title);
            Console.WriteLine(new string('=', _separatorWidth));

            if (ConsoleSettings.Nav == ConsoleSettings.Navigation.Scroll)
            {
                foreach (var item in Current.MenuItems)
                {
                    if (item.Index == Current._selectedItem)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        ItemHandler?.Invoke(item);
                        Console.ForegroundColor = ConsoleColor.Gray;

                        Current._selectedItem = item.Index;
                    }
                    else
                    {
                        ItemHandler?.Invoke(item);
                    }
                }
            }
            else
            {
                foreach (var item in Current.MenuItems)
                {
                    ItemHandler?.Invoke(item);
                }
            }
        }

        private void PrintItem(MenuItem item)
        {
            string output;

            if (ConsoleSettings.Nav == ConsoleSettings.Navigation.Numeric)
            {
                output = string.Format("{0,2}: {1}", item.Index, item.Name);
            }
            else
            {
                output = Current._selectedItem == item.Index ? 
                    $"==> {item.Name}" : 
                    $"    {item.Name}";
            }

            Console.WriteLine(output);
        }

        private void WaitForSelection()
        {
            var input = Console.ReadKey(true);

            if (ConsoleSettings.Nav == ConsoleSettings.Navigation.Numeric)
            {
                Console.WriteLine();
            }

            ProcessInput(input);
        }

        private void ProcessInput(ConsoleKeyInfo key)
        {   
            Console.Clear();

            if (ConsoleSettings.Nav == ConsoleSettings.Navigation.Numeric)
            {
                if (int.TryParse(key.KeyChar.ToString(), out int idx) && idx < Current.MenuItems.Count)
                {
                    _selectedItem = idx;
                    _readyToInvoke = true;
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
                        if (_selectedItem + 1 <= Current.MenuItems.Count - 1)
                        {
                            _selectedItem++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.K:
                        if (_selectedItem - 1 >= 0)
                        {
                            _selectedItem--;
                        }
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.L:
                        _readyToInvoke = true;

                        break;
                }
            }
        }

        private void OnNavChanged(object sender, NavChangedEventArgs e)
        {            
            Console.Clear();
            if (e.Nav == ConsoleSettings.Navigation.Numeric)
            {
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
