namespace TaskTracker.Application.DTOs.Common.Grid
{
    public class GridColumns
    {
        public string field { get; set; }
        public string title { get; set; }
        public string width { get; set; }
        public bool filterable { get; set; }
        public bool sortable { get; set; }
        public bool hidden { get; set; }
    }
}
