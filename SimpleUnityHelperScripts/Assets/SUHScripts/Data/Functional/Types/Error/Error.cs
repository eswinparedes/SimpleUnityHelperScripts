namespace SUHScripts.Functional
{
    public class Error
    {
        public virtual string Message { get; }
        public override string ToString() => Message;
        protected Error() { }
        internal Error(string Message) { this.Message = Message; }

        public static implicit operator Error(string m) => new Error(m);
    }
}
