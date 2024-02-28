using System;
using System.Linq;
using System.Collections.Generic;
using MonopolyLogic.GameProperties;
using MonopolyLogic.Parsers.Values;
using MonopolyLogic.TaskWork;
using MonopolyLogic.Spaces;
using MonopolyLogic.Utils;

namespace MonopolyLogic.Parsers
{
    public static class Functions
    {
        public sealed class SystemFunction : IFunction
        {
            private readonly Func<VirtualMachine, Value[], Value> func;

            public SystemFunction(Func<VirtualMachine, Value[], Value> func) => this.func = func;

            public Value Execute(VirtualMachine vm, Value[] args) => func.Invoke(vm, args);
        }

        private readonly static Dictionary<string, IFunction> functions = new Dictionary<string, IFunction>();

        static Functions()
        {
            Functions.Define("setspace", new SystemFunction((vm, args) => 
            {
                var intCoord = args[0].AsNumber;
                var spcSpace = args[1].AsObject as Space;

                spcSpace.SpaceRepresenter = vm.spaceRepresenterGetter();
                vm.monopolyGame.Board.Spaces[intCoord] = spcSpace;

                return NumberValue.Zero;
            }));
            
            Functions.Define("initboard", new SystemFunction((vm, args) => 
            {
                vm.monopolyGame.Board.Spaces = new Space[args.SingleOrDefault().AsNumber];

                return NumberValue.Zero;
            }));

            Functions.Define("groups", new SystemFunction((vm, args) => 
            {
                foreach (var creatingGroup in args)
                    vm.variables.AssignVariable(creatingGroup.AsObject.ToString(), new ObjectValue(new RentaPropertyGroup()));

                return NumberValue.Zero;
            }));

            Functions.Define("taskcollectionsnames", new SystemFunction((vm, args) => 
            {
                var name = args.SingleOrDefault().AsObject.ToString();
                var value = typeof(TaskCollectionsNames).GetField(name).GetValue(null);

                return new StringValue((string)value);
            }));

            Functions.Define("translate", new SystemFunction((vm, args) => 
            {
                var key = args[0].AsObject.ToString();
                var objects = from arg in args
                              where !ReferenceEquals(arg, args[0])
                              select arg.AsObject;
                var translation = LanguagePack.GetTranslation(args[0].AsObject.ToString(), objects.ToArray());

                return new StringValue(translation);
            }));

            Functions.Define("addtask", new SystemFunction((vm, args) => 
            {
                var taskCollection = args[0].AsObject.ToString();
                var patternName = args[1].AsObject.ToString();

                var patternArgs = new List<object>();
                for (int i = 2; i < args.Length; i++)
                    patternArgs.Add(args[i].AsObject);
                
                var task = vm.monopolyGame.TaskSystem.TaskPatternsRegistry.GetTaskByPattern(patternName, patternArgs.ToArray());
                vm.monopolyGame.TaskSystem.TaskCollector.AddGameTask(taskCollection, task);

                return NumberValue.Zero;
            }));

            Functions.Define("addjailfreeticket", new SystemFunction((vm, args) => 
            {
                vm.monopolyGame.Jail.JailFreeTicketsRegistry.Add(args[0].AsObject.ToString(),
                    new JailFreeTicket(args[1].AsObject.ToString()));

                return NumberValue.Zero;
            }));

            Functions.Define("setconstant", new SystemFunction((vm, args) => 
            {
                var @const = typeof(Constants).GetProperty(args[0].AsObject.ToString());
                @const.SetValue(null, args[1].AsNumber);

                return NumberValue.Zero;
            }));

            Functions.Define("getconstant", new SystemFunction((vm, args) => 
            {
                var @const = typeof(Constants).GetProperty(args[0].AsObject.ToString());
                return new NumberValue((int)@const.GetValue(null));
            }));
        }

        public static void Define(string name, IFunction function) => functions.Add(name, function);

        internal static IFunction Get(string name) => functions[name];
    }
}
