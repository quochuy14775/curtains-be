namespace curtains_be.Common;

public class ODataResponse<T>
{
    public int Count { get; set; }
    public IEnumerable<T> Value { get; set; } = [];
}
