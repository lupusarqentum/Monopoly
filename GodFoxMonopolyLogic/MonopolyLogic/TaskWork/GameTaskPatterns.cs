using System.Reflection;
using System.Linq;
using MonopolyLogic.GameProperties;
using MonopolyLogic.Spaces;

namespace MonopolyLogic.TaskWork
{
    public static class GameTaskPatterns
    {
        public static void ApplyTo(ITaskPatternsRegistry taskPatternsRegistry)
        {
            var patterns = typeof(GameTaskPatterns).GetMethods(BindingFlags.Static|BindingFlags.NonPublic);
            foreach (var pattern in patterns)
                if (pattern.Name != nameof(ApplyTo))
                    taskPatternsRegistry.AddTaskPattern(pattern);
        }

        private static GameTask PutMoney(string reportText, int amount)
        {
            return (game, player) => {
                game.Report.Invoke(LanguagePack.GetTranslation(reportText, player.Nickname, amount));
                player.Wallet.PutMoney(amount);
            };
        }

        private static GameTask TakeMoney(int amount, string rep, string tip, string desc, string act, string paid)
        {
            return (game, player) =>
            {
                game.Report.Invoke(LanguagePack.GetTranslation(rep,
                    player.Nickname, amount));
                player.AskPlayer(player, LanguagePack.GetTranslation(tip), LanguagePack.GetTranslation(desc, amount), 
                    new string[] { LanguagePack.GetTranslation(act), });

                if (player.Wallet.CanTakeMoney(amount))
                {
                    player.Wallet.TryTakeMoney(amount);
                    game.Report.Invoke(LanguagePack.GetTranslation(paid, player.Nickname));
                }
                else game.BankruptPlayer(player, null);
            };
        }

        private static GameTask GiveJailFreeTicketOrBeArrested(string ticketName)
        {
            return (game, player) =>
            {
                var isJailTicketNoOwner = !game.Jail.JailFreeTicketsRegistry.HasJailfreeTicketOwner(ticketName);
                if (isJailTicketNoOwner)
                {
                    game.Report.Invoke(LanguagePack.GetTranslation("getsjailfreeticket", player.Nickname));
                    game.Jail.JailFreeTicketsRegistry.GivePlayerTicket(player, ticketName);
                }
                else
                {
                    game.Report.Invoke(LanguagePack.GetTranslation("plisarrested", player.Nickname));
                    game.Jail.JailersRegistration.ArrestPlayer(game, player);
                }
            };
        }

        private static GameTask ArrestPlayer()
        {
            return (game, player) =>
            {
                game.Report.Invoke(LanguagePack.GetTranslation("plisarrested", player.Nickname));
                game.Jail.JailersRegistration.ArrestPlayer(game, player);
            };
        }

        private static GameTask TravelTo(int coordTo)
        {
            return (game, player) =>
            {
                game.Report.Invoke(LanguagePack.GetTranslation("tasktravel", player.Nickname, game.Board.Spaces[coordTo].Name));
                var distance = IBoard.GetDistanceFrom(player.Coord, coordTo, game.Board.Spaces.Length);
                game.Board.MoveForwardAndAct(game, player, distance);
            };
        }

        private static GameTask RepairBuildings(int houseCost, int hotelCost)
        {
            return (game, player) =>
            {
                var temp = player.Statistics.HousesAndHotelsCount;
                var amount = temp.Houses * houseCost + temp.Hotels * hotelCost;

                game.Report.Invoke(LanguagePack.GetTranslation("taskchestrepair", player.Nickname, temp.Houses,
                    temp.Hotels, houseCost, hotelCost, amount));
                player.AskPlayer(player, LanguagePack.GetTranslation("plmustpaybanktitle"),
                    LanguagePack.GetTranslation("plmustpaybankdesc", amount),
                    new string[] { LanguagePack.GetTranslation("plmustpaybankact0"), });
                if (player.Wallet.CanTakeMoney(amount))
                {
                    player.Wallet.TryTakeMoney(amount);
                    game.Report.Invoke(LanguagePack.GetTranslation("plpayedbankpay"));
                }
                else game.BankruptPlayer(player, null);
            };
        }

        private static GameTask BackMoving(int spacesToMove)
        {
            return (game, player) =>
            {
                game.Report.Invoke(LanguagePack.GetTranslation("taskbackmoving", player.Nickname, spacesToMove));
                game.Board.MoveForwardAndAct(game, player, spacesToMove);
            };
        }

        private static GameTask PlayerBirthday(int birthdayGift)
        {
            return (game, player) =>
            {
                game.Report.Invoke(LanguagePack.GetTranslation("taskbirthday0", player.Nickname, birthdayGift));

                foreach (var member in game.Players)
                {
                    if (member.Index == player.Index) continue;

                    if (!member.Wallet.CanTakeMoney(birthdayGift))
                        member.AskPlayer(member, LanguagePack.GetTranslation("birthdaymemberhaventgift0"), 
                            LanguagePack.GetTranslation("birthdaymemberhaventgift1", player.Nickname, birthdayGift), 
                            new string[] { LanguagePack.GetTranslation("birthdaymemberhaventgift2") });

                    if (member.Wallet.CanTakeMoney(birthdayGift))
                        member.Wallet.TryTransferMoney(birthdayGift, player.Wallet);
                    else
                        game.BankruptPlayer(member, player);
                }
            };
        }

        private static GameTask DirectorsBoard(int cost)
        {
            return (game, player) =>
            {
                game.Report.Invoke(LanguagePack.GetTranslation("taskdirectorsboard0", player.Nickname, cost));

                var amount = (game.Players.Length - 1) * cost;

                player.AskPlayer(player, LanguagePack.GetTranslation("taskdirectorsboard1"),
                    LanguagePack.GetTranslation("taskdirectorsboard2", cost,
                    amount), new string[] { LanguagePack.GetTranslation("taskdirectorsboard3"), });

                if (player.Wallet.CanTakeMoney(amount))
                {
                    player.Wallet.TryTakeMoney(amount);

                    foreach (var item in game.Players)
                        if (item.Index != player.Index)
                            item.Wallet.PutMoney(cost);

                    game.Report.Invoke(LanguagePack.GetTranslation("plpayedbankpay"));
                }
                else 
                    game.BankruptPlayer(player, null);
            };
        }

        private static GameTask GoToNearestPropertyFromTheGroup(string report, RentaPropertyGroup group)
        {
            return (game, player) =>
            {
                if (!(game.Board.GetNextSpace((space) =>
                {
                    return space is RentaPropertySpace rentaSpace
                    && ReferenceEquals(rentaSpace.RentaProperty.PropertyGroup, group);
                }, player.Coord) is RentaPropertySpace nearestSpace)) return;

                game.Report.Invoke(LanguagePack.GetTranslation(report));
                game.Board.MoveForward(game, player, IBoard.GetDistanceFrom(player.Coord, nearestSpace.Coord, game.Board.Spaces.Length));

                if (!nearestSpace.RentaProperty.ShouldPlayerPayRenta(player))
                {
                    nearestSpace.Action(game, player);
                    return;
                }

                game.Report.Invoke(LanguagePack.GetTranslation("plstoppedonstr", player.Nickname, nearestSpace.Name));

                if (nearestSpace.RentaProperty is Utility)
                {
                    game.Report.Invoke(LanguagePack.GetTranslation("tasknearestutility1",
                        player.Nickname, nearestSpace.RentaProperty.Name));

                    player.AskPlayer(player, LanguagePack.GetTranslation("tasknearestutility2"),
                        LanguagePack.GetTranslation("tasknearestutility3"),
                        new string[] { LanguagePack.GetTranslation("tasknearestutility4"), });

                    game.Dice.RollDiceWithReport(game);

                    nearestSpace.MakePlayerPayRenta(game, player, 
                        game.Dice.SpacesNumberToMove * (nearestSpace.RentaProperty as Utility).factors.LastOrDefault());

                    return;
                }

                nearestSpace.MakePlayerPayRenta(game, player, nearestSpace.RentaProperty.GetRenta(game) * 2);
            };
        }

    }
}
