namespace MonopolyLogic.Parsers.ast
{
    internal interface IStatement
    {
        void Execute(VirtualMachine vm);
    }
}
