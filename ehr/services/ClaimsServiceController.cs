using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMR.WebAPI.ehr.models;

namespace EMR.WebAPI.ehr.services
{
    public class ClaimsServiceController : ApiController
    {
        [HttpGet]
        [Route("api/getClaims/{dbname}")]
        public IHttpActionResult GetClaims(string dbname)
        {
            ServiceRequestStatus status;
            List<ClaimViewModel> vmList = new List<ClaimViewModel>();

            try
            {
                EHRDB db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                List<Claim> claims = db.Claims.ToList();

                foreach(Claim c in claims)
                {
                    vmList.Add(new ClaimViewModel(c));
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
        [Route("api/getNewClaim")]
        public IHttpActionResult GetNewClaim()
        {
            ServiceRequestStatus status;

            try
            {
                Claim claim = new Claim
                {
                    Patient = new Patient(),
                    Payment = new Payment()
                };

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = claim
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
        [Route("api/getClaimById/{dbname}/{id}")]
        public IHttpActionResult GetClaimById(string dbname, int id)
        {
            ServiceRequestStatus status;
            Claim claim;

            try
            {
                EHRDB db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                claim = db.Claims.Find(id);

                if (claim.Relationship == "18")
                {
                    claim.Patient = new Patient();
                    claim.Patient.FirstName = claim.PrimarySubscriber.FirstName;
                    claim.Patient.MiddleName = claim.PrimarySubscriber.MiddleName;
                    claim.Patient.LastName = claim.PrimarySubscriber.LastName;
                    claim.Patient.DateOfBirth = claim.PrimarySubscriber.DateOfBirth;
                    claim.Patient.Gender = claim.PrimarySubscriber.Gender;
                    claim.Patient.Address_1 = claim.PrimarySubscriber.Address_1;
                    claim.Patient.Address_2 = claim.PrimarySubscriber.Address_2;
                    claim.Patient.City = claim.PrimarySubscriber.City;
                    claim.Patient.State = claim.PrimarySubscriber.State;
                    claim.Patient.Zip = claim.PrimarySubscriber.Zip;
                    claim.Patient.Phone = claim.PrimarySubscriber.Phone_1;
                    claim.Patient.Email = claim.PrimarySubscriber.Email;
                }

                if (claim.Payment == null)
                {
                    claim.Payment = new Payment();
                    claim.Payment.ErrorCode = new ErrorCode();
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = claim
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
        [Route("api/getClaimsBySubscriberId/{dbname}/{id}")]
        public IHttpActionResult GetClaimsBySubsciberId(string dbname, int id)
        {
            ServiceRequestStatus status;
            List<ClaimViewModel> vmList = new List<ClaimViewModel>();

            try
            {
                EHRDB db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                List<Claim> claims = new List<Claim>();
                claims = db.Claims.Where(c => c.PrimarySubscriberId == id).ToList();

                foreach(Claim c in claims)
                {
                    vmList.Add(new ClaimViewModel(c));
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = vmList
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
        [Route("api/updateClaim/{dbname}")]
        public IHttpActionResult UpdateClaim(Claim claim, string dbname)
        {
            ServiceRequestStatus status;
            DateTime dtNow = DateTime.Now;
            Claim c;

            try
            {
                EHRDB db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                if (claim.Id > 0)
                {
                    c = db.Claims.Find(claim.Id);
                }
                else
                {
                    c = new Claim
                    {
                        DateCreated = dtNow,
                        SystemNoteKey = System.Guid.NewGuid().ToString()
                    };
                }

                c.AcceptAssignment = claim.AcceptAssignment;
                c.AmountBalance = claim.AmountBalance;
                c.AmountCopay = claim.AmountCopay;
                c.AmountTotal = claim.AmountTotal;
                c.FilingStatus = claim.FilingStatus;
                c.PrimaryPayerMemberID = claim.PrimaryPayerMemberID;
                c.SecondaryPayerMemberID = claim.SecondaryPayerMemberID;
                c.DiagnosisCodes = claim.DiagnosisCodes;
                c.Relationship = claim.Relationship;
                c.PlaceOfServiceId = claim.PlaceOfServiceId;
                c.PrimarySubscriberId = claim.PrimarySubscriberId;
                c.SecondaryPayerId = claim.SecondaryPayerId;
                c.PatientId = claim.PatientId > 0 ? claim.PatientId : null;
                c.FacilityId = claim.FacilityId;
                c.PrimaryPayerId = claim.PrimaryPayerId;
                c.SecondaryPayerId = claim.SecondaryPayerId;
                c.EmploymentRelated = claim.EmploymentRelated;
                c.OutsideLab = claim.OutsideLab;
                c.OutsideLabCharges = claim.OutsideLabCharges;
                c.DateOfService = claim.DateOfService;
                c.RenderingProviderId = claim.RenderingProviderId;
                c.BillingProviderId = claim.BillingProviderId;
                c.AssignBenefits = claim.AssignBenefits;
                c.AllowRelease = claim.AllowRelease;
                c.HasSignature = claim.HasSignature;
                c.SpecialProgram = claim.SpecialProgram;
                c.DelayReason = claim.DelayReason;
                c.NoteType = claim.NoteType;
                c.Notes = claim.Notes;
                c.HomeBound = claim.HomeBound;
                c.ClaimPayerType = claim.ClaimPayerType;

                ClaimLine[] lines1, lines2;
                lines1 = c.ClaimLines.ToArray();
                lines2 = claim.ClaimLines.ToArray();

                ClaimLine l1, l2;
                ClaimLine[] foundLines;

                // Update the existing claim lines
                for (int i = 0, n = c.ClaimLines.Count; i < n; i++)
                {
                    foundLines = claim.ClaimLines.Where(x => x.Id == lines1[i].Id).ToArray();

                    if (foundLines.Length > 0)
                    {
                        // If the claim line exists, update it
                        lines1[i].Amount = foundLines[0].Amount;
                        lines1[i].CPT = foundLines[0].CPT;
                        lines1[i].EndDate = foundLines[0].EndDate;
                        lines1[i].EPSDT = foundLines[0].EPSDT;
                        lines1[i].IsEmergency = foundLines[0].IsEmergency;
                        lines1[i].Modifier = foundLines[0].Modifier;
                        lines1[i].OrderLine = foundLines[0].OrderLine;
                        lines1[i].Pointer = foundLines[0].Pointer;
                        lines1[i].Quantity = foundLines[0].Quantity;
                        lines1[i].StartDate = foundLines[0].StartDate;
                        lines1[i].Unit = foundLines[0].Unit; 
                    }
                    else
                    {
                        // Otherwise, set its ClaimId to null
                        c.ClaimLines.ToArray()[i].ClaimId = null;
                    }
                }

                // Add any new lines to the claim
                List<ClaimLine> newLines = claim.ClaimLines.Where(x => x.Id <= 0).ToList();
                foreach (ClaimLine l in newLines)
                {
                    //l.ClaimId = c.Id;
                    c.ClaimLines.Add(l);
                }

                if (c.Id <= 0)
                {
                    db.Claims.Add(c);
                }

                db.SaveChanges();
                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = c.Id
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
        [Route("api/searchClaims/{dbname}/{parms}")]
        public IHttpActionResult SearchClaims(string dbname, string parms)
        {
            ServiceRequestStatus status;
            List<ClaimViewModel> vmList = new List<ClaimViewModel>();

            try
            {
                if (String.IsNullOrEmpty(parms) == false)
                {
                    EHRDB db = new EHRDB();
                    db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                    string fn = "", ln = "", dos = "";

                    string[] vals = parms.Split(new[] { '|' });

                    fn = vals.Length > 0 ? vals[0] : "";
                    ln = vals.Length > 1 ? vals[1] : "";
                    dos = vals.Length > 2 ? vals[2] : "";

                    List<Claim> claims = db.Claims.Where(c =>
                                           c.PrimarySubscriber.FirstName.StartsWith(fn) &&
                                           c.PrimarySubscriber.LastName.StartsWith(ln)).ToList();

                    if (String.IsNullOrEmpty(dos) == false)
                    {
                        claims = claims.Where(c => c.DateOfService == DateTime.Parse(dos)).ToList();
                    }

                    foreach (Claim c in claims)
                    {
                        vmList.Add(new ClaimViewModel(c));
                    }
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

            return Ok(new { result = status });
        }
    }
}