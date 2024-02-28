using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.AuctionWork
{
    public interface IAuctionItem
    {
        string Name { get; }
        int AuctionCost { get; }

        void SetAuctionWinner(IPlayer value);
    }
}
