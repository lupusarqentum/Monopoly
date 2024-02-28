using System.Text;
using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.ContractWork
{
    public sealed class ContractBetweenTwoPlayers : Contract
    {
        private readonly IPlayer proposer;
        private readonly IPlayer opponent;

        private readonly IContractItem[] contractItems;

        private ContractBetweenTwoPlayers(IPlayer proposer, IPlayer opponent, IContractItem[] contractItems)
        {
            this.proposer = proposer;
            this.opponent = opponent;

            this.contractItems = contractItems;
        }

        public static bool CreateContract(IPlayer proposer, IPlayer opponent, 
                                          IContractItem[] contractItems, out ContractBetweenTwoPlayers contract)
        {
            if (!(proposer != null && opponent != null && proposer.Index != opponent.Index))
            {
                contract = null;
                return false;
            }

            foreach (var contractItem in contractItems)
            {
                if (!contractItem.CheckContractValidity(proposer, opponent))
                {
                    contract = null;
                    return false;
                }
            }

            contract = new ContractBetweenTwoPlayers(proposer, opponent, contractItems);
            return true;
        }

        public override void Execute(IMonopolyGame game)
        {
            var proposerGets = new StringBuilder(LanguagePack.GetTranslation("contractmembergets", proposer.Nickname));
            var opponentGets = new StringBuilder(LanguagePack.GetTranslation("contractmembergets", opponent.Nickname));

            var proposerGotSomething = false;
            var opponentGotSomething = false;

            foreach (var contractItem in contractItems)
            {
                if (contractItem.WhatContractMemberDoesWithIt(proposer) == ContractMemberAction.Get)
                {
                    proposerGets.Append($"{contractItem.Name}, ");
                    proposerGotSomething = true;
                }
                else
                {
                    opponentGets.Append($"{contractItem.Name}, ");
                    opponentGotSomething = true;
                }

                contractItem.Execute(proposer, opponent);
            }

            if (proposerGotSomething) proposerGets.Remove(proposerGets.Length - 2, 2);
            if (opponentGotSomething) opponentGets.Remove(opponentGets.Length - 2, 2);

            game.Report.Invoke(proposerGets.ToString());
            game.Report.Invoke(opponentGets.ToString());
        }

        public override bool ProposeContract(IMonopolyGame game)
        {
            var contractAskingBuffer = new StringBuilder(LanguagePack.GetTranslation("someoneproposesyoucontract", proposer.Nickname));

            OpponentDoesSomeAction(contractAskingBuffer, ContractMemberAction.Get, LanguagePack.GetTranslation("youwillget"));
            OpponentDoesSomeAction(contractAskingBuffer, ContractMemberAction.Give, LanguagePack.GetTranslation("youwillgive"));

            contractAskingBuffer.Append(LanguagePack.GetTranslation("goodthinking"));
            game.Report.Invoke(LanguagePack.GetTranslation("contractproposingreport", proposer.Nickname, opponent.Nickname));

            var result = opponent.AskPlayer(opponent, LanguagePack.GetTranslation("someoneproposesyoucontracttip"), 
                contractAskingBuffer.ToString(), 
                new string[] { LanguagePack.GetTranslation("acceptcontract"), LanguagePack.GetTranslation("rejectcontract") }) == 0;

            if (result) game.Report.Invoke(LanguagePack.GetTranslation("contractaccepted"));
            else game.Report.Invoke(LanguagePack.GetTranslation("contractrejected"));

            return result;
        }

        private void OpponentDoesSomeAction(StringBuilder contractAskingBuffer, 
                                ContractMemberAction contractMemberAction, string actionStartString)
        {
            var opponentGivesSomething = false;
            var opponentGivingBuffer = new StringBuilder();

            foreach (var contractItem in contractItems)
            {
                if (contractItem.WhatContractMemberDoesWithIt(opponent) == contractMemberAction)
                {
                    opponentGivingBuffer.Append(contractItem.Name + ", ");
                    opponentGivesSomething = true;
                }
            }

            if (opponentGivesSomething)
            {
                contractAskingBuffer.Append(actionStartString);
                contractAskingBuffer.Append(opponentGivingBuffer.ToString());
                contractAskingBuffer.Remove(contractAskingBuffer.Length - 2, 2);
            }
        }
    }
}
