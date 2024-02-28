using System.Collections.Generic;
using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.JailWork
{
    public interface IJailersRegistration
    {
        List<IJailer> Jailers { get; }

        void ArrestPlayer(IMonopolyGame game, IPlayer player);
        void FreePlayer(IMonopolyGame game, IPlayer player);
        void DecreaseDoublesLeft(IPlayer player);
        int HowManyDoublesLeft(IPlayer player);
        bool HasPlayerDoublesLeft(IPlayer player);
    }

    public sealed class JailersRegistration : IJailersRegistration
    {
        private readonly int jailCoord;

        public List<IJailer> Jailers { get; }

        public JailersRegistration(int jailCoord)
        {
            Jailers = new List<IJailer>();
            this.jailCoord = jailCoord;
        }

        public void ArrestPlayer(IMonopolyGame game, IPlayer player)
        {
            game.Report.Invoke(LanguagePack.GetTranslation("jailplayerarrested", player.Nickname));

            if (!player.IsArrested)
                Jailers.Add(new Jailer(player));

            game.Board.SetPlayerCoordTo(player, jailCoord);
            player.IsArrested = true;
        }

        public void FreePlayer(IMonopolyGame game, IPlayer player)
        {
            game.Report.Invoke(LanguagePack.GetTranslation("jailplayerfree", player.Nickname));

            for (int i = 0; i < Jailers.Count; i++)
            {
                if (Jailers[i].Player.Index == player.Index)
                {
                    Jailers.RemoveAt(i);
                    break;
                }
            }

            player.IsArrested = false;
        }

        public void DecreaseDoublesLeft(IPlayer player)
        {
            foreach (var jailer in Jailers)
            {
                if (jailer.Player.Index == player.Index)
                {
                    jailer.DoublesLeft--;
                    return;
                }
            }
        }

        public int HowManyDoublesLeft(IPlayer player)
        {
            foreach (var jailer in Jailers)
                if (jailer.Player.Index == player.Index)
                    return jailer.DoublesLeft;

            return 0;
        }

        public bool HasPlayerDoublesLeft(IPlayer player)
            => HowManyDoublesLeft(player) > 0;
    }
}
