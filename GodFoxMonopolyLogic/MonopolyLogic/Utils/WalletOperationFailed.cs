using System;
using System.Runtime.Serialization;

namespace MonopolyLogic.Utils
{
    public class WalletOperationFailed : Exception
    {
        public WalletOperationFailed() { }

        public WalletOperationFailed(string message) : base(message) { }

        public WalletOperationFailed(string message, Exception inner) : base(message, inner) { }

        protected WalletOperationFailed(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
