using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                Dictionary<string, Claim> dictClaims = new Dictionary<string, Claim>();
                ClaimViewModel cvm;
                string fl;

                foreach (Claim c in claims)
                {
                    cvm = new ClaimViewModel(c);
                    fl = cvm.FirstName + " " + cvm.LastName;

                    if (dictClaims.ContainsKey(fl))
                    {
                        if (dictClaims[fl].Id > c.Id)
                        {
                            dictClaims[fl] = c;
                        }
                    }
                    else
                    {
                        dictClaims.Add(fl, c);
                    }
                }

                foreach (string key in dictClaims.Keys)
                {
                    vmList.Add(new ClaimViewModel(dictClaims[key]));
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

                foreach (ClaimLine line in claim.ClaimLines)
                {
                    if (line.Supplemental == null)
                    {
                        line.Supplemental = new ClaimLineSupplemental();
                    }

                    if (line.Drug == null)
                    {
                        line.Drug = new ClaimLineDrug();
                    }
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

        public List<ClaimHistoryViewModel> GetClaimHistoryById(string dbname, int id)
        {
            List<ClaimHistoryViewModel> vmList = new List<ClaimHistoryViewModel>();

            EHRDB db = new EHRDB();
            db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
            List<Claim> claims = new List<Claim>();
            claims = db.Claims.Where(c => c.PrimarySubscriberId == id)
                .OrderByDescending(c => c.DateOfService)
                .ToList();

            foreach (Claim c in claims)
            {
                vmList.Add(new ClaimHistoryViewModel(c));
            }

            return vmList;
        }

        [HttpGet]
        [Route("api/getClaimsBySubscriberId/{dbname}/{id}")]
        public IHttpActionResult GetClaimsBySubsciberId(string dbname, int id)
        {
            ServiceRequestStatus status;
            //List<ClaimHistoryViewModel> vmList = new List<ClaimHistoryViewModel>();

            try
            {
                /*
                EHRDB db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                List<Claim> claims = new List<Claim>();
                claims = db.Claims.Where(c => c.PrimarySubscriberId == id)
                    .OrderByDescending(c => c.DateOfService)
                    .ToList();

                foreach (Claim c in claims)
                {
                    vmList.Add(new ClaimHistoryViewModel(c));
                }
                */

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = GetClaimHistoryById(dbname, id)
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
        [Route("api/getBalanceBySubId/{dbname}/{id}")]
        public IHttpActionResult GetBalanceBySubId(string dbname, int id)
        {
            ServiceRequestStatus status;
            List<ClaimPaymentViewModel> vmList = new List<ClaimPaymentViewModel>();

            try
            {
                EHRDB db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                List<Claim> claims = new List<Claim>();
                claims = db.Claims.Where(c => c.PrimarySubscriberId == id)
                    .OrderByDescending(c => c.DateOfService)
                    .ToList();

                foreach (Claim c in claims)
                {
                    vmList.Add(new ClaimPaymentViewModel(c));
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

        [HttpGet]
        [Route("~/api/addClaimToBatch/{dbname}/{userId}/{claimId}/{batchId}")]
        public IHttpActionResult AddClaimToBatch(string dbname, int userId, int claimId, int batchId)
        {
            ServiceRequestStatus status;

            try
            {
                EHRDB db = new EHRDB();
                DateTime dt = new DateTime();

                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                Batch batch;

                if (batchId > 0)
                {
                    batch = db.Batches.Find(batchId);
                    List<string> ids = batch.ClaimIDs.Split(new[] { ',' }).ToList();
                    ids.Add(claimId.ToString());
                    batch.ClaimIDs = String.Join(",", ids.ToArray());
                }
                else
                {
                    batch = new Batch
                    {
                        CreatedById = userId,
                        DateCreated = DateTime.Now,
                        SystemNoteKey = System.Guid.NewGuid().ToString()
                    };

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

                #region Body Fields
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
                c.ReferringProviderId = claim.ReferringProviderId;
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
                #endregion

                #region Claim Lines
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

                        #region Update claim line drug info
                        ClaimLineDrug drug, dr;
                        dr = foundLines[0].Drug;

                        if (lines1[i].Drug == null)
                        {
                            if (String.IsNullOrEmpty(dr.Code) == false ||
                                String.IsNullOrEmpty(dr.Qualifier) == false ||
                                String.IsNullOrEmpty(dr.Unit) == false)
                            {
                                drug = new ClaimLineDrug
                                {
                                    //ClaimLine = lines1[i],
                                    Code = dr.Code,
                                    Qualifier = dr.Qualifier,
                                    Quantity = dr.Quantity,
                                    Unit = dr.Unit
                                };
                                //db.ClaimLineDrugs.Add(drug);
                                //db.SaveChanges();

                                lines1[i].Drug = drug;
                            }
                        }
                        else
                        {
                            lines1[i].Drug.Code = dr.Code;
                            lines1[i].Drug.Qualifier = dr.Qualifier;
                            lines1[i].Drug.Quantity = dr.Quantity;
                            lines1[i].Drug.Unit = dr.Unit;
                        }
                        #endregion

                        #region Supplemental Info
                        ClaimLineSupplemental supp, su;
                        su = foundLines[0].Supplemental;

                        if (lines1[i].Supplemental == null)
                        {
                            if (String.IsNullOrEmpty(su.NoteDescription) == false)
                            {
                                supp = new ClaimLineSupplemental
                                {
                                    //ClaimLine = lines1[i],
                                    NoteDescription = su.NoteDescription
                                };
                                //db.ClaimLineSupplementals.Add(supp);
                                //db.SaveChanges();

                                lines1[i].Supplemental = supp;
                            }
                        }
                        else
                        {
                            lines1[i].Supplemental.NoteDescription = su.NoteDescription;
                        }
                        #endregion


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
                #endregion

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
            int resultsCount = 0;

            try
            {
                if (String.IsNullOrEmpty(parms) == false)
                {
                    EHRDB db = new EHRDB();
                    db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);

                    List<Claim> claims = new List<Claim>();
                    SearchFilter filters = new SearchFilter(parms);

                    // Filter by Last Name
                    if (String.IsNullOrEmpty(filters.LastName) == true)
                    {
                        claims = db.Claims.ToList();
                    }
                    else
                    {
                        claims = db.Claims.Where(c =>
                                    c.PrimarySubscriber.LastName.StartsWith(filters.LastName)).ToList();
                    }

                    // Look at aging report first
                    if (filters.Type == "CLAIMS_PAYMENT")
                    {
                        claims = claims.Where(c => c.Payment != null && c.Payment.AmountBalance > 0).ToList();
                    }
                    else if (filters.Type == "CLAIMS_AGING_SUMMARY")
                    {
                        if (String.IsNullOrEmpty(filters.AgingPeriod) == false)
                        {
                            claims = claims.Where(c => c.Payment != null && c.Payment.AmountBalance > 0).ToList();

                            switch (filters.AgingPeriod)
                            {
                                case "0 TO 30":
                                    {
                                        claims = claims.Where(c =>
                                            ((DateTime.Today - c.DateCreated.Value).TotalDays <= 30)).ToList();
                                        break;
                                    }
                                case "31 TO 60":
                                    {
                                        claims = claims.Where(c =>
                                            ((DateTime.Today - c.DateCreated.Value).TotalDays > 30) &&
                                            ((DateTime.Today - c.DateCreated.Value).TotalDays <= 60)).ToList();
                                        break;
                                    }
                                case "61 TO 90":
                                    {
                                        claims = claims.Where(c =>
                                            ((DateTime.Today - c.DateCreated.Value).TotalDays > 60) &&
                                            ((DateTime.Today - c.DateCreated.Value).TotalDays <= 90)).ToList();
                                        break;
                                    }
                                case "OVER 90":
                                    {
                                        claims = claims.Where(c =>
                                            ((DateTime.Today - c.DateCreated.Value).TotalDays > 90)).ToList();
                                        break;
                                    }
                                default: break;
                            }
                        }
                    }

                    // Filter by Date of Birth
                    if (filters.DateOfBirth != null)
                    {
                        claims = claims.Where(c =>
                            filters.DateOfBirth.Value <= c.PrimarySubscriber.DateOfBirth &&
                            c.PrimarySubscriber.DateOfBirth < filters.DateOfBirth.Value.AddDays(1)).ToList();
                    }

                    // Filter by Date Created
                    if (filters.DateCreated != null)
                    {
                        claims = claims.Where(c =>
                            filters.DateCreated.Value <= c.DateCreated &&
                            c.DateCreated < filters.DateCreated.Value.AddDays(1)).ToList();
                    }

                    // Filters by Date of Service
                    /*
                    if (filters.DateOfService != null)
                    {
                        claims = claims.Where(c =>
                            filters.DateOfService.Value <= c.DateOfService &&
                            c.DateOfService < filters.DateOfService.Value.AddDays(1)).ToList();
                    }
                    */

                    // Filter by Month/Year
                    if (filters.MonthStartEnd.Count > 0)
                    {
                        if (filters.MonthStartEnd[0] != null &&
                            filters.MonthStartEnd[1] != null)
                        {
                            claims = claims.Where(c =>
                                filters.MonthStartEnd[0].Value <= c.DateCreated.Value &&
                                filters.MonthStartEnd[1].Value >= c.DateCreated.Value).ToList();
                        }
                        else if (filters.MonthStartEnd[0] != null)
                        {
                            claims = claims.Where(c =>
                                filters.MonthStartEnd[0].Value <= c.DateCreated.Value).ToList();
                        }
                        else if (filters.MonthStartEnd[1] != null)
                        {
                            claims = claims.Where(c =>
                                filters.MonthStartEnd[1].Value >= c.DateCreated.Value).ToList();
                        }
                    }

                    // Filter by Rendering Provider
                    if (filters.ProviderId > 0)
                    {
                        claims = claims.Where(c =>
                            filters.ProviderId == c.RenderingProviderId).ToList();
                    }

                    // Filter by Facility
                    if (filters.FacilityId > 0)
                    {
                        claims = claims.Where(c =>
                            filters.FacilityId == c.FacilityId).ToList();
                    }

                    if (string.IsNullOrEmpty(filters.CPT) == false)
                    {
                        claims = claims.Where(c =>
                            c.DiagnosisCodes.Contains(filters.CPT) == true).ToList();
                    }

                    // Filter by Deductible
                    if (filters.Deductible > 0)
                    {
                        claims = claims.Where(c =>
                            c.Payment != null && c.Payment.AmountDeductible > 0).ToList();
                    }

                    // Filter by Copay
                    if (filters.Copay > 0)
                    {
                        claims = claims.Where(c => c.Payment != null &&
                            c.Payment.AmountCopay > 0).ToList();
                    }

                    claims = claims.OrderBy(c => c.PrimarySubscriber.LastName.ToUpper())
                                .ThenBy(c => c.PrimarySubscriber.FirstName.ToUpper())
                                .ToList();

                    Dictionary<string, Claim> dictClaims = new Dictionary<string, Claim>();
                    List<string> skipped = new List<string>();
                    string fl;
                    int start, end, n;
                    Claim cl;

                    if (filters.Type.ToUpper() == "CLAIMS")
                    {
                        List<string> claimSubs = claims
                            .Select(c => (c.PrimarySubscriber.LastName + ", " + c.PrimarySubscriber.FirstName))
                            .Distinct()
                            .ToList();
                        resultsCount = claimSubs.Count();

                        start = (filters.PageNumber - 1) * filters.PageSize;
                        end = (start + filters.PageSize) < resultsCount ? (start + filters.PageSize) : resultsCount;
                        n = 0;

                        for (int i = 0; i < claims.Count && n < end; i++)
                        {
                            cl = claims[i];
                            fl = cl.PrimarySubscriber.LastName + ", " + cl.PrimarySubscriber.FirstName;

                            if (dictClaims.ContainsKey(fl))
                            {
                                if (dictClaims[fl].Id < cl.Id)
                                {
                                    dictClaims[fl] = cl;
                                }
                            }
                            else
                            {
                                if (skipped.Contains(fl) == false)
                                {
                                    if (n >= start)
                                    {
                                        dictClaims.Add(fl, cl);
                                    }
                                    skipped.Add(fl);
                                    n++;
                                }
                            }
                        }
                    }
                    else
                    {
                        resultsCount = claims.Count();
                        start = (filters.PageNumber - 1) * filters.PageSize;
                        end = (start + filters.PageSize) < resultsCount ? (start + filters.PageSize) : resultsCount;
                        n = 0;

                        for (int i = 0; i < claims.Count && n < end; i++)
                        {
                            if (n >= start)
                            {
                                cl = claims[i];
                                dictClaims.Add(cl.Id.ToString(), cl);
                            }
                            n++;
                        }
                    }

                    foreach (string key in dictClaims.Keys)
                    {
                        vmList.Add(new ClaimViewModel(dictClaims[key]));
                    }
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Count = resultsCount,
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

        [HttpGet]
        [Route("api/claimReport/{dbname}/{parms}")]
        public IHttpActionResult GetClaimReport(string dbname, string parms)
        {
            ServiceRequestStatus status = new ServiceRequestStatus();
            List<ClaimSummary> csum = new List<ClaimSummary>();

            try
            {
                switch (parms)
                {
                    case "CLAIMS_BYMONTH_SUMMARY":
                        {
                            csum = GetClaimMonthlyReport(dbname);
                            break;
                        }
                    case "CLAIMS_AGING_SUMMARY":
                        {
                            csum = GetClaimAgingReport(dbname);
                            break;
                        }
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = csum
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

        public List<ClaimSummary> GetClaimMonthlyReport(string dbname)
        {
            //ServiceRequestStatus status;
            //List<ClaimViewModel> vmList = new List<ClaimViewModel>();
            List<ClaimSummary> csum = new List<ClaimSummary>();

            EHRDB db = new EHRDB();
            db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);

            //string[] vals = parms.Split(new[] { '|' });
            //string type = vals.Length > 0 ? vals[0] : "";

            csum = db.Claims.GroupBy(c => new
            {
                Month = c.DateCreated.Value.Month,
                Year = c.DateCreated.Value.Year
            })
            .Select(g => new ClaimSummary
            {
                Month = g.Key.Month,
                Year = g.Key.Year,
                TotalClaims = g.Count(),
                TotalAmount = g.Sum(n => (Decimal)n.AmountTotal)
            })
            .OrderByDescending(a => a.Year)
            .ThenByDescending(a => a.Month)
            .ToList();

            return csum;
            /*
            try
            {
                if (String.IsNullOrEmpty(parms) == false)
                {
                    EHRDB db = new EHRDB();
                    db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);

                    string[] vals = parms.Split(new[] { '|' });
                    string type = vals.Length > 0 ? vals[0] : "";

                    csum = db.Claims.GroupBy(c => new
                    {
                        Month = c.DateCreated.Value.Month,
                        Year = c.DateCreated.Value.Year
                    })
                    .Select(g => new ClaimSummary
                    {
                        Month = g.Key.Month,
                        Year = g.Key.Year,
                        TotalClaims = g.Count(),
                        TotalAmount = g.Sum(n => (Decimal)n.AmountTotal)
                    })
                    .OrderByDescending(a => a.Year)
                    .ThenByDescending(a => a.Month)
                    .ToList();
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = csum
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
            */

            //return Ok(new { result = status });
        }

        /*
        [HttpGet]
        [Route("api/claimPaymentReport/{dbname}/{parms}")]
        public IHttpActionResult GetClaimPaymentReport(string dbname, string parms)
        {
            ServiceRequestStatus status;
            List<ClaimViewModel> vmList = new List<ClaimViewModel>();
            List<ClaimMonthlySummary> csum = new List<ClaimMonthlySummary>();

            try
            {
                if (String.IsNullOrEmpty(parms) == false)
                {
                    EHRDB db = new EHRDB();
                    db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);

                    List<Claim> claims = new List<Claim>();
                    List<Payment> payments = new List<Payment>();
                    List<Payment> allPayments = new List<Payment>();
                    SearchFilter filters = new SearchFilter(parms);

                    payments = db.Payments.ToList();

                    if (filters.Deductible > 0)
                    {
                        payments.Where(p => p.AmountDeductible >= filters.Deductible).ToList();
                    }

                    if (filters.Copay > 0)
                    {
                        payments.Where(p => p.AmountCopay >= filters.Copay).ToList();
                    }

                    //db.Claims.Where(c => c.Payment.AmountDeductible > 0);
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = csum
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

        public List<ClaimSummary> GetClaimAgingReport(string dbname)
        {
            //ServiceRequestStatus status;
            //List<ClaimViewModel> vmList = new List<ClaimViewModel>();
            List<ClaimSummary> csum = new List<ClaimSummary>();

            EHRDB db = new EHRDB();
            db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);

            List<Claim> openClaims = db.Claims.Where(c => c.Payment != null).ToList();
            openClaims = openClaims.Where(c => c.Payment.AmountBalance > 0).ToList();

            List<Claim> claims_0_30 = openClaims.Where(c =>
                (DateTime.Today - c.DateCreated.Value).TotalDays <= 30)
                .ToList();
            csum.Add(new ClaimSummary
            {
                AgingPeriod = "0 to 30",
                TotalAmount = claims_0_30.Sum(c => c.Payment.AmountBalance),
                TotalClaims = claims_0_30.Count
            });

            List<Claim> claims_30_60 = openClaims.Where(c =>
                (DateTime.Today - c.DateCreated.Value).TotalDays > 30 &&
                (DateTime.Today - c.DateCreated.Value).TotalDays <= 60).ToList();
            csum.Add(new ClaimSummary
            {
                AgingPeriod = "31 to 60",
                TotalAmount = claims_30_60.Sum(c => c.Payment.AmountBalance),
                TotalClaims = claims_30_60.Count
            });

            List<Claim> claims_60_90 = openClaims.Where(c =>
                (DateTime.Today - c.DateCreated.Value).TotalDays > 60 &&
                (DateTime.Today - c.DateCreated.Value).TotalDays <= 90).ToList();
            csum.Add(new ClaimSummary
            {
                AgingPeriod = "61 to 90",
                TotalAmount = claims_60_90.Sum(c => c.Payment.AmountBalance),
                TotalClaims = claims_60_90.Count
            });

            List<Claim> claims_Over_90 = openClaims.Where(c =>
                (DateTime.Today - c.DateCreated.Value).TotalDays > 90).ToList();
            csum.Add(new ClaimSummary
            {
                AgingPeriod = "Over 90",
                TotalAmount = claims_Over_90.Sum(c => c.Payment.AmountBalance),
                TotalClaims = claims_Over_90.Count
            });

            return csum;
            /*
            try
            {
                if (String.IsNullOrEmpty(parms) == false)
                {
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = csum
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
            */
        }
    }
}
