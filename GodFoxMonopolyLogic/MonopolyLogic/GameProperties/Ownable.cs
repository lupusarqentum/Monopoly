using MonopolyLogic.ContractWork;
using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.GameProperties
{
    public abstract class Ownable : IContractItem
    {
        protected IPlayer _owner;
        public abstract IPlayer Owner { get; set; }

        private readonly string name;
        public virtual string Name => name;

        private readonly int cost;
        public int Cost => cost;

        public virtual int Assets => Cost;

        public Ownable(string name, int cost)
        {
            this.name = name;
            this.cost = cost;

            _owner = null;
        }

        public virtual void Execute(IPlayer proposer, IPlayer opponent) =>
            Owner = Owner?.Index == proposer?.Index ? opponent : proposer;

        public virtual bool CheckContractValidity(IPlayer proposer, IPlayer opponent) =>
            Owner?.Index == proposer?.Index || Owner?.Index == opponent?.Index;

        public virtual ContractMemberAction WhatContractMemberDoesWithIt(IPlayer contractMember) =>
            Owner?.Index == contractMember?.Index ? ContractMemberAction.Give : ContractMemberAction.Get;

        public override string ToString() => Name;
    }
}
