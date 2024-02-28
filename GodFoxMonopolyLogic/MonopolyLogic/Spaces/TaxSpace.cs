using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.Spaces
{
    public class TaxSpace : Space
    {
        public override string Name { get; }

        public readonly int amount;

        public TaxSpace(int coord, string name, int taxAmount) : base(coord)
        {
            Name = name;
            amount = taxAmount;
        }

        internal override void Action(IMonopolyGame game, IPlayer player)
        {
            game.Report.Invoke(LanguagePack.GetTranslation("plstoppedonstr", player.Nickname, Name));
            player.AskPlayer(player, LanguagePack.GetTranslation("taxspace0"), 
                LanguagePack.GetTranslation("taxspace1", Name, amount), 
                new string[] { LanguagePack.GetTranslation("taxspace2"), });

            if (player.Wallet.CanTakeMoney(amount))
            {
                player.Wallet.TryTakeMoney(amount);
                game.Report.Invoke(LanguagePack.GetTranslation("taxspace3", amount));
                return;
            }

            game.BankruptPlayer(player, null);
        }
    }
}
