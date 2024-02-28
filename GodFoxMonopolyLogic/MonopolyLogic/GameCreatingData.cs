using MonopolyLogic.PlayerWork;

namespace MonopolyLogic
{
    public struct GameCreatingData
    {
        public readonly string[] playersNicknames;
        public readonly PlayersChoice playersChoiceHandler;
        public readonly PlayerBidding playerBidding;
        public readonly GetSpaceRepresenter spaceRepresenterGetter;

        public GameCreatingData(string[] playersNicknames, PlayersChoice playersChoiceHandler, 
                                PlayerBidding playerBidding, GetSpaceRepresenter spaceRepresenterGetter)
        {
            this.playersNicknames = playersNicknames;
            this.playersChoiceHandler = playersChoiceHandler;
            this.playerBidding = playerBidding;
            this.spaceRepresenterGetter = spaceRepresenterGetter;
        }
    }
}
