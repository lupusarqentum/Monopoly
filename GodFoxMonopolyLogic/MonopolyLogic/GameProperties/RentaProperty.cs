using MonopolyLogic.AuctionWork;
using MonopolyLogic.PlayerWork;
using MonopolyLogic.Utils;

namespace MonopolyLogic.GameProperties
{
    public abstract class RentaProperty : Ownable, IAuctionItem
    {
        private RentaPropertyGroup _propertyGroup;
        public virtual RentaPropertyGroup PropertyGroup
        {
            get => _propertyGroup;
            private protected set
            {
                _propertyGroup = value;
                _propertyGroup.Add(this);
            }
        }

        public override IPlayer Owner
        {
            get => _owner;
            set
            {
                _owner?.RemovePropertyFromOwnList(this);
                _owner = value;
                _owner?.AddPropertyToOwnList(this);
            }
        }

        internal virtual bool IsFullComplectCollected
        {
            get
            {
                var (alpha, beta) = GetOwnersPropertiesInGroupCount();
                return alpha == beta;
            }
        }

        public virtual bool IsBankOwner => Owner == null;

        public virtual int AuctionCost => Constants.AuctionStartPrice;

        public RentaProperty(string name, int cost, RentaPropertyGroup propertyGroup) : base(name, cost) 
            => PropertyGroup = propertyGroup;

        internal virtual bool ShouldPlayerPayRenta(IPlayer player) => Owner != null && Owner.Index != player.Index;
        public virtual void SetAuctionWinner(IPlayer value) => Owner = value;
        public abstract int GetRenta(IMonopolyGame game);
        internal virtual(int propertiesCollected, int propertiesCount) GetOwnersPropertiesInGroupCount()
        {
            var propertiesCollected = 0;

            int i;
            for (i = 0; i < PropertyGroup.CountOfPropertiesInGroup; i++)
                if (PropertyGroup[i].Owner?.Index == Owner?.Index)
                    propertiesCollected++;
            
            return (propertiesCollected, i);
        }

        protected virtual string GetUpInfo()
        {
            return $"{Name}:\n" +
                   $"  Owner: {(Owner?.Nickname) ?? "none"}\n" +
                   $"  Cost: {Cost}\n";
        }

        public abstract string GetInfo();
    }
}
