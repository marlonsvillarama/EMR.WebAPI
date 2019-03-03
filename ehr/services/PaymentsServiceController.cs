using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMR.WebAPI.ehr.models;

namespace EMR.WebAPI.ehr.services
{
    public class PaymentsServiceController : ApiController
    {
        [HttpGet]
        [Route("~/api/getPayments/{dbname}")]
        public IHttpActionResult GetPayments(string dbname)
        {
            ServiceRequestStatus status;
            List<PaymentViewModel> vmList = new List<PaymentViewModel>();

            try
            {
                EHRDB db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                List<Payment> payments = db.Payments.ToList();

                foreach (Payment pmt in payments)
                {
                    vmList.Add(new PaymentViewModel(pmt));
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
        [Route("~/api/getPaymentById/{dbname}/{id}")]
        public IHttpActionResult GetPaymentById(string dbname, int id)
        {
            ServiceRequestStatus status;
            Payment payment = new Payment();

            try
            {
                if (id > 0)
                {
                    EHRDB db = new EHRDB();
                    db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                    payment = db.Payments.Find(id);
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = payment
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
        [Route("~/api/getPaymentByClaimId/{dbname}/{claimId}")]
        public IHttpActionResult GetPaymentByClaimId(string dbname, int claimId)
        {
            ServiceRequestStatus status;
            Payment payment = new Payment();

            try
            {
                if (claimId > 0)
                {
                    EHRDB db = new EHRDB();
                    db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                    Claim clm = db.Claims.Find(claimId);
                    payment = clm.Payment;

                    if (payment == null)
                    {
                        payment = new Payment();
                    }
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = payment
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
        [Route("~/api/getNewPayment")]
        public IHttpActionResult GetNewPayment()
        {
            ServiceRequestStatus status;

            try
            {
                Payment payment = new Payment();

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = payment
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
        [Route("api/updatePayment/{dbname}/{claimId}")]
        public IHttpActionResult UpdatePayment(Payment payment, string dbname, int claimId)
        {
            ServiceRequestStatus status;
            Payment p;

            try
            {
                EHRDB db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);

                Claim claim = db.Claims.Find(claimId);
                if (claim.Payment == null)
                {
                    p = new Payment
                    {
                        SystemNoteKey = System.Guid.NewGuid().ToString(),
                        DateCreated = DateTime.Now
                    };
                    claim.Payment = p;
                }
                else
                {
                    p = db.Payments.Find(claim.Payment.Id);
                }

                p.Notes = payment.Notes;
                if (payment.ErrorCode != null)
                {
                    p.ErrorCode = db.ErrorCodes.Find(payment.ErrorCode.Id);
                }

                PaymentLine[] lines1, lines2;
                lines1 = p.PaymentLines.ToArray();
                lines2 = payment.PaymentLines.ToArray();

                //PaymentLine l1, l2;
                PaymentLine[] foundLines;

                // Update the existing payment lines
                for (int i = 0, n = p.PaymentLines.Count; i < n; i++)
                {
                    foundLines = payment.PaymentLines.Where(x => x.Id == lines1[i].Id).ToArray();

                    if (foundLines.Length > 0)
                    {
                        // If the payment line exists, update it
                        lines1[i].ClaimLineId = foundLines[0].ClaimLineId;
                        lines1[i].AmountPayment = foundLines[0].AmountPayment;
                        lines1[i].AmountCopay = foundLines[0].AmountCopay;
                        lines1[i].AmountDeductible = foundLines[0].AmountDeductible;
                    }
                    else
                    {
                        // Otherwise, set its ClaimId to null
                        p.PaymentLines.ToArray()[i].ClaimLineId = null;
                    }
                }

                // Add any new lines to the claim
                List<PaymentLine> newLines = payment.PaymentLines.Where(x => x.Id <= 0).ToList();
                foreach (PaymentLine l in newLines)
                {
                    //l.ClaimId = c.Id;
                    l.SystemNoteKey = System.Guid.NewGuid().ToString();
                    l.DateCreated = p.DateCreated;
                    p.PaymentLines.Add(l);
                }

                decimal totalPmt = 0, totalCopay = 0, totalDeduct = 0, total = 0, balance = 0;
                //for (int i = 0, n = p.PaymentLines.Count; i < n; i++)
                foreach (PaymentLine pl in p.PaymentLines)
                {
                    //PaymentLine pl = p.PaymentLines[i];
                    totalPmt += pl.AmountPayment.Value;
                    totalCopay += pl.AmountCopay.Value;
                    totalDeduct += pl.AmountDeductible.Value;
                    total = totalPmt + totalCopay + totalDeduct;
                    balance = claim.AmountTotal.Value - total;
                }

                p.AmountPayment = totalPmt;
                p.AmountCopay = totalCopay;
                p.AmountDeductible = totalDeduct;
                p.AmountTotal = total;
                p.AmountBalance = balance;

                if (p.Id <= 0)
                {
                    //p.Id = 1;
                    db.Payments.Add(p);
                }

                db.SaveChanges();

                // Retrieve claim history
                List<ClaimHistoryViewModel> vmList = new List<ClaimHistoryViewModel>();
                List<Claim> claims = new List<Claim>();
                claims = db.Claims.Where(c => c.PrimarySubscriberId == claim.PrimarySubscriberId)
                    .OrderByDescending(c => c.DateOfService)
                    .ToList();

                foreach (Claim c in claims)
                {
                    vmList.Add(new ClaimHistoryViewModel(c));
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = p.Id,
                    Data2 = vmList
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