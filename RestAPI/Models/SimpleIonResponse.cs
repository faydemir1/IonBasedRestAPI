/*  
 *  Developed under MIT Licence for academic purposes.
 *
 *  Author: Fikri Aydemir
 *  Date  :	21/04/2022 
 */

using IonClassLibrary.Models;

namespace RestAPI.Models
{
    public class SimpleIonResponse : IonResponseBaseModel
    {
        public string SampleProperty { get; set; }
    }
}
