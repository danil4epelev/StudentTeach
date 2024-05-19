using NLog.Web;
using System;

namespace Rent.Core.Logger
{
	public static class LogProvider
	{
		private static NLog.Logger _logger;

		public static void Init()
		{
			_logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
		}

		public static void LogInfo(string message, Exception exception = null)
		{
			_logger.Info(exception, message);
		}

		public static void LogError(string message, Exception exception = null)
		{
			_logger.Error(exception, message);
		}

		public static void LogWarning(string message, Exception exception = null)
		{
			_logger.Warn(exception, message);
		}


		public static void Shutdown()
		{
			NLog.LogManager.Shutdown();
		}
	}
}
