using MonopolyLogic.Parsers.Values;

namespace MonopolyLogic.Parsers.ast
{
    internal sealed class ValueExpression : IExpression
    {
        private readonly Value value;

        public ValueExpression(Value value) => this.value = value;

        public Value Eval(VirtualMachine vm) => value;

        public override string ToString()
        {
            return value.ToString();
        }
    }
}
