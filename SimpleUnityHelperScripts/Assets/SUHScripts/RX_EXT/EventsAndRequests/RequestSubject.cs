using System;

namespace SUHScripts
{
    public class RequestSubject<TInput, TResponse> : IRequestSubject<TInput, TResponse>
    {
        Func<TInput, TResponse> m_requestFunction;

        public RequestSubject(Func<TInput, TResponse> requestFunction)
        {
            m_requestFunction = requestFunction;
        }

        public TResponse Request(TInput input)
        {
            return m_requestFunction(input);
        }
    }
}