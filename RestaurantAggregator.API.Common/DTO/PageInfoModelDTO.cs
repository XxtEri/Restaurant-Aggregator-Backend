namespace RestaurantAggregator.API.Common.DTO;

public class PageInfoModelDTO
{
    public int Size { get; set; }
    public int Count { get; set; }
    public int Current { get; set; }

    public PageInfoModelDTO(int size, int count, int current)
    {
        Size = size;
        Count = count;
        Current = current;
    }
}