namespace TaskTracker.Application.DTOs.Common.Grid
{
    public class GridResult<T>
    {

        public GridEntity<T> Data(List<T> list, int totalCount)
        {
            var dEntity = new GridEntity<T>();
            dEntity.Items = list ?? new List<T>();
            dEntity.TotalCount = totalCount;
            dEntity.Columnses = new List<GridColumns>();
            return dEntity;
        }
    }
}
