using System;

namespace ConsoleMenu.Processors
{
    public interface IProcessor
    {
        bool ReadyToInvoke { get; }

        Menu Menu { get; set; }

        Action<MenuItem> ItemHandler { get; set; }

        void HandleInput(ConsoleKeyInfo keyInfo);

        void PrintMenu(Menu menu);
    }
}