/*  
 *  Developed under MIT Licence for academic purposes.
 *
 *  Author: Fikri Aydemir
 *  Date  :	21/04/2022 
 */

using System.Text.Json;
using System.Text.Json.Serialization;

namespace IonClassLibrary.Models
{
    public class ErrorModel
    {
        private const string STR_MoreInfo = "For more information, please contact with the enterprise architecture team at Kuveyt Türk.";

        [JsonPropertyName("correlationId"), JsonPropertyOrder(3)]
        public string CorrelationId { get; set; }

        [JsonPropertyName("code"), JsonPropertyOrder(-5)]
        public string Code { get; set; }

        [JsonPropertyName("http"), JsonPropertyOrder(-4)]
        public int? Http { get; set; }

        [JsonPropertyName("text"), JsonPropertyOrder(-3)]
        public string Text { get; set; }

        [JsonPropertyName("developerText"), JsonPropertyOrder(1)]
        public DeveloperTextModel DeveloperText { get; set; }

        [JsonPropertyName("moreInfo"), JsonPropertyOrder(2)]
        public string MoreInfo { get; set; } = STR_MoreInfo;

        public string ToJson()
        {
            var serializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            return JsonSerializer.Serialize(this, serializerOptions);
        }
    }
}
