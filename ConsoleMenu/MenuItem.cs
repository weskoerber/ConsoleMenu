using System;

namespace ConsoleMenu
{
    public class MenuItem
    {
        public bool IsMenu { get; }
        public int Index { get; }
        public string Name { get; }
        public Action Action { get; }

        // For creating the menus
        internal MenuItem(int index, string name, Action action)
        {
            Index = index;
            Name = name;
            Action = action;
        }

        // For sub-menus
        internal MenuItem(int index, string name, Menu child)
        {
            Index = index;
            Name = name;
            Action = () => child.Run();
            IsMenu = true;
        }

        // Public use
        public MenuItem(string name, Action action)
        {
            Name = name;
            Action = action;
        }
    }
}