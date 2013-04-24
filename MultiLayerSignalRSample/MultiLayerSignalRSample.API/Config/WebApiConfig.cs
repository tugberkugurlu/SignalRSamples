using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Providers;
using MultiLayerSignalRSample.API.Formatting;

namespace MultiLayerSignalRSample.API.Config
{
    public static class WebApiConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            // If ExcludeMatchOnTypeOnly is true then we don't match on type only which means
            // that we return null if we can't match on anything in the request. This is useful
            // for generating 406 (Not Acceptable) status codes.
            config.Services.Replace(typeof(IContentNegotiator),
                new DefaultContentNegotiator(excludeMatchOnTypeOnly: true));

            // Remove all the validation providers 
            // except for DataAnnotationsModelValidatorProvider
            config.Services.RemoveAll(typeof(ModelValidatorProvider),
                validator => !(validator is DataAnnotationsModelValidatorProvider));
        }

        private static void ConfigureFormatters(MediaTypeFormatterCollection formatters)
        {
            // Remove unnecessary formatters
            MediaTypeFormatter jqueryFormatter = formatters.FirstOrDefault(x => x.GetType() == typeof(JQueryMvcFormUrlEncodedFormatter));
            formatters.Remove(formatters.XmlFormatter);
            formatters.Remove(formatters.FormUrlEncodedFormatter);
            formatters.Remove(jqueryFormatter);

            // Suppressing the IRequiredMemberSelector for all formatters
            foreach (var formatter in formatters)
            {
                formatter.RequiredMemberSelector = new SuppressedRequiredMemberSelector();
            }
        }
    }
}