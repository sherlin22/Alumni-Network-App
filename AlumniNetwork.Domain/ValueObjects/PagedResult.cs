namespace AlumniNetwork.Domain.ValueObjects;

public class PagedResult<T>
{
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public IReadOnlyCollection<T> Data { get; init; } = Array.Empty<T>();
}
