/*  
 *  Developed under MIT Licence for academic purposes.
 *
 *  Author: Fikri Aydemir
 *  Date  :	21/04/2022 
 */

namespace IonClassLibrary.Models
{
    public class CollectionModel<T> : IonResourceModel
    {
        public T[] Value { get; set; }
    }
}
