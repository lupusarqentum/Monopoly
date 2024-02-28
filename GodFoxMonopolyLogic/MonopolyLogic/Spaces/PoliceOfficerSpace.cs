using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.Spaces
{
    public sealed class PoliceOfficerSpace : Space
    {
        public override string Name => LanguagePack.GetTranslation("policeofficername");

        public PoliceOfficerSpace(int coord) : base(coord) {}

        internal override void Action(IMonopolyGame game, IPlayer player)
        {
            game.Report.Invoke(LanguagePack.GetTranslation("officerjailing", player.Nickname));
            game.Jail.JailersRegistration.ArrestPlayer(game, player);
        }
    }
}
