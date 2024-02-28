namespace MonopolyLogic.Parsers.ast
{
    internal sealed class AssignmentStatement : IStatement
    {
        private readonly string name;
        private readonly IExpression expression;

        public AssignmentStatement(string name, IExpression expression)
        {
            this.name = name;
            this.expression = expression;
        }

        public void Execute(VirtualMachine vm) 
            => vm.variables.AssignVariable(name, expression.Eval(vm));

        public override string ToString() => $"{name} = {expression}";
    }
}
