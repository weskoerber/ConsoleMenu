using System;

namespace ConsoleMenu.Processors
{
    public abstract class Processor
    {
        protected Action<MenuItem> _itemHandler;

        public abstract void HandleInput(ConsoleKeyInfo keyInfo);

        public abstract void PrintMenu();

        public bool ReadyToInvoke { get; set; }

        public Menu Menu => Menu.Current;

        public abstract Action<MenuItem> ItemHandler { get; set; }
    }
}