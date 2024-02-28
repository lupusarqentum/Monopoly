using System.Text;

namespace MonopolyLogic.Parsers.Values
{
    public sealed class ArrayValue : Value
    {
        private readonly Value[] values;

        public override object AsObject
        {
            get
            {
                int[] values = new int[this.values.Length];
                for (int i = 0; i < this.values.Length; i++)
                    values[0] = this.values[0].AsNumber;
                return values;
            }
        }

        public ArrayValue(Value[] values) => this.values = values;

        public override string ToString()
        {
            var buffer = new StringBuilder("[");
            for (int i = 0; i < values.Length - 1; i++)
                buffer.Append(values[i].ToString() + ", ");
            buffer.Append(values[values.Length - 1].ToString() + "]");
            return buffer.ToString();
        }
    }
}
