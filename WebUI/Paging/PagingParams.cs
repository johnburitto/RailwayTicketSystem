namespace WebUI.Paging
{
    public class PagingParams
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public override string ToString()
        {
            return $"pageindex={PageIndex}&pagesize={PageSize}";
        }
    }
}
