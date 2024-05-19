using System;
using System.Net;

namespace Rent.Core.Exceptions
{
	public class CustomException : Exception
	{
		public HttpStatusCode StatusCode { get; private set; }
		public CustomException(string message, HttpStatusCode statusCode) : base(message)
		{
			StatusCode = statusCode;
		}

		public CustomException()
		{
		}

		public CustomException(string message) : base(message)
		{
		}

		public CustomException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
