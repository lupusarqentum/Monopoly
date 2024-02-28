using System;
using System.Reflection;
using System.Collections.Generic;

namespace MonopolyLogic.Parsers
{
    public static class TypesRegistry
    {
        private static readonly List<Type> spacesNamespaceTypes;
        private static readonly List<Type> propertiesNamespaceTypes;

        static TypesRegistry()
        {
            spacesNamespaceTypes = new List<Type>();
            propertiesNamespaceTypes = new List<Type>();

            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var _type = Type.GetType(type.FullName);

                if (_type.FullName.StartsWith("MonopolyLogic.Spaces"))
                    AddSpaceToRegistry(_type);
                else if (_type.FullName.StartsWith("MonopolyLogic.GameProperties"))
                    AddPropertyToRegistry(_type);
            }
        }

        public static void AddSpaceToRegistry(Type type)
            => spacesNamespaceTypes.Add(type);

        public static void AddPropertyToRegistry(Type type)
            => propertiesNamespaceTypes.Add(type);

        public static Type FindTypeByName(string name)
        {
            foreach (var type in spacesNamespaceTypes)
                if (type.FullName.EndsWith(name))
                    return type;

            foreach (var type in propertiesNamespaceTypes)
                if (type.FullName.EndsWith(name))
                    return type;

            throw new Exception($"Type with name \"{name}\" not found");
        }
    }
}
