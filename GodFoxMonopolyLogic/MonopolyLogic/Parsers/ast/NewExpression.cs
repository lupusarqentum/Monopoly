using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using MonopolyLogic.Parsers.Values;

namespace MonopolyLogic.Parsers.ast
{
    internal sealed class NewExpression : IExpression
    {
        private readonly string name;
        private readonly List<IExpression> arguments;

        public NewExpression(string name, List<IExpression> arguments)
        {
            this.name = name;
            this.arguments = arguments;
        }

        public Value Eval(VirtualMachine vm)
        {
            var type = TypesRegistry.FindTypeByName(name);
            var argumentsTypes = new List<Type>();
            var parameters = new List<object>();

            foreach (var argument in arguments)
            {
                var param = argument.Eval(vm).AsObject;

                argumentsTypes.Add(param.GetType());
                parameters.Add(param);
            }

            var constructor = type.GetConstructor(argumentsTypes.ToArray());
            var instance = constructor.Invoke(parameters.ToArray());

            return new ObjectValue(instance);
        }

        public override string ToString()
        {
            var buffer = new StringBuilder($"new {name}(");

            if (arguments.Count == 0) 
                return buffer.ToString() + ")";

            for (int i = 0; i < arguments.Count - 1; i++)
                buffer.Append(arguments[i].ToString() + ", ");
            buffer.Append(arguments[arguments.Count - 1].ToString() + ")");
            
            return buffer.ToString();
        }
    }
}
