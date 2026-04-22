namespace TaskTracker.Application.DTOs.Common.Grid
{
    public class AutoCompOptions
    {
        public int skip { get; set; }
        public int take { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public AzFilter.AutoCompFilters filter { get; set; }
    }
}
