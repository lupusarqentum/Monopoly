using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.Spaces
{
    public sealed class TaskSpace : Space
    {
        public override string Name { get; }
        
        private readonly string key;

        public TaskSpace(string name, string key, int coord) : base(coord)
        {
            Name = name;
            this.key = key;
        }

        internal override void Action(IMonopolyGame game, IPlayer player)
        {
            game.Report.Invoke(LanguagePack.GetTranslation("plstoppedonstr", player.Nickname, Name));
            game.TaskSystem.TaskCollector.GetRandomTask(key).Invoke(game, player);
        }
    }
}
