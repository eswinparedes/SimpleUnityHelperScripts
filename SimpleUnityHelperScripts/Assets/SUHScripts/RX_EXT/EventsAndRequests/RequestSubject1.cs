using System;

namespace SUHScripts
{
    public class RequestSubject<TInput0, TInput1, TResponse> : IRequestSubject<TInput0, TInput1, TResponse>
    {
        Func<TInput0, TInput1, TResponse> m_requestFunction;

        public RequestSubject(Func<TInput0, TInput1, TResponse> requestFunction)
        {
            m_requestFunction = requestFunction;
        }

        public TResponse Request(TInput0 input0, TInput1 input1)
        {
            return m_requestFunction(input0, input1);
        }
    }
}