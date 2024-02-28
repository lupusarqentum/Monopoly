using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.JailWork
{
    public interface IJailer
    {
        IPlayer Player { get; }
        int DoublesLeft { get; set; }
    }

    public sealed class Jailer : IJailer
    {
        public IPlayer Player { get; }
        public int DoublesLeft { get; set; }

        public Jailer(IPlayer player, int doublesLeft = 3)
        {
            Player = player;
            DoublesLeft = doublesLeft;
        }
    }
}
