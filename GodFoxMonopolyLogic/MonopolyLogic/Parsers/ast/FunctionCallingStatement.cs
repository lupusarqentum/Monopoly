using System.Text;
using System.Linq;

namespace MonopolyLogic.Parsers.ast
{
    internal sealed class FunctionCallingStatement : IStatement
    {
        private readonly string name;
        private readonly IExpression[] arguments;

        public FunctionCallingStatement(string name, IExpression[] arguments)
        {
            this.name = name;
            this.arguments = arguments;
        }

        public void Execute(VirtualMachine vm)
        {
            Functions.Get(name).Execute(vm, (from arg in arguments
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
