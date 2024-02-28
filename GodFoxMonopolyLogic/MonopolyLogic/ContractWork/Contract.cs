namespace MonopolyLogic.ContractWork
{
    public abstract class Contract
    {
        public abstract bool ProposeContract(IMonopolyGame game);
        public abstract void Execute(IMonopolyGame game);
    }
}
