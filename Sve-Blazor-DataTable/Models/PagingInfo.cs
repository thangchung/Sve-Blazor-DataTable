namespace Sve.Blazor.DataTable.Models
{
    public class PagingInfo
    {
        public int PageNr { get; private set; }

        public int PageSize { get; private set; }

        public int PageCount { get; private set; }

        public long TotalRecordCount { get; private set; }

        public string? SortColumn { get; private set; }

        public SortDirection? SortDirection { get; private set; }

        public PagingInfo(
            int pageNr,
            int pageSize,
            int pageCount,
            long totalRecordCount,
            string? sortColumn,
            SortDirection? sortDirection)
        {
            this.PageNr = pageNr;
            this.PageSize = pageSize;
            this.PageCount = pageCount;
            this.TotalRecordCount = totalRecordCount;
            this.SortColumn = sortColumn;
            this.SortDirection = sortDirection;
        }

        public PagingInfo(int pageNr, int pageSize, int pageCount, long totalRecordCount)
        {
            this.PageNr = pageNr;
            this.PageSize = pageSize;
            this.PageCount = pageCount;
            this.TotalRecordCount = totalRecordCount;
        }
    }
}
