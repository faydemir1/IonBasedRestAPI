/*  
 *  Developed under MIT Licence for academic purposes.
 *
 *  Author: Fikri Aydemir
 *  Date  :	21/04/2022 
 */

using System.Text.Json.Serialization;

namespace IonClassLibrary.Models
{
    public class Link
    {
        public const string HttpGetMethod = "GET";

        public static Link To(string routeName, object routeValues = null)
            => new Link
            {
                RouteName = routeName,
                RouteValues = routeValues,
                Method = HttpGetMethod,
                Relations = null
            };

        public static Link ToCollection(string routeName, object routeValues = null)
            => new Link
            {
                RouteName = routeName,
                RouteValues = routeValues,
                Method = HttpGetMethod,
                Relations = new[] { "collection" }
            };

        [JsonPropertyName("href"), JsonPropertyOrder(-4)]
        public string Href { get; set; }


        [JsonPropertyName("rel"), JsonPropertyOrder(-3)]
        public string[] Relations { get; set; }

        [JsonPropertyName("method"), JsonPropertyOrder(-2)]
        public string Method { get; set; } = null;

        [JsonIgnore]
        public string RouteName { get; set; }

        [JsonIgnore]
        public object RouteValues { get; set; }
    }
}
