/*  
 *  Developed under MIT Licence for academic purposes.
 *
 *  Author: Fikri Aydemir
 *  Date  :	21/04/2022 
 */

using System.Text.Json.Serialization;

namespace IonClassLibrary.Models
{
    public abstract class IonResourceModel : Link
    {
        [JsonIgnore]
        public Link Self { get; set; }
    }
}
