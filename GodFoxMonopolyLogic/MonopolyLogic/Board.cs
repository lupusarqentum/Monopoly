using System;
using MonopolyLogic.PlayerWork;
using MonopolyLogic.Spaces;
using MonopolyLogic.Utils;

namespace MonopolyLogic
{
    public interface IBoard
    {
        EventHandler PlayerIsMoved { get; }

        Space[] Spaces { get; set; }

        ISpaceRepresenter GetSpaceRepresenterByNumber(int spaceNumber);

        void LandBoard(IMonopolyGame game, IPlayer player, int doubleCount = 0);
        void MoveForward(IMonopolyGame game, IPlayer player, int spacesToMove);
        void MoveForwardAndAct(IMonopolyGame game, IPlayer player, int spacesToMove);
        void SetPlayerCoordTo(IPlayer player, int newPlayerCoord);

        Space GetNextSpace(Predicate<Space> isSuitable, int startCoord);

        static int GetDistanceFrom(int coord1, int coord2, int spacesOnBoard)
        {
            if (coord1 == coord2) return 0;
            if (coord1 < coord2) return coord2 - coord1;
            return spacesOnBoard - coord1 + coord2;
        }

        static bool IsSpaceZeroPassed(int coordFrom, int coordTo, int spacesToMove)
            => spacesToMove > 0 && coordTo < coordFrom;
    }

    public sealed class Board : IBoard
    {
        public EventHandler PlayerIsMoved { get; } = (sender, e) => { };
        
        public Space[] Spaces { get; set; }

        public ISpaceRepresenter GetSpaceRepresenterByNumber(int spaceNumber) => Spaces[spaceNumber].SpaceRepresenter;

        public void LandBoard(IMonopolyGame game, IPlayer player, int doubleCount = 0)
        {
            player.AskPlayer?.Invoke(player, LanguagePack.GetTranslation("yourmovetitle"),
                             LanguagePack.GetTranslation("yourmovedesc"), new string[] { LanguagePack.GetTranslation("yourmoveact0") });
            game.Dice.RollDiceWithReport(game);

            var @double = false;
            if (IDice.IsDouble(game.Dice.LatestDiceRollingResult))
            {
                @double = true;
                doubleCount++;

                if (doubleCount == Constants.DoublesCountToJail)
                {
                    game.Report.Invoke(LanguagePack.GetTranslation("threetimesdoublejail", player.Nickname));
                    game.Jail.JailersRegistration.ArrestPlayer(game, player);
                    return;
                }

                game.Report.Invoke(LanguagePack.GetTranslation("doublerolled", player.Nickname));
            }

            MoveForwardAndAct(game, player, game.Dice.SpacesNumberToMove);

            if (@double && !player.IsBankrupt) LandBoard(game, player, doubleCount);
        }

        public void MoveForward(IMonopolyGame game, IPlayer player, int spacesToMove)
        {
            Space to; var fromCoord = Spaces[player.Coord].Coord;
            SetPlayerCoordTo(player, GetNewPlayerPosition(player.Coord, spacesToMove));
            to = Spaces[player.Coord];

            if (IBoard.IsSpaceZeroPassed(fromCoord, to.Coord, spacesToMove))
            {
                player.Wallet.PutMoney(Constants.Salary);
                game.Report.Invoke(LanguagePack.GetTranslation("salarygetting", player.Nickname, Constants.Salary));
            }

            player.Statistics.PassedSpaces += spacesToMove;
        }

        public void MoveForwardAndAct(IMonopolyGame game, IPlayer player, int spacesToMove)
        {
            MoveForward(game, player, spacesToMove);
            Spaces[player.Coord].Action(game, player);
        }

        public void SetPlayerCoordTo(IPlayer player, int newPlayerCoord)
        {
            Space from, to;
            from = Spaces[player.Coord];
            to = Spaces[newPlayerCoord];
            player.Coord = newPlayerCoord;

            PlayerIsMoved.Invoke(this, new PlayerIsMovedEventArgs(player, newPlayerCoord));
        }

        private int GetNewPlayerPosition(int playerCoord, int spacesToMove)
        {
            if (spacesToMove >= 0) return (playerCoord + spacesToMove) % Spaces.Length;

            // spacesToMove is negative (moving back)
            var temp = playerCoord + spacesToMove;
            if (temp < 0) temp = Spaces.Length + temp;
            return temp;
        }

        public Space GetNextSpace(Predicate<Space> isSuitable, int startCoord)
        {
            for (int i = startCoord; i < Spaces.Length; i++)
                if (isSuitable(Spaces[i]))
                    return Spaces[i];

            for (int i = 0; i < startCoord; i++)
                if (isSuitable(Spaces[i]))
                    return Spaces[i];

            return null;
        }
    }
}
