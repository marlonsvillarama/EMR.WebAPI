using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMR.WebAPI.ehr.models
{
    public class USState
    {
        public USState(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public string Code
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public static Dictionary<string, string> AllStates
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {"AL", "Alabama"},
                    {"AK", "Alaska"},
                    {"AZ", "Arizona"},
                    {"AR", "Arkansas"},
                    {"CA", "California"},
                    {"CO", "Colorado"},
                    {"CT", "Connecticut"},
                    {"DE", "Delaware"},
                    {"FL", "Florida"},
                    {"GA", "Georgia"},
                    {"HI", "Hawaii"},
                    {"ID", "Idaho"},
                    {"IL", "Illinois"},
                    {"IN", "Indiana"},
                    {"IA", "Iowa"},
                    {"KS", "Kansas"},
                    {"KY", "Kentucky"},
                    {"LA", "Louisiana"},
                    {"ME", "Maine"},
                    {"MD", "Maryland"},
                    {"MA", "Massachussets"},
                    {"MI", "Michigan"},
                    {"MN", "Minnesota"},
                    {"MS", "Mississippi"},
                    {"MO", "Missouri"},
                    {"MT", "Montana"},
                    {"NE", "Nebraska"},
                    {"NV", "Nevada"},
                    {"NH", "New Hampshire"},
                    {"NJ", "New Jersey"},
                    {"NM", "New Mexico"},
                    {"NY", "New York"},
                    {"NC", "North Carolina"},
                    {"ND", "North Dakota"},
                    {"OH", "Ohio"},
                    {"OK", "Oklahoma"},
                    {"OR", "Oregon"},
                    {"PA", "Pennsylvania"},
                    {"RI", "Rhode Island"},
                    {"SC", "South Carolina"},
                    {"SD", "South Dakota"},
                    {"TN", "Tennessee"},
                    {"TX", "Texas"},
                    {"UT", "Utah"},
                    {"VT", "Vermont"},
                    {"VA", "Virginia"},
                    {"WA", "Washington"},
                    {"WV", "West Virginia"},
                    {"WI", "Wisconsin"},
                    {"WY", "Wyoming" }
                };
            }
        }
    }
}