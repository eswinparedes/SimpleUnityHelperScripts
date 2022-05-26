using System.Collections.Generic;

namespace SUHScripts.Functional
{
    public static partial class Functional
    {
        public static Validation<T> Valid<T>(T value) => new Validation<T>(value);

        // create a Validation in the Invalid state
        public static Invalid Invalid(params Error[] errors) => new Invalid(errors);
        public static Validation<R> Invalid<R>(params Error[] errors) => new Invalid(errors);
        public static Invalid Invalid(IEnumerable<Error> errors) => new Invalid(errors);
        public static Validation<R> Invalid<R>(IEnumerable<Error> errors) => new Invalid(errors);
    }
}
