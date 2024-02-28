using System.Collections.Generic;
using MonopolyLogic.PlayerWork;
using MonopolyLogic.Utils;

namespace MonopolyLogic.GameProperties
{
    public class JailFreeTicket : Ownable
    {
        public override IPlayer Owner 
        {
            get => _owner;
            set
            {
                _owner?.RemovePropertyFromOwnList(this);
                _owner = value;
                _owner?.AddPropertyToOwnList(this);
            }
        }

        public JailFreeTicket(string name) : base(LanguagePack.GetTranslation("exitfromjailfree", name), Constants.JailFine) {}

        internal void LoseIt() => Owner = null;

        public static List<JailFreeTicket> GetPlayerJailFreeTickets(IPlayer player, int jailfreeTicketsCount)
        {
            var result = new List<JailFreeTicket>();

            for (int i = 0; jailfreeTicketsCount > 0; i++)
            {
                if (player.OwnedProperties[i] is JailFreeTicket)
                {
                    result.Add(player.OwnedProperties[i] as JailFreeTicket);
                    jailfreeTicketsCount--;
                }

                if (i == player.OwnedProperties.Count - 1 && jailfreeTicketsCount > 0)
                    throw new JailfreeTicketsGettingError($"Not enough jailfree tickets that player {player.Nickname} have");
            }

            return result;
        }
    }
}
