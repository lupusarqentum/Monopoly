namespace MonopolyLogic.GameProperties
{
    public struct StreetInfo
    {
        public readonly int Cost;
        public readonly int BuildingCost;
        public readonly StreetRentas Rentas;

        public readonly int MaxBuildingsCount;

        public StreetInfo(int cost, int buildingCost, StreetRentas rentas)
        {
            Cost = cost;
            BuildingCost = buildingCost;
            Rentas = rentas;

            MaxBuildingsCount = 5;
        }
    }

    public struct StreetRentas
    {
        public int StartRenta;
        public int FullComplectRenta;
        public int[] HousesRentas;

        public StreetRentas(int StartRenta, int FullComplectRenta, params int[] HousesRentas)
        {
            this.StartRenta = StartRenta;
            this.FullComplectRenta = FullComplectRenta;
            this.HousesRentas = HousesRentas;
        }

        public int GetRentaForXBuildings(int x) => HousesRentas[x - 1];
    }

    public struct StreetBuildingsStatus
    {
        public int Houses;
        public int Hotels;

        internal static StreetBuildingsStatus Create()
            => new StreetBuildingsStatus() { Houses = 0, Hotels = 0 };

        public static StreetBuildingsStatus operator +(StreetBuildingsStatus alpha, StreetBuildingsStatus beta)
            => new StreetBuildingsStatus() { Houses = alpha.Houses + beta.Houses, Hotels = alpha.Hotels + beta.Hotels };
    }
}
