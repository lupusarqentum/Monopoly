using System.Collections.Generic;
using MonopolyLogic.GameProperties;
using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.JailWork
{
    public interface IJailFreeTicketsRegistry
    {
        Dictionary<string, JailFreeTicket> JailFreeTickets { get; }

        void DecreaseFreejailTickets(IPlayer player);
        int HowManyJailFreeTicketsHavePlayer(IPlayer player);
        void Add(string name, JailFreeTicket jailfreeTicket);
        bool HasJailfreeTicketOwner(string name);
        void GivePlayerTicket(IPlayer player, string name);
    }

    public sealed class JailFreeTicketsRegistry : IJailFreeTicketsRegistry
    {
        public Dictionary<string, JailFreeTicket> JailFreeTickets { get; }

        public JailFreeTicketsRegistry()
        {
            JailFreeTickets = new Dictionary<string, JailFreeTicket>();
        }

        public void DecreaseFreejailTickets(IPlayer player)
        {
            foreach (var item in JailFreeTickets)
            {
                if (item.Value.Owner?.Index == player.Index)
                {
                    item.Value.LoseIt();
                    return;
                }
            }
        }

        public int HowManyJailFreeTicketsHavePlayer(IPlayer player)
        {
            var result = 0;
            foreach (var item in JailFreeTickets)
                if (item.Value.Owner?.Index == player.Index)
                    result++;
            return result;
        }

        public void Add(string name, JailFreeTicket jailfreeTicket) 
            => JailFreeTickets[name] = jailfreeTicket;

        public bool HasJailfreeTicketOwner(string name) 
            => JailFreeTickets[name].Owner != null;

        public void GivePlayerTicket(IPlayer player, string name)
            => JailFreeTickets[name].Owner = player;
    }
}
