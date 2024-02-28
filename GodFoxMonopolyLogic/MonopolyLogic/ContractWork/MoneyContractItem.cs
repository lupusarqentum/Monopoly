using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.ContractWork
{
    public sealed class MoneyContractItem : IContractItem
    {
        public enum WhoGivesMoney : byte
        {
            Proposer,
            Opponent,
        }

        private readonly int amount;
        private readonly WhoGivesMoney whoGivesMoney;

        private readonly int giverId;

        public string Name => LanguagePack.GetTranslation("moneyones", amount);

        public MoneyContractItem(int amount, WhoGivesMoney whoGivesMoney, int giverId)
        {
            this.amount = amount;
            this.whoGivesMoney = whoGivesMoney;
            this.giverId = giverId;
        }

        public bool CheckContractValidity(IPlayer proposer, IPlayer opponent)
        {
            if (whoGivesMoney == WhoGivesMoney.Proposer)
                return proposer.Wallet.CanTransferMoney(amount);
            else
                return opponent.Wallet.CanTransferMoney(amount);
        }

        public void Execute(IPlayer proposer, IPlayer opponent)
        {
            if (whoGivesMoney == WhoGivesMoney.Proposer)
            {
                if (proposer.Wallet.CanTransferMoney(amount))
                    proposer.Wallet.TryTransferMoney(amount, opponent.Wallet);
                else
                    proposer.Wallet.TransferAll(opponent.Wallet);
            }
            else
            {
                if (opponent.Wallet.CanTransferMoney(amount))
                    opponent.Wallet.TryTransferMoney(amount, proposer.Wallet);
                else
                    opponent.Wallet.TransferAll(proposer.Wallet);
            }
        }

        public ContractMemberAction WhatContractMemberDoesWithIt(IPlayer contractMember) => 
            contractMember?.Index == giverId ? ContractMemberAction.Give : ContractMemberAction.Get;

        public override string ToString() => Name;
    }
}
