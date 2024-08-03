using Data.Enums;

namespace WebClient.Utilities
{
    internal class Pagination
    {
        private readonly int _pageSize;
        private readonly int _pageCount;
        private readonly int _pageIndex;

        public Pagination(IConfiguration config, int itemCount, int pageIndex)
        {
            int pageSizeSetting = Convert.ToInt32(config?["PageSize"] ?? "10");
            _pageSize = pageSizeSetting < 5 ? 10 : pageSizeSetting;

            if (itemCount % _pageSize == 0)
            {
                this._pageCount = itemCount / _pageSize;
            }
            else
            {
                this._pageCount = (itemCount / _pageSize) + 1;
            }

            if (pageIndex > this._pageCount)
            {
                this._pageIndex = this._pageCount;
            }
            else if (pageIndex < 1)
            {
                this._pageIndex = 1;
            }
            else
            {
                this._pageIndex = pageIndex;
            }
        }

        public int PageSize => this._pageSize;

        public int PageIndex => this._pageIndex;

        public int PageCount => this._pageCount;

        public int PrevPageIndex
        {
            get
            {
                int prevPageIndex = this._pageIndex - 1;
                return prevPageIndex > 0 ? prevPageIndex : 1;
            }
        }

        public int NextPageIndex
        {
            get
            {
                int nextPageIndex = this._pageIndex + 1;
                return nextPageIndex <= this._pageCount ? nextPageIndex : this._pageCount;
            }
        }

        public IEnumerable<T> GetPagedObjects<T>(IEnumerable<T> items)
        {
            return items.Skip((this.PageIndex - 1) * PageSize)
						.Take(PageSize);
		}

        public string SortingField { get; set; } = string.Empty;

        public SortingDirection SortingDirection { get; set; } = SortingDirection.Ascending;

        public string SearchString { get; set; } = string.Empty;

        public SortingDirection GetNextSortingDirection(string fieldName)
		{
			if (string.Compare(fieldName, string.IsNullOrEmpty(this.SortingField) ? "ID" : this.SortingField, true) == 0)
			{
				return this.SortingDirection == SortingDirection.Ascending ? SortingDirection.Descending : SortingDirection.Ascending;
			}
			else
			{
				return SortingDirection.Ascending;
			}
		}
	}
}
