using ConsoleMenu.Processors;
using System;
using System.Collections.Generic;

namespace ConsoleMenu
{
    public class Menu
    {
        private bool _isRunning;
        private List<MenuItem> _menuItems;

        public IReadOnlyList<MenuItem> MenuItems => _menuItems;
        public event EventHandler Closing;

        public MenuItem SelectedItem { get; set; }
        public bool IsRunning => _isRunning;
        public int SeparatorWidth { get; private set; }
        public string Title { get; private set; }
        public Menu Parent { get; set; }

        private static Menu Current { get; set; }
        public static IProcessor Processor { get; set; }

        public Menu(string title)
        {
            try
            {
                Console.CursorVisible = false;
            }
            catch (System.IO.IOException)
            {
                Environment.Exit(1);
            }

            Title = title;

            Processor ??= new SelectionProcessor();

            _menuItems = new List<MenuItem>();

            ConsoleSettings.NavChanged += OnNavChanged;
        }

        public void Run()
        {
            Current = this;
            Console.Clear();
            SelectedItem = MenuItems[0];

            _isRunning = true;
            Menu.Processor.PrintMenu(Current);
            
            while (_isRunning)
            {
                WaitForSelection();
                Menu.Processor.PrintMenu(Current);
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

            _menuItems.Add(new MenuItem(MenuItems.Count, name, child));

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

            if (width > SeparatorWidth)
            {
                SeparatorWidth = width;
            }
        }

        private void WaitForSelection()
        {
            var input = Console.ReadKey(true);

            Processor.HandleInput(input);
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
