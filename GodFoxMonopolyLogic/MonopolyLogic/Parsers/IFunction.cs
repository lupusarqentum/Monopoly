using MonopolyLogic.Parsers.Values;

namespace MonopolyLogic.Parsers
{
    public interface IFunction
    {
        Value Execute(VirtualMachine vm, Value[] args);
    }
}
