using System.Collections.Generic;

namespace MonopolyLogic.GameProperties
{
    public class RentaPropertyGroup
    {
        protected readonly List<RentaProperty> properties;

        public RentaProperty this [int index] => properties[index];
        public virtual int CountOfPropertiesInGroup => properties.Count;

        public RentaPropertyGroup() => properties = new List<RentaProperty>();

        internal virtual void Add(RentaProperty gameProperty) => properties.Add(gameProperty);
    }
}
