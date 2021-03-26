using System;

namespace TradeStats.Exceptions
{
    class SelectedTradesWrongAmountException : Exception
    {
        public SelectedTradesWrongAmountException()
        {
        }

        public SelectedTradesWrongAmountException(string message) : base(message)
        {
        }

        public SelectedTradesWrongAmountException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
