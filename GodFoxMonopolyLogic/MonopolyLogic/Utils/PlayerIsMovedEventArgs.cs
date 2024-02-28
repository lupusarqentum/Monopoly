using System;
using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.Utils
{
    public class PlayerIsMovedEventArgs : EventArgs
    {
        public IPlayer Player { get; private set; }
        public int WherePlayerWasStopped { get; private set; }

        public PlayerIsMovedEventArgs(IPlayer player, int wherePlayerWasStopped)
        {
            Player = player;
            WherePlayerWasStopped = wherePlayerWasStopped;
        }
    }
}
