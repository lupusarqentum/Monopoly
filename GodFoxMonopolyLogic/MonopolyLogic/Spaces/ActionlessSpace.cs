using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.Spaces
{
    public class ActionlessSpace : Space
    {
        public override string Name { get; }

        public ActionlessSpace(int coord, string name) : base(coord) => Name = name;

        internal override void Action(IMonopolyGame game, IPlayer player) 
            => game.Report.Invoke(LanguagePack.GetTranslation("plstoppedonstr", player.Nickname, Name));
    }
}
