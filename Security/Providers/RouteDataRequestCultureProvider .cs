using Microsoft.AspNetCore.Localization;

namespace Security.Providers
{
    public class RouteDataRequestCultureProvider : RequestCultureProvider
    {
        public int IndexOfCulture { get; set; }
        public int IndexOfUICulture { get; set; }

        public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var twoLetterCultureName = httpContext.Request.Path.Value?.Split("/")[IndexOfCulture]?.ToString();
            var twoLetterUICultureName = httpContext.Request.Path.Value?.Split("/")[IndexOfUICulture]?.ToString();

            string? culture = twoLetterCultureName == "uk" == true ? "uk-UA" : twoLetterCultureName == "en" == true ? "en-US" : null;
            string? uiCulture = twoLetterUICultureName == "uk" == true ? "uk-UA" : twoLetterUICultureName == "en" == true ? "en-US" : null;

            if (culture == null && uiCulture == null)
            {
                return NullProviderCultureResult;
            }

            culture = culture == null ? uiCulture : culture;
            uiCulture = uiCulture == null ? culture : uiCulture;

            var providerRequestCulture = new ProviderCultureResult(culture, uiCulture);

            return Task.FromResult<ProviderCultureResult?>(providerRequestCulture);
        }
    }
}
