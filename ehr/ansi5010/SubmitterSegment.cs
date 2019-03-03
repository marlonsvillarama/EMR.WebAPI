using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMR.WebAPI.ehr.models;

namespace EMR.WebAPI.ehr.ansi5010
{
    public class SubmitterSegment
    {
        Dictionary<string, string> CONFIG;
        Segment submitter;
        Segment contact;
        Segment receiver;
        int segmentCount;

        public SubmitterSegment(EHRDB db, string recName = "")
        {
            segmentCount = 0;

            Submitter sub = db.Submitters.FirstOrDefault();
            submitter = new Segment("NM1");
            submitter["NM101"] = "41";
            submitter["NM102"] = String.IsNullOrEmpty(sub.OrgName) == true ? "1" : "2";
            submitter["NM103"] = String.IsNullOrEmpty(sub.OrgName) == true ? sub.LastName : sub.OrgName;
            submitter["NM104"] = sub.FirstName;
            submitter["NM105"] = sub.MiddleName;
            submitter["NM108"] = "46";
            submitter["NM109"] = sub.Identifier;
            segmentCount++;

            contact = Segment.PER(
                "IC",
                sub.ContactName,
                sub.Phone,
                sub.Email,
                sub.Fax
            );

            Receiver rec = null;
            if (string.IsNullOrEmpty(recName))
            {
                rec = db.Receivers.FirstOrDefault();
            }
            else
            {
                List<Receiver> recs = db.Receivers.Where(x => x.Name == recName).ToList();
                if (recs.Count > 0)
                {
                    rec = recs[0];
                }
            }

            if (rec == null)
            {
                receiver = null;
                return;
            }

            rec = db.Receivers.FirstOrDefault();
            receiver = new Segment("NM1");
            receiver["NM101"] = "40";
            receiver["NM102"] = "2";
            receiver["NM103"] = rec.Name;
            receiver["NM108"] = "46";
            receiver["NM109"] = rec.Identifier;
            segmentCount++;
        }

        public Segment Submitter
        {
            get => submitter;
        }
        
        public Segment Contact
        {
            get => contact;
        }

        public Segment Receiver
        {
            get => receiver;
        }

        public int SegmentCount
        {
            get => segmentCount;
        }

        public string Output
        {
            get
            {
                return
                    Submitter.Output +
                    Contact.Output +
                    Receiver.Output;
            }
        }
    }
}