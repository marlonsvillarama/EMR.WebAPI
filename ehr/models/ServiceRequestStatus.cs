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
            Data2 = null;
            Count = 0;
        }

        public ServiceRequestStatus(bool status, Object data)
        {
            IsSuccess = status;
            Debug = "";
            Data = data;
            Data2 = null;
            Count = 0;
        }

        public bool IsSuccess;
        public string Debug;
        public Object Data;
        public Object Data2;
        public int Count;
    }
}