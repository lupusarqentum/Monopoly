using System;

namespace MonopolyLogic.Parsers.Values
{
    public abstract class Value
    {
        public virtual int AsNumber => throw new Exception("This is not a number!");
        public abstract object AsObject { get; }

        public abstract override string ToString();
    }
}
