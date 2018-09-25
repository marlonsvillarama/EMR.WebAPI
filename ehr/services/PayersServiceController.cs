using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMR.WebAPI.ehr.models;

namespace EMR.WebAPI.ehr.services
{
    public class PayersServiceController : ApiController
    {
        [HttpGet]
        [Route("~/api/getPayers")]
        public IHttpActionResult GetPayers()
        {
            ServiceRequestStatus status;
            try
            {
                EHRDB db = new EHRDB();
                List<Payer> payers = db.Payers.ToList();

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = payers
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
        [Route("~/api/getPayerById/{id}")]
        public IHttpActionResult GetPayerById(int id)
        {
            ServiceRequestStatus status;
            Payer payer = new Payer();

            try
            {
                if (id > 0)
                {
                    EHRDB db = new EHRDB();
                    payer = db.Payers.Find(id);
                }
                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = payer
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
        [Route("api/services/updatePayer")]
        public IHttpActionResult UpdateGroup(PayerViewModel vm)
        {
            ServiceRequestStatus status;
            Payer p;

            try
            {
                EHRDB db = new EHRDB();
                if (vm.Payer.Id > 0)
                {
                    p = db.Payers.Find(vm.Payer.Id);
                    if (vm.Action.ToUpper() == "DELETE")
                    {
                        db.Payers.Remove(p);
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
                    p = new Payer
                    {
                        SystemNoteKey = System.Guid.NewGuid().ToString()
                    };
                }

                p.Name = vm.Payer.Name;
                p.PayerId = vm.Payer.PayerId;
                p.Email = vm.Payer.Email;
                p.Phone_1 = vm.Payer.Phone_1;
                p.Phone_2 = vm.Payer.Phone_2;

                p.Address_1 = vm.Payer.Address_1;
                p.Address_2 = vm.Payer.Address_2;
                p.City = vm.Payer.City;
                p.State = vm.Payer.State;
                p.Zip = vm.Payer.Zip;

                if (p.Id <= 0)
                {
                    db.Payers.Add(p);
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
    }
}
