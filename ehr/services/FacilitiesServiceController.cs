using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using EMR.WebAPI.ehr.models;
using Newtonsoft.Json;

namespace EMR.WebAPI.ehr.services
{
    public class FacilitiesServiceController : ApiController
    {
        [HttpGet]
        [Route("~/api/getFacilities/{dbname}")]
        public IHttpActionResult GetFacilities(string dbname)
        {
            ServiceRequestStatus status;
            List<FacilityViewModel> vmList = new List<FacilityViewModel>();

            try
            {
                EHRDB db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                List<Facility> facilities = db.Facilities.ToList();

                foreach(Facility f in facilities)
                {
                    vmList.Add(new FacilityViewModel(f));
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = vmList
                };
            }
            catch (Exception e)
            {
                status = new ServiceRequestStatus
                {
                    IsSuccess = false,
                    Data = e
                };
            }

            return Ok(new { results = status });
        }

        [HttpGet]
        [Route("~/api/getFacilityById/{dbname}/{id}")]
        public IHttpActionResult GetFacilityById(string dbname, int id)
        {
            ServiceRequestStatus status;
            Facility facility = new Facility();

            try
            {
                if (id > 0)
                {
                    EHRDB db = new EHRDB();
                    db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                    facility = db.Facilities.Find(id);
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = facility
                };
            }
            catch(Exception e)
            {
                status = new ServiceRequestStatus
                {
                    IsSuccess = false,
                    Data = e
                };
            }

            return Ok(new { result = status });
        }

        [HttpPost]
        [Route("~/api/updateFacility/{dbname}")]
        public IHttpActionResult UpdateFacility(Facility facility, string dbname)
        {
            ServiceRequestStatus status;
            Facility f;

            try
            {
                EHRDB db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                if (facility.Id > 0)
                {
                    f = db.Facilities.Find(facility.Id);
                }
                else
                {
                    f = new Facility()
                    {
                        SystemNoteKey = System.Guid.NewGuid().ToString()
                    };
                }

                f.Name = facility.Name;
                f.Email = facility.Email;
                f.Phone = facility.Phone;
                f.Fax = facility.Fax;

                f.Address_1 = facility.Address_1;
                f.Address_2 = facility.Address_2;
                f.City = facility.City;
                f.State = facility.State;
                f.Zip = facility.Zip;

                f.NPI = facility.NPI;
                f.TaxId = facility.TaxId;

                if (f.Id <= 0)
                {
                    db.Facilities.Add(f);
                }

                db.SaveChanges();
                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = f.Id
                };
            }
            catch (Exception e)
            {
                status = new ServiceRequestStatus
                {
                    IsSuccess = false,
                    Data = e
                };
            }

            return Ok(new { result = status });
        }

        /*
        [Route("api/services/deleteFacility")]
        public IHttpActionResult DeleteFacility(int id)
        {
            ServiceRequestStatus status;
            try
            {
                Facility f;
                EHRDB db = new EHRDB();
                f = db.Facilities.Find(id);
                db.Facilities.Remove(f);
                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = null
                };
            }
            catch (Exception e)
            {
                status = new ServiceRequestStatus
                {
                    IsSuccess = false,
                    Data = e
                };
            }

            return Ok(new { result = status });
        }
        */
    }
}