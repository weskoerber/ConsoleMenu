using ConsoleMenu.Processors;
using System;
using System.Collections.Generic;

namespace ConsoleMenu
{
    public class Menu
    {
        private bool _isRunning;
        private List<MenuItem> _menuItems;
        private Menu _parent;
        private static Menu _current;
        private static Processor _processor;
        public event EventHandler Closing;

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
            Current = this;
            _menuItems = new List<MenuItem>();
        }

        public void Run()
        {
            Current = this;
            _isRunning = true;

            SelectedItem = MenuItems[0];
            
            Console.Clear();
            Menu.Processor.PrintMenu();
            
            while (_isRunning)
            {
                if (Processor.ReadyToInvoke)
                {
                    Processor.ReadyToInvoke = false;
                    SelectedItem.Action?.Invoke();
                    Menu.Processor.PrintMenu();
                }
                else
                {
                    WaitForSelection();
                }
            }
        }

        public void Close()
        {
            _isRunning = false;
            Closing?.Invoke(this, EventArgs.Empty);

            if (Parent != null)
                Current = Parent;

            Menu.Processor.PrintMenu();
        }

        public Menu Add(string name, Action action)
        {
            var item = new MenuItem(MenuItems.Count, name, action);

            _menuItems.Add(item);

            return this;
        }

        public Menu Add(string name, Action<Menu> action)
        {
            var item = new MenuItem(MenuItems.Count, name, () => action(this));

            _menuItems.Add(item);

            return this;
        }

        public Menu Add(string name, Menu child)
        {
            child.Parent = this;

            _menuItems.Add(new MenuItem(MenuItems.Count, name, child));

            return this;
        }

        private void WaitForSelection()
        {
            var input = Console.ReadKey(true);

            Processor.HandleInput(input);
        }

        public IReadOnlyList<MenuItem> MenuItems => _menuItems;

        public bool IsRunning => _isRunning;

        public string Title { get; private set; }

        public MenuItem SelectedItem { get; set; }

        public Menu Parent
        {
            get => _parent;
            set
            {
                if (value != null)
                    _parent = value;
            }
        }

        public static Processor Processor
        { 
            get
            {
                if (_processor == null)
                    _processor = new SelectionProcessor();

                return _processor;
            }
            set
            {
                if (value != null)
                    _processor = value;
            }
        }

        public static Menu Current
        {
            get => _current;
            private set
            {
                if (value != null)
                    _current = value;
            }
        }
    }
}
