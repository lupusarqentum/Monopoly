using System.Collections.Generic;
using MonopolyLogic.Parsers.Values;

namespace MonopolyLogic.Parsers
{
    public sealed class VariablesMemory
    {
        private readonly Dictionary<string, Value> variables;

        public VariablesMemory()
        {
            variables = new Dictionary<string, Value>();
        }

        public void AssignVariable(string name, Value value) => variables[name] = value;
        public Value GetVariable(string name) => variables[name];
    }
}
