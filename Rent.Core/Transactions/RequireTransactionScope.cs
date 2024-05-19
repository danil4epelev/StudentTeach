using System;
using System.Transactions;

namespace Rent.Core.Transactions
{
	public class RequiredTransactionScope : IDisposable
	{
		private readonly TransactionScope _transaction;
		private bool _disposed;

		public RequiredTransactionScope()
		{
			_transaction = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);
			_disposed = false;
		}

		public void Complete()
		{
			if (_disposed)
			{
				throw new Exception("Объект транзакции уже удален");
			}

			_transaction.Complete();
		}

		public void Dispose()
		{
			_disposed = true;
			_transaction.Dispose();
		}
	}
}
