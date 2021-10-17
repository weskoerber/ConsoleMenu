using System;

namespace ConsoleMenu.Processors
{
    public abstract class Processor
    {
        public abstract void OnConsoleInput(object sender, ConsoleKeyInfo keyInfo);

        public abstract void Redraw();

        protected Action<MenuItem> _itemHandler;

        public Menu Menu => Menu.Current;

        public abstract Action<MenuItem> ItemHandler { get; set; }
    }
}