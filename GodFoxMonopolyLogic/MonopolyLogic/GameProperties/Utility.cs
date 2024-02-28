namespace MonopolyLogic.GameProperties
{
    public class Utility : MortgagableRentaProperty
    {
        public readonly int[] factors;

        public Utility(string name, int cost, RentaPropertyGroup group, int[] factors) : base(name, cost, group) 
            => this.factors = factors;

        public override int GetRenta(IMonopolyGame game) 
            => (game.Dice.SpacesNumberToMove) * GetFactor();

        public virtual int GetFactor()
        {
            var oneOwnerProperties = 0;

            for (int i = 0; i < PropertyGroup.CountOfPropertiesInGroup; i++)
                if (Owner?.Index == PropertyGroup[i].Owner?.Index) oneOwnerProperties++;
            
            return factors[oneOwnerProperties - 1];
        }

        public override string GetInfo()
        {
            return GetUpInfo() +
                    "  Factors:\n" +
                   $"    1 Utility in group: {factors[0]}\n" +
                   $"    2 Utilities in group: {factors[1]}\n" +
                   $"    Current Factor: {GetFactor()}\n";
        }
    }
}
