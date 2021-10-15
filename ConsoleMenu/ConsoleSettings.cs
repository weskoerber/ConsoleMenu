using System;

namespace ConsoleMenu
{
    public static class ConsoleSettings
    {
        private static Navigation _nav;

        public static event EventHandler<NavChangedEventArgs> NavChanged;
        
        public enum Navigation
        {
            Numeric,
            Scroll,
        }

        public static Navigation Nav
        {
            get => _nav;
            set
            {
                if (_nav != value)
                {
                    _nav = value;
                    NavChanged?.Invoke(null, new NavChangedEventArgs(_nav));
                }
            }
        }
    }

    public class NavChangedEventArgs : EventArgs
    {
        public NavChangedEventArgs(ConsoleSettings.Navigation nav)
        {
            Nav = nav;
        }
        
        public ConsoleSettings.Navigation Nav { get; }
    }
}