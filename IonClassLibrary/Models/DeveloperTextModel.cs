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
    public class DeveloperTextModel
    {
        [JsonPropertyName("inputValidationErrors")]
        public List<InputValidationErrorModel> InputValidationErrors { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("stackTrace")]
        public string StackTrace { get; set; }


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
