namespace MonopolyLogic.GameProperties
{
    public class StreetBuildings
    {
        protected readonly Street street;

        internal virtual int BuildingsOnStreet { get; set; } = 0;

        protected readonly int BuildingBuildCost;
        protected readonly int BuildingSellingCost;
        
        protected readonly int MinBuildingsCount = 0;
        protected readonly int MaxBuildingsCount = 5;

        internal virtual int Assets => BuildingBuildCost / 2 * BuildingsOnStreet;

        internal virtual bool IsBuildingsCountMinimum => BuildingsOnStreet <= MinBuildingsCount;

        public virtual string BuildingsName
        {
            get
            {
                return BuildingsOnStreet switch
                {
                    0 => LanguagePack.GetTranslation("buildingsstatus0houses"),
                    1 => LanguagePack.GetTranslation("buildingsstatussinglehouse"),
                    5 => LanguagePack.GetTranslation("buildingsstatushotel"),
                    _ => LanguagePack.GetTranslation("buildingsstatusmultiplehouses", BuildingsOnStreet),
                };
            }
        }

        internal StreetBuildings(StreetInfo streetInfo, Street street)
        {
            BuildingBuildCost = streetInfo.BuildingCost;
            BuildingSellingCost = BuildingBuildCost / 2;

            MinBuildingsCount = 0;
            MaxBuildingsCount = streetInfo.MaxBuildingsCount;

            this.street = street;
        }

        internal virtual void ResetBuildingsCount() => BuildingsOnStreet = 0;

        internal virtual StreetBuildingsStatus GetHousesAndHotelsCount()
        {
            if (BuildingsOnStreet == MaxBuildingsCount) return new StreetBuildingsStatus() { Houses = 0, Hotels = 1 };
            return new StreetBuildingsStatus() { Houses = BuildingsOnStreet, Hotels = 0 };
        }

        internal virtual int GetRenta(StreetRentas rentas)
        {
            if (BuildingsOnStreet != 0)
                return rentas.GetRentaForXBuildings(BuildingsOnStreet);
            if (street.IsFullComplectCollected)
                return rentas.FullComplectRenta;
            return rentas.StartRenta;
        }

        internal virtual bool CanBuildHouse()
        {
            if (BuildingsOnStreet >= MaxBuildingsCount) return false;
            if (!street.Owner.Wallet.CanTakeMoney(BuildingBuildCost)) return false;
            if (!street.IsFullComplectCollected) return false;

            for (int i = 0; i < street.PropertyGroup.CountOfPropertiesInGroup; i++)
            {
                var item = street.PropertyGroup[i] as Street;
                if (item.buildings.BuildingsOnStreet < BuildingsOnStreet || item.IsMortgaged) 
                    return false;
            }

            return true;
        }

        internal virtual bool CanSellHouse()
        {
            for (int i = 0; i < street.PropertyGroup.CountOfPropertiesInGroup; i++)
                if ((street.PropertyGroup[i] as Street).buildings.BuildingsOnStreet > BuildingsOnStreet)
                    return false;

            return BuildingsOnStreet > MinBuildingsCount;
        }

        internal virtual bool TryBuildHouse(IMonopolyGame game)
        {
            if (!CanBuildHouse()) 
                return false;

            street.Owner.Wallet.TryTakeMoney(BuildingBuildCost);
            BuildingsOnStreet++;

            game.Report.Invoke(LanguagePack.GetTranslation("plbuildhouse", 
                street.Owner.Nickname, street.Name, BuildingsName, street.GetRenta()));

            return true;
        }

        internal virtual bool TrySellHouse(IMonopolyGame game)
        {
            if (!CanSellHouse()) 
                return false;

            street.Owner.Wallet.PutMoney(BuildingSellingCost);
            BuildingsOnStreet--;

            game.Report.Invoke(LanguagePack.GetTranslation("plsellhouse", 
                street.Owner.Nickname, street.Name, BuildingsName, street.GetRenta()));

            return true;
        }

        public override string ToString() => BuildingsName;
    }
}
