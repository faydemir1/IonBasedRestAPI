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
    public class IonResponseBaseModel : IonResourceModel
    {
        [JsonPropertyName("success")]
        public bool? Success { get; set; }

        [JsonPropertyName("error")]
        public ErrorModel? Error { get; set; }


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
