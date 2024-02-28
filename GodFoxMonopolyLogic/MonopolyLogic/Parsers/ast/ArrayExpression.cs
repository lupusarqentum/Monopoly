using System.Text;
using System.Collections.Generic;
using MonopolyLogic.Parsers.Values;

namespace MonopolyLogic.Parsers.ast
{
    internal sealed class ArrayExpression : IExpression
    {
        private readonly List<IExpression> elements;

        public ArrayExpression(List<IExpression> elements) => this.elements = elements;

        public Value Eval(VirtualMachine vm)
        {
            var values = new List<Value>();
            for (int i = 0; i < elements.Count; i++)
                values.Add(elements[i].Eval(vm));
            return new ArrayValue(values.ToArray());
        }

        public override string ToString()
        {
            var buffer = new StringBuilder("[");
            for (int i = 0; i < elements.Count - 1; i++)
                buffer.Append(elements[i].ToString() + ", ");
            buffer.Append(elements[elements.Count - 1].ToString() + "]");
            return buffer.ToString();
        }
    }
}
