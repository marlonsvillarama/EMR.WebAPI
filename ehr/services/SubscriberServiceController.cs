    using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMR.WebAPI.ehr.models;

namespace EMR.WebAPI.ehr.services
{
    public class SubscriberServiceController : ApiController
    {
        [HttpGet]
        [Route("api/getSubscribers/{dbname}")]
        public IHttpActionResult GetSubscribers(string dbname)
        {
            ServiceRequestStatus status;
            List<SubscriberViewModel> vmList = new List<SubscriberViewModel>();

            try
            {
                EHRDB db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                List<Subscriber> subscribers = db.Subscribers.ToList();

                foreach(Subscriber sub in subscribers)
                {
                    vmList.Add(new SubscriberViewModel(sub));
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
        [Route("api/getSubscriberById/{dbname}/{id}")]
        public IHttpActionResult GetSubscriberById(string dbname, int id)
        {
            ServiceRequestStatus status;
            Subscriber subscriber = new Subscriber();

            try
            {
                if (id > 0)
                {
                    EHRDB db = new EHRDB();
                    db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                    subscriber = db.Subscribers.Find(id);
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = subscriber
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
        [Route("api/getPatientsBySubscriberId/{dbname}/{id}")]
        public IHttpActionResult GetPatientsBySubscriberId(string dbname, int id)
        {
            ServiceRequestStatus status;
            List<DependentViewModel> vmList = new List<DependentViewModel>();

            try
            {
                EHRDB db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                Subscriber sub = db.Subscribers.Find(id);
                List<Patient> patients = new List<Patient>();
                patients = sub.Patients.ToList();

                DependentViewModel vm = new DependentViewModel();
                vm.PatientId = -1;
                vm.FirstName = sub.FirstName;
                vm.LastName = sub.LastName;
                vm.DateOfBirth = sub.DateOfBirth.Value.ToString("M/d/yyyy");
                vm.Gender = sub.Gender;
                vm.Phone = sub.Phone_1;
                vmList.Add(vm);

                patients = patients.OrderBy(x => x.LastName).ToList();
                foreach (Patient p in patients)
                {
                    vm = new DependentViewModel();
                    vm.PatientId = p.Id;
                    vm.FirstName = p.FirstName;
                    vm.LastName = p.LastName;
                    vm.DateOfBirth = p.DateOfBirth.Value.ToString("M/d/yyyy");
                    vm.Gender = p.Gender;
                    vm.Phone = p.Phone;
                    vm.Relationship = p.Relationship;
                    vmList.Add(vm);
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
        [Route("api/updateSubscriber/{dbname}")]
        public IHttpActionResult UpdateSubscriber(Subscriber subscriber, string dbname)
        {
            ServiceRequestStatus status;
            Subscriber s;

            try
            {
                EHRDB db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                if (subscriber.Id > 0)
                {
                    s = db.Subscribers.Find(subscriber.Id);
                }
                else
                {
                    s = new Subscriber
                    {
                        SystemNoteKey = System.Guid.NewGuid().ToString()
                    };
                }

                s.DateOfBirth = subscriber.DateOfBirth;
                s.Email = subscriber.Email;
                s.FirstName = subscriber.FirstName;
                s.Gender = subscriber.Gender;
                s.LastName = subscriber.LastName;
                s.MiddleName     = subscriber.MiddleName;
                s.Notes = subscriber.Notes;
                s.Phone_1 = subscriber.Phone_1;
                s.Phone_2 = subscriber.Phone_2;
                s.Suffix = subscriber.Suffix;

                s.Address_1 = subscriber.Address_1;
                s.Address_2 = subscriber.Address_2;
                s.City = subscriber.City;
                s.State = subscriber.State;
                s.Zip = subscriber.Zip;

                s.PrimaryPayerId = subscriber.PrimaryPayerId;
                s.PrimaryMemberID = subscriber.PrimaryMemberID;

                s.SecondaryPayerId = subscriber.SecondaryPayerId;
                s.SecondaryMemberID = subscriber.SecondaryMemberID;

                /*var payer = db.Payers.Find(vm.Subscriber.PrimaryPayer.Id);
                if (payer != null)
                {
                    s.PrimaryPayer = payer;
                    s.PrimaryMemberID = vm.Subscriber.PrimaryMemberID;
                }

                payer = db.Payers.Find(vm.Subscriber.SecondaryPayer.Id);
                if (payer != null)
                {
                    s.SecondaryPayer = payer;
                    s.SecondaryMemberID = vm.Subscriber.PrimaryMemberID;
                }*/

                if (s.Id <= 0)
                {
                    db.Subscribers.Add(s);
                }

                db.SaveChanges();
                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = s.Id
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
        [Route("api/updateSubscriberDependent/{dbname}/{id}")]
        public IHttpActionResult UpdateSubscriberDependent(DependentViewModel vm, string dbname, int id)
        {
            ServiceRequestStatus status;

            try
            {
                EHRDB db = new EHRDB();
                db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                Subscriber sub = db.Subscribers.Find(vm.SubscriberId);

                if (sub == null)
                {
                    status = new ServiceRequestStatus
                    {
                        IsSuccess = false,
                        Data = "Subscriber not found"
                    };
                }
                else
                {
                    Patient p;
                    if (vm.PatientId > 0)
                    {
                        p = db.Patients.Find(vm.PatientId);
                    }
                    else
                    {
                        p = new Patient();
                    }

                    if (p == null)
                    {
                        status = new ServiceRequestStatus
                        {
                            IsSuccess = false,
                            Data = "Patient is NULL"
                        };
                    }
                    else
                    {
                        p.FirstName = vm.FirstName;
                        p.LastName = vm.LastName;
                        p.DateOfBirth = DateTime.Parse(vm.DateOfBirth);
                        p.Gender = vm.Gender;
                        p.Relationship = vm.Relationship;

                        if (vm.PatientId <= 0)
                        {
                            p = db.Patients.Add(p);
                            sub.Patients.Add(p);
                        }

                        db.SaveChanges();

                        status = new ServiceRequestStatus
                        {
                            IsSuccess = true,
                            Data = p.Id
                        };
                    }
                }
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


        /*
        [Route("api/services/deleteSubscriber/{id}")]
        public IHttpActionResult DeleteSubscriber(int id)
        {
            ServiceRequestStatus status;
            try
            {
                Subscriber s;
                EHRDB db = new EHRDB();
                s = db.Subscribers.Find(id);
                db.Subscribers.Remove(s);
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