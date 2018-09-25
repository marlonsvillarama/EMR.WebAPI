using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EMR.WebAPI.ehr.models
{
    public class PayerViewModel
    {
        Payer p;
        string action;
        IEnumerable<SelectListItem> allStates;

        public PayerViewModel()
        {
            p = new Payer();
        }

        public PayerViewModel(Payer payer)
        {
            p = payer;
        }

        public string Action
        {
            get => action;
            set => action = value;
        }

        public Payer Payer
        {
            get => p;
            set => p = value;
        }

        public string Name
        {
            get => (p == null ? String.Empty : p.Name);
        }

        public string PayerID
        {
            get => (p == null ? String.Empty : p.PayerId);
        }

        public string Phone_1
        {
            get => (p == null ? String.Empty : p.Phone_1);
        }

        public string PhoneFormatted_1
        {
            get => (p == null ? String.Empty : Utilities.FormatPhone(p.Phone_1));
        }

        public string Phone_2
        {
            get => (p == null ? String.Empty : p.Phone_2);
        }

        public string PhoneFormatted_2
        {
            get => (p == null ? String.Empty : Utilities.FormatPhone(p.Phone_2));
        }

        /*public string StateID
        {
            get;
            set;
        }*/

        /*public IEnumerable<SelectListItem> States
        {
            get => allStates;
            /*get
            {
                var states = new List<SelectListItem>();
                IEnumerable<SelectListItem> usStates = FormDataModel.GetStates();
                string s = i.Address == null ? String.Empty : i.Address.State;

                foreach (var state in usStates)
                {
                    states.Add(new SelectListItem()
                    {
                        Text = state.Text,
                        Value = state.Value,
                        Selected = s == state.Value ? true : false
                    });
                }

                return states;
            }*
        }*/
    }
}