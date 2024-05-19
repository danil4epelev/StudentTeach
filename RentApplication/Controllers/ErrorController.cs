using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Rent.Core.Exceptions;
using Rent.Core.Logger;
using System.Net;
using System.Text;

namespace Rent.Api.Controllers
{
	[ApiController]
	public class ErrorController : ControllerBase
	{
		[Route("/error")]
		public IActionResult ErrorLocalDevelopment([FromServices] IWebHostEnvironment webHostEnvironment)
		{
			var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

			LogError(context);

			var statusCodeForSend = HttpStatusCode.InternalServerError;
			var messageForSend = "Internal Server Error Occurred";

			if ((context.Error as CustomException) != null)
			{
				var exception = context.Error as CustomException;
				statusCodeForSend = exception.StatusCode;
				messageForSend = context.Error.Message;
#if DEBUG
				messageForSend = messageForSend + Environment.NewLine + "StackTrace: " + exception.StackTrace;
#endif
			}

			if (context.Error.InnerException != null && context.Error.InnerException.GetType() == typeof(CustomException))
			{
				statusCodeForSend = (context.Error.InnerException as CustomException).StatusCode;
				messageForSend = context.Error.InnerException.Message;
			}


			//var result = new HttpResponseMessage(statusCodeForSend)
			//{
			//	Content = new StringContent(messageForSend),
			//	ReasonPhrase = "Exception"
			//};

			//return result;
			return Problem(detail: webHostEnvironment.EnvironmentName != "Development" ? string.Empty : context.Error.StackTrace,
							title: messageForSend);
		}

		private void LogError(IExceptionHandlerFeature context)
		{
			var message = GetLogMessage(context);

			if (context.Error is CustomException)
			{
				LogProvider.LogWarning(message);
			}
			else
			{
				LogProvider.LogError(message);
			}
		}

		private string GetLogMessage(IExceptionHandlerFeature context)
		{
			var ex = context.Error;

			var login = string.Empty;

			try
			{

				if (HttpContext.User != null)
				{
					login = HttpContext.User.Identity.Name;
				}
			}
			catch (Exception)
			{

			}

			var strLogText = new StringBuilder();
			if (!string.IsNullOrWhiteSpace(login))
			{
				strLogText.AppendLine("UserLogin: " + login);
			}
			strLogText.AppendLine("Message: " + ex.Message);
			strLogText.AppendLine("Source: " + ex.Source);
			strLogText.AppendLine("StackTrace: " + ex.StackTrace);
			strLogText.AppendLine("TargetSite: " + ex.TargetSite);

			LogInnerException(ref strLogText, ex, 0);

			var requestedURi = Request.Path;
			var requestMethod = Request.Method;
			var requestQueryString = Request.QueryString;
			var timeUtc = DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");

			var loggerMessage = timeUtc + Environment.NewLine +
								requestedURi + Environment.NewLine +
								requestMethod + Environment.NewLine +
								requestQueryString + Environment.NewLine +
								strLogText.ToString() + Environment.NewLine;
			return loggerMessage;
		}

		private void LogInnerException(ref StringBuilder logStringBuilder, Exception ex, int level)
		{

			if (ex.InnerException != null)
			{
				var innEx = ex.InnerException;
				var tabString = string.Concat(Enumerable.Repeat("\t", level + 1));
				logStringBuilder.AppendLine(tabString + "Exception: " + ex.InnerException);
				logStringBuilder.AppendLine(tabString + "Exception Message: " + innEx?.Message);
				logStringBuilder.AppendLine(tabString + "Exception Source: " + innEx?.Source);
				logStringBuilder.AppendLine(tabString + "Exception StackTrace: " + innEx?.StackTrace);
				logStringBuilder.AppendLine(tabString + "Exception TargetSite: " + innEx?.TargetSite);

				LogInnerException(ref logStringBuilder, innEx, level + 1);
			}


		}

	}
}
