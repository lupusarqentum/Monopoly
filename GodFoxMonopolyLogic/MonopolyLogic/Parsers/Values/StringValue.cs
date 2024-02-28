namespace MonopolyLogic.Parsers.Values
{
    public sealed class StringValue : Value
    {
        private readonly string value;

        public StringValue(string value) => this.value = value;

        public override object AsObject => value;

        public override string ToString() => value;
    }
}
