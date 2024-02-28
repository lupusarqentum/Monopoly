using MonopolyLogic.Parsers.Values;
using System.Linq;
using System.Text;

namespace MonopolyLogic.Parsers.ast
{
    internal sealed class FunctionCallingExpression : IExpression
    {
        private readonly string name;
        private readonly IExpression[] arguments;

        public FunctionCallingExpression(string name, IExpression[] arguments)
        {
            this.name = name;
            this.arguments = arguments;
        }

        public Value Eval(VirtualMachine vm)
        {
            return Functions.Get(name).Execute(vm, (from arg in arguments
                                                    select arg.Eval(vm)).ToArray());
        }

        public override string ToString()
        {
            var buffer = new StringBuilder(name + "(");
            for (int i = 0; i < arguments.Length - 1; i++)
                buffer.Append(arguments[i] + ", ");
            buffer.Append(arguments[arguments.Length - 1].ToString() + ")");
            return buffer.ToString();
        }
    }
}
