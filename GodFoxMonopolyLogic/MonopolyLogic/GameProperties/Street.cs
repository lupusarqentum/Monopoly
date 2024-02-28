namespace MonopolyLogic.GameProperties
{
    public class Street : MortgagableRentaProperty
    {
        internal readonly StreetBuildings buildings;

        public readonly StreetInfo streetInfo;

        public override int Assets => base.Assets + buildings.Assets;

        public Street(string name, StreetInfo streetInfo, RentaPropertyGroup group) : base(name, streetInfo.Cost, group)
        {
            this.streetInfo = streetInfo;

            buildings = new StreetBuildings(streetInfo, this);
        }

        public virtual bool TryBuildHouse(IMonopolyGame game) => buildings.TryBuildHouse(game);
        public virtual bool TrySellHouse(IMonopolyGame game) => buildings.TrySellHouse(game);

        public override int GetRenta(IMonopolyGame game) => GetRenta();

        public virtual int GetRenta() => buildings.GetRenta(streetInfo.Rentas);

        public virtual StreetBuildingsStatus GetHousesAndHotelsCount() => buildings.GetHousesAndHotelsCount();

        internal override bool CanMortgageIt()
        {
            for (int i = 0; i < PropertyGroup.CountOfPropertiesInGroup; i++)
                if (!(PropertyGroup[i] as Street).buildings.IsBuildingsCountMinimum)
                    return false;

            return base.CanMortgageIt();
        }

        public override string GetInfo()
        {
            return GetUpInfo() +
                    "  Rentas:\n" +
                   $"    Renta: {streetInfo.Rentas.StartRenta}\n" +
                   $"    FullComplect Renta: {streetInfo.Rentas.FullComplectRenta}\n" +
                   $"    1 House Renta: {streetInfo.Rentas.HousesRentas[0]}\n" +
                   $"    2 Houses Renta: {streetInfo.Rentas.HousesRentas[1]}\n" +
                   $"    3 Houses Renta: {streetInfo.Rentas.HousesRentas[2]}\n" +
                   $"    4 Houses Renta: {streetInfo.Rentas.HousesRentas[3]}\n" +
                   $"    1 Hotel Renta: {streetInfo.Rentas.HousesRentas[4]}\n" +
                   $"  Building Cost: {streetInfo.BuildingCost}\n" + 
                   $"  Buildings On Street: {buildings.BuildingsOnStreet}\n" + 
                   $"  Current Renta: {GetRenta()}\n";
        }
    }
}
