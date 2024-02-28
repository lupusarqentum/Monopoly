using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.ContractWork
{
    public interface IContractItem
    {
        string Name { get; }

        void Execute (IPlayer proposer, IPlayer opponent);
        bool CheckContractValidity(IPlayer proposer, IPlayer opponent);

        ContractMemberAction WhatContractMemberDoesWithIt(IPlayer contractMember);
    }
}
