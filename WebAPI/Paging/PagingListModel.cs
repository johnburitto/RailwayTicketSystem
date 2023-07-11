namespace WebAPI.Paging
{
    public class PagingListModel<T> : List<T>
    {
        public PagingParams PagingParams { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevoius => PagingParams.PageIndex > 1;
        public bool HasNext => PagingParams.PageIndex < TotalPages;

        private PagingListModel(List<T> source, int cout, PagingParams pagingParams)
        {
            PagingParams = pagingParams;
            TotalPages = (int)Math.Ceiling(cout / (double)pagingParams.PageSize);
            AddRange(source);
        }

        public static PagingListModel<T> Create(List<T> source, PagingParams pagingParams)
        {
            var cout = source.Count;
            var pageItems = source.Skip((pagingParams.PageIndex - 1) * pagingParams.PageSize).Take(pagingParams.PageSize).ToList();

            return new PagingListModel<T>(pageItems, cout, pagingParams);
        }

        public PagingParams NextPage()
        {
            PagingParams.PageIndex = HasNext ? PagingParams.PageIndex + 1 : PagingParams.PageIndex;

            return PagingParams;
        }

        public PagingParams PreviousPage()
        {
            PagingParams.PageIndex = HasPrevoius ? PagingParams.PageIndex - 1 : PagingParams.PageIndex;

            return PagingParams;
        }
    }
}
