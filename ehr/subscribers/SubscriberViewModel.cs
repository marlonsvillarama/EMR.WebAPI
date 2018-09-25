using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EMR.WebAPI.ehr.models
{
    public class SubscriberViewModel
    {
        Subscriber sub;

        public SubscriberViewModel(Subscriber subscriber)
        {
            sub = subscriber;
        }

        public int Id
        {
            get => sub.Id;
        }

        public string FirstName
        {
            get => sub.FirstName;
        }

        public string LastName
        {
            get => sub.LastName;
        }

        public string NameFormatted
        {
            get => String.Format("{0}, {1}", sub.LastName, sub.FirstName);
        }

        public string DateOfBirth
        {
            get => sub.DateOfBirth.Value.ToString("MMM d, yyyy");
        }

        public string Phone_1
        {
            get => sub.Phone_1;
        }

        public string Phone_2
        {
            get => sub.Phone_2;
        }

        public string Email
        {
            get => sub.Email;
        }

        public string PrimaryPayer
        {
            get => sub.PrimaryPayer.Name;
        }

        public string PrimaryMemberID
        {
            get => sub.PrimaryMemberID;
        }
    }
}