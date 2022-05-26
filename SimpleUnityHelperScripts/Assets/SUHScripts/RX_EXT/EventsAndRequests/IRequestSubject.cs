

namespace SUHScripts
{
    public interface IRequestSubject<TInput, TResponse>
    {
        TResponse Request(TInput input);
    }

    public interface IRequestSubject<TInput0, TInput1, TResponse>
    {
        TResponse Request(TInput0 input0, TInput1 input1);
    }
}