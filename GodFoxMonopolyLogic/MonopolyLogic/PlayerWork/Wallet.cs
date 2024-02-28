using MonopolyLogic.Utils;

namespace MonopolyLogic.PlayerWork
{
    public interface IWallet
    {
        int Money { get; }
        int Score { get; set; }

        void ResetWallet();
        void PutMoney(int amount);
        bool CanTakeMoney(int amount);
        bool CanTransferMoney(int amount);
        void TransferAll(IWallet recipient);

        void TryTakeMoney(int amount);
        void TryTransferMoney(int amount, IWallet other);
    }

    public sealed class Wallet : IWallet
    {
        public int Money { get; private set; }
        public int Score { get; set; }

        public Wallet(int money) => Money = money;
        public Wallet() : this(Constants.StartMoney) { }

        public void ResetWallet() => Money = 0;
        public void PutMoney(int amount) => Money += amount;
        public bool CanTakeMoney(int amount) => Money >= amount;
        public bool CanTransferMoney(int amount) => CanTakeMoney(amount);
        public void TransferAll(IWallet recipient) => TryTransferMoney(Money, recipient);

        public void TryTakeMoney(int amount)
        {
            if (CanTakeMoney(amount)) Money -= amount;
            else throw new WalletOperationFailed("There are not enough money in the wallet");
        }

        public void TryTransferMoney(int amount, IWallet other)
        {
            if (CanTransferMoney(amount))
            {
                Money -= amount;
                other.PutMoney(amount);
                other.Score += amount;
            }
            else
            {
                throw new WalletOperationFailed("There are not enough money in the wallet");
            }
        }

        public override string ToString() => Money.ToString();
    }
}
