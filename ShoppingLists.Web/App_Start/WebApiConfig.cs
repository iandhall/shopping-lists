using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ShoppingLists.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Services.Add(typeof(IExceptionLogger), new ApiExceptionLogger());
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            var serializerSettings = config.Formatters.JsonFormatter.SerializerSettings;
            serializerSettings.Formatting = Formatting.Indented;
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.MapHttpAttributeRoutes();
        }
    }
}
