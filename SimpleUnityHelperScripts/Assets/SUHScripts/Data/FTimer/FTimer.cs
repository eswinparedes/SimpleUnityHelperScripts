namespace SUHScripts
{
    public struct FTimer
    {
        public readonly float Length;
        public readonly float ElapsedTime;
        public readonly bool IsIncrementing;

        public FTimer(float length, float elapsedTime, bool isIncrementing = true)
        {
            this.Length = length;
            this.ElapsedTime = elapsedTime;
            this.IsIncrementing = isIncrementing;
        }

        public override string ToString() =>
            $"Length: {Length} Elapsed: {ElapsedTime} IsIncrementing {IsIncrementing}";
    }

}
