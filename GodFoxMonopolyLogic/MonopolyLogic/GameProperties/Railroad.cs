using System;

namespace MonopolyLogic.GameProperties
{
    public class Railroad : MortgagableRentaProperty
    {
        public readonly int startRenta;

        public Railroad(string name, int cost, RentaPropertyGroup group, int startRenta) : base(name, cost, group) 
            => this.startRenta = startRenta;

        public override int GetRenta(IMonopolyGame game) => GetRenta();
        
        public virtual int GetRenta() 
            => GetRentaFor(startRenta, GetOwnersPropertiesInGroupCount().propertiesCollected);

        public static int GetRentaFor(int startRenta, int railroadsCount)
            => (int)(startRenta * Math.Pow(2, railroadsCount - 1));

        public override string GetInfo()
        {
            return GetUpInfo() +
                    "  Rentas:\n" +
                   $"    1 Railroad in group: {startRenta}\n" +
                   $"    2 Railroads in group: {GetRentaFor(startRenta, 2)}\n" +
                   $"    3 Railroads in group: {GetRentaFor(startRenta, 3)}\n" +
                   $"    4 Railroads in group: {GetRentaFor(startRenta, 4)}\n" +
                   $"    Current Renta: {GetRenta()}\n";
        }
    }
}
