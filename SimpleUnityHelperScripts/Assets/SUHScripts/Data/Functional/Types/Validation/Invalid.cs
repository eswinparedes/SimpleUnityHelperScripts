using System.Collections.Generic;

namespace SUHScripts.Functional
{
    public struct Invalid
    {
        internal IEnumerable<Error> Errors;
        public Invalid(IEnumerable<Error> errors) { Errors = errors; }
    }
}
