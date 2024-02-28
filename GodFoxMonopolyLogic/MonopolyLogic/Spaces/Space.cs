using System.Text;
using MonopolyLogic.PlayerWork;

namespace MonopolyLogic.Spaces
{
    public abstract class Space
    {
        private ISpaceRepresenter _spaceRepresenter;
        public ISpaceRepresenter SpaceRepresenter
        {
            get => _spaceRepresenter;
            set
            {
                _spaceRepresenter = value;
                _spaceRepresenter.SetSpaceToRepresenting(this);
            }
        }

        protected int coord;
        internal int Coord => coord;

        public abstract string Name { get; }

        public Space(int coord) => this.coord = coord;

        internal abstract void Action(IMonopolyGame game, IPlayer player);
        public virtual string GetInfo() => new StringBuilder($"Space Number: {Coord}\n").ToString();
        public override string ToString() => Name;
    }
}
