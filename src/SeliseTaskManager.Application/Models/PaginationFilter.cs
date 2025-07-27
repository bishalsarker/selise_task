namespace SeliseTaskManager.Application.Interfaces.Models
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;

        public string OrderBy { get; set; } = "ASC";
    }
}
