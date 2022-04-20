/*  
 *  Developed under MIT Licence for academic purposes.
 *
 *  Author: Fikri Aydemir
 *  Date  :	21/04/2022 
 */

using IonClassLibrary.Models;

namespace RestAPI.Models
{
    public class ComplexIonResource : IonResponseBaseModel
    {
        public SimpleIonResponse SimpleResource { get; set; }
        public CollectionModel<SimpleIonResponse> SampleColllection { get; set; }
    }
}
