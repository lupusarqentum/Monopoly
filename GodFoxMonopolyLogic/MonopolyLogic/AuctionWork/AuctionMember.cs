using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.AuctionWork
{
    public interface IAuctionMember
    {
        IPlayer Player { get; }
        bool IsActive { get; set; }
    }

    public sealed class AuctionMember : IAuctionMember
    {
        public IPlayer Player { get; }
        public bool IsActive { get; set; }

        public AuctionMember(IPlayer player)
        {
            Player = player;
            IsActive = true;
        }
    }
}
