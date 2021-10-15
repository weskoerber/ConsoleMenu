using System;

namespace ConsoleMenu
{
    internal class MenuItem
    {
        public int index { get; set; }
        public string name { get; set; }
        public Action action { get; }

        public MenuItem(int index, string name, Action action)
        {
            this.index = index;
            this.name = name;
            this.action = action;
        }
    }
}