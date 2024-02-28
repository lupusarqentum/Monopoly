using System;

namespace MonopolyLogic.Utils
{
    public class DiceIsRolledEventArgs : EventArgs
    {
        public int[] DiceRollingResult { get; private set; }

        public DiceIsRolledEventArgs(int[] diceRollingResult) => DiceRollingResult = diceRollingResult;
    }
}
