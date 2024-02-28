using System.Collections.Generic;
using MonopolyLogic.PlayerWork;
using MonopolyLogic.Utils;

namespace MonopolyLogic.JailWork
{
    public interface IJailMoves
    {
        void JailMove(IMonopolyGame game, IPlayer player);
    }

    public sealed class JailMoves : IJailMoves
    {
        private enum JailerAction : byte
        {
            PayFine = 0,
            RollDice,
            UseFreejailTicket,
        }

        private readonly IJail jail;

        public JailMoves(IJail jail) => this.jail = jail;

        public void JailMove(IMonopolyGame game, IPlayer player)
        {
            game.Report.Invoke(LanguagePack.GetTranslation("jailwithplayerin", player.Nickname));

            var answer = AskJailer(player);

            switch (answer)
            {
                case JailerAction.PayFine:
                    PlayerPayFine(game, player);
                    break;

                case JailerAction.RollDice:
                    PlayerRollDice(game, player);
                    break;

                case JailerAction.UseFreejailTicket:
                    PlayerUseFreejailTicket(game, player);
                    break;
            }
        }

        private JailerAction AskJailer(IPlayer player)
        {
            string tip = LanguagePack.GetTranslation("jailerasktip");
            string description = LanguagePack.GetTranslation("jaileraskdesc", 
                jail.JailersRegistration.HowManyDoublesLeft(player), 
                jail.JailFreeTicketsRegistry.HowManyJailFreeTicketsHavePlayer(player));
            string fineOption = LanguagePack.GetTranslation("jaileraskfine");
            string doubleOption = LanguagePack.GetTranslation("jaileraskdoub");
            string freeJailUsingOption = LanguagePack.GetTranslation("jaileraskfree");

            var options = new List<string> { fineOption };
            if (jail.JailersRegistration.HowManyDoublesLeft(player) != 0) options.Add(doubleOption);
            if (jail.JailFreeTicketsRegistry.HowManyJailFreeTicketsHavePlayer(player) != 0) options.Add(freeJailUsingOption);
            var answer = player.AskPlayer(player, tip, description, options.ToArray());

            return (JailerAction)answer;
        }

        private void PlayerPayFine(IMonopolyGame game, IPlayer player)
        {
            game.Report.Invoke(LanguagePack.GetTranslation("plpaysfine0", player.Nickname));

            if (player.Wallet.CanTakeMoney(Constants.JailFine))
            {
                player.Wallet.TryTakeMoney(Constants.JailFine);
                game.Report.Invoke(LanguagePack.GetTranslation("plpaysfine1", player.Nickname));
                jail.JailersRegistration.FreePlayer(game, player);
                game.TurnPlayer(player.Index);
                return;
            }

            game.Report.Invoke(LanguagePack.GetTranslation("plnotenoughfunds", player.Nickname));
            game.BankruptPlayer(player, null);
        }

        private void PlayerRollDice(IMonopolyGame game, IPlayer player)
        {
            game.Report.Invoke(LanguagePack.GetTranslation("jailerrolls0", player.Nickname));
            jail.JailersRegistration.DecreaseDoublesLeft(player);
            
            if (IDice.IsDouble(game.Dice.RollDiceWithReport(game)))
            {
                game.Report.Invoke(LanguagePack.GetTranslation("jailerrolls1", player.Nickname));
                jail.JailersRegistration.FreePlayer(game, player);
                game.TurnPlayer(player.Index);
                return;
            }

            if (jail.JailersRegistration.HasPlayerDoublesLeft(player))
                game.Report.Invoke(LanguagePack.GetTranslation("jailerrolls2", player.Nickname));
            else
                game.Report.Invoke(LanguagePack.GetTranslation("jailerrolls3", player.Nickname));

            if (!jail.JailersRegistration.HasPlayerDoublesLeft(player))
                JailMove(game, player);
        }

        private void PlayerUseFreejailTicket(IMonopolyGame game, IPlayer player)
        {
            game.Report.Invoke(LanguagePack.GetTranslation("plusesfreejailftic", player.Nickname));

            jail.JailFreeTicketsRegistry.DecreaseFreejailTickets(player);
            jail.JailersRegistration.FreePlayer(game, player);
            game.TurnPlayer(player.Index);
        }
    }
}
