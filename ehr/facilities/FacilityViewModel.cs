using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMR.WebAPI.ehr.models
{
    public class FacilityViewModel
    {
        Facility f;

        public FacilityViewModel(Facility facility)
        {
            f = facility;
        }

        #region Properties
        public int Id
        {
            get => f.Id;
        }

        public string Name
        {
            get => f.Name;
        }

        public string Address_1
        {
            get => f.Address_1;
        }

        public string Address_2
        {
            get => f.Address_2;
        }

        public string City
        {
            get => f.City;
        }

        public string State
        {
            get => f.State;
        }

        public string Zip
        {
            get => f.Zip;
        }

        public string Phone
        {
            get => f.Phone;
        }

        public string NPI
        {
            get => f.NPI;
        }

        public string TaxId
        {
            get => f.TaxId;
        }
        #endregion
    }
}