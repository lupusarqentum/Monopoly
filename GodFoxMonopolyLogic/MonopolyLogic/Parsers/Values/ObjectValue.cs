namespace MonopolyLogic.Parsers.Values
{
    public sealed class ObjectValue : Value
    {
        private readonly object obj;

        public ObjectValue(object obj) => this.obj = obj;

        public override object AsObject => obj;

        public override string ToString() => obj.ToString();
    }
}
