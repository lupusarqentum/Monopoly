using MonopolyLogic.Parsers.Values;

namespace MonopolyLogic.Parsers.ast
{
    internal sealed class VariableAccessingExpression : IExpression
    {
        private readonly string name;

        public VariableAccessingExpression(string name) => this.name = name;

        public Value Eval(VirtualMachine vm)
        {
            return vm.variables.GetVariable(name);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
