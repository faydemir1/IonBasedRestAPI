/*  
 *  Developed under MIT Licence for academic purposes.
 *
 *  Author: Fikri Aydemir
 *  Date  :	21/04/2022 
 */

using IonClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.RegularExpressions;


namespace IonClassLibrary.Filter
{
    public class LinkOverridingFilter : IAsyncResultFilter
    {
        private readonly IServiceProvider _provider;
        private readonly IonOptions _ionOptions;
        private const string LocalHostWithHttp = "http://localhost";
        private const string LocalHostWithHttps = "https://localhost";


        public LinkOverridingFilter(IServiceProvider provider, IonOptions ionOptions)
        {
            _provider = provider;
            _ionOptions = ionOptions;
        }

        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var asObjectResult = context.Result as ObjectResult;
            bool shouldSkip = asObjectResult?.StatusCode >= 400 || asObjectResult?.Value == null || asObjectResult?.Value as IonResourceModel == null;

            if (shouldSkip)
            {
                return next();
            }

            var factory = _provider.GetRequiredService<IUrlHelperFactory>();
            var urlHelper = factory.GetUrlHelper(context);
            var overrider = new LinkOverrider(urlHelper);

            if (asObjectResult != null && asObjectResult.Value != null)
            {
                var modelAsLink = asObjectResult.Value as Link;
                if (modelAsLink != null && !string.IsNullOrEmpty(modelAsLink.Href))
                {
                    if (modelAsLink.Href.Contains(LocalHostWithHttps))
                    {
                        modelAsLink.Href = Regex.Replace(modelAsLink.Href, @"https:\/\/localhost:\d\d\d\d", string.Format("{0}/{1}", _ionOptions.ApiGatewayBaseUrl, _ionOptions.RouteName));
                    }
                    else if (modelAsLink.Href.Contains(LocalHostWithHttp))
                    {
                        modelAsLink.Href = Regex.Replace(modelAsLink.Href, @"http:\/\/localhost:\d\d\d\d", string.Format("{0}/{1}", _ionOptions.ApiGatewayBaseUrl, _ionOptions.RouteName));
                    }
                }
            }

            OverrideAllLinks(asObjectResult.Value, overrider, _ionOptions);

            return next();
        }

        private static void OverrideAllLinks(object model, LinkOverrider overrider, IonOptions ionOptions)
        {
            if (model == null) return;

            var modelAsLink = model as Link;
            if (modelAsLink != null && !string.IsNullOrEmpty(modelAsLink.Href))
            {
                if (modelAsLink.Href.Contains(LocalHostWithHttps))
                {
                    modelAsLink.Href = Regex.Replace(modelAsLink.Href, @"https:\/\/localhost:\d\d\d\d", string.Format("{0}/{1}", ionOptions.ApiGatewayBaseUrl, ionOptions.RouteName));
                }
                else if (modelAsLink.Href.Contains(LocalHostWithHttp))
                {
                    modelAsLink.Href = Regex.Replace(modelAsLink.Href, @"http:\/\/localhost:\d\d\d\d", string.Format("{0}/{1}", ionOptions.ApiGatewayBaseUrl, ionOptions.RouteName));
                }
            }

            var allProperties = model
                .GetType().GetTypeInfo()
                .GetAllProperties()
                .Where(p => p.CanRead)
                .ToArray();

            var linkProperties = allProperties
                .Where(p => p.CanWrite && p.PropertyType == typeof(Link));

            foreach (var linkProperty in linkProperties)
            {
                var originalLink = linkProperty.GetValue(model) as Link;
                var overridden = overrider.Override(originalLink);
                if (overridden == null) continue;

                if (overridden != null && !string.IsNullOrEmpty(overridden.Href))
                {
                    if (overridden.Href.Contains(LocalHostWithHttps))
                    {
                        overridden.Href = Regex.Replace(overridden.Href, @"https:\/\/localhost:\d\d\d\d", string.Format("{0}/{1}", ionOptions.ApiGatewayBaseUrl, ionOptions.RouteName));
                    }
                    else if (overridden.Href.Contains(LocalHostWithHttp))
                    {
                        overridden.Href = Regex.Replace(overridden.Href, @"http:\/\/localhost:\d\d\d\d", string.Format("{0}/{1}", ionOptions.ApiGatewayBaseUrl, ionOptions.RouteName));
                    }
                }

                linkProperty.SetValue(model, overridden);

                if (linkProperty.Name == nameof(IonResourceModel.Self))
                {
                    allProperties
                        .SingleOrDefault(p => p.Name == nameof(IonResourceModel.Href))
                        ?.SetValue(model, overridden.Href);

                    allProperties
                        .SingleOrDefault(p => p.Name == nameof(IonResourceModel.Method))
                        ?.SetValue(model, overridden.Method);

                    allProperties
                        .SingleOrDefault(p => p.Name == nameof(IonResourceModel.Relations))
                        ?.SetValue(model, overridden.Relations);
                }

                for (int i = 0; i < overridden.Relations.Length; i++)
                {
                    var theLinkString = overridden.Relations[i];
                    if (theLinkString.Contains(LocalHostWithHttps))
                    {
                        overridden.Relations[i] = Regex.Replace(theLinkString, @"https:\/\/localhost:\d\d\d\d", string.Format("{0}/{1}", ionOptions.ApiGatewayBaseUrl, ionOptions.RouteName));
                    }
                    else if (theLinkString.Contains(LocalHostWithHttp))
                    {
                        overridden.Relations[i] = Regex.Replace(theLinkString, @"http:\/\/localhost:\d\d\d\d", string.Format("{0}/{1}", ionOptions.ApiGatewayBaseUrl, ionOptions.RouteName));
                    }
                }

            }

            var arrayProperties = allProperties.Where(p => p.PropertyType.IsArray);
            OverrideLinksInArrays(arrayProperties, model, overrider, ionOptions);

            var objectProperties = allProperties.Except(linkProperties).Except(arrayProperties);
            OverrideLinksInNestedObjects(objectProperties, model, overrider, ionOptions);
        }

        private static void OverrideLinksInNestedObjects(IEnumerable<PropertyInfo> objectProperties, object model, LinkOverrider overrider, IonOptions ionOptions)
        {
            foreach (var objectProperty in objectProperties)
            {
                if (objectProperty.PropertyType == typeof(string))
                {
                    continue;
                }

                var typeInfo = objectProperty.PropertyType.GetTypeInfo();
                if (typeInfo.IsClass)
                {
                    OverrideAllLinks(objectProperty.GetValue(model), overrider, ionOptions);
                }
            }
        }

        private static void OverrideLinksInArrays(IEnumerable<PropertyInfo> arrayProperties, object model, LinkOverrider overrider, IonOptions ionOptions)
        {
            foreach (var arrayProperty in arrayProperties)
            {
                var array = arrayProperty.GetValue(model) as Array ?? new Array[0];

                foreach (var element in array)
                {
                    OverrideAllLinks(element, overrider, ionOptions);
                }
            }
        }
    }
}
