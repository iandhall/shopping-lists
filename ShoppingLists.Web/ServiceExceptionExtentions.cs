// TODO: Which status code?

//using System.Net;
//using System.Net.Http;
//using ShoppingLists.BusinessLayer.Exceptions;

//namespace ShoppingLists.Web
//{
//    public static class ServiceExceptionExtentions
//    {
//        public static HttpResponseMessage ToHttpResponseMessage(this ServiceException ex, HttpRequestMessage request)
//        {
//            return request.CreateResponse(
//                ex.StatusCode,
//                new ExceptionModel()
//                {
//                    ExceptionType = ex.GetType().Name,
//                    Message = ex.Message,
//                    UserMessage = ex.UserMessage
//                }
//            );
//        }
//    }
//}