using MonopolyLogic.ContractWork;
using MonopolyLogic.AuctionWork;
using MonopolyLogic.PlayerWork;
using MonopolyLogic.TaskWork;
using MonopolyLogic.JailWork;

namespace MonopolyLogic
{
    public delegate void Report(string text);

    public interface IMonopolyGame
    {
        Report Report { get; set; }
        IPlayer[] Players { get; set; }

        IBoard Board { get; set; }
        IAuction Auction { get; set; }
        IJail Jail { get; set; }
        ITaskSystem TaskSystem { get; set; }
        IDice Dice { get; set; }

        int PlayersCount { get; }
        int BankruptedPlayersCount { get; }

        void ProposeContract(Contract contract);
        void BankruptPlayer(IPlayer bankrupted, IPlayer bankrupter);
        IPlayer GetPlayerByNumber(int playerNumber);
        void TurnPlayer(int playerNumber);
    }

    public sealed class MonopolyGame : IMonopolyGame
    {
        public Report Report { get; set; } = (text) => { };

        public IPlayer[] Players { get; set; }

        public IBoard Board { get; set; }
        public IAuction Auction { get; set; }
        public IJail Jail { get; set; }
        public ITaskSystem TaskSystem { get; set; }
        public IDice Dice { get; set; }
        
        public int PlayersCount => Players.Length;
        public int BankruptedPlayersCount
        {
            get
            {
                var result = 0;
                for (int i = 0; i < Players.Length; i++)
                    if (Players[i].IsBankrupt)
                        result++;
                return result;
            }
        }

        public MonopolyGame(IPlayer[] players, MonopolyGameBuilder builder)
        {
            Players = players;

            Board = builder.Board;
            Auction = builder.Auction;
            Jail = builder.Jail;
            TaskSystem = builder.TaskSystem;
            Dice = builder.Dice;
        }

        public void ProposeContract(Contract contract)
        {
            if (contract.ProposeContract(this)) 
                contract.Execute(this);
        }

        public void BankruptPlayer(IPlayer bankrupted, IPlayer bankrupter) 
            => BankruptingMethods.BankruptPlayer(this, bankrupted, bankrupter);

        public IPlayer GetPlayerByNumber(int playerNumber) => Players[playerNumber];

        public void TurnPlayer(int playerNumber)
        {
            if (!Players[playerNumber].IsArrested) Board.LandBoard(this, Players[playerNumber]);
            else Jail.JailMove(this, Players[playerNumber]);
        }
    }
}
