using System.Web.Http.ExceptionHandling;
using NLog;

namespace ShoppingLists.Web
{
    public class ApiExceptionLogger : ExceptionLogger
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public override void Log(ExceptionLoggerContext context)
        {
            _log.Error(context.Exception);
        }
    }
}