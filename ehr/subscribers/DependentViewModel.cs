using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMR.WebAPI.ehr.models
{
    public class DependentViewModel
    {
        string fn, ln, dob, gender, phone, rel;
        int patId, subId;

        public DependentViewModel()
        {
            patId = subId = -1;
            fn = ln = dob = gender = phone = String.Empty;
            rel = "18";
        }

        public int SubscriberId
        {
            get => subId;
            set => subId = value;
        }

        public int PatientId
        {
            get => patId;
            set => patId = value;
        }

        public string FirstName
        {
            get => fn;
            set => fn = value;
        }

        public string LastName
        {
            get => ln;
            set => ln = value;
        }

        public string FullName
        {
            get => String.Format("{0}, {1}", ln, fn);
        }

        public string DateOfBirth
        {
            get => dob;
            set => dob = value;
        }

        public string Gender
        {
            get => gender;
            set => gender = value;
        }

        public string Phone
        {
            get => phone;
            set => phone = value;
        }

        public string Relationship
        {
            get => rel;
            set => rel = value;
        }
    }
}
