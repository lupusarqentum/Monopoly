using System;
using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.GameProperties
{
    public abstract class MortgagableRentaProperty : RentaProperty
    {
        public virtual bool IsMortgaged { get; private protected set; }

        public virtual int MortgageValue => Cost / 2;
        public virtual int MortgageLifting => MortgageValue + MortgageSaving;
        public virtual int MortgageSaving => (int)Math.Round(Cost / 20.0, MidpointRounding.AwayFromZero);

        public override int Assets => IsMortgaged ? 0 : MortgageValue;

        public MortgagableRentaProperty(string name, int cost, RentaPropertyGroup propertyGroup) : base(name, cost, propertyGroup) 
            => IsMortgaged = false;

        internal override bool ShouldPlayerPayRenta(IPlayer player) => base.ShouldPlayerPayRenta(player) && !IsMortgaged;

        public virtual bool TryLiftMortgage(IMonopolyGame game)
        {
            if (!IsMortgaged) return false;
            if (!Owner.Wallet.CanTakeMoney(MortgageLifting)) return false;

            Owner.Wallet.TryTakeMoney(MortgageLifting);
            IsMortgaged = false;
            game.Report.Invoke(LanguagePack.GetTranslation("propmortislift", Name, Owner?.Nickname));
            return true;
        }

        internal virtual void CancelMortgage() => IsMortgaged = false;

        public virtual bool MortgageIt(IMonopolyGame game)
        {
            if (CanMortgageIt())
            {
                Owner.Wallet.PutMoney(MortgageValue);
                IsMortgaged = true;
                game.Report.Invoke(LanguagePack.GetTranslation("propismort", Name, Owner?.Nickname));
                return true;
            }
            return false;
        }

        internal virtual bool CanMortgageIt() => Owner != null && (!IsMortgaged);

        public sealed override bool CheckContractValidity(IPlayer proposer, IPlayer opponent) =>
            (Owner?.Index == proposer?.Index || Owner?.Index == opponent?.Index) && IsMortgaged == false;

        protected sealed override string GetUpInfo()
        {
            return $"{Name}:\n" +
                   $"  Owner: {(Owner?.Nickname) ?? "none"}\n" +
                   $"  Cost: {Cost}\n" +
                   $"  IsMortgaged: {IsMortgaged}\n" +
                   $"  Mortgage Value: {MortgageValue}\n" +
                   $"  Mortgage Saving: {MortgageSaving}\n" +
                   $"  Mortgage Lifting: {MortgageLifting}\n";
        }
    }
}
