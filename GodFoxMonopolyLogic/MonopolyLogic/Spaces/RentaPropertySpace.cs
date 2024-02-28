using MonopolyLogic.GameProperties;
using MonopolyLogic.PlayerWork;
using MonopolyLogic.Utils;

namespace MonopolyLogic.Spaces
{
    public class RentaPropertySpace : Space
    {
        public override string Name => RentaProperty.Name;

        public RentaProperty RentaProperty { get; private set; }

        public RentaPropertySpace(int coord, RentaProperty gameProperty) : base(coord) => RentaProperty = gameProperty;

        internal override void Action(IMonopolyGame game, IPlayer player)
        {
            game.Report.Invoke(LanguagePack.GetTranslation("plstoppedonstr", player.Nickname, Name));

            if (RentaProperty.IsBankOwner)
            {
                ThinkAboutBuying(game, player);
                return;
            }

            if (RentaProperty.ShouldPlayerPayRenta(player))
            {
                MakePlayerPayRenta(game, player, RentaProperty.GetRenta(game));
                return;
            }

            game.Report.Invoke(LanguagePack.GetTranslation("rprosppayno"));
        }

        internal void MakePlayerPayRenta(IMonopolyGame game, IPlayer player, int renta)
        {
            game.Report.Invoke(LanguagePack.GetTranslation("plmuspayotherrentarep", RentaProperty.Owner.Nickname, renta));
            player.AskPlayer(player, LanguagePack.GetTranslation("plmuspayotherrentatitle"),
                LanguagePack.GetTranslation("plmuspayotherrentadesc", RentaProperty.Owner.Nickname, renta), 
                new string[] { LanguagePack.GetTranslation("plmuspayotherrentaact0", renta), });

            try
            {
                player.Wallet.TryTransferMoney(renta, RentaProperty.Owner.Wallet);
                game.Report.Invoke(LanguagePack.GetTranslation("plhaspaidrenother", player.Nickname, RentaProperty.Owner.Nickname));
            }
            catch (WalletOperationFailed)
            {
                game.BankruptPlayer(player, RentaProperty.Owner);
            }
        }

        private void ThinkAboutBuying(IMonopolyGame game, IPlayer player)
        {
            game.Report.Invoke(LanguagePack.GetTranslation("plthinabbuyingrep", player.Nickname, RentaProperty.Cost));

            var answer = player.AskPlayer(player, 
                LanguagePack.GetTranslation("plthinabbuyingtitle"), LanguagePack.GetTranslation("plthinabbuyingdesc"),
                new string[] { LanguagePack.GetTranslation("plthinabbuyingact0", RentaProperty.Cost), 
                    LanguagePack.GetTranslation("plthinabbuyingact1"), });

            if (answer == 0) BuyProperty(game, player);
            else if (answer == 1) game.Auction.HoldAuction(game, RentaProperty);
        }

        private void BuyProperty(IMonopolyGame game, IPlayer player)
        {
            if (player.Wallet.CanTakeMoney(RentaProperty.Cost))
            {
                player.Wallet.TryTakeMoney(RentaProperty.Cost);
                RentaProperty.Owner = player;
                game.Report.Invoke(LanguagePack.GetTranslation("plboughtprops", 
                    player.Nickname, Name, RentaProperty.Cost));
                return;
            }
            game.BankruptPlayer(player, null);
        }

        public override string ToString() => RentaProperty.GetInfo();
    }
}
