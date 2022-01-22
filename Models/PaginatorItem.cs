namespace DraftDesktopApp.Models
{
    public class PaginatorItem
    {
        public PaginatorItem(int number, bool isSelected)
        {
            Number = number;
            IsSelected = isSelected;
        }

        public int Number { get; }
        public bool IsSelected { get; }
    }
}
