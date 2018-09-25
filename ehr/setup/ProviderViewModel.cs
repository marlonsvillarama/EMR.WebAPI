using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMR.WebAPI.ehr.models
{
    public class ProviderViewModel
    {
        Provider p;

        public ProviderViewModel(Provider provider)
        {
            p = provider;
        }

        public int Id
        {
            get => p.Id;
        }

        public string FirstName
        {
            get => p.FirstName;
        }

        public string LastName
        {
            get => p.LastName;
        }

        public string NameFormatted
        {
            get => String.Format("{0}, {1}", p.LastName, p.FirstName);
        }

        public bool IsCompany
        {
            get => p.IsCompany;
        }

        public string Email
        {
            get => p.Email;
        }

        public string Phone_1
        {
            get => p.Phone_1;
        }

        public string Phone_2
        {
            get => p.Phone_2;
        }

        public string PhoneFormatted_2
        {
            get => Utilities.FormatPhone(p.Phone_2);
        }

        public string PhoneFormatted_1
        {
            get => Utilities.FormatPhone(p.Phone_1);
        }

        public string EIN
        {
            get => p.EIN;
        }

        public string SSN
        {
            get => p.SSN;
        }

        public string NPI
        {
            get => p.NPI;
        }

        public string License
        {
            get => p.License;
        }

        public TaxonomyCode TaxonomyCode
        {
            get => p.TaxonomyCode;
        }
    }
}