namespace BuildingBlocks.Pagination
{
    public class PaginatedResult<TEntity>(int pageIndex, int pageSize, long totalCount, IReadOnlyCollection<TEntity> items)
     where TEntity : class
    {
        public int PageIndex { get; } = pageIndex;
        public int PageSize { get; } = pageSize;
        public long TotalCount { get; } = totalCount;
        public IReadOnlyCollection<TEntity> Data { get; } = items;
    }
}
