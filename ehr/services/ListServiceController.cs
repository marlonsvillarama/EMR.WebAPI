using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMR.WebAPI.ehr.models;

namespace EMR.WebAPI.ehr.services
{
    public class ListServiceController : ApiController
    {
        // GET
        [Route("api/lists/{type}")]
        public IHttpActionResult GetList(string type)
        {
            ServiceRequestStatus status;
            bool isSuccess = true;
            object results = null;

            if (String.IsNullOrEmpty(type) == true)
            {
                isSuccess = false;
                results = null;
            }
            else
            {
                switch (type.ToLower())
                {
                    case "usstates":
                        results = USState.AllStates;
                        break;
                    case "patrels":
                        results = GetPatientRelationships();
                        break;
                    case "icdcodes":
                        results = GetICDCodes();
                        break;
                    case "cptcodes":
                        results = GetCPTCodes();
                        break;
                    case "taxonomy":
                        results = GetTaxonomyCodes();
                        break;
                    case "errorcodes":
                        results = GetErrorCodes();
                        break;
                    default:
                        break;
                }
            }

            status = new ServiceRequestStatus
            {
                IsSuccess = isSuccess,
                Data = results
            };

            return Ok(new { results = status });
        }

        private Dictionary<string, string> GetPatientRelationships()
        {
            return new Dictionary<string, string>
            {
                { "01", "Spouse" },
                { "18", "Self" },
                { "19", "Child" },
                { "20", "Employee" },
                { "21", "Unknown" },
                { "39", "Organ Donor" },
                { "40", "Cadaver Donor" },
                { "53", "Life Partner" },
                { "G8", "Other Relationship" }
            };
        }

        private Dictionary<string, string> GetICDCodes()
        {
            Dictionary<string, string> icdCodes = new Dictionary<string, string>();
            EHRDB db = new EHRDB();
            List<ICDCode> codes = db.ICDCodes.ToList();

            foreach (ICDCode c in codes)
            {
                icdCodes[c.Code] = c.Description;
            }

            return icdCodes;
        }

        private Dictionary<string, string> GetCPTCodes()
        {
            Dictionary<string, string> cptCodes = new Dictionary<string, string>();
            EHRDB db = new EHRDB();
            List<CPTCode> codes = db.CPTCodes.ToList();

            foreach (CPTCode c in codes)
            {
                cptCodes[c.Code] = c.Description;
            }

            return cptCodes;
        }

        private Dictionary<string, string> GetTaxonomyCodes()
        {
            Dictionary<string, string> taxonomyCodes = new Dictionary<string, string>();
            EHRDB db = new EHRDB();
            List<TaxonomyCode> codes = db.TaxonomyCodes.Where(x => x.Specialization != null).ToList();

            foreach (TaxonomyCode c in codes)
            {
                taxonomyCodes[c.Id.ToString()] = c.Code + "|" + c.Classification + "|" + c.Specialization;
            }

            return taxonomyCodes;
        }

        private Dictionary<string, string> GetErrorCodes()
        {
            Dictionary<string, string> errorCodes = new Dictionary<string, string>();
            EHRDB db = new EHRDB();
            List<ErrorCode> codes = db.ErrorCodes.ToList();

            foreach(ErrorCode c in codes)
            {
                errorCodes[c.Id.ToString()] = c.Description;
            }

            return errorCodes;
        }
    }
}