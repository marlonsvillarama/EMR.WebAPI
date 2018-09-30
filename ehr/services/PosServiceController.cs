using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMR.WebAPI.ehr.models;

namespace EMR.WebAPI.ehr.services
{
    public class PosServiceController : ApiController
    {
        [HttpGet]
        [Route("~/api/getPlacesOfService")]
        public IHttpActionResult GetPOS()
        {
            ServiceRequestStatus status;
            try
            {
                List<PosViewModel> places = new List<PosViewModel>();

                EHRDB db = new EHRDB();
                var results = db.PlaceOfServices.ToList();
                foreach (var r in results)
                {
                    places.Add(new PosViewModel(r));
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = places
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
        [Route("api/getPlaceOfServiceById/{id}")]
        public IHttpActionResult GetPOSById(int id)
        {
            ServiceRequestStatus status;

            try
            {
                PosViewModel pos = new PosViewModel();
                if (id > 0)
                {
                    EHRDB db = new EHRDB();
                    var result = db.PlaceOfServices.Find(id);
                    if (result != null)
                    {
                        pos = new PosViewModel(result);
                    }
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = pos
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

        [HttpPost]
        [Route("api/updatePos")]
        public IHttpActionResult UpdatePOS(PosViewModel vm)
        {
            ServiceRequestStatus status;
            PlaceOfService p;

            try
            {
                EHRDB db = new EHRDB();
                if (vm.PlaceOfService.Id > 0)
                {
                    p = db.PlaceOfServices.Find(vm.PlaceOfService.Id);
                    if (vm.Action.ToUpper() == "DELETE")
                    {
                        db.PlaceOfServices.Remove(p);
                        db.SaveChanges();
                        status = new ServiceRequestStatus
                        {
                            IsSuccess = true,
                            Data = "OK"
                        };
                        return Ok(new { result = status });
                    }
                }
                else
                {
                    p = new PlaceOfService
                    {
                        SystemNoteKey = System.Guid.NewGuid().ToString(),
                    };
                }

                p.Name = vm.PlaceOfService.Name;
                p.Code = vm.PlaceOfService.Code;
                p.Description = vm.PlaceOfService.Description;

                if (p.Id <= 0)
                {
                    db.PlaceOfServices.Add(p);
                }

                db.SaveChanges();
                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = p.Id
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
        [Route("api/services/deletePOS/{id}")]
        public IHttpActionResult DeletePOS(int id)
        {
            ServiceRequestStatus status;
            try
            {
                PlaceOfService p;
                EHRDB db = new EHRDB();
                p = db.PlaceOfServices.Find(id);
                db.PlaceOfServices.Remove(p);
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