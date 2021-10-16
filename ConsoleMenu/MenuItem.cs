using System;

namespace ConsoleMenu
{
    public class MenuItem
    {
        public bool IsMenu { get; }
        public int Index { get; }
        public string Name { get; }
        public Action Action { get; }

        internal MenuItem(int index, string name, Action action)
        {
            Index = index;
            Name = name;
            Action = action;
        }

        internal MenuItem(int index, string name, Action<Menu> action)
        {
            IsMenu = true;
        }

        public MenuItem(string name, Action action)
        {
            Name = name;
            Action = action;
        }
    }
}