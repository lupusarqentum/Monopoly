using MonopolyLogic.Spaces;

namespace MonopolyLogic
{
    public delegate ISpaceRepresenter GetSpaceRepresenter();

    public interface ISpaceRepresenter
    {
        void SetSpaceToRepresenting(Space space);
    }
}
