using System.Text;
using System.Collections.Generic;
using MonopolyLogic.GameProperties;

namespace MonopolyLogic.PlayerWork
{
    public delegate int PlayersChoice(IPlayer player, string tip, string description, string[] options);
    public delegate int PlayerBidding(IPlayer player, string tip, string description);

    public interface IPlayer
    {
        PlayersChoice AskPlayer { get; set; }
        PlayerBidding AskPlayerAboutBid { get; set; }
        IPlayerStatistics Statistics { get; }
        int Index { get; }
        string Nickname { get; }
        List<Ownable> OwnedProperties { get; }
        IWallet Wallet { get; }
        int Coord { get; set; }
        bool IsBankrupt { get; set; }
        bool IsArrested { get; set; }

        void Awake();

        void AddPropertyToOwnList(Ownable gameProperty);
        void RemovePropertyFromOwnList(Ownable gameProperty);
    }

    public sealed class Player : IPlayer
    {
        public PlayersChoice AskPlayer { get; set; }
        public PlayerBidding AskPlayerAboutBid { get; set; }

        public IPlayerStatistics Statistics { get; }

        public int Index { get; }
        public string Nickname { get; }

        public List<Ownable> OwnedProperties { get; }
        public IWallet Wallet { get; private set; }
        public int Coord { get; set; } = 0;

        public bool IsBankrupt { get; set; } = false;
        public bool IsArrested { get; set; } = false;

        public Player(int index, string nickname)
        {
            Index = index; Nickname = nickname;

            OwnedProperties = new List<Ownable>();
            
            Statistics = new PlayerStatistics(this);
        }

        public void Awake()
        {
            Wallet = new Wallet();
        }

        public void AddPropertyToOwnList(Ownable gameProperty)
            => OwnedProperties.Add(gameProperty);
        public void RemovePropertyFromOwnList(Ownable gameProperty)
            => OwnedProperties.Remove(gameProperty);

        public override string ToString()
        {
            var builder = new StringBuilder($"Player {Nickname}:\n" +
                                            $"  Id: {Index}\n" +
                                            $"  Coord: {Coord}\n" +
                                            $"  Money: {Wallet.Money}\n" +
                                            $"{(IsArrested ? "  Arrested\n" : "")}" +
                                            $"{(IsBankrupt ? "  Bankrupt\n" : "")}" +
                                            $"  Stats:\n{Statistics.GetInfo("  ")}" + 
                                            $"  Owned Properties:\n");

            foreach (var item in OwnedProperties)
                builder.Append($"    {item.Name}\n");

            return builder.ToString();
        }
    }
}
