using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using EMR.WebAPI.ehr.models;

namespace EMR.WebAPI.ehr.services
{
    public class AuthServiceController : ApiController
    {
        private string HashPassword(string pw)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(pw, salt, 10000);

            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];

            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string pwdHash = Convert.ToBase64String(hashBytes);
            return pwdHash;
        }

        private bool VerifyPassword(string pw, string dbpw)
        {
            byte[] hashBytes = Convert.FromBase64String(dbpw);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(pw, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            int ok = 1;
            for (int i=0; i<20; i++)
            {
                if (hashBytes[i + 16] != hash[i]) {
                    ok = 0;
                }
            }

            return (ok == 1);
        }

        [HttpPost]
        [Route("~/api/loginUser")]
        public IHttpActionResult LoginUser(User user)
        {
            ServiceRequestStatus status;
            User u = null;

            try
            {
                EHRDB db = new EHRDB();
                List<User> users = db.Users.ToList();
                users = db.Users.Where(x => x.UserName == user.UserName).ToList();

                bool res;
                if (users.Count > 0)
                {
                    u = users[0];
                    res = VerifyPassword(user.Password, u.Password);
                    u.Password = "< encrypted >";
                }
                else
                {
                    res = false;
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = res,
                    Data = res == true ? u : null
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
        [Route("~/api/getUserById/{id}")]
        public IHttpActionResult GetUserById(int id)
        {
            ServiceRequestStatus status;
            User user = new User();

            try
            {
                if (id > 0)
                {
                    EHRDB db = new EHRDB();
                    //db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
                    user = db.Users.Find(id);
                    user.Salt = "";
                    user.Password = "<encrypted>";
                    user.UserName = "";
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = user
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
        [Route("~/api/hash/{pw}")]
        public IHttpActionResult Hash(string pw)
        {
            ServiceRequestStatus status;

            try
            {
                string pwdHash = HashPassword(pw);
                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = pwdHash
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

        [HttpPost]
        [Route("~/api/updateUser")]
        public IHttpActionResult UpdateUser(User user)
        {
            ServiceRequestStatus status;
            User u;

            try
            {
                EHRDB db = new EHRDB();

                if (user.Id > 0)
                {
                    u = db.Users.Find(user.Id);
                }
                else
                {
                    u = new User
                    {
                        SystemNoteKey = System.Guid.NewGuid().ToString()
                    };
                }

                u.FirstName = user.FirstName;
                u.LastName = user.LastName;
                u.IsInactive = user.IsInactive;
                u.Email = user.Email;
                
                if (u.Id <= 0)
                {
                    u.UserName = user.UserName;

                    byte[] salt;
                    new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                    var pbkdf2 = new Rfc2898DeriveBytes(user.Password, salt, 10000);

                    byte[] hash = pbkdf2.GetBytes(20);
                    byte[] hashBytes = new byte[36];

                    Array.Copy(salt, 0, hashBytes, 0, 16);
                    Array.Copy(hash, 0, hashBytes, 16, 20);

                    string savedPwdHash = Convert.ToBase64String(hashBytes);

                    u.Password = savedPwdHash;
                    db.Users.Add(u);
                }

                db.SaveChanges();
                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = u.Id
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

            return Ok(new { results = status });
        }

        [HttpGet]
        [Route("~/api/changeDB/{dbname}")]
        public IHttpActionResult ChangeDB(string dbname)
        {
            ServiceRequestStatus status;
            List<ProviderViewModel> vmList = new List<ProviderViewModel>(); 

            try
            {
                EHRDB db = new EHRDB();
                List<Provider> providers = db.Providers.Where(x => x.IsCompany == false).ToList();

                foreach (var r in providers)
                {
                    vmList.Add(new ProviderViewModel(r));
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
    }
}