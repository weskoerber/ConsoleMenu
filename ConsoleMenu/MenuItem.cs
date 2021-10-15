using System;

namespace ConsoleMenu
{
    public class MenuItem
    {
        public int Index { get; }
        public string Name { get; }
        public Action Action { get; }

        internal MenuItem(int index, string name, Action action)
        {
            Index = index;
            Name = name;
            Action = action;
        }

        public MenuItem(string name, Action action)
        {
            Name = name;
            Action = action;
        }
    }
}