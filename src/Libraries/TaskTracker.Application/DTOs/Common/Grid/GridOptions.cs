namespace TaskTracker.Application.DTOs.Common.Grid
{
    public class GridOptions
    {
        public int skip { get; set; }
        public int take { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public List<AzFilter.GridSort> sort { get; set; } = new List<AzFilter.GridSort>();
        public AzFilter.GridFilters filter { get; set; } = new AzFilter.GridFilters();
        public CommonVM vm { get; set; }
        public GridOptions()
        {
            vm = new CommonVM();
        }
    }

}
