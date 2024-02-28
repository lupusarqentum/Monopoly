namespace MonopolyLogic.Parsers
{
    public sealed class VirtualMachine
    {
        public readonly VariablesMemory variables;
        public readonly IMonopolyGame monopolyGame;
        
        public readonly GetSpaceRepresenter spaceRepresenterGetter;

        public VirtualMachine(IMonopolyGame monopolyGame, GetSpaceRepresenter spaceRepresenterGetter)
        {
            variables = new VariablesMemory();
            this.monopolyGame = monopolyGame;

            this.spaceRepresenterGetter = spaceRepresenterGetter;
        }
    }
}
