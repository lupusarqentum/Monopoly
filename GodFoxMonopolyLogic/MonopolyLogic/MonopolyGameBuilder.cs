using MonopolyLogic.AuctionWork;
using MonopolyLogic.JailWork;
using MonopolyLogic.Parsers;
using MonopolyLogic.PlayerWork;
using MonopolyLogic.TaskWork;
using MonopolyLogic.Utils;
using System.IO;

namespace MonopolyLogic
{
    

    public sealed class MonopolyGameBuilder
    {
        private readonly IPlayer[] players;
        private readonly GetSpaceRepresenter spaceRepresenterGetter;

        public IBoard Board { get; set; }
        public IJail Jail { get; set; }
        public IDice Dice { get; set; }
        public IAuction Auction { get; set; }
        public ITaskSystem TaskSystem { get; set; }

        public MonopolyGameBuilder(GameCreatingData data)
        {
            players = new Player[data.playersNicknames.Length];

            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new Player(i, data.playersNicknames[i]);
                players[i].AskPlayer += data.playersChoiceHandler;
                players[i].AskPlayerAboutBid += data.playerBidding;
            }

            spaceRepresenterGetter = data.spaceRepresenterGetter;
        }

        public IMonopolyGame Build()
        {
            Board ??= new Board();
            Dice ??= new Dice();
            Jail ??= new Jail(Constants.JailCoord);
            Auction ??= new Auction();
            TaskSystem ??= new TaskSystem(new TaskCollector());

            var game = new MonopolyGame(players, this);
            
            var init = File.ReadAllText("monopolyboard1.txt");
            var vm = new VirtualMachine(game, spaceRepresenterGetter);

            var tokens = new Lexer(init).GetTokens();
            var states = new Parser(tokens).Parse();

            foreach (var state in states)
                state.Execute(vm);

            return game;
        }
    }
}
