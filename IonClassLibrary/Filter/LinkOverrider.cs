/*  
 *  Developed under MIT Licence for academic purposes.
 *
 *  Author: Fikri Aydemir
 *  Date  :	21/04/2022 
 */

using IonClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace IonClassLibrary.Filter
{
    public class LinkOverrider
    {
        private readonly IUrlHelper _urlHelper;

        public LinkOverrider(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public Link Override(Link original)
        {
            if (original == null) return null;
            var newLink = new Link
            {
                Href = _urlHelper.Link(original.RouteName, original.RouteValues),
                Method = original.Method,
                Relations = original.Relations
            };
            return newLink;
        }
    }
}
