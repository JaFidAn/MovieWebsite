namespace Application.Core;

public class PaginationParams
{
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;
    private const int MaxPageSize = 50;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}
