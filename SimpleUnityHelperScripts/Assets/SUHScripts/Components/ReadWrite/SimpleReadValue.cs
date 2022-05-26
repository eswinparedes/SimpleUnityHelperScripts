public class SimpleReadValue<T> : IReadValue<T>
{
    public SimpleReadValue(T value)
    {
        Value = value;
    }

    public T Value { get; set; }

    public T Read()
    {
        return Value;
    }
}