namespace AlumniNetwork.Application.Common;

public class PagedResponse<T>
{
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public IReadOnlyCollection<T> Data { get; set; } = Array.Empty<T>();
}
