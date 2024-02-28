using System.Collections.Generic;
using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.AuctionWork
{
    public sealed class AuctionItemList : IAuctionItem
    {
        public string Name { get; }

        public int AuctionCost
        {
            get
            {
                var _cost = 0;
                foreach (var item in auctionItems)
                    _cost += item.AuctionCost;

                return _cost;
            }
        }

        public void SetAuctionWinner(IPlayer value)
        {
            for (int i = auctionItems.Count - 1; i >= 0; i--)
                auctionItems[i].SetAuctionWinner(value);
        }

        private readonly List<IAuctionItem> auctionItems;

        public AuctionItemList(string name)
        {
            Name = name;
            auctionItems = new List<IAuctionItem>();
        }

        public void AddAuctionItemToList(IAuctionItem auctionItem) => auctionItems.Add(auctionItem);

        public override string ToString() => Name;
    }
}
