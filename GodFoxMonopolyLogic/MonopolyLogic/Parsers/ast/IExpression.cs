using MonopolyLogic.Parsers.Values;

namespace MonopolyLogic.Parsers.ast
{
    internal interface IExpression
    {
        Value Eval(VirtualMachine vm);
    }
}
