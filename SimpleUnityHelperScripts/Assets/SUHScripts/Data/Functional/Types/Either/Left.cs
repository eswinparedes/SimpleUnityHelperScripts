namespace SUHScripts.Functional
{
    public struct Left<L>
    {
        internal L Value { get; }
        internal Left(L value) { Value = value; }

        public override string ToString() => $"Left({Value})";
    }

}