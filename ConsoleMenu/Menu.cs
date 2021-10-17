using ConsoleMenu.Processors;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleMenu
{
    public class Menu
    {
        private bool _isRunning;
        private ObservableCollection<MenuItem> _menuItems;
        private Menu _parent;
        private static Menu _current;
        private static Processor _processor;
        public event EventHandler Closing;
        public static event EventHandler<ConsoleKeyInfo> KeyPressed;

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
            _menuItems = new ObservableCollection<MenuItem>();
        }

        public void Run()
        {
            _current = this;
            _isRunning = true;
            _current._menuItems.CollectionChanged += OnMenuItemsChanged;
            Processor.Redraw();

            while (IsRunning)
            {
                var cki = Console.ReadKey(true);
                KeyPressed?.Invoke(this, cki);
            }
        }

        public void Close()
        {
            _isRunning = false;
            Closing?.Invoke(this, EventArgs.Empty);

            if (Parent != null)
            {
                Current = Parent;
                Processor.Redraw();
            }
        }

        public Menu Add(string name, Action action)
        {
            var item = new MenuItem(_menuItems.Count, name, action);

            _menuItems.Add(item);

            return this;
        }

        public Menu Add(string name, Action<Menu> action)
        {
            var item = new MenuItem(_menuItems.Count, name, () => action(this));

            _menuItems.Add(item);

            return this;
        }

        public Menu Add(string name, Menu child)
        {
            child._parent = this;

            _menuItems.Add(new MenuItem(_menuItems.Count, name, child));

            return this;
        }

        private static void OnMenuItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Processor.Redraw();
        }

        public ObservableCollection<MenuItem> MenuItems => _menuItems;

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
                if (value != null && value != _processor)
                {
                    _processor = value;
                }
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
