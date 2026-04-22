namespace TaskTracker.Application.DTOs.Common.Grid
{
    public class GridEntity<T>
    {
        public IList<T> Items { get; set; }
        public int TotalCount { get; set; }
        public IList<GridColumns> Columnses { get; set; }

    }
}
