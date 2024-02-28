using System.Collections.Generic;
using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.AuctionWork
{
    public interface IAuction
    {
        void HoldAuction(IMonopolyGame game, IAuctionItem auctionItem);
    }

    public sealed class Auction : IAuction
    {
        private List<IAuctionMember> auctionMembers;
        private IAuctionItem auctionItem;

        private int currentPrice;

        public Auction() => ClearData();

        public void HoldAuction(IMonopolyGame game, IAuctionItem auctionItem)
        {
            SetData(game.Players, auctionItem);
            game.Report(LanguagePack.GetTranslation("auctionstarts", auctionItem.Name, auctionItem.AuctionCost));

            int playerNumber = -1;
            IAuctionMember winner;

            while (true)
            {
                winner = GetSingleActivePlayer();
                if (winner != null) break;

                playerNumber = GetNextPlayerNumber(playerNumber);
                var bid = Ask(playerNumber);

                if (bid > 0)
                {
                    MemberIncreasePrice(game, playerNumber, bid);
                }
                else
                {
                    auctionMembers[playerNumber].IsActive = false;
                    game.Report.Invoke(LanguagePack.GetTranslation("playerrejectauction",
                        auctionMembers[playerNumber].Player.Nickname));
                }
            }

            CheckWinner(game, winner.Player);

            ClearData();
        }

        private void CheckWinner(IMonopolyGame game, IPlayer player)
        {
            var answer = player.AskPlayer.Invoke(player, LanguagePack.GetTranslation("auctionwinnerasktip"),
                LanguagePack.GetTranslation("auctionwinneraskdesc", auctionItem.Name, auctionItem.AuctionCost), new string[] { LanguagePack.GetTranslation("auctionwinneraskact0"),
                    LanguagePack.GetTranslation("auctionwinneraskact1"), });

            if (answer == 1)
            {
                game.Report(LanguagePack.GetTranslation("playerrejectauction", player.Nickname));
                game.Report(LanguagePack.GetTranslation("everyonerejectauction"));
                return;
            }
            
            currentPrice++;
            
            if (player.Wallet.CanTakeMoney(currentPrice))
            {
                player.Wallet.TryTakeMoney(currentPrice);
                auctionItem.SetAuctionWinner(player);
                game.Report.Invoke(LanguagePack.GetTranslation("aucmemberhaswin", player.Nickname, auctionItem.Name, currentPrice));
                return;
            }

            game.BankruptPlayer(player, null);
        }

        private IAuctionMember GetSingleActivePlayer()
        {
            IAuctionMember result = null;

            foreach (var item in auctionMembers)
            {
                if (CanPlayerAuct(item))
                {
                    if (result == null) result = item;
                    else return null;
                }
            }

            return result;
        }

        private void MemberIncreasePrice(IMonopolyGame game, int playerNumber, int amount)
        {
            currentPrice += amount;
            game.Report.Invoke(LanguagePack.GetTranslation("aucmemberbids", 
                auctionMembers[playerNumber].Player.Nickname, currentPrice));
        }

        private int Ask(int playerNumber)
        {
            var biddingPlayer = auctionMembers[playerNumber].Player;
            return biddingPlayer.AskPlayerAboutBid(biddingPlayer, $"НА АУКЦИОНЕ {auctionItem.Name.ToUpper()}",
                LanguagePack.GetTranslation("auctionbiddingdesc", auctionItem.Name, currentPrice));
        }

        private int GetNextPlayerNumber(int playerNumber)
        {
            do playerNumber = (playerNumber + 1) % auctionMembers.Count;
            while (!CanPlayerAuct(playerNumber));

            return playerNumber;
        }

        private bool CanPlayerAuct(int playerNumber) 
            => CanPlayerAuct(auctionMembers[playerNumber]);
        private bool CanPlayerAuct(IAuctionMember item) 
            => item.IsActive && (!item.Player.IsBankrupt);

        private void SetData(IPlayer[] players, IAuctionItem auctionItem)
        {
            this.auctionItem = auctionItem;
            currentPrice = auctionItem.AuctionCost;
            auctionMembers = new List<IAuctionMember>();

            for (int i = 0; i < players.Length; i++)
                auctionMembers.Add(new AuctionMember(players[i]));
        }

        private void ClearData()
        {
            auctionItem = null;
            auctionMembers = null;
            currentPrice = 0;
        }
    }
}
