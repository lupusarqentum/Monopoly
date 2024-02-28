using MonopolyLogic.GameProperties;

namespace MonopolyLogic.PlayerWork
{
    public interface IPlayerStatistics
    {
        int Money { get; }

        int Score { get; }
        int PassedSpaces { get; set; }

        int Assets { get; }
        StreetBuildingsStatus HousesAndHotelsCount { get; }

        string GetInfo(string alpha);
    }

    public sealed class PlayerStatistics : IPlayerStatistics
    {
        private readonly IPlayer player;

        public int Money => player.Wallet.Money;

        public int Score => player.Wallet.Score;
        public int PassedSpaces { get; set; }

        private int _assets;
        public int Assets
        {
            get
            {
                if (player.IsBankrupt) return _assets;

                _assets = Money;
                foreach (var item in player.OwnedProperties)
                    _assets += item.Assets;
                return _assets;
            }
        }

        public StreetBuildingsStatus HousesAndHotelsCount
        {
            get
            {
                var result = StreetBuildingsStatus.Create();

                foreach (var property in player.OwnedProperties)
                    if (property is Street)
                        result += (property as Street).GetHousesAndHotelsCount();
                
                return result;
            }
        }

        public PlayerStatistics(IPlayer player)
        {
            this.player = player;

            PassedSpaces = 0;
        }

        public string GetInfo(string alpha)
        {
            return alpha + $"Money: {Money}\n" +
                   alpha + $"Assets: {Assets}\n" +
                   alpha + $"Score: {Score}\n" +
                   alpha + $"Passed Spaces: {PassedSpaces}\n";
        }

        public override string ToString() => GetInfo("");
    }
}
