using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using EMR.WebAPI.ehr.models;
using Microsoft.IdentityModel.Tokens;

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

        [HttpGet]
        [Route("~/api/tokenize")]
        public IHttpActionResult Tokenize()
        {
            var claims = new[] { new System.Security.Claims.Claim(ClaimTypes.Name, "username") };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("kjkasdfiuaselfknaliufewaebeiunn"));
            var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: "kurekinect.com",
                audience: "kurekinect.com",
                expires: DateTime.Now.AddMinutes(1),
                claims: claims,
                signingCredentials: signInCred
                );

            return Ok(token);
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

        private UserPreference CopyPreferences(UserPreference prefBase, UserPreference prefCopy)
        {
            if (prefBase.BillingProviderId != null)
            {
                prefCopy.BillingProviderId = prefBase.BillingProviderId;
            }

            if (prefBase.FacilityId != null)
            {
                prefCopy.FacilityId = prefBase.FacilityId;
            }

            if (prefBase.PlaceOfServiceId != null)
            {
                prefCopy.PlaceOfServiceId = prefBase.PlaceOfServiceId;
            }

            if (prefBase.RenderingProviderId != null)
            {
                prefCopy.RenderingProviderId = prefBase.RenderingProviderId;
            }

            return prefCopy;
        }

        [HttpGet]
        [Route("~/api/getUserPrefs/{dbId}/{userId}")]
        public IHttpActionResult GetUserPreferences(int dbId, int userId)
        {
            ServiceRequestStatus status;
            UserPreference pref = new UserPreference();

            try
            {
                EHRDB db = new EHRDB();
                List<UserPreference> prefList = db.UserPreferences.Where(x => x.AccountId == dbId).ToList();

                if (prefList.Count > 0)
                {
                    List<UserPreference> prefGlobal = prefList.Where(x => x.UserId == -1).ToList();

                    if (prefGlobal.Count > 0)
                    {
                        pref = CopyPreferences(prefGlobal[0], new UserPreference());
                        pref.AccountId = dbId;
                        pref.UserId = userId;

                        if (userId > 0)
                        {
                            List<UserPreference> prefUser = prefList.Where(x => x.UserId == userId).ToList();

                            if (prefUser.Count > 0)
                            {
                                pref = CopyPreferences(prefUser[0], pref);
                                pref.Id = prefUser[0].Id;
                                //pref.UserId = userId;
                            }
                        }
                    }
                    else
                    {
                        if (userId > 0)
                        {
                            List<UserPreference> prefUser = prefList.Where(x => x.UserId == userId).ToList();

                            if (prefUser.Count > 0)
                            {
                                pref = CopyPreferences(prefUser[0], pref);
                                pref.Id = prefUser[0].Id;
                                pref.AccountId = dbId;
                                pref.UserId = userId;
                            }
                        }
                    }
                }

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = pref
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

        // NOT USED!!!
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

        [HttpPost]
        [Route("~/api/updateAccount")]
        public IHttpActionResult UpdateAccount(Account account)
        {
            ServiceRequestStatus status;
            Account a;

            try
            {
                EHRDB db = new EHRDB();
                a = db.Accounts.Find(account.Id);

                a.IsInactive = account.IsInactive;

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = a
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
        [Route("~/api/updateUserPrefs/{userId}")]
        public IHttpActionResult UpdateUserPreferences(UserPreference pref, int userId)
        {
            ServiceRequestStatus status;
            UserPreference p;

            try
            {
                EHRDB db = new EHRDB();

                if (pref.Id > 0)
                {
                    p = db.UserPreferences.Find(pref.Id);
                }
                else
                {
                    p = new UserPreference
                    {
                        UserId = userId,
                        AccountId = pref.AccountId,
                        SystemNoteKey = Guid.NewGuid().ToString()
                    };
                }

                p = CopyPreferences(pref, p);

                if (pref.Id <= 0)
                {
                    db.UserPreferences.Add(p);
                }

                db.SaveChanges();

                status = new ServiceRequestStatus
                {
                    IsSuccess = true,
                    Data = p
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