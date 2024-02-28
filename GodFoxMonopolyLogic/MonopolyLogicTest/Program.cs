using System;
using System.Collections.Generic;
using MonopolyLogic;
using MonopolyLogic.ContractWork;
using MonopolyLogic.GameProperties;
using MonopolyLogic.PlayerWork;

namespace MonopolyLogicTest
{
    class Program
    {
        private static IMonopolyGame board;
        
        private static void Main(string[] args)
        {
            LanguagePack.SetLanguage(nameof(LanguagePack.Languages.Russian));
            
            var nicknames = new string[] { "Arkhalis", "GodFox", "diafox", "veka", };

            board = new StandartGameCreator().CreateGame(new GameCreatingData(nicknames, OnPlayersChoiceHandler, OnPlayerBids, GetSpaceRepresenter));
            board.Report += OnReport;
            //board.DiceIsRolled += OnDiceRolled;
            //board.PlayerIsMoved += OnPlayerMoved;
            
            var movingPlayerNumber = 0;
            while (true)
            {
                board.TurnPlayer(movingPlayerNumber);
                movingPlayerNumber = ++movingPlayerNumber % board.PlayersCount;
                ParseCommands();

                Console.Clear();
            }
        }

        private static void ParseCommands()
        {
            string command = null;

            while (!string.Equals(command, ""))
            {
                command = Console.ReadLine();

                ParseOneCommand(command);
            }
        }

        private static void ParseOneCommand(string command)
        {
            if (command.StartsWith("space"))
            {
                int spaceNumber;

                try
                {
                    spaceNumber = Convert.ToInt32(command[5..]);
                }
                catch (FormatException)
                {
                    return;
                }

                Console.WriteLine(board.Board.GetSpaceRepresenterByNumber(spaceNumber));
            }
            else if (command.StartsWith("player"))
            {
                int playerNumber;

                try
                {
                    playerNumber = Convert.ToInt32(command[6..]);
                }
                catch (FormatException)
                {
                    return;
                }

                Console.WriteLine(board.GetPlayerByNumber(playerNumber).ToString());
            }
            else if (command.Equals("cntr"))
            {
                IPlayer proposer, opponent;
                var contractItems = new List<IContractItem>();

                try
                {
                    Console.Write("Proposer: ");
                    proposer = board.GetPlayerByNumber(Convert.ToInt32(Console.ReadLine()));
                    Console.Write("Opponent: ");
                    opponent = board.GetPlayerByNumber(Convert.ToInt32(Console.ReadLine()));

                    Console.Write("Who gives money (enter 0-proposer, 1-opponent, 2-none:");
                    var whoGivesMoney = Convert.ToInt32(Console.ReadLine());

                    if (whoGivesMoney == 0 || whoGivesMoney == 1)
                    {
                        Console.Write("Money Giving Amount: ");
                        var moneyAmount = Convert.ToInt32(Console.ReadLine());

                        var whoGivesMoney1 = whoGivesMoney == 0 ?
                            MoneyContractItem.WhoGivesMoney.Proposer : MoneyContractItem.WhoGivesMoney.Opponent;

                        int giverId = whoGivesMoney1 == MoneyContractItem.WhoGivesMoney.Proposer ? proposer.Index : opponent.Index;
                        var moneyContractItem = new MoneyContractItem(moneyAmount, whoGivesMoney1, giverId);

                        contractItems.Add(moneyContractItem);
                    }

                    Console.WriteLine("Enter properties numbers: ");
                    string propertyNumber;
                    while (true)
                    {
                        propertyNumber = Console.ReadLine();

                        if (propertyNumber.Equals("")) break;

                        var intPropertyNumber = Convert.ToInt32(propertyNumber);
                        
                        var spaceRepre = board.Board.GetSpaceRepresenterByNumber(intPropertyNumber) as SpaceRepresenter;
                        var gprop = spaceRepre.GetProperty();
                        if (gprop != null)
                            contractItems.Add(gprop);
                    }

                    Console.Write("Enter transferring jailfreeTickets count: ");
                    var transferringJailfreeTicketsAmount = Convert.ToInt32(Console.ReadLine());
                    if (transferringJailfreeTicketsAmount != 0)
                    {
                        List<JailFreeTicket> jailFrees;

                        Console.Write("Who gives jailfree tickets (0-proposer, 1-opponent):");
                        var whoGivesTickets = Convert.ToInt32(Console.ReadLine());
                        if (whoGivesTickets == 0)
                            jailFrees = JailFreeTicket.GetPlayerJailFreeTickets(proposer, transferringJailfreeTicketsAmount);
                        else
                            jailFrees = JailFreeTicket.GetPlayerJailFreeTickets(opponent, transferringJailfreeTicketsAmount);

                        contractItems.AddRange(jailFrees);
                    }

                } catch (Exception) { return; }

                var isProcessFinished = ContractBetweenTwoPlayers.CreateContract(proposer, opponent, contractItems.ToArray(),
                                            out ContractBetweenTwoPlayers contract);
                Console.WriteLine(isProcessFinished);

                if (isProcessFinished)
                {
                    board.ProposeContract(contract);
                }
            }
            else if (command.StartsWith("scml"))
            {
                int propertyNumber;

                try
                {
                    propertyNumber = Convert.ToInt32(command[4..]);
                }
                catch (FormatException)
                {
                    return;
                }

                (board.Board.GetSpaceRepresenterByNumber(propertyNumber) as SpaceRepresenter).LiftMortgage(board);
            }
            else if (command.StartsWith("scm"))
            {
                int propertyNumber;

                try
                {
                    propertyNumber = Convert.ToInt32(command[3..]);
                }
                catch (FormatException)
                {
                    return;
                }

                (board.Board.GetSpaceRepresenterByNumber(propertyNumber) as SpaceRepresenter).MortgageIt(board);
            }
            else if (command.StartsWith("build+") || command.StartsWith("build-"))
            {
                int propertyNumber;

                try
                {
                    propertyNumber = Convert.ToInt32(command[6..]);
                }
                catch (FormatException)
                {
                    return;
                }

                var isr = board.Board.GetSpaceRepresenterByNumber(propertyNumber) as SpaceRepresenter;
                if (command[5] == '+') isr.BuildHouse(board);
                else isr.SellHouse(board);
            }
        }

        private static int OnPlayerBids(IPlayer player, string tip, string description)
        {
            Console.WriteLine(player.Nickname + ",");
            Console.WriteLine(tip);
            Console.WriteLine(description);

            int result;
            while (true)
            {
                string answer = Console.ReadLine();
                try
                {
                    result = Convert.ToInt32(answer);

                    break;
                }
                catch (FormatException)
                {
                    ParseOneCommand(answer);
                    continue;
                }
            }

            return result;
        }

        //private static void OnPlayerMoved(object s, EventArgs e)
        //{
        //    var sender = s as Board;
        //    var args = e as PlayerIsMovedEventArgs;
        //    Console.WriteLine($"{args.Player.Nickname} to {args.WherePlayerWasStopped} coord");
        //}

        private static ISpaceRepresenter GetSpaceRepresenter()
        {
            return new SpaceRepresenter();
        }

        private static void OnReport(string text)
        {
            Console.WriteLine("Rep:" + text);
        }

        /*private static void OnDiceRolled(object s, EventArgs e)
        {
            var args = e as DiceIsRolledEventArgs;
            Console.WriteLine($"НА КУБИКАХ ВЫПАЛО {args.DiceRollingResult.Item1}:{args.DiceRollingResult.Item2}");
        }*/

        private static int OnPlayersChoiceHandler (IPlayer player, string tip, string description, string[] options)
        {
            Console.WriteLine(player.Nickname + ",");
            Console.WriteLine(tip);
            Console.WriteLine();
            Console.WriteLine(description);
            Console.WriteLine();

            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i}) {options[i]}");
            }

            int optionNumber;

            while (true)
            {
                var stringAnswer = Console.ReadLine();
                try
                {
                    optionNumber = Convert.ToInt32(stringAnswer);
                }
                catch (FormatException)
                {
                    ParseOneCommand(stringAnswer);
                    continue;
                }

                if (optionNumber >= 0 && optionNumber < options.Length) break;
                else Console.WriteLine("Выбран неправильный вариант ответа2.");
            }

            return optionNumber;
        }

    }
}
