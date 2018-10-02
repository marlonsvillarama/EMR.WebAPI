using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMR.WebAPI.ehr.models;

namespace EMR.WebAPI.ehr.services
{
    public class BatchServiceController : ApiController
    {
        [HttpGet]
        [Route("api/getBatches/{dbname}")]
        public IHttpActionResult GetBatches(string dbname)
        {
            ServiceRequestStatus status;
            List<BatchViewModel> vmList = new List<BatchViewModel>();

            try
            {
                EHRDB db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                List<Batch> batches = db.Batches.ToList();

                foreach (Batch b in batches)
                {
                    vmList.Add(new BatchViewModel(b));
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
        [Route("api/getBatchById/{dbname}/{id}")]
        public IHttpActionResult GetBatchById(string dbname, int id)
        {
            ServiceRequestStatus status;
            Batch batch = new Batch();

            try
            {
                if (id > 0)
                {
                    EHRDB db = new EHRDB();
                    db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                    batch = db.Batches.Find(id);
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = batch
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

        [HttpGet]
        [Route("api/getBatchClaims/{dbname}/{id}")]
        public IHttpActionResult GetBatchClaims(string dbname, int id)
        {
            ServiceRequestStatus status;
            List<ClaimViewModel> claims = new List<ClaimViewModel>();

            try
            {
                if (id > 0)
                {
                    EHRDB db = new EHRDB();
                    db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                    Batch b = db.Batches.Find(id);

                    if (b != null)
                    {
                        string cIds = b.ClaimIDs;
                        string[] claimIds = cIds.Split(new[] { ',' });

                        for (int i = 0, n = claimIds.Length; i < n; i++)
                        {
                            if (String.IsNullOrEmpty(claimIds[i]) == false)
                            {
                                int claimId = int.Parse(claimIds[i]);
                                Claim c = db.Claims.Find(claimId);
                                claims.Add(new ClaimViewModel(c));
                            }
                        }
                    }
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = claims
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
        [Route("api/updateBatch/{dbname}")]
        public IHttpActionResult UpdateBatch(Batch batch, string dbname)
        {
            ServiceRequestStatus status;
            Batch b;

            try
            {
                EHRDB db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                if (batch.Id > 0)
                {
                    b = db.Batches.Find(batch.Id);
                }
                else
                {
                    b = new Batch
                    {
                        DateCreated = DateTime.Now,
                        SystemNoteKey = System.Guid.NewGuid().ToString()
                    };
                }

                b.ClaimIDs = batch.ClaimIDs;

                if (b.Id <= 0)
                {
                    b.CreatedById = batch.CreatedById;
                    b.Identifier = DateTime.Now.Ticks.ToString();
                    db.Batches.Add(b);
                }

                db.SaveChanges();
                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = b.Id
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