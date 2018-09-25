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
            Data = null;
        }

        public ServiceRequestStatus(bool status, Object data)
        {
            IsSuccess = status;
            Data = data;
        }

        public bool IsSuccess;
        public Object Data;
    }
}