using System.Collections.Generic;
using MonopolyLogic.GameProperties;
using MonopolyLogic.AuctionWork;
using MonopolyLogic.PlayerWork;

namespace MonopolyLogic
{
    internal static class BankruptingMethods
    {
        internal static void BankruptPlayer(IMonopolyGame self, IPlayer bankrupted, IPlayer bankrupter)
        {
            self.Report.Invoke(LanguagePack.GetTranslation("playerisbankrupt", bankrupted.Nickname));
            bankrupted.IsBankrupt = true;
            bankrupted.Coord = -1;

            foreach (var bankruptedsProperty in bankrupted.OwnedProperties)
                if (bankruptedsProperty is Street)
                    (bankruptedsProperty as Street).buildings.ResetBuildingsCount();

            if (bankrupter != null) self.BankruptPlayerByOther(bankrupted, bankrupter);
            else self.BankruptPlayerByBank(bankrupted);
        }

        private static void BankruptPlayerByOther(this IMonopolyGame self, IPlayer bankrupted, IPlayer bankrupter)
        {
            self.Report.Invoke(LanguagePack.GetTranslation("kruptedtranskrupter", bankrupted.Nickname, bankrupter.Nickname));
            bankrupted.Wallet.TryTransferMoney(bankrupted.Statistics.Money, bankrupter.Wallet);
            bankrupted.BankruptedTransfersMortgagedPropertiesTo(self, bankrupter);
            bankrupted.TransferAllPropertiesToBank();
            bankrupted.TransferAllJailfreeTicketsTo(bankrupter);
        }

        private static void BankruptPlayerByBank(this IMonopolyGame self, IPlayer bankrupted)
        {
            bankrupted.Wallet.ResetWallet();
            LoseAllJailfreeTickets(bankrupted);
            self.Report.Invoke(LanguagePack.GetTranslation("wasbankruptedbybank", bankrupted.Nickname));

            var auctionItem = new AuctionItemList(LanguagePack.GetTranslation("propofplayer", bankrupted.Nickname));
            foreach (var item in bankrupted.OwnedProperties)
            {
                if (!(item is RentaProperty)) continue;

                if (item is MortgagableRentaProperty)
                    (item as MortgagableRentaProperty).CancelMortgage();

                auctionItem.AddAuctionItemToList(item as RentaProperty);
            }

            // don't start auction if not-bankrupted players count is 1
            if (self.BankruptedPlayersCount + 1 < self.PlayersCount)
                self.Auction.HoldAuction(self, auctionItem);
        }

        private static void BankruptedTransfersMortgagedPropertiesTo(this IPlayer self, IMonopolyGame game, IPlayer other)
        {
            var transferredProperties = new List<MortgagableRentaProperty>();
            var (mortgagesLiftingCost, mortgagesSavingCost) = self.TransferMortgagedProperties(other, transferredProperties);

            game.Report.Invoke(LanguagePack.GetTranslation("plthinkmortliftorsaverep", other.Nickname));
            var answer = other.AskPlayer(other, LanguagePack.GetTranslation("plthinkmortliftorsavetitle"), 
                LanguagePack.GetTranslation("plthinkmortliftorsavedesc", self.Nickname, mortgagesSavingCost,
                mortgagesLiftingCost), new string[] { LanguagePack.GetTranslation("plthinkmortliftorsaveact0"), 
                    LanguagePack.GetTranslation("plthinkmortliftorsaveact1"), });

            if (answer == 0)
            {
                if (other.Wallet.CanTakeMoney(mortgagesSavingCost))
                {
                    other.Wallet.TryTakeMoney(mortgagesSavingCost);
                    game.Report.Invoke(LanguagePack.GetTranslation("plsolvessavemort", other.Nickname));
                    return;
                }
            }
            else
            {
                if (other.Wallet.CanTakeMoney(mortgagesLiftingCost))
                {
                    other.Wallet.TryTakeMoney(mortgagesLiftingCost);
                    game.Report.Invoke(LanguagePack.GetTranslation("plsolvesliftmort", other.Nickname));
                    foreach (var item in transferredProperties)
                        item.TryLiftMortgage(game);
                    return;
                }
            }

            game.Report.Invoke(LanguagePack.GetTranslation("plnotenoughfunds", other.Nickname));
            game.BankruptPlayer(other, null);
        }

        private static (int mortgagesLiftingCost, int mortgagesSavingCost) TransferMortgagedProperties(
            this IPlayer self, IPlayer to, List<MortgagableRentaProperty> transferredProperties)
        {
            int mortgagesLiftingCost; var mortgagesSavingCost = mortgagesLiftingCost = 0;

            for (int i = self.OwnedProperties.Count - 1; i >= 0; i--)
            {
                if (!(self.OwnedProperties[i] is MortgagableRentaProperty)) continue;
                var property = self.OwnedProperties[i] as MortgagableRentaProperty;

                if (property.IsMortgaged)
                {
                    mortgagesLiftingCost += property.MortgageLifting;
                    mortgagesSavingCost += property.MortgageSaving;

                    transferredProperties.Add(property);

                    self.OwnedProperties[i].Owner = to;
                }
            }

            return (mortgagesLiftingCost, mortgagesSavingCost);
        }

        private static void TransferAllPropertiesToBank(this IPlayer self)
        {
            for (int i = self.OwnedProperties.Count - 1; i >= 0; i--)
                if (self.OwnedProperties[i] is RentaProperty)
                    self.OwnedProperties[i].Owner = null;
        }

        private static void TransferAllJailfreeTicketsTo(this IPlayer self, IPlayer to)
        {
            var i = self.OwnedProperties.Count - 1;
            while (i >= 0)
            {
                if (self.OwnedProperties[i] is JailFreeTicket)
                {
                    (self.OwnedProperties[i] as JailFreeTicket).Owner = to;
                    continue;
                }
                i--;
            }
        }

        private static void LoseAllJailfreeTickets(this IPlayer self)
        {
            var i = self.OwnedProperties.Count - 1;
            while (i >= 0)
            {
                if (self.OwnedProperties[i] is JailFreeTicket)
                {
                    (self.OwnedProperties[i] as JailFreeTicket).LoseIt();
                    continue;
                }
                i--;
            }
        }

    }
}
