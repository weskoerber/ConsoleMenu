namespace ConsoleMenu
{
    internal static class ConsoleSettings
    {
        public static SelectionType selectionType = SelectionType.Numeric;
        
        public enum SelectionType
        {
            Numeric,
            Scroll,
        }
    }
}