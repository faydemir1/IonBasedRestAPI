/*  
 *  Developed under MIT Licence for academic purposes.
 *
 *  Author: Fikri Aydemir
 *  Date  :	21/04/2022 
 */

namespace IonClassLibrary.Models
{
    /// <summary>
    /// Class representing Ion related configuration options.
    /// See, https://ionspec.org/ for reference. 
    /// </summary>
    public class IonOptions
    {
        /// <summary>
        /// IF True, the API response messages that are returned from a BFF are formatted based on the ION specification.
        /// See, https://ionspec.org/ for reference. 
        /// </summary>
        public bool EnableIon { get; set; } = false;

        /// <summary>
        /// Represents APIGateway's (i.e. Kong's) base address.
        /// </summary>
        public string ApiGatewayBaseUrl { get; set; } = "https://kong.k8sdev.kuveytturk.com.tr";

        /// <summary>
        /// Represents name of the controller class in BFF.
        /// </summary>
        public string RouteName { get; set; } = "sample";
    }
}
