/*  
 *  Developed under MIT Licence for academic purposes.
 *
 *  Author: Fikri Aydemir
 *  Date  :	21/04/2022 
 */

using IonClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SampleController : ControllerBase
    {
        public SampleController()
        {
        }

        [HttpGet(Name = nameof(GetSampleIonResource))]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        public ActionResult<SimpleIonResponse> GetSampleIonResource()
        {
            var sampleIonResponse = new SimpleIonResponse
            {
                SampleProperty = "Sample ION Property",
                Href = Url.Link(nameof(GetSampleIonResource), null),
                Method = Link.HttpGetMethod,
                Success = true
            };

            return sampleIonResponse;
        }

        [HttpGet("{ionResourceId}", Name = nameof(GetSampleIonResourceById))]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        public ActionResult<SimpleIonResponse> GetSampleIonResourceById(int ionResourceId)
        {
            var sampleIonResponse = new SimpleIonResponse
            {
                SampleProperty = "Sample ION Property #" + ionResourceId,
                Href = Url.Link(nameof(GetSampleIonResourceById), new { ionResourceId }),
                Method = Link.HttpGetMethod,
                Success = true
            };

            return sampleIonResponse;
        }

        [HttpGet(Name = nameof(GetSampleIonResourceCollection))]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        public ActionResult<CollectionModel<SimpleIonResponse>> GetSampleIonResourceCollection()
        {
            var sampleResource1 = GetSampleIonResourceById(1).Value;
            sampleResource1.Success = null;

            var sampleResource2 = GetSampleIonResourceById(2).Value;
            sampleResource2.Success = null;

            var sampleResource3 = GetSampleIonResourceById(3).Value;
            sampleResource3.Success = null;

            var theList = new List<SimpleIonResponse>();
            theList.Add(sampleResource1);
            theList.Add(sampleResource2);
            theList.Add(sampleResource3);

            var collection = new CollectionModel<SimpleIonResponse>
            {
                Self = Link.ToCollection(nameof(GetSampleIonResourceCollection)),
                Value = theList.ToArray()
            };

            return collection;
        }

        [HttpGet(Name = nameof(GetSampleComplexIonResource))]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        public ActionResult<ComplexIonResource> GetSampleComplexIonResource()
        {
            var simpleResponse = GetSampleIonResourceById(1).Value;
            simpleResponse.Success = null;
            var collec = GetSampleIonResourceCollection().Value;

            var complexIonResponse = new ComplexIonResource
            {
                SimpleResource = simpleResponse,
                SampleColllection = collec,
                Href = Link.To(nameof(GetSampleComplexIonResource)).Href,
                Success = true
            };

            return complexIonResponse;
        }
    }
}
