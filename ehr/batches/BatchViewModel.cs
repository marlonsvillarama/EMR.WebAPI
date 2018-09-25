using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMR.WebAPI.ehr.models
{
    public class BatchViewModel
    {
        private Batch b;

        public BatchViewModel(Batch batch)
        {
            b = batch;
        }

        #region Properties
        public int Id
        {
            get => b.Id;
        }

        public string Identifier
        {
            get => b.Identifier;
        }

        public string DateCreated
        {
            get => b.DateCreated.Value.ToString("MMM d, yyyy");
        }

        public string TimeCreated
        {
            get => b.DateCreated.Value.ToString("h:mm:ss tt");
        }

        public string CreatedBy
        {
            get
            {
                string cby = "";
                EHRDB db = new EHRDB();
                User user = db.Users.Find(b.CreatedById);

                if (user == null)
                {
                    cby = "< Unknown >";
                }
                else
                {
                    cby = String.Format("{0}, {1}", user.LastName, user.FirstName);
                }

                return cby;
            }
        }

        public string ClaimIds
        {
            get => b.ClaimIDs;
        }

        public string Status
        {
            get => b.Status;
        }
        #endregion
    }
}