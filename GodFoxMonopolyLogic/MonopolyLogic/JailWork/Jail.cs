using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.JailWork
{
    public interface IJail
    {
        IJailersRegistration JailersRegistration { get; }
        IJailFreeTicketsRegistry JailFreeTicketsRegistry { get; }

        public void JailMove(IMonopolyGame game, IPlayer player);
    }

    public sealed class Jail : IJail
    {
        private readonly IJailMoves jailMoves;

        public IJailersRegistration JailersRegistration { get; }
        public IJailFreeTicketsRegistry JailFreeTicketsRegistry { get; }

        public Jail(int jailCoord)
        {
            JailersRegistration = new JailersRegistration(jailCoord);
            JailFreeTicketsRegistry = new JailFreeTicketsRegistry();

            jailMoves = new JailMoves(this);
        }

        public void JailMove(IMonopolyGame game, IPlayer player) 
            => jailMoves.JailMove(game, player);
    }
}
