namespace SUHScripts.Functional
{
    public static partial class Functional
    {
        public static Left<L> Left<L>(L l) => new Left<L>(l);
        public static Right<R> Right<R>(R r) => new Right<R>(r);
    }
}

