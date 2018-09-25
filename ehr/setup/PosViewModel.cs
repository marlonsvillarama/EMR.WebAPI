using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EMR.WebAPI.ehr.models
{
    public class PosViewModel
    {
        PlaceOfService p;
        string action;

        public PosViewModel()
        {
            p = new PlaceOfService();
        }

        public PosViewModel(PlaceOfService pos)
        {
            p = pos;
        }

        public string Action
        {
            get => action;
            set => action = value;
        }

        public PlaceOfService PlaceOfService
        {
            get => p;
            set => p = value;
        }

        public int Id
        {
            get => (p == null ? -1 : p.Id);
        }

        public string Code
        {
            get => (p == null ? String.Empty : p.Code);
        }

        public string Name
        {
            get => (p == null ? String.Empty : p.Name);
        }

        public string Description
        {
            get => (p == null ? String.Empty : p.Description);
        }
    }
}