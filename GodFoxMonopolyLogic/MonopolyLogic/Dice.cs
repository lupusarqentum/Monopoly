using System;
using MonopolyLogic.Utils;

namespace MonopolyLogic
{
    public interface IDice
    {
        static Random Random = new Random();
        
        static bool IsDouble(int[] p)
        {
            if (p.Length < 2)
                return true;

            for (int i = 1; i < p.Length; i++)
                if (p[i] != p[0])
                    return false;

            return true;
        }

        public event EventHandler DiceIsRolled;
        
        public int SpacesNumberToMove { get; }
        public int[] LatestDiceRollingResult { get; }
        
        public int[] RollDice();
        public int[] RollDiceWithReport(IMonopolyGame game);
    }

    public sealed class Dice : IDice
    {
        public event EventHandler DiceIsRolled = (sender, e) => { };

        public int SpacesNumberToMove => LatestDiceRollingResult[0] + LatestDiceRollingResult[1];
        public int[] LatestDiceRollingResult { get; private set; }

        public int[] RollDice()
        {
            var result = new int[2] { IDice.Random.Next(1, 7), IDice.Random.Next(1, 7) };
            LatestDiceRollingResult = result;

            DiceIsRolled.Invoke(this, new DiceIsRolledEventArgs(result));

            return result;
        }

        public int[] RollDiceWithReport(IMonopolyGame game)
        {
            var result = RollDice();
            game.Report.Invoke(LanguagePack.GetTranslation("dicerolled0", result[0], result[1]));
            return result;
        }
    }
}
