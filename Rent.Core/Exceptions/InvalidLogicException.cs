using System;
using System.Net;

namespace Rent.Core.Exceptions
{
	public class InvalidLogicException : CustomException
	{
		private static readonly HttpStatusCode statusCode = HttpStatusCode.Conflict;
		public InvalidLogicException(string message) : base(message, statusCode)
		{

		}

		public InvalidLogicException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
