namespace MonopolyLogic.Parsers.Values
{
    public sealed class NumberValue : Value
    {
        public static readonly NumberValue Zero = new NumberValue(0);

        private readonly int value;

        public NumberValue(int value) => this.value = value;

        public override int AsNumber => value;

        public override object AsObject => value;

        public override string ToString() => value.ToString();
    }
}
