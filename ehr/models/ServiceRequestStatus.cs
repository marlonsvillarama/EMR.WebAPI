using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMR.WebAPI.ehr.models
{
    public class ServiceRequestStatus
    {
        public ServiceRequestStatus(bool status = true)
        {
            IsSuccess = status;
            Debug = "";
            Data = null;
        }

        public ServiceRequestStatus(bool status, Object data)
        {
            IsSuccess = status;
            Debug = "";
            Data = data;
        }

        public bool IsSuccess;
        public string Debug;
        public Object Data;
    }
}